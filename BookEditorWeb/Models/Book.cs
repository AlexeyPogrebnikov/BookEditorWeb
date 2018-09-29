using System.Collections.Generic;

namespace BookEditorWeb.Models
{
	public class Book
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public IEnumerable<Author> Authors { get; set; }

		public int NumberOfPages { get; set; }

		public string Publisher { get; set; }

		public int? PublicationYear { get; set; }

		public string Isbn { get; set; }

		public string Image { get; set; }
	}
}