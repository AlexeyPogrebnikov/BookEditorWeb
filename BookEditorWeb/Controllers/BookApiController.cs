using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookEditorWeb.Extensions;
using BookEditorWeb.Models;
using BookEditorWeb.Services;
using BookEditorWeb.Services.Validation;

namespace BookEditorWeb.Controllers
{
	public class BookApiController : ApiController
	{
		private readonly BookRepository _bookRepository = new BookRepository();
		private readonly BookValidator _bookValidator = new BookValidator();

		[HttpGet]
		public IEnumerable<Book> GetAll(string sidx, SortDirection sord)
		{
			IEnumerable<Book> books = _bookRepository.GetAll();

			return books.Sort(sidx, sord);
		}

		public HttpResponseMessage Add(AddBookRequest request)
		{
			var book = new Book
			{
				Title = request.Title,
				Authors = request.Authors,
				NumberOfPages = request.NumberOfPages,
				Publisher = request.Publisher,
				PublicationYear = request.PublicationYear,
				Isbn = request.Isbn
			};

			ValidationResult validationResult = _bookValidator.Validate(book);
			if (!validationResult.IsValid)
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, validationResult.Errors.First());

			_bookRepository.Add(book);

			return Request.CreateResponse(HttpStatusCode.OK);
		}

		[HttpPost]
		public void Remove(DeleteBookRequest request)
		{
			_bookRepository.Remove(request.BookId);
		}
	}
}