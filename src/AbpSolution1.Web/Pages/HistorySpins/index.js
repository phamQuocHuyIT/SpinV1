$(function () {
    var l = abp.localization.getResource('AbpSolution1');
    const service = abpSolution1.service.config.spin.historySpin;

    var dataTable = $('#HistorySpinTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(service.getAll, function () {
                return {
                    filter: $('#HistorySpinsearch').val() // gửi filter lên backend
                };
            }),
            columnDefs: [
                { title: l('SpinName'), className: "text-center", data: "historySpin.spinName" },
                { title: l('CustomerName'), data: "historySpin.customerName" },
                { title: l('DateReward'), className: "text-center", data: "historySpin.rewardDateString" },
                { title: l('ProductName'), className: "text-center", data: "historySpin.productName" },
                { title: l('EmpployeeName'), data: "historySpin.employeeName" },
                

            ]
        })
    );

    // Reload lại bảng khi gõ tìm kiếm
    $('#HistorySpinsearch').on('keyup', function () {
        dataTable.ajax.reload();
    });


});
