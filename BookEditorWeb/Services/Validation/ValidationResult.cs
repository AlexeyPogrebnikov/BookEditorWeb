using System.Collections.Generic;
using System.Linq;

namespace BookEditorWeb.Services.Validation
{
	public class ValidationResult
	{
		private readonly List<string> _errors = new List<string>();

		public bool IsValid => !_errors.Any();

		public IEnumerable<string> Errors => _errors;

		public void AddError(string error)
		{
			_errors.Add(error);
		}
	}
}