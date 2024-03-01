using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Oms.Domain.AggregateRoots;
using Oms.Domain.Interfaces;
using Oms.Domain.Models;
using Oms.Domain.Repositorys;
using OneForAll.Core;
using OneForAll.Core.Extension;
using OneForAll.Core.Upload;
using OneForAll.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain
{
    /// <summary>
    /// 微信小程序支付设置
    /// </summary>
    public class OmsWxPaySettingManager : OmsBaseManager, IOmsWxPaySettingManager
    {
        private readonly IUploader _uploader;
        private readonly IOmsWxPaySettingRepository _repository;
        private readonly string UPLOAD_PATH = "/upload/tenants/{0}/wxpay/{1}/";// 文件存储路径
        //private readonly string VIRTUAL_PATH = "/resources/tenants/{0}/wxpay/{1}/";// 虚拟路径：根据Startup配置设置,返回给前端访问资源

        public OmsWxPaySettingManager(
            IMapper mapper,
            IUploader uploader,
            IHttpContextAccessor httpContextAccessor,
            IOmsWxPaySettingRepository repository) : base(mapper, httpContextAccessor)
        {
            _uploader = uploader;
            _repository = repository; ;
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>用户分页</returns>
        public async Task<PageList<OmsWxPaySetting>> GetPageAsync(int pageIndex, int pageSize, string key)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;
            var data = await _repository.GetPageAsync(pageIndex, pageSize, key);
            return new PageList<OmsWxPaySetting>(data.Total, data.PageIndex, data.PageSize, data.Items);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(OmsWxPaySettingForm form)
        {
            var count = await _repository.CountAsync();
            if (count >= 5)
                return BaseErrType.Overflow;
            var data = await _repository.GetAsync(w => w.AppId == form.AppId || w.AppName == form.AppName);
            if (data != null)
                return BaseErrType.DataExist;

            data = _mapper.Map<OmsWxPaySetting>(form);
            data.SysTenantId = LoginUser.SysTenantId;
            data.CreatorId = LoginUser.Id;
            data.CreatorName = LoginUser.Name;
            return await ResultAsync(() => _repository.AddAsync(data));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> UpdateAsync(OmsWxPaySettingForm form)
        {
            var data = await _repository.GetAsync(w => w.AppId == form.AppId);
            if (data != null && data.Id != form.Id)
                return BaseErrType.DataExist;
            if (data.AppName != form.AppName)
            {
                // 修改了应用名称，判断是否有重复
                var nameData = await _repository.GetAsync(w => w.AppName == form.AppName);
                if (nameData != null && nameData.Id != form.Id)
                    return BaseErrType.DataExist;
            }

            data = await _repository.FindAsync(form.Id);
            if (data == null)
                return BaseErrType.DataNotFound;

            _mapper.Map(form, data);
            data.CreatorId = LoginUser.Id;
            data.CreatorName = LoginUser.Name;
            return await ResultAsync(() => _repository.SaveChangesAsync());
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">用户id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids)
        {
            var data = await _repository.GetListAsync(ids);
            if (!data.Any())
                return BaseErrType.DataNotFound;

            return await ResultAsync(() => _repository.DeleteRangeAsync(data));
        }

        /// <summary>
        /// 上传商户证书
        /// </summary>
        /// <param name="id">数据id</param>
        /// <param name="filename">文件名</param>
        /// <param name="file">文件流</param>
        /// <returns>上传结果</returns>
        public async Task<IUploadResult> UploadCertificateAsync(Guid id, string filename, Stream file)
        {
            var maxSize = 1 * 1024 * 1024;
            var result = new UploadResult();
            var data = await _repository.FindAsync(id);
            if (data == null)
                return result;

            var extension = Path.GetExtension(filename);
            var relativePath = UPLOAD_PATH.Fmt(LoginUser.SysTenantId, id);
            var uploadPath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + relativePath);
            if (new ValidateZipType().Validate(filename, file))
            {
                result = await _uploader.WriteAsync(file, uploadPath, filename, maxSize) as UploadResult;
                // 解压文件
                var zipFileName = Path.Combine(uploadPath, filename);
                ZipFile.ExtractToDirectory(zipFileName, uploadPath);
                File.Delete(zipFileName);

                // 设置返回虚拟路径
                if (result.State == UploadEnum.Success)
                {
                    data.CertificateUrl = Path.Combine(relativePath, "apiclient_cert.pem");
                    data.CertificateKeyUrl = Path.Combine(relativePath, "apiclient_key.pem");
                    if (data.Error.Contains("上传微信支付证书"))
                        data.Error = "";// 上传证书后刷新异常提示
                    var errType = await _repository.UpdateAsync(data);
                    result.State = errType > 0 ? UploadEnum.Success : UploadEnum.Error;
                }
            }
            else
            {
                result.State = UploadEnum.TypeError;
            }
            return result;
        }
    }
}
