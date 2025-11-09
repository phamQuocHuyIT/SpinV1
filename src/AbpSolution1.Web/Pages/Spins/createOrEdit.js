$(function () {
    var l = abp.localization.getResource('AbpSolution1');
    console.log("✅ JS loaded - createOrEdit.js");

    const productService = abpSolution1.service.config.product.product;
    const customerService = abpSolution1.service.administration.customer.customer;
    const spinService = abpSolution1.service.config.spin.spin;

    var productList = [];
    var customerList = [];

    const addProductBtn = $('#Add_Product');
    const addCustomerBtn = $('#Add_Customer');

    addProductBtn.prop('disabled', true).text('Đang tải sản phẩm...');
    addCustomerBtn.prop('disabled', true).text('Đang tải khách hàng...');

    // =============================
    // 🔹 Load danh sách sản phẩm & khách hàng
    // =============================
    function loadProducts() {
        return productService.getAll({}).then(result => {
            productList = (result.items || []).map(x => x.product || x);
            addProductBtn.prop('disabled', false).text('Thêm sản phẩm');
        });
    }

    function loadCustomers() {
        return customerService.getAll({}).then(result => {
            customerList = (result.items || []).map(x => x.customer || x);
            addCustomerBtn.prop('disabled', false).text('Thêm khách hàng');
        });
    }

    // =============================
    // 🔹 Tạo dropdown
    // =============================
    function createProductSelect(selectedId) {
        const select = $('<select class="product-select form-control"></select>');
        productList.forEach(p => {
            const option = $('<option></option>').val(p.id).text(p.name).attr('data-code', p.code);
            if (p.id === selectedId) option.prop('selected', true);
            select.append(option);
        });
        return select;
    }

    function createCustomerSelect(selectedId) {
        const select = $('<select class="customer-select form-control"></select>');
        customerList.forEach(c => {
            const option = $('<option></option>').val(c.id).text(c.fullName).attr('data-code', c.code || '');
            if (c.id === selectedId) option.prop('selected', true);
            select.append(option);
        });
        return select;
    }

    // =============================
    // 🔹 Thêm dòng sản phẩm / khách hàng
    // =============================
    function addProductRow(product) {
        const container = $('#SpinForm .product-container');
        const row = $('<div class="product-row d-flex align-items-center" style="margin-bottom:8px;"></div>');
        const select = createProductSelect(product?.productId).css("flex", "1");
        const percent = $('<input type="number" class="product-percent form-control" placeholder="%" style="width:80px;margin-left:8px;" />')
            .val(product?.proportion || 0);
        const isDefault = $('<label style="margin-left:8px;display:flex;align-items:center;">Mặc định <input type="checkbox" class="product-isdefault" style="margin-left:5px;" /></label>')
            .find('input').prop('checked', product?.isDefault || false).end();
        const removeBtn = $('<button type="button" class="btn btn-danger btn-sm remove-product" style="margin-left:8px;">🗑️</button>');

        row.append(select, percent, isDefault, removeBtn);
        container.append(row);
    }

    function addCustomerRow(customer) {
        const container = $('#SpinForm .customer-container');
        const row = $('<div class="customer-row d-flex align-items-center" style="margin-bottom:8px;"></div>');
        const select = createCustomerSelect(customer?.customerId).css("flex", "1");
        const spinCount = $('<input type="number" class="customer-spin-count form-control" placeholder="Số lần quay" style="width:120px;margin-left:8px;" />')
            .val(customer?.spinCount || 0);
        const removeBtn = $('<button type="button" class="btn btn-danger btn-sm remove-customer" style="margin-left:8px;">🗑️</button>');

        row.append(select, spinCount, removeBtn);
        container.append(row);
    }

    // =============================
    // 🔹 Load dữ liệu spin nếu chỉnh sửa
    // =============================
    function loadSpinData(spinId) {
        spinService.getAll({ id: spinId }).done(function (result) {
            if (!result.items || !result.items.length) return;

            const spin = result.items[0].spin;

            // Load products
            if (spin.products?.length) {
                spin.products.forEach(p => addProductRow(p));
            }

            // Load customers
            if (spin.customers?.length) {
                spin.customers.forEach(c => addCustomerRow(c));
            }
        });
    }

    // =============================
    // 🔹 Load tất cả dữ liệu trước khi render spin
    // =============================
    Promise.all([loadProducts(), loadCustomers()]).then(() => {
        const spinId = parseInt($('#SpinForm input[name="ids"]').val()) || null;
        if (spinId) loadSpinData(spinId);
    });

    // =============================
    // 🔹 Nút thêm mới
    // =============================
    addProductBtn.click(function (e) { e.preventDefault(); addProductRow(); });
    addCustomerBtn.click(function (e) { e.preventDefault(); addCustomerRow(); });

    // =============================
    // 🔹 Remove row
    // =============================
    $(document).on('click', '.remove-product', function () { $(this).closest('.product-row').remove(); });
    $(document).on('click', '.remove-customer', function () { $(this).closest('.customer-row').remove(); });

    // =============================
    // 🔹 Submit
    // =============================
    $('#Sub').on('click', function (e) {
        e.preventDefault();
        const formData = $('#SpinForm').serializeFormToObject();
        const spinData = formData.spin || {};
        const products = [], customers = [];

        $('#SpinForm .product-row').each(function () {
            const $row = $(this);
            products.push({
                productId: parseInt($row.find('.product-select').val()) || null,
                proportion: parseFloat($row.find('.product-percent').val()) || 0,
                isDefault: $row.find('.product-isdefault').is(':checked')
            });
        });

        $('#SpinForm .customer-row').each(function () {
            const $row = $(this);
            customers.push({
                customerId: parseInt($row.find('.customer-select').val()) || null,
                spinCount: parseInt($row.find('.customer-spin-count').val()) || 0
            });
        });

        spinData.products = products;
        spinData.customers = customers;

        // Xác định là create hay edit
        spinData.id = parseInt($('#SpinForm input[name="ids"]').val()) || null;
        spinData.isEdit = !!spinData.id;

        console.log("📦 Dữ liệu gửi về service:", spinData);
        abpSolution1.service.config.spin.spin.createOrEdit(spinData)
            .done(() => {
                abp.notify.info(l('SavedSuccessfully'));
                abp.event.trigger('spin.createdOrEdited');
                Modal.closest();
            });
    });
});
