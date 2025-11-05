$(function () {
    var l = abp.localization.getResource('AbpSolution1');

    // 🔹 Modal thêm & sửa cho Customer
    var createModal = new abp.ModalManager(abp.appPath + 'Customers/CreateOrEditModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Customers/CreateOrEditModal');

    // 🔹 DataTable chính hiển thị danh sách khách hàng
    var dataTable = $('#CustomersTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: true,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(abpSolution1.administration.customer.customer.getList), // 👈 đổi sang đúng service proxy
            columnDefs: [
                {
                    title: l('Actions'),
                    rowAction: {
                        items: [
                            {
                                text: l('Edit'),
                                visible: abp.auth.isGranted('AbpSolution1.Customers.Edit'),
                                action: function (data) {
                                    editModal.open({ id: data.record.customer.id });
                                }
                            },
                            {
                                text: l('Delete'),
                                visible: abp.auth.isGranted('AbpSolution1.Customers.Delete'),
                                confirmMessage: function (data) {
                                    return l('AreYouSureToDelete', data.record.customer.fullName);
                                },
                                action: function (data) {
                                    abpSolution1.administration.customer.customer
                                        .delete(data.record.customer.id)
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
                { title: l('Address'), data: "customer.address" },
                { title: l('PhoneNumber'), data: "customer.numberPhone" },
                { title: l('Rank'), data: "customer.rank" },
                { title: l('Total'), data: "customer.total" },
                { title: l('IsDeleted'), data: "customer.isDelete" }
            ]
        })
    );

    // 🔹 Sau khi thêm mới
    createModal.onResult(function () {
        abp.notify.success(l('CreatedSuccessfully'));
        dataTable.ajax.reload();
    });

    // 🔹 Sau khi sửa
    editModal.onResult(function () {
        abp.notify.success(l('SavedSuccessfully'));
        dataTable.ajax.reload();
    });

    // 🔹 Nút "Thêm khách hàng"
    $('#NewCustomerButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
