using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwitterSearchApp.Controllers;
using TwitterSearchApp.Models;
using System.Web.Mvc;
using System.Linq;
using MvcContrib.TestHelper;
using Twitterizer;
using System.Collections.Generic;

namespace TwitterSearchAppTests
{
    [TestClass]
    public class TwitterSearchControllerTests
    {
        [TestMethod]
        public void InitialView()
        {
            TwitterSearchController controller = GetController();
            var result = controller.Search(new TwitterSearchModel()) as ViewResult;

            Assert.IsNotNull(result, "Expected a ViewResult");
            Assert.IsNull(result.Model);
        }

        [TestMethod]
        public void Display10TweetsPerPage()
        {
            TwitterSearchController controller = GetController();
            TwitterSearchModel model = new TwitterSearchModel();
            model.SearchText = "#test";
            
            var result = controller.Search(model) as ViewResult;
            Assert.IsNotNull(result, "Expected a ViewResult");

            var resultModel = result.Model as TwitterSearchModel;
            Assert.IsNotNull(resultModel, "Expected a TwitterSearchModel");
            Assert.AreEqual(10, resultModel.SearchResultModel.TweetResults.Count());
        }
               
        private TwitterSearchController GetController()
        {
            TestControllerBuilder builder = new TestControllerBuilder();
            TwitterSearchController controller = new TwitterSearchController();
            builder.InitializeController(controller);
            return controller;
        }

        /// <summary>
        /// Determine whether or not the two sets of results contain any overlapping Tweets
        /// </summary>
        private bool ResultsOverlap(IEnumerable<TwitterSearchResult> firstResults, IEnumerable<TwitterSearchResult> secondResults)
        {
            foreach (var result in firstResults)
            {
                var match = secondResults.SingleOrDefault(r => r.Id == result.Id);
                if (match != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
