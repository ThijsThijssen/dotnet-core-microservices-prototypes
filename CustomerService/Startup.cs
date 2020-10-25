using CustomerService.Connections;
using CustomerService.DAOs;
using CustomerService.Handlers;
using CustomerService.Listeners;
using CustomerService.Publishers;
using CustomerService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CustomerService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CustomerContext>(
                opt => opt.UseInMemoryDatabase("Customers"),
                contextLifetime: ServiceLifetime.Singleton);

            services.AddControllers();

            services.AddSingleton<IConnectionProvider>(
                new ConnectionProviderImpl(
                    //connectionString: "amqp://guest:guest@rabbitmq:5672/",
                    hostName:"rabbitmq",
                    username: "guest",
                    password: "guest"));

            services.AddSingleton<ICustomerDao, CustomerDaoImpl>();
            services.AddSingleton<ICustomerService, CustomerServiceImpl>();
            services.AddSingleton<IOrderEventHandler, OrderEventHandler>();

            services.AddSingleton(
                x => new CustomerEventPublisher(
                    x.GetService<IConnectionProvider>()
                ));

            services.AddSingleton<IOrderEventSubscriber>(x =>
                new Subscriber(
                    connectionProvider: x.GetService<IConnectionProvider>(),
                    exchange: "OrderEvents",
                    routingKey: "order.events",
                    exchangeType: "topic")
                );

            services.AddSingleton<IHostedService, OrderEventListener>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
