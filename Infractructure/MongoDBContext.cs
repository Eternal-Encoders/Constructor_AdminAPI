using Constructor_API.Core.Shared;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Constructor_API.Infractructure
{
    public class MongoDBContext
    {
        private IMongoDatabase _database;
        private readonly IConfiguration _configuration;
        public IClientSessionHandle _session;
        public MongoClient MongoClient { get; set; }
        private readonly List<Func<Task>> _commands = new List<Func<Task>>();

        public MongoDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
            MongoClient = new MongoClient(_configuration["ConnectionStrings:Server"]);
            if (MongoClient != null ) 
                _database = MongoClient.GetDatabase(_configuration["ConnectionStrings:DatabaseName"]);
        }

        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }

        public async Task<int> SaveChanges()
        {
            using (_session = await MongoClient.StartSessionAsync())
            {
                //_session.StartTransaction();
                //var commandTasks = _commands.Select(c => c());
                //await Task.WhenAll(commandTasks);
                //await _session.CommitTransactionAsync();
                //_commands.Clear();
                //return _commands.Count;

                try
                {
                    _session.StartTransaction();
                    var commandTasks = _commands.Select(c => c());
                    await Task.WhenAll(commandTasks);
                    await _session.CommitTransactionAsync();
                    return _commands.Count;
                }
                //catch (Exception ex)
                //{
                //    await _session.AbortTransactionAsync();
                //    throw ex;
                //    return 0;
                //}
                finally
                {
                    _commands.Clear();
                }
            }
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }

        public void Dispose()
        {
            _session?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
