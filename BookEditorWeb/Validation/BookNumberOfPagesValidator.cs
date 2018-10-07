using BookEditorWeb.Models;

namespace BookEditorWeb.Validation
{
	public class BookNumberOfPagesValidator : IBookPropertyValidator
	{
		public void Validate(Book book, ValidationResult validationResult)
		{
			if (book.NumberOfPages <= 0 || book.NumberOfPages > 10000)
				validationResult.AddError("Количество страниц: Поле должно быть больше 0 и не более 10000");
		}

		public int Order => 3;
	}
}