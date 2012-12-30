using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twitterizer;

namespace TwitterSearchApp.TwitterClient
{
    public interface ITwitterizerSearchService
    {
        TwitterResponse<TwitterSearchResultCollection> Search(string query, SearchOptions searchOptions);
    }
}
