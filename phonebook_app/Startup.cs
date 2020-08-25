using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authorization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MessageCarrier.kafkawrapper;
using MessageCarrier.Messangerwrapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PhonebookWrite.Persistence;
using PhonebookWrite.Persistence.wrapper;
using PhonebookWrite.Service;

namespace PhonebookWrite
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
            services.AddScoped<IUserService, UserService>();
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //builder.RegisterType<PhonebookController>().PropertiesAutowired();
            //builder.Register(c => CassandraDBRepository.Instance(ConfigReader.GetValue<string>("CASSANDRA_SERVER_NAME"), ConfigReader.GetValue<string>("CASSANDRA_KEYSPACE_NAME"))).As<IDBRepository>();
            builder.Register(c => new KafkaWrapper(ConfigReader.GetValue<string>("KafkaConnectionString"))).As<IMessangerWrapper>();
            //builder.Register(c => new DBRepository(ConfigReader.GetValue<string>("DapperConnectionString"))).As<IDBRepository>();

            builder.Register(c => new ConnectionWrapper(ConfigReader.GetValue<string>("DapperConnectionString"))).As<IConnectionWrapper>();
            //builder.Register(c => new MessagePublisher(this.AutofacContainer)).As<IMessagePublisher>();
            builder.RegisterType<MessagePublisher>().As<IMessagePublisher>();
            builder.RegisterType<DBRepository>().As<IDBRepository>().WithParameter( new TypedParameter(typeof(string), ConfigReader.GetValue<string>("DapperConnectionString")) );
            builder.RegisterType<PhonebookService>().As<IPhonebookService>().InstancePerLifetimeScope();
            //builder.Register(c => new PhonebookService()).As<IPhonebookService>().InstancePerLifetimeScope();
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
            //IDBRepository db = this.AutofacContainer.Resolve<IDBRepository>();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
