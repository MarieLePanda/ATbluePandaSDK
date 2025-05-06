using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using ATbluePandaSDK.Models;
namespace ATPandaSDK.Models.Feed
{
    // BskyTimeline myDeserializedClass = JsonConvert.Deserialize<BskyTimeline>(myJsonResponse);
    public class AspectRatio
    {
        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }
    }

    public class Label
    {
        public string src { get; set; }
        public string uri { get; set; }
        public string cid { get; set; }
        public string val { get; set; }
        public DateTime cts { get; set; }
    }

    public class Associated
    {
        [JsonPropertyName("list")]
        public int Lists { get; set; }

        [JsonPropertyName("feedgen")]
        public int Feedgens { get; set; }

        [JsonPropertyName("starterPacks")]
        public int StarterPacks { get; set; }

        [JsonPropertyName("labeler")]
        public bool Labeler { get; set; }

        [JsonPropertyName("chat")]
        public Chat Chat { get; set; }
    }

    public class Author : User
    {
        [JsonPropertyName("labels")]
        public List<Label> Labels { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("associated")]
        public Associated Associated { get; set; }
    }

    public class Chat
    {
        [JsonPropertyName("allowIncoming")]
        public string AllowIncoming { get; set; }
    }

    public class Embed
    {
        [JsonPropertyName("$type")]
        public string Type { get; set; }

        [JsonPropertyName("record")]
        public Record Record { get; set; }

        [JsonPropertyName("images")]
        public List<Image> Images { get; set; }

        [JsonPropertyName("external")]
        public External External { get; set; }
    }

    public class Embed4
    {
        [JsonPropertyName("$type")]
        public string Type { get; set; }

        [JsonPropertyName("images")]
        public List<Image> Images { get; set; }
    }

    public class External
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }

    public class Feed
    {
        [JsonPropertyName("post")]
        public Post Post { get; set; }

        [JsonPropertyName("feedContext")]
        public string FeedContext { get; set; }

        [JsonPropertyName("reply")]
        public Reply Reply { get; set; }
    }

    public class GrandparentAuthor : User
    {
        [JsonPropertyName("labels")]
        public List<Label> Labels { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
    }

    public class Image
    {
        [JsonPropertyName("alt")]
        public string Alt { get; set; }

        [JsonPropertyName("aspectRatio")]
        public AspectRatio AspectRatio { get; set; }

//        [JsonPropertyName("image")]
//        public Image Image { get; set; }

        [JsonPropertyName("thumb")]
        public string Thumb { get; set; }

        [JsonPropertyName("fullsize")]
        public string Fullsize { get; set; }
    }

    public class Image2
    {
        [JsonPropertyName("$type")]
        public string Type { get; set; }

        [JsonPropertyName("ref")]
        public Ref Ref { get; set; }

        [JsonPropertyName("mimeType")]
        public string MimeType { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }
    }

    public class Parent
    {
        [JsonPropertyName("cid")]
        public string Cid { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonPropertyName("author")]
        public Author Author { get; set; }

        [JsonPropertyName("record")]
        public Record Record { get; set; }

        [JsonPropertyName("replyCount")]
        public int ReplyCount { get; set; }

        [JsonPropertyName("repostCount")]
        public int RepostCount { get; set; }

        [JsonPropertyName("likeCount")]
        public int LikeCount { get; set; }

        [JsonPropertyName("quoteCount")]
        public int QuoteCount { get; set; }

        [JsonPropertyName("indexedAt")]
        public DateTime IndexedAt { get; set; }

        [JsonPropertyName("viewer")]
        public Viewer Viewer { get; set; }

        [JsonPropertyName("labels")]
        public List<Label> Labels { get; set; }

        [JsonPropertyName("$type")]
        public string Type { get; set; }

        [JsonPropertyName("embed")]
        public Embed Embed { get; set; }
    }

    public class Post
    {
        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonPropertyName("cid")]
        public string Cid { get; set; }

        [JsonPropertyName("author")]
        public Author Author { get; set; }

        [JsonPropertyName("record")]
        public Record Record { get; set; }

        [JsonPropertyName("replyCount")]
        public int ReplyCount { get; set; }

        [JsonPropertyName("repostCount")]
        public int RepostCount { get; set; }

        [JsonPropertyName("likeCount")]
        public int LikeCount { get; set; }

        [JsonPropertyName("quoteCount")]
        public int QuoteCount { get; set; }

        [JsonPropertyName("indexedAt")]
        public DateTime IndexedAt { get; set; }

        [JsonPropertyName("viewer")]
        public Viewer Viewer { get; set; }

        [JsonPropertyName("labels")]
        public List<Label> Labels { get; set; }

        [JsonPropertyName("embed")]
        public Embed Embed { get; set; }
    }

    public class Record
    {
        [JsonPropertyName("$type")]
        public string Type { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("langs")]
        public List<string> Langs { get; set; }

        [JsonPropertyName("reply")]
        public Reply Reply { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("embed")]
        public Embed Embed { get; set; }

        [JsonPropertyName("cid")]
        public string Cid { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonPropertyName("author")]
        public Author Author { get; set; }

        [JsonPropertyName("value")]
        public Value Value { get; set; }

        [JsonPropertyName("labels")]
        public List<Label> Labels { get; set; }

        [JsonPropertyName("likeCount")]
        public int LikeCount { get; set; }

        [JsonPropertyName("replyCount")]
        public int ReplyCount { get; set; }

        [JsonPropertyName("repostCount")]
        public int RepostCount { get; set; }

        [JsonPropertyName("quoteCount")]
        public int QuoteCount { get; set; }

        [JsonPropertyName("indexedAt")]
        public DateTime IndexedAt { get; set; }

        [JsonPropertyName("embeds")]
        public List<Embed> Embeds { get; set; }
    }

    public class Ref
    {
        [JsonPropertyName("$link")]
        public string Link { get; set; }
    }

    public class Reply
    {
        [JsonPropertyName("parent")]
        public Parent Parent { get; set; }

        [JsonPropertyName("root")]
        public BskyTimeline Root { get; set; }

        [JsonPropertyName("grandparentAuthor")]
        public GrandparentAuthor GrandparentAuthor { get; set; }

        public Post post { get; set; }
        public List<Reply> replies { get; set; }
        public ThreadContext threadContext { get; set; }
    }

    public class ThreadPost
    {
        [JsonPropertyName("$type")]
        public string type { get; set; }
        public Post post { get; set; }
        public List<Reply> replies { get; set; }
        public ThreadContext threadContext { get; set; }
    }

    public class ThreadContext
    {
        public string rootAuthorLike { get; set; }
    }
    public class BskyThread : Response
    {
        public ThreadPost thread { get; set; }
    }

    public class BskyTimeline : Response
    {
        [JsonPropertyName("feed")]
        public List<Feed> Feed { get; set; }

        [JsonPropertyName("cursor")]
        public string Cursor { get; set; }
    }

    public class Root2
    {
        [JsonPropertyName("cid")]
        public string Cid { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonPropertyName("author")]
        public Author Author { get; set; }

        [JsonPropertyName("record")]
        public Record Record { get; set; }

        [JsonPropertyName("replyCount")]
        public int ReplyCount { get; set; }

        [JsonPropertyName("repostCount")]
        public int RepostCount { get; set; }

        [JsonPropertyName("likeCount")]
        public int LikeCount { get; set; }

        [JsonPropertyName("quoteCount")]
        public int QuoteCount { get; set; }

        [JsonPropertyName("indexedAt")]
        public DateTime IndexedAt { get; set; }

        [JsonPropertyName("viewer")]
        public Viewer Viewer { get; set; }

        [JsonPropertyName("labels")]
        public List<Label> Labels { get; set; }

        [JsonPropertyName("$type")]
        public string Type { get; set; }

        [JsonPropertyName("embed")]
        public Embed Embed { get; set; }
    }

    public class Value
    {
        [JsonPropertyName("$type")]
        public string Type { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonPropertyName("embed")]
        public Embed Embed { get; set; }

        [JsonPropertyName("langs")]
        public List<string> Langs { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public class Viewer
    {
        [JsonPropertyName("like")]
        public string Like { get; set; }

        [JsonPropertyName("repost")]
        public string Repost { get; set; }

        [JsonPropertyName("muted")]
        public bool Muted { get; set; }

        [JsonPropertyName("blocking")]
        public string Blocking { get; set; }

        [JsonPropertyName("blockedBy")]
        public bool BlockedBy { get; set; }

        [JsonPropertyName("following")]
        public string Following { get; set; }

        [JsonPropertyName("followedBy")]
        public string FollowedBy { get; set; }

        [JsonPropertyName("threadMuted")]
        public bool ThreadMuted { get; set; }

        [JsonPropertyName("embeddingDisabled")]
        public bool EmbeddingDisabled { get; set; }

        [JsonPropertyName("knownFollowers")]
        public KnownFollowers KnownFollowers { get; set; }

    }



}
