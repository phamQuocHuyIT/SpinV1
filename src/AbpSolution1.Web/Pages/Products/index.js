$(function () {
    var l = abp.localization.getResource('AbpSolution1');
    var createOrEditModal = new abp.ModalManager(abp.appPath + 'Products/CreateOrEditModal');
    const service = abpSolution1.service.config.product.product;

    var dataTable = $('#ProductTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(service.getAll, function () {
                return {
                    filter: $('#Productsearch').val() // gửi filter lên backend
                };
            }),
            columnDefs: [
                {
                    title: l('Actions'),
                    rowAction: {
                        items: [
                            {
                                text: l('Edit'),
                                visible: abp.auth.isGranted('AbpSolution1.Administration.Products.Edit'),
                                action: function (data) {
                                    createOrEditModal.open({ id: data.record.product.id });
                                }
                            },
                            {
                                text: l('Delete'),
                                confirmMessage: function (data) {
                                    return l('ProductDeletionConfirmationMessage', data.record.product.name);
                                },
                                visible: abp.auth.isGranted('AbpSolution1.Administration.Products.Delete'),
                                action: function (data) {
                                    service.delete(data.record.product.id)
                                        .then(function () {
                                            abp.notify.success(l('DeletedSuccessfully'));
                                            dataTable.ajax.reload();
                                        });
                                }
                            }
                        ]
                    }
                },
                { title: l('Code'), data: "product.code" },
                { title: l('Name'), data: "product.name" },
                { title: l('Note'), data: "product.note" },
                {
                    title: l('IsActive'),
                    data: "product.isActive",
                    render: function (data) {
                        if (data) {
                            // ✅ Nếu true → chấm màu xanh
                            return '<span class="status-dot" style="display:inline-block;width:10px;height:10px;border-radius:50%;background-color:#28a745;"></span>';
                        } else {
                            // ❌ Nếu false → chấm màu xám
                            return '<span class="status-dot" style="display:inline-block;width:10px;height:10px;border-radius:50%;background-color:#6c757d;"></span>';
                        }
                    },
                    className: "text-center"
                },

            ]
        })
    );

    // Reload lại bảng khi gõ tìm kiếm
    $('#Productsearch').on('keyup', function () {
        dataTable.ajax.reload();
    });

    // Khi lưu modal xong, reload bảng
    createOrEditModal.onResult(function () {
        abp.notify.success(l('SavedSuccessfully'));
        dataTable.ajax.reload();
    });

    // Nút thêm mới
    $('#NewProductButton').click(function (e) {
        e.preventDefault();
        createOrEditModal.open();
    });
});
