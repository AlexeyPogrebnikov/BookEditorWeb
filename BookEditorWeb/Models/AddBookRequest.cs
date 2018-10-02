
using System.Collections.Generic;

namespace BookEditorWeb.Models
{
	public class AddBookRequest
	{
		public string Title { get; set; }

		public IEnumerable<Author> Authors { get; set; }
	}
}