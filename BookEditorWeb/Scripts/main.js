$(function() {
	function authorsFormatter(cellvalue) {
		var authors = "";
		$.each(cellvalue, function(index, author) {
			authors += author.FirstName + " " + author.LastName + "<br>";
		});
		return authors;
	}

	function imageFormatter(cellvalue) {
		if (cellvalue) {
			return "<img src=\"data:image/jpeg;base64," + cellvalue + "\" />";
		}
		return "";
	}

	$("#book-list").jqGrid({
			url: "/api/BookApi/GetAll",
			datatype: "json",
			colNames: ["Id", "Заголовок", "Список авторов", "Количество страниц", "Название издательства", "Год публикации", "ISBN", "Изображение"],
			colModel: [
				{ name: "Id", index: "Id", hidden: true },
				{ name: "Title", index: "Title" },
				{ name: "Authors", index: "Authors", formatter: authorsFormatter },
				{ name: "NumberOfPages", index: "NumberOfPages" },
				{ name: "Publisher", index: "Publisher" },
				{ name: "PublicationYear", index: "PublicationYear" },
				{ name: "Isbn", index: "Isbn" },
				{ name: "Image", index: "Image", formatter: imageFormatter }
			],
			pager: "#book-list-navigator",
			height: 'auto'
		})
		.navGrid("#book-list-navigator", { view: true, del: true },
		{},
		{},
		{
			url: "/api/BookApi/Remove",
			onclickSubmit: function(params, index) {
				var bookId = $("#book-list").getCell(index, 'Id');
				return { bookId: bookId };
			}
		});
});