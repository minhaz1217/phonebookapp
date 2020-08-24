using System;
using System.Collections.Generic;
using System.Linq;
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
using phonebook_app.Persistence;
using phonebook_app.Service;
using phonebook_app_read;
using phonebook_practice_app.Persistence;
using phonebook_practice_app.Persistence.wrapper;

namespace phonebook_app
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        public ILifetimeScope AutofacContainer { get; private set; }
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
            //builder.RegisterType<PhonebookController>().PropertiesAutowired();
            //builder.Register(c => CassandraDBRepository.Instance(ConfigReader.GetValue<string>("CASSANDRA_SERVER_NAME"), ConfigReader.GetValue<string>("CASSANDRA_KEYSPACE_NAME"))).As<IDBRepository>();
            builder.Register(c => new KafkaWrapper(ConfigReader.GetValue<string>("KafkaConnectionString"))).As<IMessangerWrapper>();
            builder.Register(c => new DBRepository(this.AutofacContainer)).As<IDBRepository>();

            builder.Register(c => new ConnectionWrapper(this.AutofacContainer, ConfigReader.GetValue<string>("DapperConnectionString"))).As<IConnectionWrapper>();
            builder.Register(c => new MessagePublisher(this.AutofacContainer)).As<IMessagePublisher>();

            builder.Register(c => new PhonebookService(this.AutofacContainer)).As<IPhonebookService>();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // the codes to create the tables are in the dbrepository, this is here ONLY TO create the tables.
            IDBRepository db = this.AutofacContainer.Resolve<IDBRepository>();
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
