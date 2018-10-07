using BookEditorWeb.Models;

namespace BookEditorWeb.Validation
{
	public interface IBookValidator
	{
		ValidationResult Validate(Book book);
	}
}