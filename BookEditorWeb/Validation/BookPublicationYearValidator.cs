using BookEditorWeb.Models;

namespace BookEditorWeb.Validation
{
	public class BookPublicationYearValidator : IBookPropertyValidator
	{
		public void Validate(Book book, ValidationResult validationResult)
		{
			if (!book.PublicationYear.HasValue)
				return;

			if (book.PublicationYear < 1800)
				validationResult.AddError("Год публикации: Поле должно быть не меньше 1800");
		}

		public int Order => 5;
	}
}