namespace Sardonyx.Framework.Client
{
    public abstract class BaseService
    {
        private readonly BaseClient _client;
        protected BaseClient Client => _client;

        private BaseService() { throw new InvalidOperationException("Cannot initialize a service without a BaseClient instance"); }
        public BaseService(BaseClient client)
        {
            _client = client;
        }
    }
}
