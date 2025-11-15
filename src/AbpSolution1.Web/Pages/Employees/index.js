$(function () {
    var l = abp.localization.getResource('AbpSolution1');
    var createOrEditModal = new abp.ModalManager(abp.appPath + 'Employees/CreateOrEditModal');
    const service = abpSolution1.service.administration.employee.employee;

    var dataTable = $('#EmployeeTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(service.getAll, function () {
                return {
                    filter: $('#EmployeeSearch').val() // gửi filter lên backend
                };
            }),
            columnDefs: [
                {
                    title: l('Actions'),
                    rowAction: {
                        items: [
                            {
                                text: l('Edit'),
                                visible: abp.auth.isGranted('AbpSolution1.Administration.Employees.Edit'),
                                action: function (data) {
                                    console.log(data.record.employee);
                                    createOrEditModal.open({ id: data.record.employee.id });
                                }
                            },
                            {
                                text: l('Delete'),
                                confirmMessage: function (data) {
                                    return l('DepartmentDeletionConfirmationMessage', data.record.employee.name);
                                },
                                visible: abp.auth.isGranted('AbpSolution1.Administration.Employees.Delete'),
                                action: function (data) {
                                    service.delete(data.record.employee.id)
                                        .then(function () {
                                            abp.notify.success(l('DeletedSuccessfully'));
                                            dataTable.ajax.reload();
                                        });
                                }
                            }
                        ]
                    }
                },
                { title: l('Code'), data: "employee.code" },
                { title: l('FullName'), data: "employee.fullName" },
                { title: l('DOB'), data: "employee.dob", className: "text-center" },
                { title: l('Gender'), data: "employee.generText" },
                { title: l('Address'), data: "employee.address" },
                

            ]
        })
    );

    // Reload lại bảng khi gõ tìm kiếm
    $('#EmployeeSearch').on('keyup', function () {
        dataTable.ajax.reload();
    });

    // Khi lưu modal xong, reload bảng
    createOrEditModal.onResult(function () {
        abp.notify.success(l('SavedSuccessfully'));
        dataTable.ajax.reload();
    });

    // Nút thêm mới
    $('#NewEmployeeButton').click(function (e) {
        e.preventDefault();
        createOrEditModal.open();
    });
});
