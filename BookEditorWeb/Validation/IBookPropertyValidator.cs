using BookEditorWeb.Models;

namespace BookEditorWeb.Validation
{
	public interface IBookPropertyValidator
	{
		void Validate(Book book, ValidationResult validationResult);

		int Order { get; }
	}
}