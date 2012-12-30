using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Twitterizer;

namespace TwitterSearchApp.TwitterClient
{
    public class SessionStateCache : ILocalCache
    {
        private const string PAGE_NUMBER = "PAGE_NUMBER";
        private const string PAGE_RESULTS = "CURRENT_PAGE_RESULTS";
        private const string MAX_ID = "MAX_ID";

        private HttpSessionStateBase _sessionState;

        public SessionStateCache(HttpSessionStateBase sessionState)
        {
            if (sessionState == null)
            {
                throw new ArgumentNullException("sessionState");
            }

            _sessionState = sessionState;
        }

        public int? PageNumber
        {
            get
            {
                if (_sessionState[PAGE_NUMBER] != null)
                {
                    return (int)_sessionState[PAGE_NUMBER];
                }

                return null;
            }
            set
            {
                _sessionState[PAGE_NUMBER] = value;
            }
        }

        public TwitterSearchResultCollection PageResults
        {
            get
            {
                if (_sessionState[PAGE_RESULTS] != null)
                {
                    return (TwitterSearchResultCollection)_sessionState[PAGE_RESULTS];
                }

                return null;
            }
            set
            {
                _sessionState[PAGE_RESULTS] = value;
            }
        }

        public decimal? MaxId
        {
            get
            {
                if (_sessionState[MAX_ID] != null)
                {
                    return (decimal)_sessionState[MAX_ID];
                }

                return null;
            }
            set
            {
                _sessionState[MAX_ID] = value;
            }
        }
    }
}