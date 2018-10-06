using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookEditorWeb.Models;

namespace BookEditorWeb.Services
{
	public class BookRepository
	{
		private static readonly IList<Book> Books = new List<Book>();
		private static readonly object SyncRoot = new object();
		private static int _currentId = 1;

		public void Add(Book book)
		{
			lock (SyncRoot)
			{
				book.Id = _currentId;
				Books.Add(book);
				_currentId++;
			}
		}

		public IEnumerable<Book> GetAll()
		{
			lock (SyncRoot)
			{
				return Books;
			}
		}

		private static string GetBookImagePath(int bookId)
		{
			return HttpContext.Current.Server.MapPath($"~/Content/img/books/{bookId}.jpg");
		}

		public void Remove(int id)
		{
			lock (SyncRoot)
			{
				Books.Remove(Books.FirstOrDefault(book => book.Id == id));
			}
		}
	}
}