using Silvestre.Cardano.Integration.DbSyncAPI.Database;
using Silvestre.Cardano.Integration.DbSyncAPI.Services;

namespace Silvestre.Cardano.Integration.DbSyncAPI
{
    public class Startup
    {
        public Startup(IWebHostEnvironment environment)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true, true);

            //if (environment.IsDevelopment())
            //{
            //    configurationBuilder.AddJsonFile($"appsettings.Secrets.json", true, true);
            //}

            Configuration = configurationBuilder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<DatabaseProxy>(sp => new DatabaseProxy(Npgsql.NpgsqlFactory.Instance, Configuration.GetConnectionString("cardano-db-sync")));
            services.AddGrpc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.SetupDatabase();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<EpochService>();
                endpoints.MapGrpcService<BlockService>();
                endpoints.MapGrpcService<StakePoolService>();
                endpoints.MapGrpcService<TransactionService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
