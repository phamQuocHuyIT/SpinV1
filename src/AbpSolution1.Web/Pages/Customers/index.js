$(function () {
    var l = abp.localization.getResource('AbpSolution1');
    var createOrEditModal = new abp.ModalManager(abp.appPath + 'Customers/CreateOrEditModal');
    const service = abpSolution1.service.administration.customer.customer;

    var dataTable = $('#CustomerTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(service.getAll, function () {
                return {
                    filter: $('#Customersearch').val() // gửi filter lên backend
                };
            }),
            columnDefs: [
                {
                    title: l('Actions'),
                    rowAction: {
                        items: [
                            {
                                text: l('Edit'),
                                visible: abp.auth.isGranted('AbpSolution1.Administration.Customers.Edit'),
                                action: function (data) {
                                    console.log(data.record.Customer);
                                    createOrEditModal.open({ id: data.record.customer.id });
                                }
                            },
                            {
                                text: l('Delete'),
                                confirmMessage: function (data) {
                                    return l('CustomerDeletionConfirmationMessage', data.record.customer.name);
                                },
                                visible: abp.auth.isGranted('AbpSolution1.Administration.Customers.Delete'),
                                action: function (data) {
                                    service.delete(data.record.customer.id)
                                        .then(function () {
                                            abp.notify.success(l('DeletedSuccessfully'));
                                            dataTable.ajax.reload();
                                        });
                                }
                            }
                        ]
                    }
                },
                { title: l('Code'), data: "customer.code" },
                { title: l('FullName'), data: "customer.fullName" },
                { title: l('DOB'), data: "customer.dob", className: "text-center" },
                { title: l('Gender'), data: "customer.generText" },
                { title: l('Address'), data: "customer.address" },
                { title: l('TotalPurchase'), data: "customer.totalPurchase", className: "text-center" },
                { title: l('RankedName'), data: "customer.rankedName" },
            ]
        })
    );

    // Reload lại bảng khi gõ tìm kiếm
    $('#Customersearch').on('keyup', function () {
        dataTable.ajax.reload();
    });

    // Khi lưu modal xong, reload bảng
    createOrEditModal.onResult(function () {
        abp.notify.success(l('SavedSuccessfully'));
        dataTable.ajax.reload();
    });

    // Nút thêm mới
    $('#NewCustomerButton').click(function (e) {
        e.preventDefault();
        createOrEditModal.open();
    });
});
