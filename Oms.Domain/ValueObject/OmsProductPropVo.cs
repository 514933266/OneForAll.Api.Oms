using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Domain.ValueObject
{
    /// <summary>
    /// 商品属性
    /// </summary>
    public class OmsProductPropVo
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Value { get; set; }
    }
}
