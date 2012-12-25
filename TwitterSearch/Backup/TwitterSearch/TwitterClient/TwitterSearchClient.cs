using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using Twitterizer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TwitterSearchApp.TwitterClient
{
    public static class TwitterSearchClient
    {
        private const string TWITTER_SEARCH_URI = "http://search.twitter.com/search.json";
        private const int TWITTER_PAGE_SIZE = 100;
        private const string MAX_ID = "MAX_ID";
        private const string CACHED_PAGE_RESULTS = "CURRENT_PAGE_RESULTS";
        private const string CACHED_PAGE_NUMBER = "CACHED_PAGE_NUMBER";

        /// <summary>
        /// Returns search results in a structure ready for consumption by the client.
        /// </summary>
        /// <param name="query">Search text to query for</param>
        /// <param name="clientPage">Page number requested for display by the client UI</param>
        /// <param name="clientPageSize">Page size requested for display by the client UI</param>
        /// <param name="newSearch">Whether or not recent results should be pulled from Twitter (i.e. do not pass a max_id)</param>
        /// <returns>Search results required for the requested client page.  Results returned will not exceed clientPageSize</returns>
        public static IEnumerable<TwitterSearchResult> Search(string query, int clientPage, int clientPageSize, bool newSearch)
        {
            // Validate client input
            InputValidator.ValidateClientPage(clientPage);
            InputValidator.ValidateClientPageSize(clientPageSize);

            // Perform conversions to map client pages to Twitter pages
            int twitterPageNumber = PaginationConverter.DetermineTwitterPageNumber(clientPage, clientPageSize, TWITTER_PAGE_SIZE);
            int startIndex = PaginationConverter.DetermineStartIndex(clientPage, clientPageSize, twitterPageNumber, TWITTER_PAGE_SIZE);

            // Get the results required for the client
            return GetTwitterResults(query, twitterPageNumber, newSearch)
                                                    .Skip(startIndex)
                                                    .Take(clientPageSize);
        }

        /// <summary>
        /// Returns search results.  Returns results from cache if available, otherwise fetches results from Twitter API.
        /// </summary>
        private static TwitterSearchResultCollection GetTwitterResults(string query, int twitterPageNumber, bool newSearch)
        {
            // Check cache for data
            if (!newSearch &&
                twitterPageNumber == (int)HttpContext.Current.Session[CACHED_PAGE_NUMBER] &&
                HttpContext.Current.Session[CACHED_PAGE_RESULTS] != null)
            {
                return (TwitterSearchResultCollection)HttpContext.Current.Session[CACHED_PAGE_RESULTS];
            }

            // Build search parameters
            SearchOptions searchOptions = new SearchOptions();
            searchOptions.NumberPerPage = TWITTER_PAGE_SIZE;
            searchOptions.PageNumber = twitterPageNumber;

            if (!newSearch && HttpContext.Current.Session[MAX_ID] != null)
            {
                searchOptions.MaxId = (long)HttpContext.Current.Session[MAX_ID];
            }

            // Execute search
            TwitterResponse<TwitterSearchResultCollection> twitterResponse = Twitterizer.TwitterSearch.Search(query, searchOptions);

            // Throw errors, connection failures back to the UI for proper handling
            if (twitterResponse.Result != RequestResult.Success)
            {
                throw new TwitterSearchException(twitterResponse.Result, query);
            }

            // Twitterizer doesn't pass the max_id back, so that needs to be parsed separately
            JToken token = JObject.Parse(twitterResponse.Content);
            HttpContext.Current.Session[MAX_ID] = (long)token.SelectToken("max_id");

            // Cache results for future requests
            HttpContext.Current.Session[CACHED_PAGE_RESULTS] = twitterResponse.ResponseObject;
            HttpContext.Current.Session[CACHED_PAGE_NUMBER] = searchOptions.PageNumber;

            return twitterResponse.ResponseObject;
        }
    }
}