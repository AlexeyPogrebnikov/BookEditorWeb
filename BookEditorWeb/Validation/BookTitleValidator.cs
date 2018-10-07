using BookEditorWeb.Models;

namespace BookEditorWeb.Validation
{
	public class BookTitleValidator : IBookPropertyValidator
	{
		public void Validate(Book book, ValidationResult validationResult)
		{
			if (string.IsNullOrWhiteSpace(book.Title))
			{
				validationResult.AddError("Заголовок: Поле является обязательным");
				return;
			}

			if (book.Title.Length > 30)
				validationResult.AddError("Заголовок: Поле должно быть не более 30 символов");
		}

		public int Order => 1;
	}
}