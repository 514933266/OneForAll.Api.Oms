using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oms.Application.Dtos;
using Oms.Application.Interfaces;
using Oms.Domain.Models;
using Oms.Host.Filters;
using Oms.Public.Models;
using OneForAll.Core;
using OneForAll.Core.OAuth;
using OneForAll.Core.Upload;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Oms.Host.Controllers
{
    /// <summary>
    /// 微信小程序支付设置
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoleType.ADMIN)]
    public class OmsWxPaySettingsController : BaseController
    {
        private readonly IOmsWxPaySettingService _service;
        public OmsWxPaySettingsController(IOmsWxPaySettingService service)
        {
            _service = service;
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>用户列表</returns>
        [HttpGet]
        [Route("{pageIndex}/{pageSize}")]
        [CheckPermission(Action = ConstPermission.EnterView)]
        public async Task<PageList<OmsWxPaySettingDto>> GetListAsync(int pageIndex, int pageSize, [FromQuery] string key)
        {
            return await _service.GetPageAsync(pageIndex, pageSize, key);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        [HttpPost]
        [CheckPermission(Action = ConstPermission.EnterView)]
        public async Task<BaseMessage> AddAsync([FromBody] OmsWxPaySettingForm form)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.AddAsync(form);

            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("添加成功");
                case BaseErrType.Overflow: return msg.Success("最大支持5个商户绑定");
                default: return msg.Fail("添加失败");
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">表单</param>
        /// <returns>结果</returns>
        [HttpPut]
        [CheckPermission(Action = ConstPermission.EnterView)]
        public async Task<BaseMessage> UpdateAsync([FromBody] OmsWxPaySettingForm entity)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.UpdateAsync(entity);

            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("修改成功");
                case BaseErrType.DataExist: return msg.Fail("商户号已绑定");
                default: return msg.Fail("修改失败");
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">配置id</param>
        /// <returns>消息</returns>
        [HttpPatch]
        [Route("Batch/IsDeleted")]
        [CheckPermission(Action = ConstPermission.EnterView)]
        public async Task<BaseMessage> DeleteAsync([FromBody] IEnumerable<Guid> ids)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.DeleteAsync(ids);

            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("删除成功");
                case BaseErrType.DataNotFound: return msg.Success("请先选择要删除的数据");
                default: return msg.Fail("删除失败");
            }
        }

        /// <summary>
        /// 上传商户证书
        /// </summary>
        [HttpPost]
        [Route("{id}/Certificates")]
        [CheckPermission(Action = ConstPermission.EnterView)]
        public async Task<BaseMessage> UploadCertificateAsync(Guid id, [FromForm] IFormCollection form)
        {
            var msg = new BaseMessage();
            if (form.Files.Count > 0)
            {
                var file = form.Files[0];

                var callbacks = await _service.UploadCertificateAsync(id, file.FileName, file.OpenReadStream());

                switch (callbacks.State)
                {
                    case UploadEnum.Success: return msg.Success("上传成功");
                    case UploadEnum.Overflow: return msg.Fail("文件超出限制大小");
                    case UploadEnum.TypeError: return msg.Fail("请选择pem证书文件上传");
                    case UploadEnum.Error: return msg.Fail("上传过程中发生未知错误");
                }
            }
            return msg.Fail("上传失败，请选择文件");
        }
    }
}
