namespace EasyHttp.Infrastructure
{
    using System.Web;

    /// <summary>
    /// Class for getting URL segments from Object.
    /// </summary>
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