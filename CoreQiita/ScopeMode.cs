using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreQiita
{
    /// <summary>
    /// Qiitaの操作権限 列挙型
    /// </summary>
    public enum ScopeMode : byte
    {
        /// <summary>
        /// 読み取り権限
        /// </summary>
        READ = 0,
        /// <summary>
        /// 書き込み権限
        /// </summary>
        WRITE = 1,
        /// <summary>
        /// 読み書き権限
        /// </summary>
        READ_WRITE = 2
    }
}
