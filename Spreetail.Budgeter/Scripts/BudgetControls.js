$(function () {
    LoadCategoryDropDown();
});

$("#item-close").click(() => {
    clearItemInput();
});

//category save---
$("#cat-submit").click(() => {
    var model = {
        CategoryID: $('#cat-id').val(),
        CategoryName: $('#cat-input').val(),
        BudgetID: budgetID,
        BudgetAmount: $("#cat-amount").val()
    };


    $.post(CATEGORY_SAVE, model, function (res) {
        console.log(res);

        if (res.Errors.length > 0) {
            $("#cat-errors").html("");
            $.each(res.Errors, (i, e) => {
                $("#cat-errors").append($("<div>").addClass("alert-danger").html(e));
            });

        } else {
            $("#no-categories").hide();
            clearCategory();
            LoadCategoryDropDown();
            ReloadCategory(res.CategoryID);
            $('#cat-input-modal').modal('hide');
        }
    });
});


$("#add-an-item").click(() => {
    if ($("#category-select option").length > 0) {
        clearItemInput();
        $("#item-input-modal").modal('show');
    } else {
        $("#no-categories").show();
    }
});

//enable / disable reoccuring rate entry
$('#item-reoccuring').click(() => {
    if ($('#item-reoccuring').is(":checked")) {
        $("#item-reoccuring-options").prop("disabled", false);
    } else {
        $("#item-reoccuring-options").prop("disabled", true);
    }
});

//items save---
$("#item-submit").click(() => {
    //save reoccuring
    var saveReoccuring = $('#item-reoccuring').is(":checked");

    if (saveReoccuring) {
        var reoccuringmodel = {
            ReoccuringRate: $('#item-reoccuring-rate').val(),
            ReoccuringUnit: $('#item-reoccuring-unit').val(),
            StartDate: $('#item-date').val(),
            EndDate: $('#reoccuring-end-date').val(),
            CategoryID: $('#category-select').val(),
            ReoccuringCost: $('#item-cost').val(),
            BudgetID: budgetID,
            Name: $('#item-name').val(),
            ReoccuringItemID: $('#reoccuring-id').val()
        };

        $.post(REOCCURING_SAVE, reoccuringmodel, function (res) {

            if (res.Errors.length > 0) {
                $("#item-errors").html("");
                $.each(res.Errors, (i, e) => {
                    $("#item-errors").append($("<div>").addClass("alert-danger").html(e));
                });

            } else {
                ReloadCategory($('#category-select').val());
                $('#item-input-modal').modal('hide');
                clearItemInput();
            }

        });
    } else {
        var itemmodel = {
            CategoryID: $('#category-select').val(),
            ItemName: $('#item-name').val(),
            PurchaseDate: $('#item-date').val(),
            Cost: $('#item-cost').val(),
            ItemID: $('#item-id').val(),
            BudgetID: budgetID
        };
        $.post(ITEM_SAVE, itemmodel, function (res) {
            if (res.Errors.length > 0) {
                $("#item-errors").html("");
                $.each(res.Errors, (i, e) => {
                    $("#item-errors").append($("<div>").addClass("alert-danger").html(e));
                });

            } else {
                ReloadCategory($('#category-select').val());
                $('#item-input-modal').modal('toggle');
                clearItemInput();
            }
        });
    }
});

$("#item-del").click(() => {

    if ($('#reoccuring-del-id').val() > 0) {
        var model = { id: $('#reoccuring-del-id').val() };
        $.post(REOCCURING_DELETE, model, function (res) {
            ReloadCategory($('#category-del-id').val());
            $('#item-delete-modal').modal("hide");
            $("#reoccuring-warning").hide();
        });
    } else {
        var model = { id: $('#item-del-id').val() };
        $.post(ITEM_DELETE, model, function (res) {
            ReloadCategory($('#category-del-id').val());
            $('#item-delete-modal').modal("hide");
            $("#reoccuring-warning").hide();
        });
    }

});

