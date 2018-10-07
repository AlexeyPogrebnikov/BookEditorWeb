using System.Web.Http;
using System.Web.Mvc;
using BookEditorWeb.Repositories;
using BookEditorWeb.Sample;
using BookEditorWeb.Services;
using BookEditorWeb.Validation;
using Unity;
using Unity.Lifetime;
using Unity.Mvc5;

namespace BookEditorWeb
{
	public static class UnityConfig
	{
		public static UnityContainer RegisterComponents()
		{
			var container = new UnityContainer();

			DependencyResolver.SetResolver(new UnityDependencyResolver(container));
			GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

			container.RegisterType<IBookRepository, BookRepository>(new ContainerControlledLifetimeManager());
			container.RegisterType<IBookImageRepository, BookImageRepository>(new ContainerControlledLifetimeManager());
			container.RegisterType<IEmbeddedResourceService, EmbeddedResourceService>();
			container.RegisterType<ISampleDataGenerator, SampleDataGenerator>();
			container.RegisterType<IBookValidator, BookValidator>();
			container.RegisterType<IBookPropertyValidator, BookTitleValidator>("Title");
			container.RegisterType<IBookPropertyValidator, BookAuthorsValidator>("Authors");

			return container;
		}
	}
}