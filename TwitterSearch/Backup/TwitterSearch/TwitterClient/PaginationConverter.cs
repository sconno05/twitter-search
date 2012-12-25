using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterSearchApp.TwitterClient
{
    /// <summary>
    /// Contains calculations to translate between Twitter and client requested pages.
    /// </summary>
    internal static class PaginationConverter
    {
        /// <summary>
        /// Determines the required Twitter page number for the requested client page number.
        /// </summary>
        /// <param name="clientPage">Page number requested for display by the client UI</param>
        /// <param name="clientPageSize">Page size requested for display by the client UI</param>
        /// <param name="twitterPageSize">Page size for requests to Twitter's search API</param>
        /// <returns>The required Twitter page number.</returns>
        internal static int DetermineTwitterPageNumber(int clientPage, int clientPageSize, int twitterPageSize)
        {
            return (int)Math.Ceiling(((double)clientPageSize / (double)twitterPageSize) * clientPage);
        }

        /// <summary>
        /// Determines the required start index within the Twitter page results to get the requested client page results.
        /// </summary>
        /// <param name="clientPage">Page number requested for display by the client UI</param>
        /// <param name="clientPageSize">Page size requested for display by the client UI</param>
        /// <param name="twitterPage">Twitter results page number</param>
        /// <param name="twitterPageSize">Page size for requests to Twitter's search API</param>
        /// <returns></returns>
        internal static int DetermineStartIndex(int clientPage, int clientPageSize, int twitterPage, int twitterPageSize)
        {
            return ((clientPage - 1) * clientPageSize) - ((twitterPage - 1) * twitterPageSize);
        }
    }
}