using BookEditorWeb.Models;

namespace BookEditorWeb.Validation
{
	public class BookPublisherValidator : IBookPropertyValidator
	{
		public void Validate(Book book, ValidationResult validationResult)
		{
			if (string.IsNullOrWhiteSpace(book.Publisher))
				return;

			if (book.Publisher.Length > 30)
				validationResult.AddError("Название издательства: Поле должно быть не более 30 символов");
		}

		public int Order => 4;
	}
}