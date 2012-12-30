using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwitterSearchApp.Models;
using System.Net;
using System.IO;
using TwitterSearchApp.TwitterClient;

namespace TwitterSearchApp.Controllers
{
    /// <summary>
    /// Controller responsible for handling Twitter search queries.
    /// </summary>
    public class TwitterSearchController : Controller
    {
        /// <summary>
        /// Action for serving the ASP.NET version of this application.
        /// </summary>
        /// <param name="searchText">The query's search text (e.g. hash tag)</param>
        /// <param name="requestedPage">The page of data requested for display</param>
        /// <returns></returns>
        public ActionResult Search(TwitterSearchModel model)
        {
            if (String.IsNullOrEmpty(model.SearchText))
            {
                return View();
            }

            PopulateSearchResultModel(model);
            return View(model);
        }

        private void PopulateSearchResultModel(TwitterSearchModel model)
        {
            // Retrieve data and populate the search result model for display
            model.SearchResultModel = new TwitterSearchResultModel();
            try
            {
                TwitterSearchClient searchClient = new TwitterSearchClient(
                    new SessionStateCache(this.HttpContext.Session),
                    new TwitterizerSearchService());
                model.SearchResultModel.TweetResults = searchClient.Search(
                    model.SearchText,
                    model.ClientPage,
                    model.ClientPageSize,
                    model.NewSearch);
            }
            catch (TwitterSearchException twitterSearchException)
            {
                model.SearchResultModel.ErrorMessage = "An error occurred when attempting to search Twitter's records, please try again later.";
            }
            catch (Exception exception)
            {
                model.SearchResultModel.ErrorMessage = "An unexpected error occurred, yikes!";
            }
        }

        /// <summary>
        /// Action for serving the Backbone version of this application
        /// </summary>
        /// <returns></returns>
        public ActionResult BackboneSearch()
        {
            return View();
        }
    }
}
