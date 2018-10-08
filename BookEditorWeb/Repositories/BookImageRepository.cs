using System.Collections.Generic;
using BookEditorWeb.Models;
using BookEditorWeb.Services;

namespace BookEditorWeb.Repositories
{
	public class BookImageRepository : IBookImageRepository
	{
		private readonly IDictionary<int, BookImage> _bookImages = new Dictionary<int, BookImage>();
		private readonly object _syncRoot = new object();
		private int _currentId = 1;

		public BookImage GetById(int id)
		{
			lock (_syncRoot)
			{
				return _bookImages[id];
			}
		}

		public BookImage Save(BookImage bookImage)
		{
			lock (_syncRoot)
			{
				if (bookImage.BookImageId == 0)
				{
					bookImage.BookImageId = _currentId;
					_currentId++;
				}

				_bookImages[bookImage.BookImageId] = bookImage;
			}

			return bookImage;
		}

		public void Remove(int id)
		{
			lock (_syncRoot)
			{
				_bookImages.Remove(id);
			}
		}
	}
}