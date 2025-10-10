using Cracker_Shop.Repository;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Cracker_Shop.Repository.IRepository;

namespace Cracker_Shop.DependencyInjection
{
    public static class Bindings
    {

        public static void AddCommonRepositories(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IDbConnection>(sp => new SqlConnection(connectionString));

            services.AddScoped<ICompanyRepository, CompanyRepository>();


            services.AddScoped<IMasterRepository, MasterRepository>();

        }






    }

}
