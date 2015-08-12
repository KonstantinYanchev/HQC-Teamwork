namespace EasyHttp.Specs.Specs
{
    using System.IO;
    using System.Reflection;

    using EasyHttp.Http;

    [Subject(typeof(HttpClient))]
    public class when_making_a_GET_provided_filename
    {
        private static HttpClient httpClient;

        private static string filename;

        private Establish context = () =>
            {
                httpClient = new HttpClient();

                filename = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "image.gif");

                File.Delete(filename);
            };

        private Because of =
            () => httpClient.GetAsFile("http://www.jetbrains.com/img/logos/logo_jetbrains.gif", filename);

        private It should_download_file_to_specified_filename = () => File.Exists(filename).ShouldBeTrue();
    }
}