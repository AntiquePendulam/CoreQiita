using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoreQiita
{
    public class Tokens
    {
        public string Token { get; private set; }
        public Users Users { get;private set; }
        public Items Items { get;private set; }

        internal HttpClient client = new HttpClient() {BaseAddress = new Uri(Url.BASE_URL) };

        /// <summary>
        /// GetToken
        /// </summary>
        /// <param name="token">Access Token</param>
        public Tokens(string token)
        {
            this.Token = token;
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            Users = new Users()
            {
                Client = client
            };
            Items = new Items()
            {
                Client = client
            };
            
        }

        /// <summary>
        /// Delete Token
        /// Successful:True/ Error:False
        /// </summary>
        /// <returns>bool</returns>
        public bool TokenDelete()
        {
            var task = TokenDeleteAsync();
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Delete Token Async
        /// Successful:True/ Error:False
        /// </summary>
        /// <returns>bool</returns>
        public async Task<bool> TokenDeleteAsync()
        {
            var message = await client.DeleteAsync($"/api/v2/access_tokens/{Token}");
            return (int)message.StatusCode == 204;
        }

        public bool DeleteItem(string item_id)
        {
            var async = DeleteItemAsync(item_id);
            async.Wait();
            return async.Result;
        }

        public async Task<bool> DeleteItemAsync(string item_id)
        {
            var message = await client.DeleteAsync($"/api/v2/access_tokens/{Token}");
            return (int)message.StatusCode == 200;
        }

        public LikeData[] Likes(string item_id)
        {
            var async = LikesAsync(item_id);
            async.Wait();
            return async.Result;
        }
        public async Task<LikeData[]> LikesAsync(string item_id)
        {
            var message = await client.GetAsync($"/api/v2/items/{item_id}/likes");
            var response = await message.Content.ReadAsStringAsync();
            var Result = JsonConvert.DeserializeObject<LikeData[]>(response);
            return Result;
        }
    }


    [JsonObject]
    public class LikeData
    {
        [JsonProperty("created_at")]
        internal string _Date { get; set; }

        private DateTime _date;
        public DateTime Date
        {
            get
            {
                if (_date != DateTime.MinValue) return _date;
                _date = DateTime.Parse(_Date);
                return _date;
            }
        }

        [JsonProperty("user")]
        public UserJson User { get; internal set; } 
    }
}
