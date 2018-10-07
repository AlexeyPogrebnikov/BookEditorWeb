using BookEditorWeb.Models;

namespace BookEditorWeb.Repositories
{
	public interface IBookImageRepository
	{
		BookImage GetById(int id);
		BookImage Save(BookImage bookImage);
		void Remove(int id);
	}
}