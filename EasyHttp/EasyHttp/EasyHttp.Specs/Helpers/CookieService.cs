namespace EasyHttp.Specs.Helpers
{
    using System.Net;

    using ServiceStack.Common.Web;
    using ServiceStack.ServiceInterface;

    public class CookieService : RestServiceBase<CookieInfo>
    {
        public override object OnGet(CookieInfo request)
        {
            if (!Request.Cookies.ContainsKey(request.Name))
            {
                return new HttpResult { StatusCode = HttpStatusCode.NotFound };
            }

            return new CookieInfo { Name = request.Name, Value = Request.Cookies[request.Name].Value };
        }

        public override object OnPut(CookieInfo request)
        {
            Response.Cookies.AddCookie(new Cookie(request.Name, request.Value));
            return new HttpResult { StatusCode = HttpStatusCode.OK };
        }
    }
}