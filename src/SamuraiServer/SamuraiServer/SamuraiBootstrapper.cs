using System.Data.Entity.Migrations;
using System.Reflection;
using Autofac;
using System.Configuration;
using Autofac.Integration.Mvc;
using SamuraiServer.Data;
using SamuraiServer.Data.Impl.Sql;
using SamuraiServer.Data.Providers;
using SamuraiServer.Migrations;

namespace SamuraiServer
{
    public class IdeastrikeBootstrapper
    {
        private const string SqlClient = "System.Data.SqlClient";

        public IContainer ConfigureApplicationContainer(ILifetimeScope existingContainer)
        {
            var builder = new ContainerBuilder();

            if (ConfigurationManager.ConnectionStrings.Count > 0 && ConfigurationManager.ConnectionStrings["Ideastrike"] != null)
                builder.RegisterType<SamuraiContext>()
                    .WithParameter(new NamedParameter("nameOrConnectionString", ConfigurationManager.ConnectionStrings["Ideastrike"].ConnectionString + ";MultipleActiveResultSets=true"))
                    .AsSelf()
                    .InstancePerLifetimeScope();

            else
                builder.RegisterType<SamuraiContext>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(Player).Assembly)
                   .Where(t => t.Name.EndsWith("Repository"))
                   .Where(t => t.Name.StartsWith("InMemory"))
                   .AsImplementedInterfaces()
                   .InstancePerHttpRequest();

            builder.RegisterAssemblyTypes(typeof(Player).Assembly)
                   .Where(t => t.Name.EndsWith("Provider"))
                   .AsImplementedInterfaces()
                   .InstancePerHttpRequest();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<CombatCalculator>().AsImplementedInterfaces().SingleInstance();
            return builder.Build();
        }

        private static void DoMigrations()
        {
            var settings = new SamuraiDbConfiguration();
            var migrator = new DbMigrator(settings);
            migrator.Update();
        }

        //protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines)
        //{
        //    var formsAuthConfiguration =
        //        new FormsAuthenticationConfiguration
        //            {
        //                RedirectUrl = "~/login",
        //                UserMapper = container.Resolve<IUserRepository>(),
        //            };

        //    FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
        //}

        //protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        //{
        //    pipelines.OnError.AddItemToEndOfPipeline((context, exception) =>
        //                                                 {
        //                                                     var message = string.Format("Exception: {0}", exception);
        //                                                     new ElmahErrorHandler.LogEvent(message).Raise();
        //                                                     return null;
        //                                                 });

        //    DoMigrations();
        //}
    }
}