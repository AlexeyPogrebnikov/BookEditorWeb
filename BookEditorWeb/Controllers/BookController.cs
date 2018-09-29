using System.Web.Mvc;

namespace BookEditorWeb.Controllers
{
	public class BookController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}