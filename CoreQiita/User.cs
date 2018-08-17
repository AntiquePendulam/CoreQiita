using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace CoreQiita
{
    public class Users
    {
        private string USER_ID { get; set; }
        public AuthUserJson AuthUser { get; private set; }

        public string USER_URL { get; private set; }

        private HttpClient _client;
        internal HttpClient Client
        {
            get
            {
                return _client;
            }
            set
            {
                _client = value;
                UserJson.Client = _client;
            }
        }

        /// <summary>
        /// Get User by user_id
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public UserJson GetUser(string user_id)
        {
            USER_URL = $"{Url.BASE_USER_URL}users/{user_id}";
            return GetUser<UserJson>();
        }

        /// <summary>
        /// Get User by page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="per_page"></param>
        /// <returns></returns>
        public UserJson[] GetAllUsers(int page, int per_page = 1)
        {
            var userAsync = GetAllUsersAsync(page,per_page);
            userAsync.Wait();
            return userAsync.Result;
        }

        /// <summary>
        /// Get User by page :Async
        /// </summary>
        /// <param name="page"></param>
        /// <param name="per_page"></param>
        /// <returns></returns>
        public async Task<UserJson[]> GetAllUsersAsync(int page,int per_page = 1)
        {
            var message = await Client.GetAsync($"{Url.BASE_USER_URL}users?page={page}&per_page{per_page}");
            var response = await message.Content.ReadAsStringAsync();
            var Result = JsonConvert.DeserializeObject<UserJson[]>(response);
            return Result;
        }

        /// <summary>
        /// Get Auth User
        /// </summary>
        /// <returns>Auth User</returns>
        public AuthUserJson GetUser()
        {
            if (AuthUser != null) return AuthUser;
            USER_URL = $"{Url.BASE_USER_URL}authenticated_user";
            return AuthUser = GetUser<AuthUserJson>();
        }

        /// <summary>
        /// Get User
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <returns></returns>
        private Type GetUser<Type>()
        {
            var getUserAsync = GetUserAsync<Type>();
            getUserAsync.Wait();
            return getUserAsync.Result;
        }

        /// <summary>
        /// Get User Async
        /// </summary>
        /// <typeparam name="Type">UserJson or AuthUserJson</typeparam>
        /// <returns></returns>
        public async Task<Type> GetUserAsync<Type>()
        {
            var message = await Client.GetAsync(USER_URL);
            var response = await message.Content.ReadAsStringAsync();
            var Result = JsonConvert.DeserializeObject<Type>(response);
            return Result;
        }
    }
    
    /// <summary>
    /// JsonObject for User
    /// 
    /// GetAll()
    /// GetAlProperties
    /// </summary>
    [JsonObject]
    public class UserJson
    {
        [JsonProperty("description")]
        public string Description { get;internal set; }

        [JsonProperty("facebook_id")]
        public string FacebookId { get; internal set; }

        [JsonProperty("followees_count")]
        public int FolloweesCount { get; internal set; }

        [JsonProperty("followers_count")]
        public int FollowersCount { get; internal set; }

        [JsonProperty("github_login_name")]
        public string GitHubName { get; internal set; }

        [JsonProperty("id")]
        public string Id { get; internal set; }

        [JsonProperty("items_count")]
        public int ItemsCount { get; internal set; }

        [JsonProperty("linkedin_id")]
        public string LinkedInId { get; internal set; }

        [JsonProperty("location")]
        public string Location { get; internal set; }

        [JsonProperty("name")]
        public string Name { get; internal set; }

        [JsonProperty("organization")]
        public string Organization { get; internal set; }

        [JsonProperty("permanent_id")]
        public int PermanentId { get; internal set; }

        [JsonProperty("profile_image_url")]
        public string ProfileImageUrl { get; internal set; }

        [JsonProperty("twitter_screen_name")]
        public string TwitterName { get; internal set; }

        [JsonProperty("website_url")]
        public string WebsiteUrl { get; internal set; }

        internal static HttpClient Client {private get; set; }

        public string[] GetAll()
        {
            string[] vs = {Description,FacebookId,FolloweesCount.ToString(),FollowersCount.ToString(),GitHubName,Id,ItemsCount.ToString(),
                        LinkedInId,Location,Name,Organization,PermanentId.ToString(),ProfileImageUrl,TwitterName,WebsiteUrl};
            return vs;
        }

        #region Get Folowees
        public UserJson[] GetFolowees(int page = 1,int per_page = 1)
        {
            var async = GetFoloweesAsync(page,per_page);
            async.Wait();
            return async.Result;
        }
        public async Task<UserJson[]> GetFoloweesAsync(int page = 1, int per_page = 1)
        {
            var message = await Client.GetAsync($"{Url.BASE_USER_URL}users/{Id}/followees?page={page}&per_page{per_page}");
            var response = await message.Content.ReadAsStringAsync();
            var Result = JsonConvert.DeserializeObject<UserJson[]>(response);
            return Result;
        }
        #endregion

        #region GetFolowers
        public UserJson[] GetFolowers(int page = 1, int per_page = 1)
        {
            var async = GetFolowersAsync(page, per_page);
            async.Wait();
            return async.Result;
        }
        public async Task<UserJson[]> GetFolowersAsync(int page = 1, int per_page = 1)
        {
            var message = await Client.GetAsync($"{Url.BASE_USER_URL}users/{Id}/followers?page={page}&per_page{per_page}");
            var response = await message.Content.ReadAsStringAsync();
            var Result = JsonConvert.DeserializeObject<UserJson[]>(response);
            return Result;
        }
        #endregion
    }

    /// <summary>
    /// JsonObject for AuthUser
    /// Override UserJson
    /// 
    /// GetAll()
    /// GetAlProperties
    /// </summary>
    [JsonObject]
    public class AuthUserJson : UserJson
    {
        [JsonProperty("image_monthly_upload_limit")]
        public int ImageUploadLimit { get; internal set; }

        [JsonProperty("image_monthly_upload_remaining")]
        public int ImageUploadRemaining { get; internal set; }

        [JsonProperty("team_only")]
        public bool TeamOnly { get; internal set; }

        public new string[] GetAll()
        {
            string[] vs = {Description,FacebookId,FolloweesCount.ToString(),FollowersCount.ToString(),GitHubName,Id,ItemsCount.ToString(),
                        LinkedInId,Location,Name,Organization,PermanentId.ToString(),ProfileImageUrl,TwitterName,WebsiteUrl,ImageUploadLimit.ToString(),
                        ImageUploadRemaining.ToString(),TeamOnly.ToString()};
            return vs;
        }
    }
}
