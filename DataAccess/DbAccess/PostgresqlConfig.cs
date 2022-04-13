using Microsoft.Extensions.Configuration;

namespace DataAccess.DbAccess
{
    public class PostgresqlConfig
    {
        public PostgresqlConfig(IConfiguration config)
        {
            Host = config.GetSection("ConnectionParams")["host"];
            Port = Int32.Parse(config.GetSection("ConnectionParams")["port"]);
            UserName = config.GetSection("ConnectionParams")["username"];
            DataBase = config.GetSection("ConnectionParams")["database"];
            Password = config.GetSection("ConnectionParams")["password"];
        }
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string DataBase { get; set; }
        public string Password { get; set; }        
    }
}
