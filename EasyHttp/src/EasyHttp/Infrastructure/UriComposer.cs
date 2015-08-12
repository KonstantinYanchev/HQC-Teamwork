namespace EasyHttp.Infrastructure
{
    using System;

    public class UriComposer
    {
        private readonly ObjectToUrlParameters _objectToUrlParameters;

        private readonly ObjectToUrlSegments _objectToUrlSegments;

        public UriComposer()
        {
            this._objectToUrlParameters = new ObjectToUrlParameters();
            this._objectToUrlSegments = new ObjectToUrlSegments();
        }

        public string Compose(string baseuri, string uri, object query, bool parametersAsSegments)
        {
            var returnUri = uri;
            if (!string.IsNullOrEmpty(baseuri))
            {
                returnUri = baseuri.EndsWith("/") ? baseuri : string.Concat(baseuri, "/");
                returnUri += uri.StartsWith("/", StringComparison.InvariantCulture) ? uri.Substring(1) : uri;
            }

            if (parametersAsSegments)
            {
                returnUri = query != null
                                ? string.Concat(returnUri, this._objectToUrlSegments.ParametersToUrl(query))
                                : returnUri;
            }
            else
            {
                returnUri = query != null
                                ? string.Concat(returnUri, this._objectToUrlParameters.ParametersToUrl(query))
                                : returnUri;
            }

            return returnUri;
        }
    }
}