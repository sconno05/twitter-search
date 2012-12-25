using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterSearchApp.TwitterClient
{
    /// <summary>
    /// Validates input from client UI
    /// </summary>
    internal static class InputValidator
    {
        /// <summary>
        /// Verifies the page number passed from the client UI is valid
        /// </summary>
        /// <param name="clientPage">Page number requested for display by the client UI</param>
        internal static void ValidateClientPage(int clientPage)
        {
            if (clientPage < 1)
            {
                throw new ArgumentOutOfRangeException("value must be greater than 1", "clientPage");
            }
        }

        /// <summary>
        /// Verifies the page size passed from the client UI is valid
        /// </summary>
        /// <param name="clientPageSize">Page size requested for display by the client UI</param>
        internal static void ValidateClientPageSize(int clientPageSize)
        {
            if (clientPageSize < 1)
            {
                throw new ArgumentOutOfRangeException("value must be greater than 1", "clientPageSize");
            }
        }
    }
}