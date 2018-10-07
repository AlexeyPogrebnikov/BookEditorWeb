using System.Linq;
using BookEditorWeb.Models;
using BookEditorWeb.Validation;
using NUnit.Framework;

namespace BookEditorWeb.Tests.Validation
{
	[TestFixture]
	public class BookAuthorsValidatorTests
	{
		private BookAuthorsValidator _validator = new BookAuthorsValidator();

		[SetUp]
		public void SetUp()
		{
			_validator = new BookAuthorsValidator();
		}

		[Test]
		public void Validate_Errors_contains_one_item_if_Authors_is_null()
		{
			// arrange
			var validationResult = new ValidationResult();

			var book = new Book { Authors = null };

			// act
			_validator.Validate(book, validationResult);

			// assert
			string[] errors = validationResult.Errors.ToArray();
			Assert.AreEqual(1, errors.Length);
			Assert.AreEqual("Список авторов: книга должна содержать хотя бы одного автора", errors[0]);
		}

		[Test]
		public void Validate_IsValid_is_true_if_Authors_contains_one_item()
		{
			// arrange
			var validationResult = new ValidationResult();

			var book = new Book
			{
				Authors = new[]
				{
					new Author
					{
						FirstName = "Ivan",
						LastName = "Petrov"
					}
				}
			};

			// act
			_validator.Validate(book, validationResult);

			// assert
			Assert.IsTrue(validationResult.IsValid);
		}

		[Test]
		public void Validate_Errors_contains_one_item_if_Authors_is_empty()
		{
			// arrange
			var validationResult = new ValidationResult();

			var book = new Book { Authors = new Author[0] };

			// act
			_validator.Validate(book, validationResult);

			// assert
			string[] errors = validationResult.Errors.ToArray();
			Assert.AreEqual(1, errors.Length);
			Assert.AreEqual("Список авторов: книга должна содержать хотя бы одного автора", errors[0]);
		}

		[Test]
		public void Validate_IsValid_is_false_if_FirstName_is_empty()
		{
			// arrange
			var validationResult = new ValidationResult();

			var book = new Book
			{
				Authors = new[]
				{
					new Author
					{
						FirstName = string.Empty,
						LastName = "Petrov"
					}
				}
			};

			// act
			_validator.Validate(book, validationResult);

			// assert
			string[] errors = validationResult.Errors.ToArray();
			Assert.AreEqual(1, errors.Length);
			Assert.AreEqual("Список авторов: Имя автора обязательно для заполнения", errors[0]);
		}

		[Test]
		public void Validate_IsValid_is_false_if_LastName_is_empty()
		{
			// arrange
			var validationResult = new ValidationResult();

			var book = new Book
			{
				Authors = new[]
				{
					new Author
					{
						FirstName = "Sidor",
						LastName = string.Empty
					}
				}
			};

			// act
			_validator.Validate(book, validationResult);

			// assert
			string[] errors = validationResult.Errors.ToArray();
			Assert.AreEqual(1, errors.Length);
			Assert.AreEqual("Список авторов: Фамилия автора обязательна для заполнения", errors[0]);
		}

		[Test]
		public void Validate_IsValid_is_false_if_length_of_FirstName_is_21()
		{
			// arrange
			var validationResult = new ValidationResult();

			var book = new Book
			{
				Authors = new[]
				{
					new Author
					{
						FirstName = new string('f', 21),
						LastName = "Petrov"
					}
				}
			};

			// act
			_validator.Validate(book, validationResult);

			// assert
			string[] errors = validationResult.Errors.ToArray();
			Assert.AreEqual(1, errors.Length);
			Assert.AreEqual("Список авторов: Имя автора должно быть не более 20 символов", errors[0]);
		}

		[Test]
		public void Validate_IsValid_is_false_if_length_of_LastName_is_21()
		{
			// arrange
			var validationResult = new ValidationResult();

			var book = new Book
			{
				Authors = new[]
				{
					new Author
					{
						FirstName = "Sidor",
						LastName = new string('l', 21)
					}
				}
			};

			// act
			_validator.Validate(book, validationResult);

			// assert
			string[] errors = validationResult.Errors.ToArray();
			Assert.AreEqual(1, errors.Length);
			Assert.AreEqual("Список авторов: Фамилия автора должна быть не более 20 символов", errors[0]);
		}
	}
}