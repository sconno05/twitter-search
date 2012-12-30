using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Twitterizer;

namespace TwitterSearchApp.Models
{
    /// <summary>
    /// Composite model holds data for both search input and search result output.
    /// </summary>
    public class TwitterSearchModel
    {
        public TwitterSearchModel()
        {
            this.ClientPage = 1;
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Enter a hashtag")]
        public string SearchText { get; set; }

        public int ClientPage { get; set; }

        private const int CLIENT_PAGE_SIZE = 10;
        public int ClientPageSize
        {
            get { return CLIENT_PAGE_SIZE; }
        }

        public bool NewSearch { get; set; }

        public TwitterSearchResultModel SearchResultModel { get; set; }
    }

    /// <summary>
    /// Contains search result data
    /// </summary>
    public class TwitterSearchResultModel
    {
        public IEnumerable<TwitterSearchResult> TweetResults { get; set; }
        public string ErrorMessage { get; set; }
    }
}