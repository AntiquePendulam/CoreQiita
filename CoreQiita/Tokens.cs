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
        public Users Users { get; set; }

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
            var message = await client.DeleteAsync($"{Url.BASE_URL}/api/v2/access_tokens/{Token}");
            if ((int)message.StatusCode == 204) return true;
            else return false;
        }
    }
}
