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
    /// 投稿記事クラス
    /// </summary>
    public class Items
    {

        #region
        /// <summary>
        /// 認証中のユーザの投稿を取得します
        /// </summary>
        /// <param name="page">ページ番号 1-100</param>
        /// <param name="per_page">ページ内の要素数 1-100</param>
        /// <returns>ユーザの投稿内容(配列)</returns>
        public ItemData[] GetAuthUserItems(int page = 1,int per_page = 5)
        {
            return GetAuthUserItemsAsync(page, per_page).Result;
        }

        /// <summary>
        /// 認証中のユーザの投稿を非同期で取得します
        /// </summary>
        /// <param name="page">ページ番号 1-100</param>
        /// <param name="per_page">ページ内の要素数 1-100</param>
        /// <returns>ユーザの投稿内容(配列)</returns>
        public async Task<ItemData[]> GetAuthUserItemsAsync(int page = 1,int per_page = 5)
        {
            var url = $"api/v2/authenticated_user/items?page={page}&per_page={per_page}";
            return await GetItemsAsync(url);
        }
        #endregion

        #region GetUserItems
        /// <summary>
        /// ユーザの投稿を取得します
        /// </summary>
        /// <param name="user_id">ユーザID</param>
        /// <param name="page">ページ番号 1-100</param>
        /// <param name="per_page">ページ内の要素数 1-100</param>
        /// <returns>ユーザの投稿内容(配列)</returns>
        public ItemData[] GetUserItems(string user_id,int page = 1, int per_page = 5)
        {
            var async = GetUserItemsAsync(user_id, page, per_page);
            async.Wait();
            return async.Result;
        }
        /// <summary>
        /// ユーザの投稿を非同期で取得します
        /// </summary>
        /// <param name="user_id">ユーザID</param>
        /// <param name="page">ページ番号 1-100</param>
        /// <param name="per_page">ページ内の要素数 1-100</param>
        /// <returns>ユーザの投稿内容(配列)</returns>
        public async Task<ItemData[]> GetUserItemsAsync(string user_id,int page = 1,int per_page = 5)
        {
            var url = $"api/v2/users/{user_id}/items?page={page}&per_page={per_page}";
            return await GetItemsAsync(url);
        }
        #endregion

        #region GetALLItem
        /// <summary>
        /// 投稿の一覧を作成日時降順で取得します
        /// </summary>
        /// <param name="page">ページ番号　100</param>
        /// <param name="per_page">ページ内の要素数 100まで</param>
        /// <returns>投稿内容(配列)</returns>
        public ItemData[] GetAllItem(int page = 1,int per_page = 5)
        {
            var async = GetAllItemAsync(page, per_page);
            async.Wait();
            return async.Result;
        }

        /// <summary>
        /// 投稿の一覧を作成日時降順で非同期取得します
        /// </summary>
        /// <param name="page">ページ番号　100</param>
        /// <param name="per_page">ページ内の要素数 100まで</param>
        /// <returns>投稿内容(配列)</returns>
        public async Task<ItemData[]> GetAllItemAsync(int page = 1,int per_page = 5)
        {
            var url = $"api/v2/items?page={page}&per_page={per_page}";
            return await GetItemsAsync(url);
        }
        #endregion

        private async Task<ItemData[]> GetItemsAsync(string url)
        {
            var message = await Tokens.client.GetAsync(url);
            var response = await message.Content.ReadAsStringAsync();
            var Result = JsonConvert.DeserializeObject<ItemData[]>(response);
            return Result;
        }

        /// <summary>
        /// 記事のIDから内容を取得します
        /// </summary>
        /// <param name="item_id">記事ID</param>
        /// <returns>投稿内容</returns>
        public ItemData GetItem(string item_id)
        {
            return GetItemAsync(item_id).Result;
        }

        /// <summary>
        /// 記事のIDから内容を非同期取得します
        /// </summary>
        /// <param name="item_id">記事ID</param>
        /// <returns>投稿内容</returns>
        public async Task<ItemData> GetItemAsync(string item_id)
        {
            var message = await Tokens.client.GetAsync($"api/v2/items/{item_id}");
            var response = await message.Content.ReadAsStringAsync();
            var Result = JsonConvert.DeserializeObject<ItemData>(response);
            return Result;
        }

        /// <summary>
        /// 記事を新しく作成します
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <param name="tags">タグ</param>
        /// <param name="body">本文</param>
        /// <param name="gist">本文中のコードをGistに投稿するかどうか(GitHubとの連携が必要)</param>
        /// <param name="isprivate">限定共有記事か公開記事か</param>
        /// <param name="tweet">ツイッターに投稿するか(Twitterとの連携が必要)</param>
        /// <returns>投稿の成否</returns>
        public bool Create(string title, Tag tags, string body, bool gist = false, bool isprivate = false, bool tweet = false)
        {
            var t = new Tag[] { tags };
            return Create(title, t, body, gist, isprivate, tweet);
        }

        /// <summary>
        /// 記事を新しく作成します
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <param name="tags">タグ</param>
        /// <param name="body">本文</param>
        /// <param name="gist">本文中のコードをGistに投稿するかどうか(GitHubとの連携が必要)</param>
        /// <param name="isprivate">限定共有記事か公開記事か</param>
        /// <param name="tweet">ツイッターに投稿するか(Twitterとの連携が必要)</param>
        /// <returns>投稿の成否</returns>
        public bool Create(string title, Tag[] tags, string body, bool gist = false, bool isprivate = false, bool tweet = false)
        {
            return CreateAsync(title, tags, body, gist, isprivate, tweet).Result;
        }

        /// <summary>
        /// 記事を非同期的に新しく作成します
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <param name="tags">タグ</param>
        /// <param name="body">本文</param>
        /// <param name="gist">本文中のコードをGistに投稿するかどうか(GitHubとの連携が必要)</param>
        /// <param name="isprivate">限定共有記事か公開記事か</param>
        /// <param name="tweet">ツイッターに投稿するか(Twitterとの連携が必要)</param>
        /// <returns>投稿の成否</returns>
        public async Task<bool> CreateAsync(string title, Tag tags, string body, bool gist = false, bool isprivate = false, bool tweet = false)
        {
            var t = new Tag[] { tags };
            return await CreateAsync(title, t, body, gist, isprivate, tweet);
        }

        /// <summary>
        /// 記事を非同期的に新しく作成します
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <param name="tags">タグ</param>
        /// <param name="body">本文</param>
        /// <param name="gist">本文中のコードをGistに投稿するかどうか(GitHubとの連携が必要)</param>
        /// <param name="isprivate">限定共有記事か公開記事か</param>
        /// <param name="tweet">ツイッターに投稿するか(Twitterとの連携が必要)</param>
        /// <returns>投稿の成否</returns>
        public async Task<bool> CreateAsync(string title, Tag[] tags, string body, bool gist = false, bool isprivate = false, bool tweet = false)
        {
            var data = new PostItemData()
            {
                Body = body,
                Gist = gist,
                Private = isprivate,
                Tags = tags,
                Title = title,
                Tweet = tweet
            };

            var jsondata = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsondata, Encoding.UTF8, ContentType.Json);
            var response = await Tokens.client.PostAsync("api/v2/items", content);
            return (int)response.StatusCode == 201;
        }

        /// <summary>
        /// 投稿につけられたいいねの日時とユーザーデータを取得します
        /// </summary>
        /// <param name="item_id">記事ID</param>
        /// <returns>いいねのデータ</returns>
        public LikeData[] GetLikeUsers(string item_id)
        {
            return GetLikeUsersAsync(item_id).Result;
        }

        /// <summary>
        /// 投稿につけられたいいねの日時とユーザーデータを非同期取得します
        /// </summary>
        /// <param name="item_id">記事ID</param>
        /// <returns>いいねのデータ</returns>
        public async Task<LikeData[]> GetLikeUsersAsync(string item_id)
        {
            var message = await Tokens.client.GetAsync($"/api/v2/items/{item_id}/likes");
            var response = await message.Content.ReadAsStringAsync();
            var Result = JsonConvert.DeserializeObject<LikeData[]>(response);
            return Result;
        }

        /// <summary>
        /// 投稿を削除します
        /// </summary>
        /// <param name="item_id">記事ID</param>
        /// <returns>削除の成否</returns>
        public bool DeleteItem(string item_id)
        {
            return DeleteItemAsync(item_id).Result;
        }

        /// <summary>
        /// 投稿を非同期で削除します
        /// </summary>
        /// <param name="item_id">記事ID</param>
        /// <returns>削除の成否</returns>
        public async Task<bool> DeleteItemAsync(string item_id)
        {
            var message = await Tokens.client.DeleteAsync($"api/v2/items/{item_id}");
            return (int)message.StatusCode == 204;
        }

        /// <summary>
        /// 記事を更新します
        /// </summary>
        /// <param name="item_id">記事ID</param>
        /// <param name="title">タイトル</param>
        /// <param name="tags">タグ</param>
        /// <param name="body">本文</param>
        /// <param name="isprivate">限定共有にするか</param>
        /// <returns></returns>
        public bool PatchItem(string item_id, string title, Tag[] tags, string body, bool isprivate = false)
        {
            return PatchItemAsync(item_id, title, tags, body, isprivate).Result;
        }

        //HttpClientにPatch実装してほしいなぁ
        private readonly HttpMethod patchmethod = new HttpMethod("PATCH");
        /// <summary>
        /// 記事を非同期で更新します
        /// </summary>
        /// <param name="item_id">記事ID</param>
        /// <param name="title">タイトル</param>
        /// <param name="tags">タグ</param>
        /// <param name="body">本文</param>
        /// <param name="isprivate">限定共有にするか</param>
        /// <returns></returns>
        public async Task<bool> PatchItemAsync(string item_id, string title, Tag[] tags, string body, bool isprivate = false)
        {
            var jsondata = new PatchItemData()
            {
                Body = body,
                Private = isprivate,
                Tags = tags,
                Title = title
            };

            var json = JsonConvert.SerializeObject(jsondata);
            var content = new StringContent(json, Encoding.UTF8, ContentType.Json);
            var request = new HttpRequestMessage(patchmethod, $"https://qiita.com/api/v2/items/{item_id}") { Content = content };

            var message = await Tokens.client.SendAsync(request);
            return (int)message.StatusCode == 200;
        }

        /// <summary>
        /// いいねを取り消します
        /// </summary>
        /// <param name="item_id">記事ID</param>
        /// <returns>取り消しの成否</returns>
        public bool DeleteLike(string item_id)
        {
            return DeleteLikeAsync(item_id).Result;
        }

        /// <summary>
        /// 非同期でいいねを取り消します
        /// </summary>
        /// <param name="item_id">記事ID</param>
        /// <returns>取り消しの成否</returns>
        public async Task<bool> DeleteLikeAsync(string item_id)
        {
            var message = await Tokens.client.DeleteAsync($"api/v2/items/{item_id}/like");
            return (int)message.StatusCode == 204;
        }

        /// <summary>
        /// いいねを付けます
        /// </summary>
        /// <param name="item_id">記事ID</param>
        /// <returns>いいねの成否</returns>
        public bool Like(string item_id)
        {
            return LikeAsync(item_id).Result;
        }

        /// <summary>
        /// 非同期でいいねを付けます
        /// </summary>
        /// <param name="item_id">記事ID</param>
        /// <returns>いいねの成否</returns>
        public async Task<bool> LikeAsync(string item_id)
        {
            var message = await Tokens.client.PutAsync($"api/v2/items/{item_id}/like", new StringContent(""));
            return (int)message.StatusCode == 204;
        }

        /// <summary>
        /// いいねを付けているか調べます
        /// </summary>
        /// <param name="item_id">記事ID</param>
        /// <returns>いいねの成否</returns>
        public bool IsLike(string item_id)
        {
            return IsLikeAsync(item_id).Result;
        }

        /// <summary>
        /// いいねを付けているか非同期で調べます
        /// </summary>
        /// <param name="item_id">記事ID</param>
        /// <returns>いいねの成否</returns>
        public async Task<bool> IsLikeAsync(string item_id)
        {
            var message = await Tokens.client.GetAsync($"api/v2/items/{item_id}/like");
            return (int)message.StatusCode == 204;
        }
    }

    [JsonObject]
    internal class PostItemData
    {
        [JsonProperty("body")]
        internal string Body { get; set; }

        [JsonProperty("gist")]
        internal bool Gist { get; set; }

        [JsonProperty("private")]
        internal bool Private { get; set; }

        [JsonProperty("tags")]
        internal Tag[] Tags { get; set; }

        [JsonProperty("title")]
        internal string Title { get; set; }

        [JsonProperty("tweet")]
        internal bool Tweet { get; set; }
    }

    [JsonObject]
    internal class PatchItemData
    {
        [JsonProperty("body")]
        internal string Body { get; set; }

        [JsonProperty("private")]
        internal bool Private { get; set; }

        [JsonProperty("tags")]
        internal Tag[] Tags { get; set; }

        [JsonProperty("title")]
        internal string Title { get; set; }
    }

    /// <summary>
    /// 投稿記事データ
    /// </summary>
    [JsonObject]
    public class ItemData
    {
        #region ItemData Properties
        /// <summary>
        /// HTML形式の本文
        /// </summary>
        [JsonProperty("rendered_body")]
        public string RenderedBody { get;internal set; }

        /// <summary>
        /// Markdown形式の本文
        /// </summary>
        [JsonProperty("body")]
        public string Body { get; internal set; }

        /// <summary>
        /// コメント数
        /// </summary>
        [JsonProperty("comments_count")]
        public int CommentsCount { get; internal set; }

        [JsonProperty("created_at")]
        internal string _Date { get; set; }

        private DateTime _date;

        /// <summary>
        /// 作成日時(DateTime型)
        /// </summary>
        public DateTime Date
        {
            get
            {
                if (_date != DateTime.MinValue) return _date;
                _date = DateTime.Parse(_Date);
                return _date;
            }
        }

        /// <summary>
        /// Qiita:Teamグループ
        /// </summary>
        [JsonProperty("group")]
        public Group Group { get; internal set; }

        /// <summary>
        /// 記事ID
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; internal set; }

        /// <summary>
        /// いいねの数
        /// </summary>
        [JsonProperty("likes_count")]
        public int LikesCount { get; internal set; }

        /// <summary>
        /// 限定共有状態か
        /// </summary>
        [JsonProperty("private")]
        public bool isPrivate { get; internal set; }

        /// <summary>
        /// 記事につけられたタグ
        /// </summary>
        [JsonProperty("tags")]
        public Tag[] Tags { get; internal set; }

        /// <summary>
        /// 記事タイトル
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; internal set; }


        [JsonProperty("updated_at")]
        internal string _UpdateDate { get; set; }


        private DateTime _updatedate;
        /// <summary>
        /// 最終更新日時
        /// </summary>
        public DateTime UpdateDate
        {
            get
            {
                if (_updatedate != DateTime.MinValue) return _updatedate;
                _updatedate = DateTime.Parse(_UpdateDate);
                return _updatedate;
            }
        }

        /// <summary>
        /// 記事のURL
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; internal set; }

        /// <summary>
        /// 投稿ユーザ
        /// </summary>
        [JsonProperty("user")]
        public UserJson User { get; internal set; }

        /// <summary>
        /// 閲覧数
        /// </summary>
        [JsonProperty("page_views_count")]
        public int? Views { get; internal set; }
        #endregion

        /// <summary>
        /// 記事をストックします
        /// </summary>
        /// <returns>ストックの成否</returns>
        public bool Stock()
        {
            return StockAsync().Result;
        }

        /// <summary>
        /// 記事を非同期でストックします
        /// </summary>
        /// <returns>ストックの成否</returns>
        public async Task<bool> StockAsync()
        {
            var message = await Tokens.client.PutAsync($"api/v2/items/{Id}/stock", new StringContent(""));
            return (int)message.StatusCode == 204;
        }

        /// <summary>
        /// 記事をストックから削除します
        /// </summary>
        /// <returns>削除の成否</returns>
        public bool DeleteStock()
        {
            return DeleteStockAsync().Result;
        }
        /// <summary>
        /// 記事をストックから非同期で削除します
        /// </summary>
        /// <returns>削除の成否</returns>
        public async Task<bool> DeleteStockAsync()
        {
            var message = await Tokens.client.DeleteAsync($"api/v2/items/{Id}/stock");
            return (int)message.StatusCode == 204;
        }

        /// <summary>
        /// 記事がストックされているか確認します
        /// </summary>
        /// <returns>ストックの有無</returns>
        public bool isStock()
        {
            return isStockAsync().Result;
        }
        /// <summary>
        /// 記事がストックされているか非同期で確認します
        /// </summary>
        /// <returns>ストックの有無</returns>
        public async Task<bool> isStockAsync()
        {
            var message = await Tokens.client.GetAsync($"api/v2/items/{Id}/stock");
            return (int)message.StatusCode == 204;
        }
    }

    /// <summary>
    /// いいねのデータ
    /// </summary>
    [JsonObject]
    public class LikeData
    {
        [JsonProperty("created_at")]
        internal string _Date { get; set; }

        private DateTime _date;
        /// <summary>
        /// いいねが付けられた日時
        /// </summary>
        public DateTime Date
        {
            get
            {
                if (_date != DateTime.MinValue) return _date;
                _date = DateTime.Parse(_Date);
                return _date;
            }
        }
        /// <summary>
        /// いいねをつけたユーザ
        /// </summary>
        [JsonProperty("user")]
        public UserJson User { get; internal set; }
    }

    /// <summary>
    /// Qiita:Teamのグループデータ
    /// </summary>
    [JsonObject]
    public class Group
    {
        [JsonProperty("created_at")]
        internal string _Date { get; set; }


        private DateTime _date;
        /// <summary>
        /// 作成日時
        /// </summary>
        public DateTime Date
        {
            get
            {
                if (_date != DateTime.MinValue) return _date;
                _date = DateTime.Parse(_Date);
                return _date;
            }
        }

        /// <summary>
        /// グループID
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; internal set; }

        /// <summary>
        /// グループの名前
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; internal set; }

        /// <summary>
        /// 非公開グループかどうか
        /// </summary>
        [JsonProperty("private")]
        public bool isPrivate { get; internal set; }

        [JsonProperty("updated_at")]
        internal string _UpdateDate { get; set; }


        private DateTime _updatedate;
        /// <summary>
        /// 最終更新日時
        /// </summary>
        public DateTime UpdateDate
        {
            get
            {
                if (_updatedate != DateTime.MinValue) return _updatedate;
                _updatedate = DateTime.Parse(_UpdateDate);
                return _updatedate;
            }
        }

        /// <summary>
        /// グループのチーム上での一意な名前
        /// </summary>
        [JsonProperty("url_name")]
        public string UrlName { get; internal set; }
    }
}
