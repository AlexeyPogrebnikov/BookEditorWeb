"use strict";

$(function() {
	function setSorting(jqgridOptions) {
		const sortname = localStorage.getItem("sortname");
		if (sortname) {
			jqgridOptions.sortname = sortname;
		}
		const sortorder = localStorage.getItem("sortorder");
		if (sortorder) {
			jqgridOptions.sortorder = sortorder;
		}
	}

	function guid() {
		function s4() {
			return Math.floor((1 + Math.random()) * 0x10000)
				.toString(16)
				.substring(1);
		}

		return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
	}

	function isInteger(str) {
		return /^\+?(0|[1-9]\d*)$/.test(str);
	}

	function isEmptyOrWhiteSpace(str) {
		return str == null || str.match(/^ *$/) !== null;
	}

	function validateAuthors(value) {
		for (let i = 0; i < value.length; i++) {
			if (isEmptyOrWhiteSpace(value[i].FirstName)) {
				return [false, "Список авторов: Имя автора обязательно для заполнения"];
			}
			if (isEmptyOrWhiteSpace(value[i].LastName)) {
				return [false, "Список авторов: Фамилия автора обязательна для заполнения"];
			}
		}
		return [true, ""];
	}

	function validateNumberOfPages(value) {
		if (!isInteger(value)) {
			return [false, "Количество страниц: Поле должно быть целым числом"];
		}
		const numberOfPages = parseInt(value);
		if (numberOfPages <= 0 || numberOfPages > 10000) {
			return [false, "Количество страниц: Поле должно быть больше 0 и не более 10000"];
		}
		return [true, ""];
	}

	function validatePublicationYear(value) {
		if (isEmptyOrWhiteSpace(value)) {
			return [true, ""];
		}

		if (!isInteger(value)) {
			return [false, "Год публикации: Поле должно быть целым числом"];
		}
		const publicationYear = parseInt(value);
		if (publicationYear < 1800) {
			return [false, "Год публикации: Поле должно быть не меньше 1800"];
		}
		return [true, ""];
	}

	function isValidIsbnCheckSum(isbn) {
		let isbnNumbers = [
			new Number(isbn[0]),
			new Number(isbn[1]),
			new Number(isbn[2]),
			new Number(isbn[4]),
			new Number(isbn[6]),
			new Number(isbn[7]),
			new Number(isbn[8]),
			new Number(isbn[9]),
			new Number(isbn[11]),
			new Number(isbn[12]),
			new Number(isbn[13]),
			new Number(isbn[14]),
			new Number(isbn[16])
		];

		var checkSum = 0;

		for (let i = 1; i <= isbnNumbers.length; i++) {
			let isbnNumber = isbnNumbers[i - 1];

			if (i % 2 === 0)
				checkSum += 3 * isbnNumber;
			else
				checkSum += isbnNumber;
		}

		return checkSum % 10 === 0;
	}

	function validateIsbn(value) {
		if (isEmptyOrWhiteSpace(value)) {
			return [true, ""];
		}

		if (value.length !== 17) {
			return [false, "ISBN: Длина поля должна быть 17 символов"];
		}

		let testFormat = /^\d\d\d-\d-\d\d\d\d-\d\d\d\d-\d$/.test(value);
		if (!testFormat) {
			return [false, "ISBN: Неверный формат (xxx-x-xxxx-xxxx-x, где x - число)"];
		}

		if (!isValidIsbnCheckSum(value)) {
			return [false, "ISBN: Неверная контрольная сумма"];
		}

		return [true, ""];
	}

	function addBookErrorHandler(data) {
		return data.responseJSON.Message;
	}

	function addAuthorRow(container, addAfter, author) {
		const html = `
<div class="author-editor-row">
	<div class="author-full-name">
		<span class="author-first-name-caption">Имя:</span>
		<input type="text" class="author-first-name FormElement ui-widget-content ui-corner-all" maxlength="20" />
		<span class="author-last-name-caption">Фамилия:</span>
		<input type="text" class="author-last-name FormElement ui-widget-content ui-corner-all" maxlength="20" />
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
		if (addAfter) {
			addAfter.after(html);
		} else {
			container.append(html);
		}
	}

	$(document).on("click",
		".add-author",
		function() {
			addAuthorRow($("#author-editor"), $(this).parent());
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

	function authorsJsonFormatter(cellvalue) {
		return JSON.stringify(cellvalue);
	}

	function imageFormatter(cellvalue) {
		if (cellvalue) {
			let uuid = guid();
			return `<img src="/BookImage/GetById/${cellvalue}?${uuid}" class="book-grid-image" />`;
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
		let authors = [
			{
				FirstName: "",
				LastName: ""
			}
		];

		if (options.rowId !== "_empty") {
			const authorsJson = $("#book-list").getCell(options.rowId, "Authors_Json");
			authors = JSON.parse(authorsJson);
		}

		authors.forEach(function(author) {
			addAuthorRow(element.find("#author-editor"), null, author);
		});

		return element;
	}

	function getAuthors(elem) {
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

	function viewBookImageInEditor(imageId, imageEditor) {
		imageEditor.find("#book-image-id").val(imageId);
		let uuid = guid();
		const html = `<img src="/BookImage/GetById/${imageId}?${uuid}" />`;
		imageEditor.find("#book-image-canvas").html(html);
	}

	$(document).on("change",
		"#book-image-file",
		function() {
			const formData = new FormData();
			formData.append("imageId", $("#book-image-id").val());
			formData.append("image", $("#book-image-file")[0].files[0]);
			$.ajax({
				url: "/BookImage/Upload",
				data: formData,
				type: "POST",
				contentType: false,
				processData: false,
				success: function(data) {
					viewBookImageInEditor(data, $("#book-image-editor"));
				}
			});
		});

	function createImageUploader(value, options) {
		const html = `
<div>
	<div id="book-image-editor">
		<input type="file" id="book-image-file" />
		<input type="hidden" id="book-image-id" />
		<div id="book-image-canvas"></div>
	</div>
</div>
`;
		if (options.rowId !== "_empty") {
			const imageId = $("#book-list").getCell(options.rowId, "ImageId_Json");
			const element = $(html);
			viewBookImageInEditor(imageId, element);
			return element;
		}
		return html;
	}

	function getImageId(elem) {
		if (elem.length === 0)
			return "";

		return $(elem).find("#book-image-id").val();
	}

	const jqgridOptions = {
		url: "/api/BookApi/GetAll",
		datatype: "json",
		colNames: [
			"BookId", "Заголовок", "Список авторов", "Authors_Json", "Количество страниц", "Название издательства",
			"Год публикации",
			"ISBN", "Изображение", "ImageId_Json"
		],
		colModel: [
			{ name: "BookId", hidden: true, editable: true, editrules: { edithidden: false } },
			{
				name: "Title",
				index: "Title",
				editable: true,
				edittype: "text",
				editoptions: { maxlength: 30 },
				editrules: { required: true }
			},
			{
				name: "Authors",
				formatter: authorsFormatter,
				editable: true,
				edittype: "custom",
				editoptions: { custom_element: createAuthorEditor, custom_value: getAuthors },
				sortable: false,
				editrules: { custom: true, custom_func: validateAuthors }
			},
			{
				name: "Authors_Json",
				jsonmap: "Authors",
				hidden: true,
				formatter: authorsJsonFormatter
			},
			{
				name: "NumberOfPages",
				editable: true,
				align: "center",
				sortable: false,
				editrules: { required: true, custom: true, custom_func: validateNumberOfPages }
			},
			{ name: "Publisher", editable: true, sortable: false, editoptions: { maxlength: 30 } },
			{
				name: "PublicationYear",
				index: "PublicationYear",
				editable: true,
				align: "center",
				editrules: { custom: true, custom_func: validatePublicationYear }
			},
			{
				name: "Isbn",
				editable: true,
				align: "center",
				sortable: false,
				editrules: { custom: true, custom_func: validateIsbn }
			},
			{
				name: "ImageId",
				editable: true,
				formatter: imageFormatter,
				edittype: "custom",
				editoptions: { custom_element: createImageUploader, custom_value: getImageId },
				sortable: false,
				width: 250
			},
			{
				name: "ImageId_Json",
				jsonmap: "ImageId",
				hidden: true
			}
		],
		pager: "#book-list-navigator",
		height: "auto",
		onSortCol: function(index, iCol, sortorder) {
			localStorage.setItem("sortname", index);
			localStorage.setItem("sortorder", sortorder);
		}
	};
	setSorting(jqgridOptions);

	$("#book-list").jqGrid(jqgridOptions)
		.navGrid("#book-list-navigator",
			{ view: true, del: true, search: false },
			{
				url: "/api/BookApi/Save",
				width: 650,
				modal: true,
				left: 100,
				top: 100,
				viewPagerButtons: false,
				closeAfterEdit: true
			},
			{
				url: "/api/BookApi/Save",
				width: 650,
				modal: true,
				left: 100,
				top: 100,
				recreateForm: true,
				errorTextFormat: addBookErrorHandler,
				closeAfterAdd: true
			},
			{
				url: "/api/BookApi/Remove",
				onclickSubmit: function(params, index) {
					const bookId = $("#book-list").getCell(index, "BookId");
					return { bookId: bookId };
				}
			});
});