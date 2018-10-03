using System.Collections.Generic;
using System.Web.Http;
using BookEditorWeb.Extensions;
using BookEditorWeb.Models;
using BookEditorWeb.Services;

namespace BookEditorWeb.Controllers
{
	public class BookApiController : ApiController
	{
		private readonly BookRepository _bookRepository = new BookRepository();

		[HttpGet]
		public IEnumerable<Book> GetAll(string sidx, SortDirection sord)
		{
			IEnumerable<Book> books = _bookRepository.GetAll();

			return books.Sort(sidx, sord);
		}

		public void Add(AddBookRequest request)
		{
			_bookRepository.Add(new Book
			{
				Title = request.Title,
				Authors = request.Authors
			});
		}

		[HttpPost]
		public void Remove(DeleteBookRequest request)
		{
			_bookRepository.Remove(request.BookId);
		}
	}
}