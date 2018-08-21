using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace CoreQiita
{
    /// <summary>
    /// ユーザクラス
    /// </summary>
    public class Users
    {
        private readonly StringContent none = new StringContent("");
        private string USER_ID { get; set; }
        private AuthUserJson AuthUser { get; set; }

        #region GetUser
        /// <summary>
        /// ユーザーを取得します
        /// </summary>
        /// <param name="user_id">ユーザID/</param>
        /// <returns>ユーザ情報</returns>
        public UserJson GetUser(string user_id)
        {
            return GetUserAsync(user_id).Result;
        }

        /// <summary>
        /// ユーザを非同期取得します
        /// </summary>
        /// <param name="user_id">ユーザID</param>
        /// <returns>ユーザ情報</returns>
        public async Task<UserJson> GetUserAsync(string user_id)
        {
            var message = await Tokens.client.GetAsync($"api/v2/users/{user_id}");
            var response = await message.Content.ReadAsStringAsync();
            var Result = JsonConvert.DeserializeObject<UserJson>(response);
            return Result;
        }
        #endregion


        #region UserJson[]

        #region GetAllUsers
        /// <summary>
        /// 全ユーザを作成日時降順で取得します
        /// </summary>
        /// <param name="page">ページ番号 1-100</param>
        /// <param name="per_page">ページ要素数 1-100</param>
        /// <returns>ユーザ情報(配列)</returns>
        public UserJson[] GetAllUsers(int page = 1, int per_page = 5)
        {
            return GetAllUsersAsync(page, per_page).Result;
        }

        /// <summary>
        /// 全ユーザを作成日時降順で非同期取得します
        /// </summary>
        /// <param name="page">ページ番号 1-100</param>
        /// <param name="per_page">ページ要素数 1-100</param>
        /// <returns>ユーザ情報(配列)</returns>
        public async Task<UserJson[]> GetAllUsersAsync(int page = 1,int per_page = 5)
        {
            var url = $"api/v2/users?page={page}&per_page{per_page}";
            return await GetUsersAsync(url);
        }
        #endregion

        #region GetStockers
        /// <summary>
        /// 記事をストックしているユーザーを取得します
        /// </summary>
        /// <param name="item_id">記事ID</param>
        /// <param name="page">ページ番号 1-100</param>
        /// <param name="per_page">ページ要素数 1-100</param>
        /// <returns>ユーザ情報(配列)</returns>
        public UserJson[] GetStockers(string item_id,int page = 1,int per_page = 5)
        {
            return GetStockerAsync(item_id, page, per_page).Result;
        }

        /// <summary>
        /// 記事をストックしているユーザーを非同期取得します
        /// </summary>
        /// <param name="item_id">記事ID</param>
        /// <param name="page">ページ番号 1-100</param>
        /// <param name="per_page">ページ要素数 1-100</param>
        /// <returns>ユーザ情報(配列)</returns>
        public async Task<UserJson[]> GetStockerAsync(string item_id, int page = 1, int per_page = 5)
        {
            var url = $"api/v2/items/{item_id}/stockers?page={page}&per_page={per_page}";
            return await GetUsersAsync(url);
        }
        #endregion

        private async Task<UserJson[]> GetUsersAsync(string url)
        {
            var message = await Tokens.client.GetAsync(url);
            var response = await message.Content.ReadAsStringAsync();
            var Result = JsonConvert.DeserializeObject<UserJson[]>(response);
            return Result;
        }
        #endregion


        #region GetAuthUser
        /// <summary>
        /// 認証中のユーザを取得します
        /// </summary>
        /// <returns>ユーザ情報</returns>
        public AuthUserJson GetAuthUser()
        {
            if (AuthUser != null) return AuthUser;
            return GetAuthUserAsync().Result;
        }

        /// <summary>
        /// 認証中のユーザを非同期取得します
        /// </summary>
        /// <returns>ユーザ情報</returns>
        public async Task<AuthUserJson> GetAuthUserAsync()
        {
            if (AuthUser != null) return AuthUser;
            var message = await Tokens.client.GetAsync($"api/v2/authenticated_user");
            var response = await message.Content.ReadAsStringAsync();
            var Result = JsonConvert.DeserializeObject<AuthUserJson>(response);
            return Result;
        }

        #endregion

        #region Get Folowees
        /// <summary>
        /// ユーザをフォローしているユーザを取得します
        /// </summary>
        /// <param name="user_id">ユーザID</param>
        /// <param name="page">ページ番号 1-100</param>
        /// <param name="per_page">ページ要素数 1-100</param>
        /// <returns>ユーザ情報(配列)</returns>
        public UserJson[] GetFolowees(string user_id, int page = 1, int per_page = 5)
        {
            return GetFoloweesAsync(user_id, page, per_page).Result;
        }
        /// <summary>
        /// ユーザをフォローしているユーザを非同期取得します
        /// </summary>
        /// <param name="user_id">ユーザID</param>
        /// <param name="page">ページ番号 1-100</param>
        /// <param name="per_page">ページ要素数 1-100</param>
        /// <returns>ユーザ情報(配列)</returns>
        public async Task<UserJson[]> GetFoloweesAsync(string user_id, int page = 1, int per_page = 5)
        {
            var message = await Tokens.client.GetAsync($"api/v2/users/{user_id}/followees?page={page}&per_page{per_page}");
            var response = await message.Content.ReadAsStringAsync();
            var Result = JsonConvert.DeserializeObject<UserJson[]>(response);
            return Result;
        }
        #endregion

        #region GetFolowers
        /// <summary>
        /// ユーザがフォローしているユーザを取得します
        /// </summary>
        /// <param name="user_id">ユーザID</param>
        /// <param name="page">ページ番号 1-100</param>
        /// <param name="per_page">ページ要素数 1-100</param>
        /// <returns>ユーザ情報(配列)</returns>
        public UserJson[] GetFolowers(string user_id, int page = 1, int per_page = 5)
        {
            var async = GetFolowersAsync(user_id, page, per_page);
            async.Wait();
            return async.Result;
        }

        /// <summary>
        /// ユーザがフォローしているユーザを非同期取得します
        /// </summary>
        /// <param name="user_id">ユーザID</param>
        /// <param name="page">ページ番号 1-100</param>
        /// <param name="per_page">ページ要素数 1-100</param>
        /// <returns>ユーザ情報(配列)</returns>
        public async Task<UserJson[]> GetFolowersAsync(string user_id, int page = 1, int per_page = 5)
        {
            var message = await Tokens.client.GetAsync($"api/v2/users/{user_id}/followers?page={page}&per_page{per_page}");
            var response = await message.Content.ReadAsStringAsync();
            var Result = JsonConvert.DeserializeObject<UserJson[]>(response);
            return Result;
        }
        #endregion

        /// <summary>
        /// フォロー状態か取得します
        /// </summary>
        /// <returns>True:フォロー中 False:未フォロー</returns>
        public bool IsFollowing(string user_id)
        {
            return IsFollowingAsync(user_id).Result;
        }

        /// <summary>
        /// フォロー状態か非同期取得します
        /// </summary>
        /// <returns>True:フォロー中 False:未フォロー</returns>
        public async Task<bool> IsFollowingAsync(string user_id)
        {
            var message = await Tokens.client.GetAsync($"api/v2/users/{user_id}/following");
            if ((int)message.StatusCode == 204) return true;
            else return false;
        }

        /// <summary>
        /// このユーザをフォローします
        /// </summary>
        /// <returns>フォローの成否</returns>
        public bool Follow(string user_id)
        {
            return FollowAsync(user_id).Result;
        }
        /// <summary>
        /// このユーザをフォローします
        /// </summary>
        /// <returns>フォローの成否</returns>
        public async Task<bool> FollowAsync(string user_id)
        {
            var message = await Tokens.client.PutAsync($"api/v2/users/{user_id}/following", none);
            return (int)message.StatusCode == 204;
        }

        /// <summary>
        /// フォローを解除します
        /// </summary>
        /// <param name="user_id">ユーザID</param>
        /// <returns>フォロー解除の成否</returns>
        public bool UnFollow(string user_id)
        {
            return UnFollowAsync(user_id).Result;
        }

        /// <summary>
        /// フォローを非同期で解除します
        /// </summary>
        /// <param name="user_id">ユーザID</param>
        /// <returns>フォロー解除の成否</returns>
        public async Task<bool> UnFollowAsync(string user_id)
        {
            var message = await Tokens.client.DeleteAsync($"api/v2/users/{user_id}/following");
            return (int)message.StatusCode == 204;
        }
    }
    
    /// <summary>
    /// ユーザデータ
    /// </summary>
    [JsonObject]
    public class UserJson
    {
        #region JsonProperties
        /// <summary>
        /// 自己紹介
        /// </summary>
        [JsonProperty("description")]
        public string Description { get;internal set; }

        /// <summary>
        /// facebookID
        /// </summary>
        [JsonProperty("facebook_id")]
        public string FacebookId { get; internal set; }

        /// <summary>
        /// フォロー数
        /// </summary>
        [JsonProperty("followees_count")]
        public int FolloweesCount { get; internal set; }

        /// <summary>
        /// フォロワー数
        /// </summary>
        [JsonProperty("followers_count")]
        public int FollowersCount { get; internal set; }

        /// <summary>
        /// GitHub上の名前
        /// </summary>
        [JsonProperty("github_login_name")]
        public string GitHubName { get; internal set; }

        /// <summary>
        /// ID
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; internal set; }

        /// <summary>
        /// 投稿数
        /// </summary>
        [JsonProperty("items_count")]
        public int ItemsCount { get; internal set; }

        /// <summary>
        /// LinkedInID
        /// </summary>
        [JsonProperty("linkedin_id")]
        public string LinkedInId { get; internal set; }

        /// <summary>
        /// 居住地
        /// </summary>
        [JsonProperty("location")]
        public string Location { get; internal set; }

        /// <summary>
        /// 名前
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; internal set; }

        /// <summary>
        /// 所属組織
        /// </summary>
        [JsonProperty("organization")]
        public string Organization { get; internal set; }

        /// <summary>
        /// 整数ID
        /// </summary>
        [JsonProperty("permanent_id")]
        public int PermanentId { get; internal set; }

        /// <summary>
        /// プロファイル画像のURL
        /// </summary>
        [JsonProperty("profile_image_url")]
        public string ProfileImageUrl { get; internal set; }

        /// <summary>
        /// ツイッター上の名前
        /// </summary>
        [JsonProperty("twitter_screen_name")]
        public string TwitterName { get; internal set; }

        /// <summary>
        /// WEBサイトのURL
        /// </summary>
        [JsonProperty("website_url")]
        public string WebsiteUrl { get; internal set; }
        #endregion

        /// <summary>
        /// すべての情報を文字列配列で取得します
        /// </summary>
        /// <returns>全データ</returns>
        public string[] GetAll()
        {
            string[] vs = {Description,FacebookId,FolloweesCount.ToString(),FollowersCount.ToString(),GitHubName,Id,ItemsCount.ToString(),
                        LinkedInId,Location,Name,Organization,PermanentId.ToString(),ProfileImageUrl,TwitterName,WebsiteUrl};
            return vs;
        }


    }

    /// <summary>
    /// 認証中のユーザデータ (UserJsonをオーバーロード)
    /// </summary>
    [JsonObject]
    public class AuthUserJson : UserJson
    {
        /// <summary>
        /// 月あたりにアップロードできる画像の総容量
        /// </summary>
        [JsonProperty("image_monthly_upload_limit")]
        public int ImageUploadLimit { get; internal set; }

        /// <summary>
        /// 月あたりにアップロードできる画像の残容量
        /// </summary>
        [JsonProperty("image_monthly_upload_remaining")]
        public int ImageUploadRemaining { get; internal set; }

        /// <summary>
        /// Qiita:Team専用モードか
        /// </summary>
        [JsonProperty("team_only")]
        public bool TeamOnly { get; internal set; }

        /// <summary>
        /// すべての情報を文字列配列で取得
        /// </summary>
        /// <returns>ユーザデータ</returns>
        public new string[] GetAll()
        {
            string[] vs = {Description,FacebookId,FolloweesCount.ToString(),FollowersCount.ToString(),GitHubName,Id,ItemsCount.ToString(),
                        LinkedInId,Location,Name,Organization,PermanentId.ToString(),ProfileImageUrl,TwitterName,WebsiteUrl,ImageUploadLimit.ToString(),
                        ImageUploadRemaining.ToString(),TeamOnly.ToString()};
            return vs;
        }
    }
}
