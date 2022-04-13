using Npgsql;

namespace DataAccess.DbAccess;

public interface IPostgresqlConnection
{
    NpgsqlConnection GetSqlConnection();
}