﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oms.HttpService.Models
{
    /// <summary>
    /// 定时服务下线
    /// </summary>
    public class JobTaskDownLineFormRequest
    {
        /// <summary>
        /// 应用程序id
        /// </summary>
        [Required]
        [StringLength(50)]
        public string AppId { get; set; }

        /// <summary>
        /// 应用程序密钥
        /// </summary>
        [Required]
        [StringLength(50)]
        public string AppSecret { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Sign { get; set; }
    }
}
