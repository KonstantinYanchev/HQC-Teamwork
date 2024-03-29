namespace EasyHttp.Infrastructure
{
    using System;
    using Contracts;

    /// <summary>
    /// Class used to compose URIs.
    /// </summary>
    public class UriComposer : IUriComposer
    {
        private readonly ObjectToUrlParameters objectToUrlParameters;

        private readonly ObjectToUrlSegments objectToUrlSegments;

        /// <summary>
        /// Class used to compose URIs.
        /// </summary>
        public UriComposer()
        {
            this.objectToUrlParameters = new ObjectToUrlParameters();
            this.objectToUrlSegments = new ObjectToUrlSegments();
        }
         
        /// <summary>
        /// Method for composing URI.
        /// </summary>
        /// <param name="baseuri"> Base URI for the URI which we are composing.</param>
        /// <param name="uri">URI for extending base URI.</param>
        /// <param name="query">Query string.</param>
        /// <param name="parametersAsSegments">Are parameters split as segments.</param>
        /// <returns>Complete URI string.</returns>
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
                                ? string.Concat(returnUri, this.objectToUrlSegments.ParametersToUrl(query))
                                : returnUri;
            }
            else
            {
                returnUri = query != null
                                ? string.Concat(returnUri, this.objectToUrlParameters.ParametersToUrl(query))
                                : returnUri;
            }

            return returnUri;
        }
    }
}