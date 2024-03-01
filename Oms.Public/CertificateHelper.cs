using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Oms.Public
{
    /// <summary>
    /// 证书
    /// </summary>
    public static class CertificateHelper
    {
        /// <summary>
        /// 获取证书序列号
        /// </summary>
        public static string GetSerialNumber(string path)
        {
            X509Certificate cert = X509Certificate.CreateFromCertFile(path);
            return cert.GetSerialNumberString();
        }
    }
}
