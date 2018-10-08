using System.Web;
using System.Web.Mvc;
using BookEditorWeb.Models;
using BookEditorWeb.Repositories;
using BookEditorWeb.Services;
using Newtonsoft.Json;

namespace BookEditorWeb.Controllers
{
	public class BookImageController : Controller
	{
		private readonly IBookImageRepository _bookImageRepository;

		public BookImageController(IBookImageRepository bookImageRepository)
		{
			_bookImageRepository = bookImageRepository;
		}

		public ActionResult GetById(int id)
		{
			BookImage bookImage = _bookImageRepository.GetById(id);

			return File(bookImage.Content, "image/jpg");
		}

		[HttpPost]
		public ContentResult Upload(int? imageId, HttpPostedFileBase image)
		{
			var bytes = new byte[image.ContentLength];

			image.InputStream.Read(bytes, 0, image.ContentLength);

			BookImage bookImage = _bookImageRepository.Save(new BookImage
			{
				BookImageId = imageId.GetValueOrDefault(),
				Content = bytes
			});

			return Content(JsonConvert.SerializeObject(bookImage.BookImageId));
		}
	}
}