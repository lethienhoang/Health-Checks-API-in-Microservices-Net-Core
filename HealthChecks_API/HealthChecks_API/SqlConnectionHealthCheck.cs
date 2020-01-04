using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace HealthChecks_API
{
    public class SqlConnectionHealthCheck : IHealthCheck
    {
        private readonly string _defaultSqlQuery = "Select 1";
        private string _connectionString { get; }

        public SqlConnectionHealthCheck(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                if (_defaultSqlQuery != null)
                {
                    var command = connection.CreateCommand();
                    command.CommandText = _defaultSqlQuery;

                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }

            return HealthCheckResult.Healthy();
        }
    }
}
