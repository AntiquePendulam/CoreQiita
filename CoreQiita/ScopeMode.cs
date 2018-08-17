using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreQiita
{
    public enum ScopeMode : byte
    {
        [Content("read_qiita")]
        READ = 0,
        [Content("write_qiita")]
        WRITE = 1,
        [Content("read_qiita+write_qiita")]
        READ_WRITE = 2
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class ContentAttribute : Attribute
    {
        private string mode;
        public ContentAttribute(string mode) { this.mode = mode; }
        public string Mode { get { return this.mode; } }
    }
}
