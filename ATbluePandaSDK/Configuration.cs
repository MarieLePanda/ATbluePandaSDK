﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATPandaSDK
{
    public class Configuration
    {
        public static string BaseUrl            = "https://bsky.social/xrpc/";
        public static string Feed               = "app.bsky.feed.post";
        public static string Auth               = "com.atproto.server.createSession";
        public static string CreateRecord       = "com.atproto.repo.createRecord";
        public static string DeleteRecord       = "com.atproto.repo.deleteRecord";
        public static string ListRecords        = "com.atproto.repo.listRecords";
        public static string LikeCollection     = "app.bsky.feed.like";
        public static string FollowCollection   = "app.bsky.graph.follow";
        public static string GetPostThread      = "app.bsky.feed.getPostThread";
        public static string GetFeed            = "app.bsky.feed.getFeed";
        public static string GetAuthorFeed      = "app.bsky.feed.getAuthorFeed";
        public static string GetTimeline        = "app.bsky.feed.getTimeline";
        public static string MuteUser           = "app.bsky.graph.mute";
        public static string UnMuteUser         = "app.bsky.graph.unmute";
        public static string BlockUser          = "app.bsky.graph.block";
        public static string GetProfile         = "app.bsky.actor.getProfile";
        public static string GetBlocks          = "app.bsky.graph.getBlocks";
    }
}
