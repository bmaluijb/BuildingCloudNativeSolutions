using System.Collections.Generic;

namespace NationalCookies.Data.Interfaces
{
    public interface ICookieService
    {
        List<Cookie> GetAllCookies();
    }
}
