using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using BookEditorWeb.Models;
using BookEditorWeb.Services;
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

			FillSampleData(container);
		}

		private static void FillSampleData(UnityContainer container)
		{
			var bookRepository = container.Resolve<IBookRepository>();
			var bookImageRepository = container.Resolve<IBookImageRepository>();

			BookImage bookImage = bookImageRepository.Save(new BookImage
			{
				Content = GetImageContent()
			});

			bookRepository.Add(new Book
			{
				Title = "Атлант расправил плечи",
				Authors = new[]
				{
					new Author
					{
						FirstName = "Айн",
						LastName = "Рэнд"
					}
				},
				NumberOfPages = 1131,
				Publisher = "Альпина Паблишер",
				PublicationYear = 2018,
				Isbn = "978-5-9614-4579-4",
				ImageId = bookImage.Id
			});

			bookRepository.Add(new Book
			{
				Title = "Пикник на обочине",
				Authors = new[]
				{
					new Author
					{
						FirstName = "Аркадий",
						LastName = "Стругацкий"
					},
					new Author
					{
						FirstName = "Борис",
						LastName = "Стругацкий"
					}
				},
				NumberOfPages = 256,
				Publisher = "АСТ, Neoclassic",
				PublicationYear = 2007,
				Isbn = "978-5-17-045438-9"
				//Image = null
			});
		}

		private static byte[] GetImageContent()
		{
			using (Image image = Image.FromFile(GetBookImagePath(1)))
			{
				using (var stream = new MemoryStream())
				{
					image.Save(stream, image.RawFormat);
					return stream.ToArray();
				}
			}
		}

		private static string GetBookImagePath(int bookId)
		{
			return HttpContext.Current.Server.MapPath($"~/Content/img/books/{bookId}.jpg");
		}
	}
}