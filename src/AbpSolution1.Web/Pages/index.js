$(function () {
    (() => {
        const $ = document.querySelector.bind(document);

        let timeRotate = 7000; // 7 giây
        let currentRotate = 0;
        let isRotating = false;
        let listGift = []; // sẽ được cập nhật từ API
        const wheel = $('.wheel');
        const btnWheel = $('.btn--wheel');
        const showMsg = $('.msg');

        // =====< Hàm tạo lại giao diện vòng quay >=====
        window.updateWheel = (newList) => {
            if (!newList || !newList.length) {
                abp.notify.warn('Vòng quay chưa có phần thưởng!');
                wheel.innerHTML = '';
                listGift = [];
                return;
            }

            listGift = newList.map(x => ({
                text: x.text,
                percent: x.percent
            }));

            wheel.innerHTML = '';
            const size = listGift.length;
            const rotate = 360 / size;
            const skewY = 90 - rotate;

            listGift.forEach((item, index) => {
                const elm = document.createElement('li');
                elm.className = 'li-wheel';
                elm.style.transform = `rotate(${rotate * index}deg) skewY(-${skewY}deg)`;
                const textClass = index % 2 === 0 ? 'text-1' : 'text-2';
                elm.innerHTML = `<p style="transform: skewY(${skewY}deg) rotate(${rotate / 2}deg);" class="text ${textClass}"><b>${item.text}</b></p>`;
                wheel.appendChild(elm);
            });

            abp.notify.success(`✅ Vòng quay đã được cập nhật với ${size} phần thưởng!`);
        };

        // =====< Hàm quay vòng quay >=====
        const rotateWheel = (currentRotate, index, rotate) => {
            wheel.style.transform = `rotate(${currentRotate - index * rotate - rotate / 2}deg)`;
        };

        // =====< Hàm lấy phần thưởng dựa theo tỷ lệ >=====
        const getGift = (randomNumber) => {
            let currentPercent = 0;
            for (let i = 0; i < listGift.length; i++) {
                currentPercent += listGift[i].percent;
                if (randomNumber <= currentPercent) return { ...listGift[i], index: i };
            }
            return listGift[listGift.length - 1];
        };

        // =====< Hàm hiển thị kết quả >=====
        const showGift = (gift) => {
            setTimeout(() => {
                isRotating = false;
                showMsg.innerHTML = `🎉 Chúc mừng bạn đã nhận được "<b>${gift.text}</b>"`;
            }, timeRotate);
        };

        // =====< Hàm bắt đầu quay >=====
        const start = () => {
            if (!listGift.length) {
                abp.notify.warn('⚠️ Chưa có phần thưởng nào trong vòng quay!');
                return;
            }
            showMsg.innerHTML = '';
            isRotating = true;
            const random = Math.random();
            const gift = getGift(random);
            const size = listGift.length;
            const rotate = 360 / size;
            currentRotate += 360 * 10;
            rotateWheel(currentRotate, gift.index, rotate);
            showGift(gift);
        };

        btnWheel.addEventListener('click', () => {
            if (!isRotating) start();
        });
    })();

    // ================================
    // 🔍 Xử lý tìm kiếm khách hàng
    // ================================
    const $input = $('#searchCustomer');
    const $suggestions = $('#customerSuggestions');
    const $selectedInfo = $('#selectedCustomerInfo'); // thông tin chi tiết khách hàng
    let selectedCustomerId = null;
    let typingTimer;

    function showCustomerInfo(customer) {
        $('#customerName').text(customer.fullName);
        $('#customerCode').text(customer.code);
        $('#customerDOB').text(customer.dob ? new Date(customer.dob).toLocaleDateString() : '');
        $('#customerAddress').text(customer.address || '');
        $('#customerPhone').text(customer.numberPhone || '');
        $('#customerGender').text(customer.gender === 1 ? 'Nam' : customer.gender === 2 ? 'Nữ' : 'Khác');
        $('#customerTotalPurchase').text(customer.totalPurchase || 0);
        $selectedInfo.removeClass('d-none');
    }

    // =====< Hàm tải vòng quay (customerId có thể null) >=====
    async function loadWheel(customerId) {
        try {
            const spinData = await abpSolution1.service.config.spin.spin.getSpinByEmployee({
                customerId
            });

            if (!spinData.items?.length) {
                abp.notify.warn(customerId ? 'Không tìm thấy vòng quay cho khách hàng này!' : 'Chưa có vòng quay mặc định!');
                updateWheel([]);
                return;
            }

            const spin = spinData.items[0].spin;
            const products = spin.products || [];
            if (!products.length) {
                abp.notify.warn(customerId ? 'Vòng quay này chưa có sản phẩm!' : 'Vòng quay mặc định chưa có sản phẩm!');
                updateWheel([]);
                return;
            }

            const listGift = products.map(p => ({
                text: `${p.productName}`,
                percent: p.proportion / 100
            }));

            updateWheel(listGift);

        } catch (error) {
            console.error('Lỗi khi tải vòng quay:', error);
            abp.notify.error(customerId ? 'Không thể tải vòng quay cho khách hàng!' : 'Không thể tải vòng quay mặc định!');
            updateWheel([]);
        }
    }

    // =====< Clear customer và load vòng quay mặc định >=====
    $('#clearCustomer').on('click', function () {
        selectedCustomerId = null;
        $selectedInfo.addClass('d-none');
        $input.val('');
        $('.msg').html('');
        loadWheel(null); // load vòng quay default
    });

    // =====< Lấy danh sách khách hàng từ server >=====
    async function fetchCustomers(keyword) {
        try {
            const result = await abpSolution1.service.administration.customer.customer.getAll({
                filter: keyword,
                maxResultCount: 10
            });
            renderSuggestions(result.items || []);
        } catch (err) {
            console.error('❌ Lỗi khi tải khách hàng:', err);
        }
    }

    function renderSuggestions(list) {
        $suggestions.empty();
        if (!list.length) return $suggestions.hide();
        list.forEach(c => {
            const li = $('<li>')
                .addClass('list-group-item list-group-item-action')
                .text(c.customer.fullName)
                .data('id', c.customer.id)
                .data('customer', c.customer);
            $suggestions.append(li);
        });
        $suggestions.fadeIn(150);
    }

    $input.on('input', function () {
        const keyword = $(this).val().trim();
        clearTimeout(typingTimer);
        if (!keyword.length) return $suggestions.fadeOut(100);
        typingTimer = setTimeout(() => fetchCustomers(keyword), 300);
    });

    $suggestions.on('click', 'li', function () {
        const name = $(this).text();
        selectedCustomerId = $(this).data('id');
        const customer = $(this).data('customer');
        $input.val(name);
        $suggestions.fadeOut(100);
        abp.notify.info('Đang tải vòng quay cho khách hàng: ' + name);
        showCustomerInfo(customer);
        loadWheel(selectedCustomerId);
    });

    $(document).on('click', function (e) {
        if (!$(e.target).closest('.customer-search').length) {
            $suggestions.fadeOut(100);
        }
    });

    // =====< Load vòng quay mặc định ngay khi trang load >=====
    loadWheel(null);
});
