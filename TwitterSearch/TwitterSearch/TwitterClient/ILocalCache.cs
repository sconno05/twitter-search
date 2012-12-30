using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twitterizer;

namespace TwitterSearchApp.TwitterClient
{
    public interface ILocalCache
    {
        int? PageNumber { get; set; }

        TwitterSearchResultCollection PageResults { get; set; }

        decimal? MaxId { get; set; }
    }
}
