using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwilioRegistration.BusinessLogic.Helpers
{
    public sealed class RedisConnection
    {
        public ConnectionMultiplexer Multiplexer { get; set; }
        public IDatabase Database { get; set; }

        private static RedisConnection _Instance { get; set; }

        public static RedisConnection Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new RedisConnection();
                }
                return _Instance;
            }
        }

        private RedisConnection()
        {
            Multiplexer = ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["RedisConnectionString"]);
            int databaseId;
            if (int.TryParse(ConfigurationManager.AppSettings["RedisDatabase"], out databaseId))
            {
                Database = Multiplexer.GetDatabase(databaseId);
            }
            else
            {
                Database = Multiplexer.GetDatabase();
            }
        }
    }
}
