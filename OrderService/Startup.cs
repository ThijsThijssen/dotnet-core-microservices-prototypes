using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderService.Connections;
using OrderService.DAOs;
using OrderService.Handlers;
using OrderService.Listeners;
using OrderService.Publishers;
using OrderService.Services;

namespace OrderService
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
            services.AddDbContext<OrderContext>(opt => opt.UseInMemoryDatabase("Orders"), contextLifetime: ServiceLifetime.Singleton);

            services.AddControllers();

            services.AddSingleton<IConnectionProvider>(new ConnectionProviderImpl(hostName: "localhost"));

            services.AddSingleton<IOrderDao, OrderDaoImpl>();
            services.AddSingleton<IOrderService, OrderServiceImpl>();
            services.AddSingleton<ICustomerEventHandler, CustomerEventHandler>();

            services.AddSingleton(x => new OrderEventPublisher(x.GetService<IConnectionProvider>()));

            services.AddSingleton<ICustomerEventSubscriber>(x =>
                new Subscriber(
                    connectionProvider: x.GetService<IConnectionProvider>(),
                    exchange: "CustomerEvents",
                    routingKey: "customer.events",
                    exchangeType: "topic"));

            services.AddSingleton<IHostedService, CustomerEventListener>();
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
