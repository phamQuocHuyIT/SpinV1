$(function () {
    var l = abp.localization.getResource('AbpSolution1');
    console.log("✅ JS loaded");

    var departmentId = $('#DepartmentId');                // dropdown bạn tạo
    var employee_DepartmentId = $('#Employee_DepartmentId'); // input tự động render
    var employee_Id = $('#Employee_Id'); // input tự động render

    // 1️⃣ Ẩn toàn bộ div cha chứa input Employee_DepartmentId
    employee_DepartmentId.closest('.mb-3').hide();
    employee_Id.closest('.mb-3').hide();

    employee_DepartmentId.val(departmentId.val());

    // 2️⃣ Khi chọn dropdown phòng ban → gán lại giá trị cho input ẩn
    departmentId.on('change', function () {
        var selectedValue = departmentId.val();
        console.log("Selected DepartmentId:", selectedValue);
        employee_DepartmentId.val(selectedValue);
    });
});
