namespace EasyHttp.Infrastructure
{
    using System.Web;

    public class ObjectToUrlParameters : ObjectToUrl
    {
        protected override string PathStartCharacter
        {
            get
            {
                return "?";
            }
        }

        protected override string PathSeparatorCharacter
        {
            get
            {
                return "&";
            }
        }

        protected override string BuildParam(PropertyValue propertyValue)
        {
            return string.Join("=", propertyValue.Name, HttpUtility.UrlEncode(propertyValue.Value));
        }
    }
}