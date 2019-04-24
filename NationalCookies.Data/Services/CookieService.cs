using NationalCookies.Data.Interfaces;
using System.Collections.Generic;

namespace NationalCookies.Data.Services
{
    public class CookieService : ICookieService
    {
        private readonly CosmosDBConnector _cosmos;

        public CookieService(CosmosDBConnector cosmos)
        {
            this._cosmos = cosmos;
        }

        public List<Cookie> GetAllCookies()
        {
            List<Cookie> cookies;

            //get the cookies from the database
            cookies = this._cosmos.RetrieveAllCookies();

            return cookies;
        }
    }
}
