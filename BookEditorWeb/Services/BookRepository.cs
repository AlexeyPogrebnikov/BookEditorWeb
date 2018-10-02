using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookEditorWeb.Helpers;
using BookEditorWeb.Models;

namespace BookEditorWeb.Services
{
	public class BookRepository
	{
		private static readonly IList<Book> Books;

		static BookRepository()
		{
			Books = new List<Book>
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

		public void Add(Book book)
		{
			Books.Add(book);
		}

		public IEnumerable<Book> GetAll()
		{
			return Books;
		}

		private static string GetBookImagePath(int bookId)
		{
			return HttpContext.Current.Server.MapPath($"~/Content/img/books/{bookId}.jpg");
		}

		public void Remove(int id)
		{
			Books.Remove(Books.FirstOrDefault(book => book.Id == id));
		}
	}
}