using System.Collections.Generic;
using System.Linq;
using BookEditorWeb.Models;

namespace BookEditorWeb.Validation
{
	public class BookValidator : IBookValidator
	{
		private readonly IEnumerable<IBookPropertyValidator> _propertyValidators;

		public BookValidator(IEnumerable<IBookPropertyValidator> propertyValidators)
		{
			_propertyValidators = propertyValidators
				.OrderBy(propertyValidator => propertyValidator.Order)
				.ToArray();
		}

		public ValidationResult Validate(Book book)
		{
			var validationResult = new ValidationResult();

			foreach (IBookPropertyValidator propertyValidator in _propertyValidators)
			{
				propertyValidator.Validate(book, validationResult);
				if (!validationResult.IsValid)
					return validationResult;
			}

			return validationResult;
		}
	}
}