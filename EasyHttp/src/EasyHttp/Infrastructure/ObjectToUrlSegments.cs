namespace EasyHttp.Infrastructure
{
    using System.Web;

    public class ObjectToUrlSegments : ObjectToUrl
    {
        protected override string PathStartCharacter
        {
            get
            {
                return "/";
            }
        }

        protected override string PathSeparatorCharacter
        {
            get
            {
                return "/";
            }
        }

        protected override string BuildParam(PropertyValue propertyValue)
        {
            return HttpUtility.UrlEncode(propertyValue.Value);
        }
    }
}