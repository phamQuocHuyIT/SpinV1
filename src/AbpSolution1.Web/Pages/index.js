$(function () {
    // =======================================
    // 🧲 BIẾN LƯU LỊCH SỬ QUAY
    // =======================================
    let historySpinData = {
        customerId: null,
        spinId: null,
        productId: null
    };

    (() => {
        const $ = document.querySelector.bind(document);

        let timeRotate = 7000;
        let currentRotate = 0;
        let isRotating = false;
        let listGift = [];
        const wheel = $('.wheel');
        const btnWheel = $('.btn--wheel');

        // ===============================
        // 🎡 CẬP NHẬT VÒNG QUAY
        // ===============================
        window.updateWheel = (newList) => {
            if (!newList || !newList.length) {
                abp.notify.warn('Vòng quay chưa có phần thưởng!');
                wheel.innerHTML = '';
                listGift = [];
                return;
            }

            listGift = newList.map(x => ({
                id: x.id,       // PRODUCT ID
                text: x.text,
                percent: x.percent
            }));

            wheel.innerHTML = '';
            const size = listGift.length;
            const rotate = 360 / size;
            const skewY = 90 - rotate;

            const colors = ['#f94144', '#f3722c', '#f8961e', '#f9c74f', '#90be6d', '#43aa8b', '#577590', '#7400b8'];

            listGift.forEach((item, index) => {
                const elm = document.createElement('li');
                elm.className = 'li-wheel';
                elm.style.transform = `rotate(${rotate * index}deg) skewY(-${skewY}deg)`;
                const color = colors[index % colors.length];

                elm.innerHTML =
                    `<p style="transform: skewY(${skewY}deg) rotate(${rotate / 2}deg);
                    background-color:${color}; color:white; padding:10px 0;" 
                    class="text"><b>${item.text}</b></p>`;
                wheel.appendChild(elm);
            });

            abp.notify.success(`Vòng quay đã được cập nhật với ${size} phần thưởng!`);
        };

        const rotateWheel = (currentRotate, index, rotate) => {
            wheel.style.transform = `rotate(${currentRotate - index * rotate - rotate / 2}deg)`;
        };

        const getGift = (randomNumber) => {
            let currentPercent = 0;

            for (let i = 0; i < listGift.length; i++) {
                currentPercent += listGift[i].percent;
                if (randomNumber <= currentPercent)
                    return { ...listGift[i], index: i };
            }

            return listGift[listGift.length - 1];
        };

        // =======================================
        // 🎆 HIỂN THỊ QUÀ + BẮN PHÁO HOA
        // =======================================
        const showGift = async (gift) => {
            setTimeout(async () => {
                isRotating = false;

                // 🔥 Pháo hoa
                launchFireworks();

                // 💾 Lưu lịch sử: lấy ProductId
                historySpinData.productId = gift.id;

                // 🔥 CALL API LƯU LỊCH SỬ
                try {
                    await abpSolution1.service.abpSoluation1CommonAppSevice.createHistorySpin(historySpinData);
                } catch (err) {
                    console.error("Lỗi lưu lịch sử:", err);
                }

                Swal.fire({
                    title: '🎉 Chúc mừng!',
                    html: `Bạn đã nhận được:<br><b>${gift.text}</b>`,
                    icon: 'success',
                    showConfirmButton: true,
                    confirmButtonText: 'OK',
                    background: 'rgba(255,255,255,0.9)',
                    backdrop: 'rgba(0,0,0,0.2)',
                });

            }, timeRotate);
        };

        const start = () => {
            if (!listGift.length) {
                abp.notify.warn('Chưa có phần thưởng nào!');
                return;
            }

            isRotating = true;
            const random = Math.random();
            const gift = getGift(random);
            const size = listGift.length;
            const rotate = 360 / size;

            currentRotate += 360 * 10;
            rotateWheel(currentRotate, gift.index, rotate);

            showGift(gift);
        };

        btnWheel.addEventListener('click', () => { if (!isRotating) start(); });
    })();


    // ===================================================
    // 🎆 PHÁO HOA
    // ===================================================
    function launchFireworks() {
        const canvas = document.getElementById('fireworksCanvas');
        const ctx = canvas.getContext('2d');
        canvas.style.display = 'block';
        canvas.width = window.innerWidth;
        canvas.height = window.innerHeight;

        const fireworks = [];

        class Particle {
            constructor(x, y) {
                this.x = x;
                this.y = y;
                this.vx = (Math.random() - 0.5) * 10;
                this.vy = (Math.random() - 0.5) * 10;
                this.alpha = 1;
                this.size = Math.random() * 3 + 2;
                this.color = `hsl(${Math.random() * 360}, 100%, 60%)`;
            }
            update() {
                this.x += this.vx;
                this.y += this.vy;
                this.vy += 0.03;
                this.alpha -= 0.01;
            }
            draw(ctx) {
                if (this.alpha <= 0) return;
                ctx.save();
                ctx.globalAlpha = this.alpha;
                ctx.fillStyle = this.color;
                ctx.beginPath();
                ctx.arc(this.x, this.y, this.size, 0, Math.PI * 2);
                ctx.fill();
                ctx.restore();
            }
        }

        class Firework {
            constructor(x, y) {
                this.x = x;
                this.y = y;
                this.particles = [];

                for (let i = 0; i < 80; i++) {
                    this.particles.push(new Particle(x, y));
                }
            }
            update() { this.particles.forEach(p => p.update()); }
            draw(ctx) { this.particles.forEach(p => p.draw(ctx)); }
        }

        const fireworkInterval = setInterval(() => {
            for (let i = 0; i < 3; i++) {
                fireworks.push(new Firework(
                    Math.random() * canvas.width,
                    Math.random() * canvas.height * 0.5
                ));
            }
        }, 500);

        function animate() {
            ctx.fillStyle = "rgba(0, 0, 0, 0.2)";
            ctx.fillRect(0, 0, canvas.width, canvas.height);
            fireworks.forEach(f => { f.update(); f.draw(ctx); });
            requestAnimationFrame(animate);
        }

        animate();

        setTimeout(() => {
            clearInterval(fireworkInterval);
            ctx.clearRect(0, 0, canvas.width, canvas.height);
            canvas.style.display = 'none';
        }, 3000);
    }


    // ============================================
    // 🔍 TÌM KIẾM KHÁCH HÀNG
    // ============================================
    const $input = $('#searchCustomer');
    const $suggestions = $('#customerSuggestions');
    const $selectedInfo = $('#selectedCustomerInfo');
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

        // LƯU CUSTOMER ID
        historySpinData.customerId = customer.id;
    }

    // ============================================
    // 🎡 TẢI VÒNG QUAY THEO KHÁCH HÀNG
    // ============================================
    async function loadWheel(customerId) {
        try {
            const spinData = await abpSolution1.service.config.spin.spin.getSpinByEmployee({ customerId });

            if (!spinData.items?.length) {
                updateWheel([]);
                $('#wheelTitle').text('🎡 Vòng quay may mắn');
                return;
            }

            const spin = spinData.items[0].spin;

            // LƯU SPIN ID
            historySpinData.spinId = spin.id;

            $('#wheelTitle').text(spin.name || '🎡 Vòng quay may mắn');

            const products = spin.products || [];
            if (!products.length) {
                updateWheel([]);
                return;
            }

            const listGift = products.map(p => ({
                id: p.productId,   // PRODUCT ID
                text: p.productName,
                percent: p.proportion / 100
            }));

            updateWheel(listGift);

        } catch (error) {
            console.error(error);
            updateWheel([]);
            $('#wheelTitle').text('🎡 Vòng quay may mắn');
        }
    }


    $('#clearCustomer').on('click', function () {
        selectedCustomerId = null;
        $selectedInfo.addClass('d-none');
        $input.val('');
        historySpinData = { customerId: null, spinId: null, productId: null };
        loadWheel(null);
    });

    async function fetchCustomers(keyword) {
        try {
            const result = await abpSolution1.service.administration.customer.customer.getAll({
                filter: keyword,
                maxResultCount: 10
            });
            renderSuggestions(result.items || []);
        } catch (err) {
            console.error(err);
        }
    }

    function renderSuggestions(list) {
        $suggestions.empty();
        if (!list.length) return $suggestions.hide();

        list.forEach(c => {
            const li = $('<li>')
                .addClass('list-group-item list-group-item-action')
                .text(`${c.customer.code} - ${c.customer.fullName}`)
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
        const customer = $(this).data('customer');
        selectedCustomerId = customer.id;

        $input.val($(this).text());
        $suggestions.fadeOut(100);

        abp.notify.info('Đang tải vòng quay cho khách hàng: ' + customer.fullName);

        showCustomerInfo(customer);
        loadWheel(selectedCustomerId);
    });

    $(document).on('click', function (e) {
        if (!$(e.target).closest('.customer-search').length) {
            $suggestions.fadeOut(100);
        }
    });

    // LOAD VÒNG QUAY MẶC ĐỊNH
    loadWheel(null);
});
