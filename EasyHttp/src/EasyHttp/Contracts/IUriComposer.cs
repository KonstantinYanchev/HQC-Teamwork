namespace EasyHttp.Contracts
{
    public interface IUriComposer
    {
        /// <summary>
        /// Method for composing URI.
        /// </summary>
        /// <param name="baseuri"> Base URI for the URI which we are composing.</param>
        /// <param name="uri">URI for extending base URI.</param>
        /// <param name="query">Query string.</param>
        /// <param name="parametersAsSegments">Are parameters split as segments.</param>
        /// <returns>Complete URI string.</returns>
        string Compose(string baseuri, string uri, object query, bool parametersAsSegments);
    }
}