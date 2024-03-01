using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.ValueObject
{
    /// <summary>
    /// 收货人信息
    /// </summary>
    public class OmsOrderReceiverVo
    {
        /// <summary>
        /// 收货人姓名
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Name { get; set; } = "";

        /// <summary>
        /// 电话
        /// </summary>
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = "";

        /// <summary>
        /// 收货地址
        /// </summary>
        [Required]
        public OmsOrderReceiverAddress Address { get; set; }
    }

    /// <summary>
    /// 收货人信息-地址
    /// </summary>
    public class OmsOrderReceiverAddress
    {
        /// <summary>
        /// 省份
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Province { get; set; } = "";

        /// <summary>
        /// 城市
        /// </summary>
        [Required]
        [StringLength(10)]
        public string City { get; set; } = "";

        /// <summary>
        /// 区县
        /// </summary>
        [Required]
        [StringLength(10)]
        public string District { get; set; } = "";

        /// <summary>
        /// 详细地址
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Address { get; set; } = "";
    }
}
