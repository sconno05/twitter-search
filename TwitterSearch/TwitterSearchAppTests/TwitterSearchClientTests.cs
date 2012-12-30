using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwitterSearchApp.TwitterClient;
using Rhino.Mocks;
using Twitterizer;
using System.Collections.Generic;
using System.Linq;

namespace TwitterSearchAppTests
{
    [TestClass]
    public class TwitterSearchClientTests
    {
        private ILocalCache _localCache;
        private ITwitterizerSearchService _searchService;

        [TestInitialize]
        public void Setup()
        {
            _localCache = MockRepository.GenerateMock<ILocalCache>();
            _searchService = MockRepository.GenerateMock<ITwitterizerSearchService>();
        }

        [TestMethod]
        public void StoreTweetsLocally()
        {
            TwitterResponse<TwitterSearchResultCollection> response = new TwitterResponse<TwitterSearchResultCollection>();
            response.ResponseObject = new TwitterSearchResultCollection();
            response.ResponseObject.Add(new TwitterSearchResult() { Id = 1, Text = "This is just a #test" });

            _searchService
                .Expect(s => s.Search(Arg<string>.Is.Anything, Arg<SearchOptions>.Is.Anything))
                .Return(response);

            _localCache.Expect(c => c.PageResults = response.ResponseObject);

            TwitterSearchClient client = new TwitterSearchClient(_localCache, _searchService);
            client.Search("#test", 1, 10, true);

            _searchService.VerifyAllExpectations();
            _localCache.VerifyAllExpectations();
        }

        [TestMethod]
        public void RetrieveTweetsFromCache()
        {
            var cachedResults = new TwitterSearchResultCollection();
            cachedResults.Add(new TwitterSearchResult() { Id = 1, Text = "This is just a #test" });

            _searchService.AssertWasNotCalled(s => s.Search(Arg<string>.Is.Anything, Arg<SearchOptions>.Is.Anything));

            _localCache.Expect(c => c.PageResults).Return(cachedResults);
            _localCache.Expect(c => c.PageNumber).Return(1);

            TwitterSearchClient client = new TwitterSearchClient(_localCache, _searchService);
            var results = client.Search("#test", 1, 10, false);
            Assert.IsTrue(AreEquivalent(cachedResults, results));

            _searchService.VerifyAllExpectations();
            _localCache.VerifyAllExpectations();
        }

        [TestMethod]
        public void BypassCacheForRecentResults()
        {
            // Setup the cache
            var cachedResults = new TwitterSearchResultCollection();
            cachedResults.Add(new TwitterSearchResult() { Id = 1, Text = "This is just a #test" });
            _localCache.Stub(c => c.PageResults).Return(cachedResults);
            _localCache.Stub(c => c.PageNumber).Return(1);

            // Setup the Twitterizer response
            TwitterResponse<TwitterSearchResultCollection> response = new TwitterResponse<TwitterSearchResultCollection>();
            response.ResponseObject = new TwitterSearchResultCollection();
            response.ResponseObject.Add(new TwitterSearchResult() { Id = 1, Text = "This is just a #test" });
            response.ResponseObject.Add(new TwitterSearchResult() { Id = 2, Text = "This is just another #test" });
            _searchService
                .Expect(s => s.Search(Arg<string>.Is.Anything, Arg<SearchOptions>.Is.Anything))
                .Return(response);

            TwitterSearchClient client = new TwitterSearchClient(_localCache, _searchService);
            var results = client.Search("#test", 1, 10, true); //newSearch means ignore the cache
            Assert.IsFalse(AreEquivalent(cachedResults, results));
            Assert.IsTrue(AreEquivalent(response.ResponseObject, results));

            _searchService.VerifyAllExpectations();
            _localCache.VerifyAllExpectations();
        }

        [TestMethod]
        public void MaintainPaginationConsistency()
        {
            // Setup the cache
            _localCache.Expect(c => c.PageNumber).Return(10);
            _localCache.Expect(c => c.MaxId).Return(999);

            // Setup the Twitterizer response
            TwitterResponse<TwitterSearchResultCollection> response = new TwitterResponse<TwitterSearchResultCollection>();
            response.ResponseObject = new TwitterSearchResultCollection();
            response.ResponseObject.Add(new TwitterSearchResult() { Id = 1, Text = "This is just a #test" });
            response.ResponseObject.Add(new TwitterSearchResult() { Id = 2, Text = "This is just another #test" });
            _searchService
                .Expect(s => s.Search(Arg<string>.Is.Anything, Arg<SearchOptions>.Matches(so => so.MaxId == 999 && so.PageNumber == 2)))
                .Return(response);

            TwitterSearchClient client = new TwitterSearchClient(_localCache, _searchService);
            var results = client.Search("#test", 11, 10, false);
            Assert.IsTrue(AreEquivalent(response.ResponseObject, results));

            _searchService.VerifyAllExpectations();
            _localCache.VerifyAllExpectations();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ClientPage_CannotBeZero()
        {
            TwitterSearchClient client = new TwitterSearchClient(_localCache, _searchService);
            client.Search("#test", 0, 10, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ClientPageSize_CannotBeZero()
        {
            TwitterSearchClient client = new TwitterSearchClient(_localCache, _searchService);
            client.Search("#test", 1, 0, true);
        }

        private bool AreEquivalent(IEnumerable<TwitterSearchResult> results1, IEnumerable<TwitterSearchResult> results2)
        {
            if (results1.Count() != results2.Count())
            {
                return false;
            }

            foreach (var result in results1)
            {
                if (results2.SingleOrDefault(r => r.Id == result.Id) == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
