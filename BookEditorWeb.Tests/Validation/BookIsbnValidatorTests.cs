using System.Linq;
using BookEditorWeb.Models;
using BookEditorWeb.Validation;
using NUnit.Framework;

namespace BookEditorWeb.Tests.Validation
{
	[TestFixture]
	public class BookIsbnValidatorTests
	{
		private BookIsbnValidator _validator;

		[SetUp]
		public void SetUp()
		{
			_validator = new BookIsbnValidator();
		}

		[Test]
		public void Validate_Errors_contains_one_item_if_length_of_Isbn_is_not_17()
		{
			// arrange
			var validationResult = new ValidationResult();

			var book = new Book { Isbn = "12" };

			// act
			_validator.Validate(book, validationResult);

			// assert
			string[] errors = validationResult.Errors.ToArray();
			Assert.AreEqual(1, errors.Length);
			Assert.AreEqual("Isbn: Длина поля должна быть 17 символов", errors[0]);
		}

		[Test]
		public void Validate_IsValid_is_true_if_length_of_Isbn_is_null()
		{
			// arrange
			var validationResult = new ValidationResult();

			var book = new Book { Isbn = null };

			// act
			_validator.Validate(book, validationResult);

			// assert
			Assert.IsTrue(validationResult.IsValid);
		}

		[Test]
		public void Validate_Errors_contains_one_item_if_Isbn_has_wrong_format()
		{
			// arrange
			var validationResult = new ValidationResult();

			var book = new Book { Isbn = "-78-5-9614-4579-4" };

			// act
			_validator.Validate(book, validationResult);

			// assert
			string[] errors = validationResult.Errors.ToArray();
			Assert.AreEqual(1, errors.Length);
			Assert.AreEqual("Isbn: Неверный формат (xxx-x-xxxx-xxxx-x, где x - число)", errors[0]);
		}

		[Test]
		public void Validate_IsValid_is_true_if_length_of_Isbn_is_valid()
		{
			// arrange
			var validationResult = new ValidationResult();

			var book = new Book { Isbn = "978-5-9614-4579-4" };

			// act
			_validator.Validate(book, validationResult);

			// assert
			Assert.IsTrue(validationResult.IsValid);
		}

		[Test]
		public void Validate_Errors_contains_one_item_if_Isbn_has_wrong_checksum()
		{
			// arrange
			var validationResult = new ValidationResult();

			var book = new Book { Isbn = "978-0-3064-0615-6" };

			// act
			_validator.Validate(book, validationResult);

			// assert
			string[] errors = validationResult.Errors.ToArray();
			Assert.AreEqual(1, errors.Length);
			Assert.AreEqual("Isbn: Неверная контрольная сумма", errors[0]);
		}
	}
}