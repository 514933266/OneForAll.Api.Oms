using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using Newtonsoft.Json;
using OneForAll.EFCore;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Hosting;
using OneForAll.Core.Extension;
using Oms.Host.Model;
using Oms.Host.Models;
using Autofac.Core;
using Microsoft.AspNetCore.Mvc;
using Oms.Host.Filters;
using OneForAll.Core.Upload;
using OneForAll.File;
using Oms.Host.Hubs;
using Oms.HttpService.Models;
using Oms.Public.Models;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;
using Oms.Host.Providers;
using Oms.Host.QuartzJobs;
using OneForAll.Core.Utility;
using NPOI.SS.Formula.Functions;

namespace Oms.Host
{
    public class Startup
    {
        private const string CORS = "Cors";
        private const string AUTH = "Auth";
        private const string QUARTZ = "Quartz";
        private const string BASE_HOST = "Oms.Host";
        private const string BASE_APPLICATION = "Oms.Application";
        private const string BASE_DOMAIN = "Oms.Domain";
        private const string BASE_REPOSITORY = "Oms.Repository";
        private readonly string HTTP_SERVICE_KEY = "HttpService";
        private readonly string HTTP_SERVICE = "Oms.HttpService";
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }



        public void ConfigureServices(IServiceCollection services)
        {
            #region Cors

            var corsConfig = new CorsConfig();
            Configuration.GetSection(CORS).Bind(corsConfig);
            if (corsConfig.Origins.Contains("*") || !corsConfig.Origins.Any())
            {
                // 不限制跨域
                services.AddCors(option => option.AddPolicy(CORS, policy => policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                ));
            }
            else
            {
                services.AddCors(option => option.AddPolicy(CORS, policy => policy
                    .WithOrigins(corsConfig.Origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod().
                    AllowCredentials()
                ));
            }

            #endregion

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                typeof(ApiVersion).GetEnumNames().ToList().ForEach(version =>
                {
                    c.SwaggerDoc(version, new OpenApiInfo
                    {
                        Version = version,
                        Title = $"商户中心-接口文档 {version}",
                        Description = $"OneForAll Oms Web API {version}"
                    });
                });


                var xmlHostFile = BASE_HOST.Append(".xml");
                var xmlAppFile = BASE_APPLICATION.Append(".xml");
                var xmlDomainFile = BASE_DOMAIN.Append(".xml");
                var xmlHostPath = Path.Combine(AppContext.BaseDirectory, xmlHostFile);
                var xmlAppPath = Path.Combine(AppContext.BaseDirectory, xmlAppFile);
                var xmlDomainPath = Path.Combine(AppContext.BaseDirectory, xmlDomainFile);
                c.IncludeXmlComments(xmlHostPath, true);
                c.IncludeXmlComments(xmlAppPath);
                c.IncludeXmlComments(xmlDomainPath);

                c.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
                {
                    Description = "身份授权，直接在下框中输入Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });
            #endregion

            #region Quartz

            var quartzConfig = new QuartzScheduleJobConfig();
            Configuration.GetSection(QUARTZ).Bind(quartzConfig);
            // 注册QuartzJobs目录下的定时任务
            if (quartzConfig != null)
            {
                services.AddSingleton(quartzConfig);
                services.AddSingleton<IJobFactory, ScheduleJobFactory>();
                services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
                services.AddHostedService<QuartzJobHostService>();
                var jobNamespace = BASE_HOST.Append(".QuartzJobs");
                quartzConfig.ScheduleJobs.ForEach(e =>
                {
                    var typeName = jobNamespace + "." + e.TypeName;
                    var jobType = Assembly.Load(BASE_HOST).GetType(typeName);
                    if (jobType != null)
                    {
                        e.JobType = jobType;
                        services.AddSingleton(e.JobType);
                    }
                });
            }
            #endregion

            #region Http

            var serviceConfig = new HttpServiceConfig();
            Configuration.GetSection(HTTP_SERVICE_KEY).Bind(serviceConfig);
            var props = ReflectionHelper.GetPropertys(serviceConfig);
            props.ForEach(e =>
            {
                services.AddHttpClient(e.Name, c =>
                {
                    c.BaseAddress = new Uri(e.GetValue(serviceConfig).ToString());
                    c.DefaultRequestHeaders.Add("ClientId", ClientClaimType.Id);
                });
            });

            #endregion

            #region IdentityServer4

            var authConfig = new AuthConfig();
            Configuration.GetSection(AUTH).Bind(authConfig);
            services.AddAuthentication(authConfig.Type)
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = authConfig.Url;
                options.RequireHttpsMetadata = false;
            });

            #endregion

            #region SingleR
            services.AddSignalR();
            #endregion

            #region AutoMapper
            services.AddAutoMapper(config =>
            {
                config.AllowNullDestinationValues = false;
            }, Assembly.Load(BASE_HOST));
            #endregion

            #region DI

            services.AddDbContext<OmsContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:Default"]));
            services.AddSingleton<IUploader, Uploader>();
            services.AddScoped<ITenantProvider, TenantProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<HttpServiceConfig>();
            services.AddSingleton(authConfig);

            #endregion

            #region Redis
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration["Redis:ConnectionString"];
                options.InstanceName = Configuration["Redis:InstanceName"];
            });
            #endregion

            #region Mvc
            services.AddControllers(options =>
            {
                options.Filters.Add<AuthorizationFilter>();
                options.Filters.Add<ApiModelStateFilter>();
                options.Filters.Add<ExceptionFilter>();
                options.EnableEndpointRouting = false;
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            #endregion
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // 基础
            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IEFCoreRepository<>));

            builder.RegisterAssemblyTypes(Assembly.Load(BASE_APPLICATION))
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.Load(BASE_DOMAIN))
                .Where(t => t.Name.EndsWith("Manager"))
                .AsImplementedInterfaces();

            builder.RegisterType(typeof(OmsContext)).Named<DbContext>("OmsContext");
            builder.RegisterAssemblyTypes(Assembly.Load(BASE_REPOSITORY))
               .Where(t => t.Name.EndsWith("Repository"))
               .WithParameter(ResolvedParameter.ForNamed<DbContext>("OmsContext"))
               .AsImplementedInterfaces();

            // Http
            builder.RegisterAssemblyTypes(Assembly.Load(HTTP_SERVICE))
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    typeof(ApiVersion).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                    {
                        c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{version}");
                    });
                });
            }

            DirectoryHelper.Create(Path.Combine(Directory.GetCurrentDirectory(), @"upload"));
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"upload")),
                RequestPath = new PathString("/resources"),
                OnPrepareResponse = (c) =>
                {
                    c.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                }
            });

            app.UseCors(CORS);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseMiddleware<ApiLogMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
