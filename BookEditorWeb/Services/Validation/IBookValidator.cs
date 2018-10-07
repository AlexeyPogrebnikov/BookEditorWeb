using BookEditorWeb.Models;

namespace BookEditorWeb.Services.Validation
{
	public interface IBookValidator
	{
		ValidationResult Validate(Book book);
	}
}