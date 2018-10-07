using BookEditorWeb.Models;
using BookEditorWeb.Services;

namespace BookEditorWeb.Sample
{
	public class SampleDataGenerator : ISampleDataGenerator
	{
		private readonly IBookImageRepository _bookImageRepository;
		private readonly IBookRepository _bookRepository;
		private readonly IEmbeddedResourceService _embeddedResourceService;

		public SampleDataGenerator(IBookRepository bookRepository, IBookImageRepository bookImageRepository, IEmbeddedResourceService embeddedResourceService)
		{
			_bookRepository = bookRepository;
			_bookImageRepository = bookImageRepository;
			_embeddedResourceService = embeddedResourceService;
		}

		public void Generate()
		{
			AddAtlasShruggedBook();
			AddRoadsidePicnicBook();
		}

		private void AddAtlasShruggedBook()
		{
			byte[] imageContent = _embeddedResourceService.GetBinaryContent("BookEditorWeb.Sample.atlas_shrugged.jpg");

			BookImage bookImage = _bookImageRepository.Save(new BookImage
			{
				Content = imageContent
			});

			_bookRepository.Add(new Book
			{
				Title = "Атлант расправил плечи",
				Authors = new[]
				{
					new Author
					{
						FirstName = "Айн",
						LastName = "Рэнд"
					}
				},
				NumberOfPages = 1131,
				Publisher = "Альпина Паблишер",
				PublicationYear = 2018,
				Isbn = "978-5-9614-4579-4",
				ImageId = bookImage.Id
			});
		}

		private void AddRoadsidePicnicBook()
		{
			_bookRepository.Add(new Book
			{
				Title = "Пикник на обочине",
				Authors = new[]
				{
					new Author
					{
						FirstName = "Аркадий",
						LastName = "Стругацкий"
					},
					new Author
					{
						FirstName = "Борис",
						LastName = "Стругацкий"
					}
				},
				NumberOfPages = 256,
				Publisher = "АСТ, Neoclassic",
				PublicationYear = 2007,
				Isbn = "978-5-17-045438-9"
			});
		}
	}
}