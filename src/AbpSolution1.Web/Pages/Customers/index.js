$(function() {
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
            ajax: abp.libs.datatables.createAjax(service.getAll),
            columnDefs: [
                {
                    title: l('Actions'),
                    rowAction: {
                        items: [
                            {
                                text: l('Edit'),
                                visible: abp.auth.isGranted('AbpSolution1.Administration.Customers.Edit'),
                                action: function(data) {
                                    createOrEditModal.open({ id: data.record.customer.id });
                                }
                            },
                            {
                                text: l('Delete'),
                                confirmMessage: function(data) {
                                    return l('CustomerDeletionConfirmationMessage', data.record.customer.fullName);
                                },
                                visible: abp.auth.isGranted('AbpSolution1.Administration.Customers.Delete'),
                                action: function(data) {
                                    service.delete(data.record.customer.id)
                                        .then(function() {
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
                { title: l('NumberPhone'), data: "customer.numberPhone" },
                { title: l('Rank'), data: "customer.rank" },
                { title: l('Total'), data: "customer.total" },
                { title: l('IsActive'), 
                  data: "customer.isActive",
                  render: function(data) {
                    return data ? '<i class="fas fa-check text-success"></i>' : '<i class="fas fa-times text-danger"></i>';
                  }
                }
            ]
        })
    );

    // Khi modal lưu xong, reload bảng
    createOrEditModal.onResult(function() {
        abp.notify.success(l('SavedSuccessfully'));
        dataTable.ajax.reload();
    });

    // Nút thêm mới
    $('#NewCustomerButton').click(function(e) {
        e.preventDefault();
        createOrEditModal.open();
    });
});
