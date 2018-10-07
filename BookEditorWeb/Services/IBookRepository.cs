using System.Collections.Generic;
using BookEditorWeb.Models;

namespace BookEditorWeb.Services
{
	public interface IBookRepository
	{
		Book GetById(int id);
		IEnumerable<Book> GetAll();
		void Save(Book book);
		void Remove(int id);
	}
}