function editItem(idSelector) {
    clearItemInput();

    var tr = $("#" + idSelector);
    $('#category-select').val(tr.data("category-id"));
    $('#item-name').val(tr.data("item-name"));
    $('#item-date').val(tr.data("purchase-date"));
    $('#item-cost').val(tr.data("cost"));
    $('#item-id').val(tr.data("id"));
    $('#item-input-modal').modal('show');
    $('#reoccuring-id').val(tr.data("reoccuring-id"));

    if (tr.data("reoccuring-id") > 0) {
        var model = { id: tr.data("reoccuring-id") };
        $.post(REOCCURING_GET, model, function (res) {
            $("#reoccuring-warning").show();
            $("#item-reoccuring-rate").val(res.ReoccuringRate);
            $("#item-reoccuring-unit").val(res.ReoccuringUnit);

            var endDate = new Date(parseInt(res.EndDate.replace(/[^0-9 +]/g, '')));
            var monthPlaceholder = "";
            if ((endDate.getMonth() + 1).toString().length == 1) {
                monthPlaceholder = "0";
            }
            var dayPlaceholder = "";
            if (endDate.getDay().toString().length == 1) {
                dayPlaceholder = "0";
            }
            var setDate = endDate.getFullYear() + "-" + monthPlaceholder + (endDate.getMonth() + 1) + "-" + dayPlaceholder + endDate.getDay();
            $("#reoccuring-end-date").val(setDate);
            $("#item-reoccuring").prop("checked", true);
            $("#item-reoccuring").prop("disabled", true);
            $("#item-reoccuring-options").prop("disabled", false);

        });
    } else {
        $("#item-reoccuring").prop("checked", false);
        $("#item-reoccuring").prop("disabled", true);
        $("#item-reoccuring-options").prop("disabled", true);
    }
}

function deleteItemPrompt(idSelector) {
    clearItemInput();

    var tr = $("#" + idSelector);

    $('#item-del-id').val(tr.data("id"));
    $('#reoccuring-del-id').val(tr.data("reoccuring-id"));
    $('#category-del-id').val(tr.data("category-id"));

    $('#item-delete-modal').modal("show");

    if (tr.data("occuring-id") > 0) {
        $("#reoccuring-warning").show();
    }
}



function clearItemInput() {
    $("#item-errors").html("");
    $("#item-name").val("");
    $("#item-cost").val("");
    $("#item-date").val(DISPLAY_DATE);
    $("#category-select").val("");
    $("#item-reoccuring").val("");
    $("#item-reoccuring-rate").val("");
    $("#item-reoccuring-unit").val("");
    $("#reoccuring-end-date").val(DISPLAY_DATE);
    $("#reoccuring-warning").hide();
    $("#item-reoccuring").prop("checked", false);
    $("#item-reoccuring-options").prop("disabled", true);
    $("#item-id").val("-1");
    $("#reoccuring-id").val("-1");
}

function clearCategory() {
    $('#cat-id').val("-1");
    $('#cat-input').val("");
    $("#cat-errors").html("");
    $("#cat-amount").val("");
}

function SaveItem(model) {
    $.post(ITEM_SAVE, model, function (res) {

        if (res.CategoryID > 0) {
            ReloadCategory(res.CategoryID);
        }
    });

}

function LoadCategoryDropDown() {
    var model = { budgetID: budgetID };
    var catSelect = $("#category-select");

    $.post(CATEGORY_GET_ALL, model, function (res) {
        catSelect.empty();
        $(res).each(function () {
            catSelect.append($("<option>").attr("value", this.CategoryID).html(this.CategoryName));
        });
    });
}

function ReloadCategory(id) {
    var model = { ID: id };
    $.post(CATEGORY_SPENDING_GET, { ID: id, date: budgetDate }, function (res) {
        var category = CategoryDiv(id);
        if (category.length > 0) {
            category.html(res);
        } else {
            $("#spending-records").append(res);
        }
    });
}

function CategoryDiv(id) {
    return $('#category-spending-' + id);
}