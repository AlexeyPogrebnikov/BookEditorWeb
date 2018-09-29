$(function() {
	function authorsFormatter(cellvalue) {
		var authors = "";
		$.each(cellvalue, function(index, author) {
			authors += author.FirstName + " " + author.LastName + "<br>";
		});
		return authors;
	}

	function imageFormatter(cellvalue) {
		return "<img src=\"data:image/jpeg;base64," + cellvalue + "\" />";
	}

	$("#book-list").jqGrid({
		url: "api/BookApi/GetAll",
		datatype: "json",
		colNames: ["Заголовок", "Список авторов", "Количество страниц", "Название издательства", "Год публикации", "ISBN", "Изображение"],
		colModel: [
			{ name: "Title", index: "Title" },
			{ name: "Authors", index: "Authors", formatter: authorsFormatter },
			{ name: "NumberOfPages", index: "NumberOfPages" },
			{ name: "Publisher", index: "Publisher" },
			{ name: "PublicationYear", index: "PublicationYear" },
			{ name: "Isbn", index: "Isbn" },
			{ name: "Image", index: "Image", formatter: imageFormatter }
		]
	});
});