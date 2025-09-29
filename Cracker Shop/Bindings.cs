using Cracker_Shop.Repository;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Cracker_Shop.DependencyInjection
{
    public static class Bindings
    {

        public static void AddCommonRepositories(this IServiceCollection services, string connectionString)
        {
            // Dapper IDbConnection
            services.AddScoped<IDbConnection>(sp => new SqlConnection(connectionString));

            services.AddScoped<ICompanyRepository, CompanyRepository>();



        }



    

      
    }

}
