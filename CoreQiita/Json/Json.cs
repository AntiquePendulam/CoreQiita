using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoreQiita.Json
{
    class Json
    {

    }

    [JsonObject]
    class TokenJson
    {
        [JsonProperty("token")]
        public string Code { get; set; }

        internal void Tokens(string code) { this.Code = code; }
    }
}
