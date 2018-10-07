using BookEditorWeb.Models;

namespace BookEditorWeb.Validation
{
	public class BookValidator : IBookValidator
	{
		public ValidationResult Validate(Book book)
		{
			var validationResult = new ValidationResult();

			ValidateTitle(book, validationResult);

			return validationResult;
		}

		private static void ValidateTitle(Book book, ValidationResult validationResult)
		{
			if (string.IsNullOrWhiteSpace(book.Title))
			{
				validationResult.AddError("Заголовок: Поле является обязательным");
				return;
			}

			if (book.Title.Length > 30)
				validationResult.AddError("Заголовок: Поле должно быть не более 30 символов");
		}
	}
}