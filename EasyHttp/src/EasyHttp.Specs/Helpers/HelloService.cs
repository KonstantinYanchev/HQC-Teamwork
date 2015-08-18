namespace EasyHttp.Specs.Helpers
{
    using ServiceStack.ServiceInterface;

    public class HelloService : RestServiceBase<Hello>
    {
        public override object OnGet(Hello request)
        {
            return new HelloResponse { Result = "Hello, " + request.Name };
        }

        public override object OnPut(Hello request)
        {
            return new HelloResponse { Result = "Hello, " + request.Name };
        }

        public override object OnPost(Hello request)
        {
            return new HelloResponse { Result = "Hello, " + request.Name };
        }

        public override object OnDelete(Hello request)
        {
            return new HelloResponse { Result = "Hello, " + request.Name };
        }
    }
}