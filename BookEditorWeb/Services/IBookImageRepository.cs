using BookEditorWeb.Models;

namespace BookEditorWeb.Services
{
	public interface IBookImageRepository
	{
		BookImage GetById(int id);
		BookImage Save(BookImage bookImage);
		void Remove(int id);
	}
}