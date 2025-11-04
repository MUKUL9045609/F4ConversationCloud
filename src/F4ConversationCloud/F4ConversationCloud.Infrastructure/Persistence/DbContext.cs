using F4ConversationCloud.Application.Common.Models.MetaCloudApiModel.Templates;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Infrastructure.Persistence;

public class DbContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public DbContext(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("SqlConnection") ?? throw new ArgumentNullException(nameof(configuration));
        _httpContextAccessor = httpContextAccessor;
    }
    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    //public int SessionUserId => Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
    public int? SessionUserId
    {
        get
        {
            var user = _httpContextAccessor?.HttpContext?.User;
            var claim = user?.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null && int.TryParse(claim.Value, out int userId))
            {
                return userId;
            }
            return 0;
        }
    }
}

