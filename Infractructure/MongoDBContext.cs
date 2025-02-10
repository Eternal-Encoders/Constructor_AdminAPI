using ConstructorAdminAPI.Core.Shared;
using MongoDB.Driver;

namespace ConstructorAdminAPI.Infractructure
{
    public class MongoDBContext : IUnitOfWork
    {
        private IMongoDatabase Database { get; set; }
        public IClientSessionHandle Session { get; set; }
        public MongoClient MongoClient { get; set; }
        private readonly List<Func<Task>> _commands;
        private readonly IConfiguration _configuration;

        public MongoDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _commands = new List<Func<Task>>();
            MongoClient = new MongoClient(_configuration["ConnectionStrings:Server"]);
            Database = MongoClient.GetDatabase(_configuration["ConnectionStrings:DatabaseName"]);
        }

        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }

        public async Task<bool> SaveChanges()
        {
            using (Session = await MongoClient.StartSessionAsync())
            {
                Session.StartTransaction();
                var commandTasks = _commands.Select(c => c());
                await Task.WhenAll(commandTasks);
                await Session.CommitTransactionAsync();
            }

            return (_commands.Count > 0);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return Database.GetCollection<T>(name);
        }

        public void Dispose()
        {
            Session?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
