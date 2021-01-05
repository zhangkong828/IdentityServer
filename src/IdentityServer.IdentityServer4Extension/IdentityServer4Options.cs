using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class IdentityServer4Options
    {
        /// <summary>
        /// 登录url
        /// </summary>
        public string LoginUrl { get; set; }
        /// <summary>
        /// 登出url
        /// </summary>
        public string LogoutUrl { get; set; }

        /// <summary>
        /// 是否启用自定清理Token
        /// </summary>
        public bool EnableTokenCleanup { get; set; } = true;

        /// <summary>
        /// 清理token周期（单位秒），默认1小时
        /// </summary>
        public int TokenCleanupInterval { get; set; } = 3600;

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string DbConnectionStrings { get; set; }

        public string MigrationsAssembly { get; set; }
    }
}
