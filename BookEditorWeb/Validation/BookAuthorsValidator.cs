using System.Linq;
using BookEditorWeb.Models;

namespace BookEditorWeb.Validation
{
	public class BookAuthorsValidator : IBookPropertyValidator
	{
		public void Validate(Book book, ValidationResult validationResult)
		{
			if (book.Authors == null || !book.Authors.Any())
			{
				validationResult.AddError("Список авторов: книга должна содержать хотя бы одного автора");
				return;
			}

			foreach (Author author in book.Authors)
			{
				if (string.IsNullOrWhiteSpace(author.FirstName))
				{
					validationResult.AddError("Список авторов: Имя автора обязательно для заполнения");
					return;
				}

				if (author.FirstName.Length > 20)
				{
					validationResult.AddError("Список авторов: Имя автора должно быть не более 20 символов");
					return;
				}

				if (string.IsNullOrWhiteSpace(author.LastName))
				{
					validationResult.AddError("Список авторов: Фамилия автора обязательна для заполнения");
					return;
				}

				if (author.LastName.Length > 20)
				{
					validationResult.AddError("Список авторов: Фамилия автора должна быть не более 20 символов");
					return;
				}
			}
		}

		public int Order => 2;
	}
}