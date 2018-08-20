using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace CoreQiita
{
    /// <summary>
    /// タグクラス
    /// </summary>
    public class Tags
    {
        /// <summary>
        ///　一覧からタグデータを取得します
        /// </summary>
        /// <param name="page">ページ番号</param>
        /// <param name="per_page">ページ要素数</param>
        /// <param name="mode">ソートモード</param>
        /// <returns>タグデータ(配列)</returns>
        public TagData[] GetTags(int page = 1, int per_page = 5, SortMode mode = SortMode.Count)
        {
            return GetTagsAsync(page, per_page, mode).Result;
        }

        private readonly string[] sort = { "count", "name" };
        /// <summary>
        ///　一覧からタグデータを非同期で取得します
        /// </summary>
        /// <param name="page">ページ番号</param>
        /// <param name="per_page">ページ要素数</param>
        /// <param name="mode">ソートモード</param>
        /// <returns>タグデータ(配列)</returns>
        public async Task<TagData[]> GetTagsAsync(int page = 1, int per_page = 5, SortMode mode = SortMode.Count)
        {
            var message = await Tokens.client.GetAsync($"api/v2/tags?page={page}&per_page={per_page}&sort={sort[(int)mode]}");
            var response = await message.Content.ReadAsStringAsync();
            var jsondata = JsonConvert.DeserializeObject<TagData[]>(response);
            return jsondata;
        }

        /// <summary>
        ///　ユーザがフォローしているタグデータを取得します
        /// </summary>
        /// <param name="user_id">ユーザID</param>
        /// <param name="page">ページ番号</param>
        /// <param name="per_page">ページ要素数</param>
        /// <returns>タグデータ(配列)</returns>
        public TagData[] GetUserFollowTags(string user_id, int page = 1, int per_page = 5)
        {
            return GetUserFollowTagsAsync(user_id, page, per_page).Result;
        }

        /// <summary>
        ///　ユーザがフォローしているタグデータを取得します
        /// </summary>
        /// <param name="user_id">ユーザID</param>
        /// <param name="page">ページ番号</param>
        /// <param name="per_page">ページ要素数</param>
        /// <returns>タグデータ(配列)</returns>
        public async Task<TagData[]> GetUserFollowTagsAsync(string user_id, int page = 1, int per_page = 5)
        {
            var message = await Tokens.client.GetAsync($"api/v2/users/{user_id}/following_tags?page={page}&per_page={per_page}");
            var response = await message.Content.ReadAsStringAsync();
            var jsondata = JsonConvert.DeserializeObject<TagData[]>(response);
            return jsondata;
        }

        /// <summary>
        /// タグIDからタグデータを取得します
        /// </summary>
        /// <param name="tag_id">タグID</param>
        /// <returns>タグデータ</returns>
        public TagData GetTagData(string tag_id)
        {
            return GetTagDataAsync(tag_id).Result;
        }

        /// <summary>
        /// タグIDからタグデータを非同期取得します
        /// </summary>
        /// <param name="tag_id">タグID</param>
        /// <returns>タグデータ</returns>
        public async Task<TagData> GetTagDataAsync(string tag_id)
        {
            if (tag_id.IndexOf("#") != -1) tag_id = tag_id.Replace("#", "sharp");
            var message = await Tokens.client.GetAsync($"api/v2/tags/{tag_id}");
            var response = await message.Content.ReadAsStringAsync();
            var jsondata = JsonConvert.DeserializeObject<TagData>(response);
            return jsondata;
        }

        /// <summary>
        /// タグを非同期でフォローします
        /// </summary>
        /// <param name="tag_id">タグID</param>
        /// <returns>フォローの成否</returns>
        public bool Follow(string tag_id)
        {
            return FollowAsync(tag_id).Result;
        }

        /// <summary>
        /// タグを非同期でフォローします
        /// </summary>
        /// <param name="tag_id">タグID</param>
        /// <returns>フォローの成否</returns>
        public async Task<bool> FollowAsync(string tag_id)
        {
            if (tag_id.IndexOf("#") != -1) tag_id = tag_id.Replace("#", "sharp");
            var message = await Tokens.client.PostAsync("api/v2/tags/:tag_id/following", new StringContent(""));
            return (int)message.StatusCode == 204;
        }
    }
    /// <summary>
    /// 記事タグ
    /// </summary>
    [JsonObject]
    public class Tag
    {
        /// <summary>
        /// タグ名
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// タグを作成します
        /// </summary>
        /// <param name="name">タグ名</param>
        public Tag(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 文字列配列から複数のタグを作成します
        /// </summary>
        /// <param name="tag">タグ(可変長)</param>
        /// <returns>タグ配列</returns>
        public static Tag[] BuildTags(params string[] tag)
        {
            var tags = tag.Select(value => new Tag(value)).ToArray();
            return tags;
        }
    }

    /// <summary>
    /// タグデータクラス
    /// </summary>
    [JsonObject]
    public class TagData
    {
        /// <summary>
        /// タグをフォローしているユーザ数
        /// </summary>
        [JsonProperty("followers_count")]
        public int FollowersCount { get; internal set; }

        /// <summary>
        /// アイコン画像のURL
        /// </summary>
        [JsonProperty("icon_url")]
        public string IconUrl { get; internal set; }

        /// <summary>
        /// タグID
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; internal set; }

        /// <summary>
        /// タグが付けられた投稿の数
        /// </summary>
        [JsonProperty("items_count")]
        public string ItemsCount { get; internal set; }
    }
}
