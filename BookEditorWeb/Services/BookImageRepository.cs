using System.Collections.Generic;
using BookEditorWeb.Models;

namespace BookEditorWeb.Services
{
	public class BookImageRepository
	{
		private static readonly IDictionary<int, BookImage> BookImages = new Dictionary<int, BookImage>();
		private static readonly object SyncRoot = new object();
		private static int _currentId = 1;

		public BookImage GetById(int id)
		{
			lock (SyncRoot)
			{
				return BookImages[id];
			}
		}

		public BookImage Save(BookImage bookImage)
		{
			lock (SyncRoot)
			{
				if (bookImage.Id == 0)
				{
					bookImage.Id = _currentId;
					_currentId++;
				}

				BookImages[bookImage.Id] = bookImage;
			}

			return bookImage;
		}
	}
}