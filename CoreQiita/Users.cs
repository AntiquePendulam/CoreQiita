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

        #region GetUser by user_id
        /// <summary>
        /// Get User by user_id
        /// </summary>
        /// <param name="user_id">User id</param>
        /// <returns>UserData</returns>
        public UserJson GetUser(string user_id)
        {
            var async = GetUserAsync(user_id);
            async.Wait();
            return async.Result;
        }

        /// <summary>
        /// Get User by user_id
        /// </summary>
        /// <param name="user_id">User id</param>
        /// <returns>UserData</returns>
        public async Task<UserJson> GetUserAsync(string user_id)
        {
            var url = $"api/v2/users/{user_id}";
            return await GetUserAsync<UserJson>(url);
        }
        #endregion

        #region GetAllUsers
        /// <summary>
        /// Get User by page
        /// </summary>
        /// <param name="page">page number</param>
        /// <param name="per_page">per page users</param>
        /// <returns>UsersData(Array)</returns>
        public UserJson[] GetAllUsers(int page = 1, int per_page = 5)
        {
            var userAsync = GetAllUsersAsync(page,per_page);
            userAsync.Wait();
            return userAsync.Result;
        }

        /// <summary>
        /// Get User by page :Async
        /// </summary>
        /// <param name="page">page number</param>
        /// <param name="per_page">per page users</param>
        /// <returns>UsersData(Array)</returns>
        public async Task<UserJson[]> GetAllUsersAsync(int page = 1,int per_page = 5)
        {
            var url = $"api/v2/users?page={page}&per_page{per_page}";
            return await GetUserAsync<UserJson[]>(url);
        }
        #endregion

        #region GetStockers
        /// <summary>
        /// Get Stocker by item_id
        /// </summary>
        /// <param name="item_id">article id</param>
        /// <param name="page">page number</param>
        /// <param name="per_page">per page users</param>
        /// <returns>UsersData(Array)</returns>
        public UserJson[] GetStockers(string item_id,int page = 1,int per_page = 5)
        {
            var async = GetStockerAsync(item_id,page,per_page);
            async.Wait();
            return async.Result;
        }

        /// <summary>
        /// Get Stocker Async by item_id
        /// </summary>
        /// <param name="item_id">article id</param>
        /// <param name="page">page number</param>
        /// <param name="per_page">per page users</param>
        /// <returns>UsersData(Array)</returns>
        public async Task<UserJson[]> GetStockerAsync(string item_id, int page = 1, int per_page = 5)
        {
            var url = $"api/v2/items/{item_id}/stockers?page={page}&per_page={per_page}";
            return await GetUserAsync<UserJson[]>(url);
        }
        #endregion

        #region Get Auth User
        /// <summary>
        /// Get Auth User
        /// </summary>
        /// <returns>Auth User Data</returns>
        public AuthUserJson GetUser()
        {
            if (AuthUser != null) return AuthUser;
            var async = GetUserAsync();
            async.Wait();
            return async.Result;
        }

        /// <summary>
        /// Get Auth User Async
        /// </summary>
        /// <returns>Auth User Data</returns>
        public async Task<AuthUserJson> GetUserAsync()
        {
            if (AuthUser != null) return AuthUser;
            var url = $"api/v2/authenticated_user";

            return await GetUserAsync<AuthUserJson>(url);
        }
        #endregion

        /// <summary>
        /// Get User Async by url
        /// </summary>
        /// <typeparam name="Type">DataType: UserJson or AuthUserJson</typeparam>
        /// <param name="url">Url: do not inclue Host</param>
        /// <returns>UserData</returns>
        public async Task<Type> GetUserAsync<Type>(string url)
        {
            var message = await Tokens.client.GetAsync(url);
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
        #region JsonProperties
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
        #endregion

        public string[] GetAll()
        {
            string[] vs = {Description,FacebookId,FolloweesCount.ToString(),FollowersCount.ToString(),GitHubName,Id,ItemsCount.ToString(),
                        LinkedInId,Location,Name,Organization,PermanentId.ToString(),ProfileImageUrl,TwitterName,WebsiteUrl};
            return vs;
        }

        #region Get Folowees
        public UserJson[] GetFolowees(int page = 1,int per_page = 5)
        {
            var async = GetFoloweesAsync(page,per_page);
            async.Wait();
            return async.Result;
        }
        public async Task<UserJson[]> GetFoloweesAsync(int page = 1, int per_page = 5)
        {
            var message = await Tokens.client.GetAsync($"api/v2/users/{Id}/followees?page={page}&per_page{per_page}");
            var response = await message.Content.ReadAsStringAsync();
            var Result = JsonConvert.DeserializeObject<UserJson[]>(response);
            return Result;
        }
        #endregion

        #region GetFolowers
        public UserJson[] GetFolowers(int page = 1, int per_page = 5)
        {
            var async = GetFolowersAsync(page, per_page);
            async.Wait();
            return async.Result;
        }
        public async Task<UserJson[]> GetFolowersAsync(int page = 1, int per_page = 5)
        {
            var message = await Tokens.client.GetAsync($"api/v2/users/{Id}/followers?page={page}&per_page{per_page}");
            var response = await message.Content.ReadAsStringAsync();
            var Result = JsonConvert.DeserializeObject<UserJson[]>(response);
            return Result;
        }
        #endregion

        public bool isFollowing()
        {
            var async = isFollowingAsync();
            async.Wait();
            return async.Result;
        }
        public async Task<bool> isFollowingAsync()
        {
            var message = await Tokens.client.GetAsync($"api/v2/users/{Id}/following");
            if ((int)message.StatusCode == 204) return true;
            else return false;
        }

        public bool Following()
        {
            var async = FollowingAsync();
            async.Wait();
            return async.Result;
        }
        public async Task<bool> FollowingAsync()
        {
            var message = await Tokens.client.PutAsync($"api/v2/users/{Id}/following",new StringContent(""));
            if ((int)message.StatusCode == 204) return true;
            else return false;
        }
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
