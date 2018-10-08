using System.Text.RegularExpressions;
using BookEditorWeb.Models;

namespace BookEditorWeb.Validation
{
	public class BookIsbnValidator : IBookPropertyValidator
	{
		static readonly Regex IsbnRegex = new Regex("^\\d\\d\\d-\\d-\\d\\d\\d\\d-\\d\\d\\d\\d-\\d$", RegexOptions.Compiled);

		public void Validate(Book book, ValidationResult validationResult)
		{
			string isbn = book.Isbn;

			if (string.IsNullOrWhiteSpace(isbn))
				return;

			if (isbn.Length != 17)
			{
				validationResult.AddError("ISBN: Длина поля должна быть 17 символов");
				return;
			}

			if (!IsbnRegex.IsMatch(isbn))
			{
				validationResult.AddError("ISBN: Неверный формат (xxx-x-xxxx-xxxx-x, где x - число)");
				return;
			}

			if (!ValidCheckSum(isbn))
				validationResult.AddError("ISBN: Неверная контрольная сумма");
		}

		private static bool ValidCheckSum(string isbn)
		{
			int[] isbnNumbers =
			{
				CharToInt(isbn, 0),
				CharToInt(isbn, 1),
				CharToInt(isbn, 2),
				CharToInt(isbn, 4),
				CharToInt(isbn, 6),
				CharToInt(isbn, 7),
				CharToInt(isbn, 8),
				CharToInt(isbn, 9),
				CharToInt(isbn, 11),
				CharToInt(isbn, 12),
				CharToInt(isbn, 13),
				CharToInt(isbn, 14),
				CharToInt(isbn, 16)
			};

			var checkSum = 0;

			for (int i = 1; i <= isbnNumbers.Length; i++)
			{
				int isbnNumber = isbnNumbers[i - 1];

				if (i % 2 == 0)
					checkSum += 3 * isbnNumber;
				else
					checkSum += isbnNumber;
			}

			return checkSum % 10 == 0;
		}

		private static int CharToInt(string isbn, int index)
		{
			return (int) char.GetNumericValue(isbn[index]);
		}

		public int Order => 6;
	}
}