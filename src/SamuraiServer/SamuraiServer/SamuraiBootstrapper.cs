using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using MongoDB.Driver;
using SamuraiServer.Data;
using SamuraiServer.Data.Impl.Sql;
using SamuraiServer.Data.Providers;
using SamuraiServer.Migrations;

namespace SamuraiServer
{
    public class SamuraiBootstrapper
    {
        public IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

#if DEBUG
            RegisterInMemorySource(builder);
#else
            RegisterRemoteSource(builder);
#endif

            builder.RegisterAssemblyTypes(typeof(Player).Assembly)
                   .Where(t => t.Name.EndsWith("Provider"))
                   .AsImplementedInterfaces()
                   .InstancePerHttpRequest();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<CombatCalculator>().AsImplementedInterfaces().SingleInstance();

            return builder.Build();
        }

        private static void RegisterRemoteSource(ContainerBuilder builder)
        {
            if (ConfigurationManager.ConnectionStrings.Count > 0 && ConfigurationManager.ConnectionStrings["Samurai"] != null)
                builder.RegisterType<SamuraiContext>()
                       .WithParameter(new NamedParameter("nameOrConnectionString", ConfigurationManager.ConnectionStrings["Samurai"].ConnectionString + ";MultipleActiveResultSets=true"))
                       .AsSelf()
                       .InstancePerLifetimeScope();
            else
                builder.RegisterType<SamuraiContext>()
                       .AsSelf()
                       .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(Player).Assembly)
               .Where(t => t.Name.EndsWith("Repository"))
               .Where(t => t.Name.StartsWith("Sql"))
               .AsImplementedInterfaces()
               .InstancePerHttpRequest();

            //var connString = ConfigurationManager.AppSettings["MONGOHQ_URL"];
            //var databaseName = connString.Split('/').Last();
            //var server = MongoServer.Create(connString);
            //var database = server.GetDatabase(databaseName);

            //if (!database.CollectionExists("Messages"))
            //    database.CreateCollection("Messages");

            //builder.RegisterInstance(server)
            //       .As<MongoServer>();
            //builder.RegisterInstance(database)
            //       .As<MongoDatabase>();
            //builder.RegisterInstance(database.GetCollection<GameState>("GameState"))
            //       .As<MongoCollection<GameState>>();

            //builder.RegisterAssemblyTypes(typeof(Player).Assembly)
            //   .Where(t => t.Name.EndsWith("Repository"))
            //   .Where(t => t.Name.StartsWith("MongoDb"))
            //   .AsImplementedInterfaces()
            //   .InstancePerHttpRequest();
        }

        private static void RegisterInMemorySource(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof (Player).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .Where(t => t.Name.StartsWith("InMemory"))
                .AsImplementedInterfaces()
                .InstancePerHttpRequest();
        }

        public static void DoMigrations()
        {
            var settings = new SamuraiDbConfiguration();
            var migrator = new DbMigrator(settings);
            migrator.Update();
        }
    }
}