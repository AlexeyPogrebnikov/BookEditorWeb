using System.Collections.Generic;
using System.Web.Http;
using BookEditorWeb.Models;
using BookEditorWeb.Services;

namespace BookEditorWeb.Controllers
{
	public class BookApiController : ApiController
	{
		private readonly BookRepository _bookRepository = new BookRepository();

		[HttpGet]
		public IEnumerable<Book> GetAll()
		{
			return _bookRepository.GetAll();
		}
	}
}