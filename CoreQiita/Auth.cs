using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoreQiita
{
    public class Auth
    {
        public string AuthURL { get; private set; }
        public Tokens Token { get; set; }

        private string ClientID { get; set; }
        private string ClientSecret { get; set; }

        public Auth(string client_id, string client_secret, ScopeMode mode = ScopeMode.READ_WRITE)
        {
            AuthURL = $"{Url.BASE_URL}api/v2/oauth/authorize?client_id={client_id}&scope={GetMode(mode)}";
            this.ClientID = client_id;
            this.ClientSecret = client_secret;
        }

        private string GetMode(ScopeMode mode)
        {
            var memberInfo = mode.GetType().GetMember(mode.ToString()).First();
            var attribute = Attribute.GetCustomAttribute(memberInfo, typeof(ContentAttribute)) as ContentAttribute;
            return attribute.Mode;
        }

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
    class PostJson
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
    class ResponseJson
    {
        [JsonProperty("token")]
        internal string Token { get; set; }
    }
}
