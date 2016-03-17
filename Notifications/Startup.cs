using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Notifications.Hubs;
using Owin;
using StructureMap;

namespace Notifications
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = new Container();

            UseSignalr(app, container);

            UseApi(app, container);
        }

        private static void UseSignalr(IAppBuilder app, IContainer container)
        {
            var configuration = new HubConfiguration();
            var resolver = configuration.Resolver;
            resolver.Register(typeof(IHubActivator), () => new StructureMapHubActivator(container));

            container.Configure(c =>
            {
                c.For<IHubConnectionContext<IMatchEvents>>().Use(() =>
                    resolver.Resolve<IConnectionManager>().GetHubContext<MatchHub, IMatchEvents>().Clients);
            });

            app.MapSignalR("/signalr", configuration);
        }

        private static void UseApi(IAppBuilder app, IContainer container)
        {
            var configuration = new HttpConfiguration();
            configuration.Services.Replace(typeof(IHttpControllerActivator), new StructureMapControllerActivator(container));

            configuration.MapHttpAttributeRoutes();

            app.UseWebApi(configuration);
        }

        private class StructureMapControllerActivator : IHttpControllerActivator
        {
            private readonly IContainer _container;

            public StructureMapControllerActivator(IContainer container)
            {
                _container = container;
            }

            public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
            {
                var context = _container.GetNestedContainer();
                request.RegisterForDispose(context);
                return context.GetInstance(controllerType) as IHttpController;
            }
        }

        private class StructureMapHubActivator : IHubActivator
        {
            private readonly IContainer _container;

            public StructureMapHubActivator(IContainer container)
            {
                _container = container;
            }

            public IHub Create(HubDescriptor descriptor)
            {
                return (IHub)_container.GetInstance(descriptor.HubType);
            }
        }
    }
}