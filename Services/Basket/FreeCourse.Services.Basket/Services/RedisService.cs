using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Services
{
    public class RedisService
    {
        private readonly string _host;
        private readonly int _port;

        private ConnectionMultiplexer _connectionMultiplexer;
        public RedisService(string host,int port)
        {
            _host = host;
            _port = port;
        }
        public ConnectionMultiplexer Connect() => _connectionMultiplexer = ConnectionMultiplexer.Connect($"{_host}:{_port}");
        public IDatabase GetDb(int db = 1) => _connectionMultiplexer.GetDatabase(db);
        public RedisKey[] GetKeys(IDatabase db,string pattern="*")
        {
            if (_connectionMultiplexer==null || _connectionMultiplexer.IsConnected==false)
            {
                throw new Exception("No Connect");
            }

            EndPoint endPoint = _connectionMultiplexer.GetEndPoints().First();
            return _connectionMultiplexer.GetServer(endPoint).Keys(database: db.Database, pattern: pattern).ToArray();
        }
    }
}
