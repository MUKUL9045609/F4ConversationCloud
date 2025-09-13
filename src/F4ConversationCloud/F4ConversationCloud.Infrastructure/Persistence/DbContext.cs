using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace F4ConversationCloud.Infrastructure.Persistence;

public class DbContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    public DbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("SqlConnection") ?? throw new ArgumentNullException(nameof(configuration));
    }
    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}

