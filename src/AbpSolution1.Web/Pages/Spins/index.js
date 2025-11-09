$(function () {
    var l = abp.localization.getResource('AbpSolution1');
    var createOrEditModal = new abp.ModalManager(abp.appPath + 'Spins/CreateOrEditModal');
    const service = abpSolution1.service.config.spin.spin;

    var dataTable = $('#SpinTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(service.getAll, function () {
                return {
                    filter: $('#SpinSearch').val() // gửi filter lên backend
                };
            }),
            columnDefs: [
                {
                    title: l('Actions'),
                    rowAction: {
                        items: [
                            {
                                text: l('Edit'),
                                visible: abp.auth.isGranted('AbpSolution1.Administration.Spins.Edit'),
                                action: function (data) {
                                    console.log(data.record.Spin);
                                    createOrEditModal.open({ id: data.record.spin.id });
                                }
                            },
                            {
                                text: l('Delete'),
                                confirmMessage: function (data) {
                                    return l('DepartmentDeletionConfirmationMessage', data.record.spin.name);
                                },
                                visible: abp.auth.isGranted('AbpSolution1.Administration.Spins.Delete'),
                                action: function (data) {
                                    service.delete(data.record.spin.id)
                                        .then(function () {
                                            abp.notify.success(l('DeletedSuccessfully'));
                                            dataTable.ajax.reload();
                                        });
                                }
                            }
                        ]
                    }
                },
                { title: l('Code'), data: "spin.code" },
                { title: l('Name'), data: "spin.name" },
                

            ]
        })
    );

    // Reload lại bảng khi gõ tìm kiếm
    $('#SpinSearch').on('keyup', function () {
        dataTable.ajax.reload();
    });

    // Khi lưu modal xong, reload bảng
    createOrEditModal.onResult(function () {
        abp.notify.success(l('SavedSuccessfully'));
        dataTable.ajax.reload();
    });

    // Nút thêm mới
    $('#NewSpinButton').click(function (e) {
        e.preventDefault();
        createOrEditModal.open();
    });
});
