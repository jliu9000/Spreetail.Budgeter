$("#purchase-sum-prompt").click(() => {
    $("#purchase-sum-modal").modal("show");
});

$("#purchase-sum-go").click(() => {
    window.location = PURCHASE_SUM_URL + "/" + budgetID + "?startdate=" + $("#ps-start-date").val() + "&enddate=" + $("#ps-end-date").val();
});