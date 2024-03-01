using Oms.Domain.AggregateRoots;
using Oms.Domain.Models;
using OneForAll.Core;
using OneForAll.Core.Upload;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.Interfaces
{
    /// <summary>
    /// 微信小程序支付设置
    /// </summary>
    public interface IOmsWxPaySettingManager
    {
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>用户分页</returns>
        Task<PageList<OmsWxPaySetting>> GetPageAsync(int pageIndex, int pageSize, string key);

        /// <summary>
        /// 添加/修改
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddAsync(OmsWxPaySettingForm form);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        Task<BaseErrType> UpdateAsync(OmsWxPaySettingForm form);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">用户id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids);

        /// <summary>
        /// 上传商户证书
        /// </summary>
        /// <param name="id">数据id</param>
        /// <param name="filename">文件名</param>
        /// <param name="file">文件流</param>
        /// <returns>上传结果</returns>
        Task<IUploadResult> UploadCertificateAsync(Guid id, string filename, Stream file);
    }
}
