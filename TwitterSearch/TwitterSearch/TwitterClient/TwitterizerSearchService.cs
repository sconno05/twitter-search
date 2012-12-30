using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Twitterizer;

namespace TwitterSearchApp.TwitterClient
{
    internal class TwitterizerSearchService : ITwitterizerSearchService
    {
        public TwitterResponse<TwitterSearchResultCollection> Search(string query, SearchOptions searchOptions)
        {
            return TwitterSearch.Search(query, searchOptions);
        }
    }
}