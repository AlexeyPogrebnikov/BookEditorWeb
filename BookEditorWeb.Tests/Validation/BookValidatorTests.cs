using System.Linq;
using BookEditorWeb.Models;
using BookEditorWeb.Validation;
using NUnit.Framework;

namespace BookEditorWeb.Tests.Validation
{
	[TestFixture]
	public class BookValidatorTests
	{
		[Test]
		public void Validate_Errors_contains_one_item_if_Title_is_null()
		{
			// arrange
			BookValidator validator = new BookValidator(new IBookPropertyValidator[] { new BookTitleValidator(), new BookAuthorsValidator() });

			var book = new Book { Title = null };

			// act
			ValidationResult validationResult = validator.Validate(book);

			// assert
			string[] errors = validationResult.Errors.ToArray();
			Assert.AreEqual(1, errors.Length);
			Assert.AreEqual("Заголовок: Поле является обязательным", errors[0]);
		}

		[Test]
		public void Validate_IsValid_is_true_if_Title_is_assigned()
		{
			// arrange
			BookValidator validator = new BookValidator(new[] { new BookTitleValidator() });

			var book = new Book { Title = "Design Patterns" };

			// act
			ValidationResult validationResult = validator.Validate(book);

			// assert
			Assert.IsTrue(validationResult.IsValid);
		}
	}
}