using System.Collections.Generic;
using BookEditorWeb.Models;

namespace BookEditorWeb.Services
{
	public interface IBookRepository
	{
		void Add(Book book);
		IEnumerable<Book> GetAll();
		void Remove(int id);
		Book GetById(int id);
	}
}