namespace NancyxUnitAsyncDuplicateRequests
{
    using System.Diagnostics;
    using Nancy.Owin;
    using Nancy;
    using Owin;
    using System;
    using MidFunc = System.Func<
        System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>,
    System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>
    >;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app
                .Use(MyMiddleware.DoIt())
                .UseNancy();
        }
    }

    public class MyBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            Debug.WriteLine("APP STARTUP");

            pipelines.BeforeRequest += (ctx) =>
            {
                var env = ctx.GetOwinEnvironment();
                Debug.WriteLine("BEFORE REQUEST : " + env["owin.RequestId"]);
                return null;
            };
        }

        protected override void RequestStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);
            Debug.WriteLine("REQUEST STARTUP");
        }
    }

    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => "Boo";
        }
    }

    public static class MyMiddleware
    {
        public static MidFunc DoIt()
        {
            return next => async env =>
            {
                string requestId;
                if (env.ContainsKey("owin.RequestId"))
                {
                    requestId = env["owin.RequestId"].ToString();
                }
                else
                {
                    requestId = Guid.NewGuid().ToString();
                    env["owin.RequestId"] = requestId;
                }

                await next(env);

            };
        }
    }
}
