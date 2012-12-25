using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Twitterizer;

namespace TwitterSearchApp.TwitterClient
{
    /// <summary>
    /// Represents an exception caused by a call to Twitter's search API
    /// </summary>
    public class TwitterSearchException : Exception
    {
        public TwitterSearchException(RequestResult searchResult, string query)
            : base(string.Format(
                "Twitter search error. TwitterSearchResult: {0}; query: {1}",
                Enum.GetName(typeof(RequestResult), searchResult),
                query))
        {
        }
    }
}