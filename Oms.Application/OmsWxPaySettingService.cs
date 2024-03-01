using AutoMapper;
using Oms.Application.Dtos;
using Oms.Application.Interfaces;
using Oms.Domain.AggregateRoots;
using Oms.Domain.Interfaces;
using Oms.Domain.Models;
using OneForAll.Core;
using OneForAll.Core.Upload;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Application
{
    /// <summary>
    /// 微信小程序支付设置
    /// </summary>
    public class OmsWxPaySettingService : IOmsWxPaySettingService
    {
        private readonly IMapper _mapper;
        private readonly IOmsWxPaySettingManager _manager;

        public OmsWxPaySettingService(
            IMapper mapper,
            IOmsWxPaySettingManager manager)
        {
            _mapper = mapper;
            _manager = manager;
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>分页</returns>
        public async Task<PageList<OmsWxPaySettingDto>> GetPageAsync(int pageIndex, int pageSize, string key)
        {
            var data = await _manager.GetPageAsync(pageIndex, pageSize, key);
            var items = _mapper.Map<IEnumerable<OmsWxPaySetting>, IEnumerable<OmsWxPaySettingDto>>(data.Items);
            return new PageList<OmsWxPaySettingDto>(data.Total, data.PageSize, data.PageIndex, items);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(OmsWxPaySettingForm form)
        {
            return await _manager.AddAsync(form);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="form">用户</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> UpdateAsync(OmsWxPaySettingForm form)
        {
            return await _manager.UpdateAsync(form);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">用户id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids)
        {
            return await _manager.DeleteAsync(ids);
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
            return await _manager.UploadCertificateAsync(id, filename, file);
        }
    }
}
