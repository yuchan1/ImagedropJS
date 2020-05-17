// Members

// Grobal
var tableData = null;
var table = null;
var rowPosition = null;

// Index
(function index() {
    var getsUrl = "/Members/Gets";

    $("#enable").prop("checked", true);
    $("#all").prop("checked", false);

    $.getJSON(getsUrl, null, function (response) {
        tableData = response;

        table = createTable();
        table.setData(tableData);
    });
})();

// Create table
function createTable() {
    var displayNames = getDisplayNames();

    var modalUrl = "/Members/Modal";
    var editUrl = "/Members/Edit";

    return new Tabulator("#table", {
        data: tableData,
        headerSort: false,
        height: getCalculateHeight(),
        index: "Order",
        // layout: "fitDataStretch",
        movableRows: true,
        reactiveData: true,
        selectable: 1,
        tooltips: true,
        tooltipsHeader: true,

        columns: [
            { rowHandle: true, formatter: "handle", frozen: true, width: 30, minWidth: 30, frozen: true, },
            { title: displayNames["id"], field: "Id", visible: true, frozen: true, },
            { title: displayNames["name"], field: "Name", frozen: true, },
            { title: displayNames["remarks"], field: "Remarks", frozen: true, },
            { title: displayNames["order"], field: "Order", align: "right", },
            { title: displayNames["isDeleted"], field: "IsDeleted", formatter: "tickCross", formatterParams: { crossElement: "" }, visible: true, align: "center" },
            {
                title: displayNames["createdAt"], field: "CreatedAt", visible: true, formatter: function (cell, formatterParams, onRendered) {
                    return moment(cell.getValue()).format("YYYY-MM-DD HH:mm:ss");
                },
            },
            {
                title: displayNames["updatedAt"], field: "UpdatedAt", visible: true, formatter: function (cell, formatterParams, onRendered) {
                    return moment(cell.getValue()).format("YYYY-MM-DD HH:mm:ss");
                },
            },
            { title: displayNames["rowVersion"], field: "RowVersion", visible: true, }
        ],

        rowClick: function (e, row) {
            row.select();
            rowPosition = row.getPosition();
        },

        rowDblClick: function (e, row) {
            row.select();
            rowPosition = row.getPosition();

            $.ajax({
                type: "GET",
                url: modalUrl,
                contentType: "application/text; charset=UTF-8",
                dataType: "text",
                headers: { __RequestVerificationToken: getAjaxAntiForgeryToken() },
                data: { "id": row.getData().Id },
            }).then(function (response) {
                $("#modalDialog").html(response);

                $("#modalTitle").html("編集");
                $("#form0").attr("action", editUrl);
                $("#modalSubmit").attr("value", "更新");
                $("#Id").prop("readonly", true);
                $("#Id").css("background-color", "lightgray");

                $("#Order").attr("max", table.getDataCount());

                // $("#modalDelete").prop("disabled", false);

                $("#modalDialog").modal();
            });
        },

        rowMoved: function (row) {
            var data = row.getData();
            data.Order = row.getPosition() + 1;
            data.CreatedAt = moment(data.CreatedAt).format("YYYY-MM-DD HH:mm:ss.SSS");
            data.UpdatedAt = moment(data.UpdatedAt).format("YYYY-MM-DD HH:mm:ss.SSS");
            data.RowVersion = getRowVersion(data.RowVersion);

            movableSort(data);
        },
    });
}

// Radio button
$("#enable").click(function () {
    $("#enable").prop("checked", true);
    $("#all").prop("checked", false);
    table.setFilter("IsDeleted", "=", false);
});

$("#all").click(function () {
    $("#enable").prop("checked", false);
    $("#all").prop("checked", true);
    table.setFilter();
});

// Search, clear button
$("#search").click(function () {
    table.setFilter("WorkPartName", "like", $("#keyword").val());
});

$("#clear").click(function () {
    $("#keyword").val("");
    table.setFilter("WorkPartName", "like", $("#keyword").val());
});

// New button
$("#new").click(function () {
    var modalUrl = "/Members/Modal";
    var createUrl = "/Members/Create";

    $.ajax({
        type: "GET",
        url: modalUrl,
        headers: { __RequestVerificationToken: getAjaxAntiForgeryToken() },
    }).then(function (response) {
        $("#modalDialog").html(response);

        $("#modalTitle").html("新規登録");
        $("#form0").attr("action", createUrl);
        $("#modalSubmit").attr("value", "登録");
        $("#Id").prop("readonly", false);
        $("#Id").css("background-color", "transparent");

        var order = (rowPosition != null ? rowPosition + 2 : table.getDataCount());
        $("#Order").val(order);
        $("#Order").attr("max", table.getDataCount() + 1);

        $("#modalDelete").prop("disabled", true);

        $("#modalDialog").modal();
    });
});

// Grid movable sort
function movableSort(data) {
    var sortUrl = "/Members/Sort";

    $.ajax({
        type: "POST",
        url: sortUrl,
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        headers: { __RequestVerificationToken: getAjaxAntiForgeryToken() },
        data: JSON.stringify(data),
    }).then(function () {
        onSuccess();
    }, function (response) {
        onFailuer(response);
    });
}

// Modal delete button
$(document).on("click", "#modalDelete", function () {
    var deleteUrl = "/Members/Delete";
    var id = Number($("#Id").val());

    if (confirm("削除してもよろしいですか？")) {
        $.ajax({
            type: "POST",
            url: deleteUrl,
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            dataType: "json",
            headers: { __RequestVerificationToken: getAjaxAntiForgeryToken() },
            data: { "id": id },
        }).then(function () {
            onSuccess();
        }, function (response) {
            onFailuer(response);
        });
    }
});

// Modal open / close
$("#modalDialog").on("shown.bs.modal", function () {
    $("#Name").focus();
});

$("#modalDialog").on("hide.bs.modal", function () {
    $("#create").prop("disabled", false);
    $("#edit").prop("disabled", false);
});

// Ajax success / failure
function onSuccess() {
    var getsUrl = "/Members/Gets";

    $.getJSON(getsUrl, null, function (response) {
        tableData = response;
        table.setData(tableData);
        $("#enable").prop("checked") ? table.setFilter("IsDeleted", "=", false) : null;

        $("#modalDialog").attr("class", "modal fade in") ? $("#modalDialog").modal("hide") : null;
        console.log("success");
    });
}

function onFailure(response) {
    var message = JSON.parse(response.responseText).Message;
    console.log("failure : " + message);
    alert(message)
}

function getCalculateHeight() {
    var windowHeight = $(window).height();
    var headerHeight = $("#table").offset().top;
    var footerHeight = $("footer").height();
    var height = windowHeight - headerHeight - footerHeight - 120;

    return height;
};

$(window).on("resize", function () {
    // table.setHeight(getCalculateHeight());
});

function getRowVersion(rowVersionArray) {
    var rowVersion = window.btoa(String.fromCharCode.apply(null, rowVersionArray));
    return rowVersion;
}