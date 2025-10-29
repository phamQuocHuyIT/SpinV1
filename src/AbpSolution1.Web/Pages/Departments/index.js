﻿$(function () {
    var l = abp.localization.getResource('AbpSolution1');
    var createOrEditModal = new abp.ModalManager(abp.appPath + 'Departments/CreateOrEditModal');
    const service = abpSolution1.service.administration.department.department;

    var dataTable = $('#DepartmentTable').DataTable(
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
                                visible: abp.auth.isGranted('AbpSolution1.Administration.Departments.Edit'),
                                action: function (data) {
                                    createOrEditModal.open({ id: data.record.department.id });
                                }
                            },
                            {
                                text: l('Delete'),
                                confirmMessage: function (data) {
                                    return l('DepartmentDeletionConfirmationMessage', data.record.department.name);
                                },
                                visible: abp.auth.isGranted('AbpSolution1.Administration.Departments.Delete'),
                                action: function (data) {
                                    service.delete(data.record.department.id)
                                        .then(function () {
                                            abp.notify.success(l('DeletedSuccessfully'));
                                            dataTable.ajax.reload();
                                        });
                                }
                            }
                        ]
                    }
                },
                { title: l('Code'), data: "department.code" },
                { title: l('Name'), data: "department.name" },
                { title: l('Note'), data: "department.note" },
                { title: l('IsActive'), data: "department.isActive" },
                

            ]
        })
    );

    // Khi modal lưu xong, reload bảng
    createOrEditModal.onResult(function () {
        abp.notify.success(l('SavedSuccessfully'));
        dataTable.ajax.reload();
    });

    // Nút thêm mới
    $('#NewDepartmentButton').click(function (e) {
        e.preventDefault();
        createOrEditModal.open();
    });
});
