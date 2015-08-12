namespace EasyHttp.Specs.Helpers
{
    public class DataSpecificationBase : IAssemblyContext
    {
        private ServiceStackHost _appHost;

        private int _port;

        void IAssemblyContext.OnAssemblyComplete()
        {
            this._appHost.Dispose();
        }

        void IAssemblyContext.OnAssemblyStart()
        {
            this._port = 16000;
            var listeningOn = "http://localhost:" + this._port + "/";
            this._appHost = new ServiceStackHost();
            this._appHost.Init();
            this._appHost.Start(listeningOn);
        }
    }
}