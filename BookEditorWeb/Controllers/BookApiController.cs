using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using BookEditorWeb.Helpers;
using BookEditorWeb.Models;

namespace BookEditorWeb.Controllers
{
	public class BookApiController : ApiController
	{
		[HttpGet]
		public IList<Book> GetAll()
		{
			return new[]
			{
				new Book
				{
					Id = 1,
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
					Image = ImageToBase64Converter.Convert(GetBookImagePath(1))
				},
				new Book
				{
					Id = 2,
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
					Isbn = "978-5-17-045438-9",
					Image = null
				}
			};
		}

		private static string GetBookImagePath(int bookId)
		{
			return HttpContext.Current.Server.MapPath(string.Format("~/Content/img/books/{0}.jpg", bookId));
		}
	}
}