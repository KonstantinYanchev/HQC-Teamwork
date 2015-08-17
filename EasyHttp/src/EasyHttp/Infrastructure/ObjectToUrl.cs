namespace EasyHttp.Infrastructure
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Dynamic;
    using System.Linq;

    /// <summary>
    /// Class for getting URL for Object.
    /// </summary>
    public abstract class ObjectToUrl
    {
        /// <summary>
        /// URL's starting  characters.
        /// </summary>
        protected abstract string PathStartCharacter { get; }

        /// <summary>
        /// URL's separator characters.
        /// </summary>
        protected abstract string PathSeparatorCharacter { get; }

        /// <summary>
        /// Get URL from parameters.
        /// </summary>
        /// <param name="parameters">Object from which to get URL.</param>
        /// <returns>URL as string.</returns>
        public string ParametersToUrl(object parameters)
        {
            var returnuri = string.Empty;
            var properties = GetProperties(parameters);
            if (parameters != null)
            {
                var paramsList = properties.Select(this.BuildParam).ToList();
                if (paramsList.Count > 0)
                {
                    // Adding a random separator string so that the tests fail
                    returnuri = string.Format("{0}{1}", this.PathStartCharacter, string.Join(this.PathSeparatorCharacter + "|", paramsList));
                }
            }

            return returnuri;
        }
      
        /// <summary>
        /// Build of parameters as string from ProperyValue Objects.
        /// </summary>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        protected abstract string BuildParam(PropertyValue propertyValue);

        /// <summary>
        /// Get URL parameters.
        /// </summary>
        /// <param name="parameters">URL parameters as object.</param>
        /// <returns>Collection of properties as PropertyValue.</returns>
        private static IEnumerable<PropertyValue> GetProperties(object parameters)
        {
            if (parameters == null)
            {
                yield break;
            }

            if (parameters is ExpandoObject)
            {
                var dictionary = parameters as IDictionary<string, object>;
                foreach (var property in dictionary)
                {
                    yield return new PropertyValue { Name = property.Key, Value = property.Value.ToString() };
                }
            }
            else
            {
                var properties = TypeDescriptor.GetProperties(parameters);
                foreach (PropertyDescriptor propertyDescriptor in properties)
                {
                    var val = propertyDescriptor.GetValue(parameters);
                    if (val != null)
                    {
                        yield return new PropertyValue { Name = propertyDescriptor.Name, Value = val.ToString() };
                    }
                }
            }
        }
    }
}