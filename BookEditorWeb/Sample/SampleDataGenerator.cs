using BookEditorWeb.Models;
using BookEditorWeb.Repositories;
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
			AddSecretsOfTheJavaScriptNinjaBook();
			AddCodeCompleteBook();
			AddDesignPatternsBook();
		}

		private void AddAtlasShruggedBook()
		{
			BookImage bookImage = AddBookImage("atlas_shrugged.jpg");

			_bookRepository.Save(new Book
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
				ImageId = bookImage.BookImageId
			});
		}

		private void AddRoadsidePicnicBook()
		{
			_bookRepository.Save(new Book
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

		private void AddSecretsOfTheJavaScriptNinjaBook()
		{
			BookImage bookImage = AddBookImage("secrets_of_the_javascript_ninja.jpg");

			_bookRepository.Save(new Book
			{
				Title = "Секреты JavaScript ниндзя",
				Authors = new[]
				{
					new Author
					{
						FirstName = "Джон",
						LastName = "Резиг"
					},
					new Author
					{
						FirstName = "Беэр",
						LastName = "Бибо"
					}
				},
				NumberOfPages = 416,
				Publisher = "Вильямс",
				PublicationYear = 2016,
				Isbn = "978-5-8459-1959-5",
				ImageId = bookImage.BookImageId
			});
		}

		private void AddCodeCompleteBook()
		{
			BookImage bookImage = AddBookImage("code_complete.jpg");

			_bookRepository.Save(new Book
			{
				Title = "Совершенный код",
				Authors = new[]
				{
					new Author
					{
						FirstName = "Стив",
						LastName = "Макконнелл"
					}
				},
				NumberOfPages = 896,
				Publisher = "Microsoft Press",
				PublicationYear = 2017,
				Isbn = "978-5-7502-0064-1",
				ImageId = bookImage.BookImageId
			});
		}

		private void AddDesignPatternsBook()
		{
			BookImage bookImage = AddBookImage("design_patterns.jpg");

			_bookRepository.Save(new Book
			{
				Title = "Design Patterns",
				Authors = new[]
				{
					new Author
					{
						FirstName = "Эрих",
						LastName = "Гамма"
					},
					new Author
					{
						FirstName = "Ричард",
						LastName = "Хелм"
					},
					new Author
					{
						FirstName = "Ральф",
						LastName = "Джонсон"
					},
					new Author
					{
						FirstName = "Джон",
						LastName = "Влиссидес"
					}
				},
				NumberOfPages = 416,
				Publisher = "Addison Wesley",
				PublicationYear = 1994,
				Isbn = "978-0-2016-3361-0",
				ImageId = bookImage.BookImageId
			});
		}

		private BookImage AddBookImage(string name)
		{
			byte[] imageContent = _embeddedResourceService.GetBinaryContent($"BookEditorWeb.Sample.{name}");

			BookImage bookImage = _bookImageRepository.Save(new BookImage
			{
				Content = imageContent
			});

			return bookImage;
		}
	}
}