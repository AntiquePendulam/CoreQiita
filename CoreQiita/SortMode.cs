using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreQiita
{
    /// <summary>
    /// ソートモード 列挙型
    /// </summary>
    public enum SortMode
    {
        /// <summary>
        /// 投稿数でソート
        /// </summary>
        Count = 0,

        /// <summary>
        /// 名前順でソート
        /// </summary>
        Name = 1
    }
}
