using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using BookEditorWeb.Sample;
using Unity;

namespace BookEditorWeb
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			UnityContainer container = UnityConfig.RegisterComponents();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			container.Resolve<ISampleDataGenerator>().Generate();
		}
	}
}