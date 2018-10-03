﻿"use strict";

$(function() {
	function addAuthorRow(container, author) {
		const html = `
<div class="author-editor-row">
	<div class="author-full-name">
		<span class="author-first-name-caption">Имя:</span>
		<input type="text" class="author-first-name FormElement ui-widget-content ui-corner-all" />
		<span class="author-last-name-caption">Фамилия:</span>
		<input type="text" class="author-last-name FormElement ui-widget-content ui-corner-all" />
	</div>
	<button class="add-author">Добавить</button>
	<button class="remove-author">Удалить</button>
</div>
`;
		if (author) {
			const authorEditorRow = $(html);
			$(authorEditorRow).find(".author-first-name").val(author.FirstName);
			$(authorEditorRow).find(".author-last-name").val(author.LastName);
			container.append(authorEditorRow);
			return;
		}
		container.append(html);
	}

	$(document).on("click",
		".add-author",
		function() {
			addAuthorRow($("#author-editor"));
		});

	$(document).on("click",
		".remove-author",
		function() {
			const authorEditorRowCount = $(this).parent().parent().find(".author-editor-row").length;
			if (authorEditorRowCount > 1)
				$(this).parent().remove();
		});

	function authorsFormatter(cellvalue) {
		let authors = "";
		$.each(cellvalue,
			function(index, author) {
				authors += author.FirstName + " " + author.LastName + "<br>";
			});
		return authors;
	}

	function authorsRawFormatter(cellvalue) {
		return JSON.stringify(cellvalue);
	}

	function imageFormatter(cellvalue) {
		if (cellvalue) {
			return `<img src="data:image/jpeg;base64,${cellvalue}" />`;
		}
		return "";
	}

	function createAuthorEditor(value, options) {
		const html = `
<div>
	<div id="author-editor">
	</div>
</div>
			`;
		const element = $(html);
		const authorsRaw = $("#book-list").getCell(options.rowId, "Authors_Raw");
		const authors = JSON.parse(authorsRaw);
		authors.forEach(function(author) {
			addAuthorRow(element.find("#author-editor"), author);
		});
		return element;
	}

	function getAuthors(elem, operation, value) {
		if (elem.length === 0)
			return "";

		const authorFullNames = $(elem).find(".author-full-name");
		const authors = [];
		authorFullNames.each(function(index, authorFullName) {
			const firstName = $(authorFullName).find(".author-first-name").val();
			const lastName = $(authorFullName).find(".author-last-name").val();
			authors.push({
				FirstName: firstName,
				LastName: lastName
			});
		});
		return authors;
	}

	$("#book-list").jqGrid({
			url: "/api/BookApi/GetAll",
			datatype: "json",
			colNames: [
				"Id", "Заголовок", "Список авторов", "Authors_Raw", "Количество страниц", "Название издательства",
				"Год публикации",
				"ISBN", "Изображение"
			],
			colModel: [
				{ name: "Id", index: "Id", hidden: true },
				{ name: "Title", index: "Title", editable: true, edittype: "text", editoptions: { maxlength: 30 } },
				{
					name: "Authors",
					index: "Authors",
					formatter: authorsFormatter,
					editable: true,
					edittype: "custom",
					editoptions: { custom_element: createAuthorEditor, custom_value: getAuthors }
				},
				{
					name: "Authors_Raw",
					jsonmap: "Authors",
					hidden: true,
					formatter: authorsRawFormatter
				},
				{ name: "NumberOfPages", index: "NumberOfPages", align: "center" },
				{ name: "Publisher", index: "Publisher" },
				{ name: "PublicationYear", index: "PublicationYear", align: "center" },
				{ name: "Isbn", index: "Isbn", align: "center" },
				{ name: "Image", index: "Image", formatter: imageFormatter }
			],
			pager: "#book-list-navigator",
			height: "auto"
		})
		.navGrid("#book-list-navigator",
			{ view: true, del: true },
			{
				width: 650,
				modal: true,
				left: 100,
				top: 100,
				viewPagerButtons: false
			},
			{
				url: "/api/BookApi/Add",
				width: 650,
				modal: true,
				left: 100,
				top: 100,
				recreateForm: true
			},
			{
				url: "/api/BookApi/Remove",
				onclickSubmit: function(params, index) {
					const bookId = $("#book-list").getCell(index, "Id");
					return { bookId: bookId };
				}
			});
});