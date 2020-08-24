using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using KafkaConnection.kafkawrapper;
using KafkaConnection.Messangerwrapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using phonebook_app_read.Controllers;
using phonebook_app_read.Persistence;
using phonebook_app_read.Persistence.wrapper;
using phonebook_app_read.Service;

namespace phonebook_app_read
{
    public class Startup
    {
        public ILifetimeScope AutofacContainer { get; private set; }

        public IConfiguration Configuration { get; }
        public Startup(IWebHostEnvironment env)
        {
            // In ASP.NET Core 3.0 `env` will be an IWebHostEnvironment, not IHostingEnvironment.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddControllers();
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // If you want to set up a controller for, say, property injection
            // you can override the controller registration after populating services.
            builder.RegisterType<PhonebookController>().PropertiesAutowired();
            builder.Register(c => CassandraDBRepository.Instance(this.AutofacContainer)).As<IDBRepository>();
            builder.RegisterType<PhonebookReadService>().As<IPhonebookReadService>();
            builder.Register(c => PhonebookElasticSearch.Instance()).As<IPhonebookElasticSearch>();
            builder.RegisterType<PhonebookConsumerService>().As<IPhonebookConsumerService>();
            builder.Register(c => new KafkaWrapper(ConfigReader.GetValue<string>("KafkaHost"))).As<IMessangerWrapper>();
            builder.Register(c=> new CassandraWrapper(ConfigReader.GetValue<string>("CASSANDRA_SERVER_NAME"), ConfigReader.GetValue<string>("CASSANDRA_KEYSPACE_NAME"))).As<ICassandraWrapper>();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            Thread StaticCaller = new Thread(new ThreadStart((new PhonebookConsumerService(this.AutofacContainer)).RegisterMethods));
            StaticCaller.Start();
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
