using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BookEditorWeb.Models;

namespace BookEditorWeb.Extensions
{
	public static class BookSortExtensions
	{
		public static IEnumerable<Book> Sort(this IEnumerable<Book> books, string propertyName, SortDirection sortDirection)
		{
			if (string.IsNullOrEmpty(propertyName))
				return books;

			PropertyInfo property = typeof(Book).GetProperty(propertyName);
			Func<Book, object> keySelector = book => property.GetValue(book, null);

			if (sortDirection == SortDirection.Desc)
				return books.OrderByDescending(keySelector);

			return books.OrderBy(keySelector);
		}
	}
}