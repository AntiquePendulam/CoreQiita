using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoreQiita
{
    /// <summary>
    /// 認証クラス
    /// </summary>
    public class Auth
    {
        /// <summary>
        /// 認証用URL
        /// </summary>
        public string AuthURL { get; private set; }

        /// <summary>
        /// 認証中のトークン
        /// </summary>
        public Tokens Token { get;private set; }

        private string ClientID { get; set; }
        private string ClientSecret { get; set; }

        private readonly string[] scopemode = { "read_qiita", "write_qiita", "read_qiita+write_qiita" };

        /// <summary>
        /// OAuthでQiitaに接続します
        /// </summary>
        /// <param name="client_id">クライアントID</param>
        /// <param name="client_secret">クライアントシークレット</param>
        /// <param name="mode">Qiita権限)</param>
        public Auth(string client_id, string client_secret, ScopeMode mode = ScopeMode.READ_WRITE)
        {
            AuthURL = $"{Url.BASE_URL}api/v2/oauth/authorize?client_id={client_id}&scope={scopemode[(int)mode]}";
            this.ClientID = client_id;
            this.ClientSecret = client_secret;
        }

        /// <summary>
        /// アクセストークンを取得します
        /// </summary>
        /// <param name="code">ユーザ認証後のコード</param>
        /// <returns>トークン</returns>
        public Tokens GetToken(string code)
        {
            var tokenJson = new PostJson(ClientID, ClientSecret, code);
            var json = JsonConvert.SerializeObject(tokenJson);
            var content = new StringContent(json, Encoding.UTF8, ContentType.Json);

            var getResponse = JsonPost(content);
            getResponse.Wait();

            Token = new Tokens(getResponse.Result);

            return Token;
        }

        private static async Task<string> JsonPost(StringContent content)
        {
            string data;
            using (var client = new HttpClient() { BaseAddress = new Uri(Url.BASE_URL)})
            {
                var httpResponse = await client.PostAsync("api/v2/access_tokens", content);
                data = await httpResponse.Content.ReadAsStringAsync();
            }
            var result = JsonConvert.DeserializeObject<ResponseJson>(data);
            if (result.Token == null) return "Code Error.";
            return result.Token;
        }
    }
    [JsonObject]
    internal class PostJson
    {
        [JsonProperty("client_id")]
        private string Id { get; set; }

        [JsonProperty("client_secret")]
        private string Secret { get; set; }

        [JsonProperty("code")]
        private string Code { get; set; }

        internal PostJson(string id,string secret, string code)
        {
            this.Id = id;
            this.Secret = secret;
            this.Code = code;
        }
    }

    [JsonObject]
    internal class ResponseJson
    {
        [JsonProperty("token")]
        internal string Token { get; set; }
    }
}

/*
********************************************************
    Copyright (c) 2018 AntiqueR
    Released under the MIT license
    https://opensource.org/licenses/mit-license.php
*******************************************************
*/
