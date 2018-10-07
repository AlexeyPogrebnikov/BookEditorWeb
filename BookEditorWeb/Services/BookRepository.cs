using System.Collections.Generic;
using System.Linq;
using BookEditorWeb.Models;

namespace BookEditorWeb.Services
{
	public class BookRepository
	{
		private static readonly IDictionary<int, Book> Books = new Dictionary<int, Book>();
		private static readonly object SyncRoot = new object();
		private static int _currentId = 1;

		public void Add(Book book)
		{
			lock (SyncRoot)
			{
				book.Id = _currentId;
				Books[book.Id] = book;
				_currentId++;
			}
		}

		public IEnumerable<Book> GetAll()
		{
			lock (SyncRoot)
			{
				return Books.Values.ToArray();
			}
		}

		public void Remove(int id)
		{
			lock (SyncRoot)
			{
				Books.Remove(id);
			}
		}

		public Book GetById(int id)
		{
			lock (SyncRoot)
			{
				return Books[id];
			}
		}
	}
}