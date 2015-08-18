namespace EasyHttp.Specs.Helpers
{
    using System.ComponentModel;

    // Define the Web Services AppHost
    public class ServiceStackHost : AppHostHttpListenerBase
    {
        public ServiceStackHost()
            : base("StarterTemplate HttpListener", typeof(HelloService).Assembly)
        {
        }

        public override void Configure(Container container)
        {
            Routes.Add<Hello>("/hello").Add<Hello>("/hello/{Name}");
            Routes.Add<Files>("/fileupload/{Name}").Add<Files>("/fileupload");
            Routes.Add<CookieInfo>("/cookie").Add<CookieInfo>("/cookie/{Name}");
            Routes.Add<Redirect>("/redirector").Add<Redirect>("/redirector/redirected");
        }
    }
}