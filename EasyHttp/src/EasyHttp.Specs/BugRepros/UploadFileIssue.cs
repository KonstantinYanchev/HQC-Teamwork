namespace EasyHttp.Specs.BugRepros
{
    using System.Collections.Generic;
    using System.IO;

    using EasyHttp.Http;
    using EasyHttp.Infrastructure;

    public class file_upload_was_failing_because_fieldname_for_file_field_was_not_being_set
    {
        private static HttpClient httpClient;

        private static HttpResponse response;

        private Establish context = () => { httpClient = new HttpClient(); };

        private Because of = () =>
            {
                var filename = Path.Combine("Helpers", "test.xml");

                IDictionary<string, object> data = new Dictionary<string, object>();

                data.Add("runTest", " Run Test ");

                IList<FileData> files = new List<FileData>();

                files.Add(new FileData { FieldName = "file", ContentType = "text/xml", Filename = filename });

                httpClient.Post("https://loandelivery.intgfanniemae.com/mismoxtt/mismoValidator.do", data, files);

                response = httpClient.Response;
            };

        private It should_say_that_operation_was_successful =
            () => response.RawText.ShouldNotContain("Please select a file to test.");
    }
}