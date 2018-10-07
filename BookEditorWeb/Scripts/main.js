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

	function addBookErrorHandler(data) {
		return data.responseJSON.Message;
	}

	function addAuthorRow(container, author) {
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

	function authorsJsonFormatter(cellvalue) {
		return JSON.stringify(cellvalue);
	}

	function imageFormatter(cellvalue) {
		if (cellvalue) {
			return `<img src="/BookImage/GetById/${cellvalue}" class="book-grid-image" />`;
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
			addAuthorRow(element.find("#author-editor"), author);
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
		const html = `<img src="/BookImage/GetById/${imageId}" />`;
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
			"Id", "Заголовок", "Список авторов", "Authors_Json", "Количество страниц", "Название издательства",
			"Год публикации",
			"ISBN", "Изображение", "ImageId_Json"
		],
		colModel: [
			{ name: "Id", hidden: true },
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
			{ name: "Isbn", editable: true, align: "center", sortable: false },
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
					const bookId = $("#book-list").getCell(index, "Id");
					return { bookId: bookId };
				}
			});
});