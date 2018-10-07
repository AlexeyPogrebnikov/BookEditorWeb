using System.Collections.Generic;
using System.Linq;
using BookEditorWeb.Models;
using BookEditorWeb.Services;

namespace BookEditorWeb.Repositories
{
	public class BookRepository : IBookRepository
	{
		private readonly IDictionary<int, Book> _books = new Dictionary<int, Book>();
		private readonly object _syncRoot = new object();
		private int _currentId = 1;

		public Book GetById(int id)
		{
			lock (_syncRoot)
			{
				return _books[id];
			}
		}

		public IEnumerable<Book> GetAll()
		{
			lock (_syncRoot)
			{
				return _books.Values.ToArray();
			}
		}

		public void Save(Book book)
		{
			lock (_syncRoot)
			{
				if (book.Id == 0)
				{
					book.Id = _currentId;
					_currentId++;
				}

				_books[book.Id] = book;
			}
		}

		public void Remove(int id)
		{
			lock (_syncRoot)
			{
				_books.Remove(id);
			}
		}
	}
}