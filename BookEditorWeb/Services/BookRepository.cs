using System.Collections.Generic;
using System.Linq;
using BookEditorWeb.Models;

namespace BookEditorWeb.Services
{
	public class BookRepository : IBookRepository
	{
		private readonly IDictionary<int, Book> _books = new Dictionary<int, Book>();
		private readonly object _syncRoot = new object();
		private int _currentId = 1;

		public void Add(Book book)
		{
			lock (_syncRoot)
			{
				book.Id = _currentId;
				_books[book.Id] = book;
				_currentId++;
			}
		}

		public IEnumerable<Book> GetAll()
		{
			lock (_syncRoot)
			{
				return _books.Values.ToArray();
			}
		}

		public void Remove(int id)
		{
			lock (_syncRoot)
			{
				_books.Remove(id);
			}
		}

		public Book GetById(int id)
		{
			lock (_syncRoot)
			{
				return _books[id];
			}
		}
	}
}