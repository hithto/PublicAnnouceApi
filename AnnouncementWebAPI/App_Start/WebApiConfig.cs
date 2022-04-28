using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Routing;

namespace AnnouncementWebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.EnableCors();

            //EnableCorsAttribute cors = new EnableCorsAttribute("http://localhost:50000/mmei/", "*", "GET,POST");
            //config.EnableCors(cors);

            //config.EnableCors(new EnableCorsAttribute(origins:"http://localhost:3000",headers:"*", methods:"*"));

            //var constraints = new { httpMethod = new HttpMethodConstraint(HttpMethod.Options) };
            //config.Routes.IgnoreRoute("OPTIONS", "*pathInfo", constraints);

            EnableCrossSiteRequests(config);

            // Web API 設定和服務

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static void EnableCrossSiteRequests(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute(
                origins: "*",
                headers: "*",
                methods: "*");
            config.EnableCors(cors);
        }
    }
}
