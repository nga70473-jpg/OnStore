// ============ GIỎ HÀNG POPUP ============

// Mở popup giỏ hàng
function openCartPopup() {
    document.getElementById('cartPopup').classList.add('active');
    loadCartItems();
}

// Đóng popup giỏ hàng
function closeCartPopup() {
    document.getElementById('cartPopup').classList.remove('active');
}

// Đóng popup khi nhấn ESC
document.addEventListener('keydown', function (e) {
    if (e.key === 'Escape') {
        closeCartPopup();
    }
});

// Load giỏ hàng
function loadCartItems() {
    $.ajax({
        url: '/Cart/GetCartItems',
        type: 'GET',
        success: function (response) {
            if (response.success) {
                renderCartItems(response.data);
            } else {
                console.error('Lỗi load giỏ hàng:', response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error('Lỗi AJAX:', error);
        }
    });
}

// Render giỏ hàng
function renderCartItems(cartData) {
    var cartItemsContainer = $('#cartItems');
    var cartEmpty = $('#cartEmpty');
    var cartCountBadge = $('#cartCountBadge');

    if (!cartData.Items || cartData.Items.length === 0) {
        cartEmpty.show();
        cartItemsContainer.hide();
        cartCountBadge.text('0');
        updateCartCount(0);
        return;
    }

    cartEmpty.hide();
    cartItemsContainer.show();

    var html = '';
    cartData.Items.forEach(function (item) {
        html += `
            <div class="cart-item" data-cart-item-id="${item.CartItemId}">
                <img src="${item.ImageUrl || 'https://via.placeholder.com/80'}" 
                     alt="${item.ProductName}" 
                     class="cart-item-image">
                
                <div class="cart-item-info">
                    <div class="cart-item-name">${item.ProductName}</div>
                    <div class="cart-item-price">${formatPrice(item.Price)} đ</div>
                    
                    <div class="cart-item-quantity">
                        <button class="qty-btn" onclick="updateCartQuantity(${item.CartItemId}, ${item.Quantity - 1})">
                            <i class="fa-solid fa-minus"></i>
                        </button>
                        <input type="number" class="qty-input" value="${item.Quantity}" 
                               min="1" max="${item.Stock}" readonly>
                        <button class="qty-btn" onclick="updateCartQuantity(${item.CartItemId}, ${item.Quantity + 1})">
                            <i class="fa-solid fa-plus"></i>
                        </button>
                        <button class="btn-remove" onclick="removeCartItem(${item.CartItemId})">
                            <i class="fa-solid fa-trash"></i>
                        </button>
                    </div>
                </div>
            </div>
        `;
    });

    cartItemsContainer.html(html);

    // Update summary
    $('#cartSubTotal').text(formatPrice(cartData.SubTotal) + ' đ');
    $('#cartShippingFee').text(formatPrice(cartData.ShippingFee) + ' đ');
    $('#cartTotal').text(formatPrice(cartData.Total) + ' đ');
    $('#cartCountBadge').text(cartData.TotalItems);

    updateCartCount(cartData.TotalItems);
}

// Thêm sản phẩm vào giỏ
function addToCart(productId, productName, productPrice, productImage) {
    $.ajax({
        url: '/Cart/AddToCart',
        type: 'POST',
        data: {
            productId: productId,
            quantity: 1
        },
        success: function (response) {
            if (response.success) {
                // Hiển thị thông báo
                showNotification('success', response.message);

                // Cập nhật số lượng giỏ hàng
                updateCartCount(response.totalItems);

                // Mở popup giỏ hàng
                openCartPopup();
            } else {
                showNotification('error', response.message);
            }
        },
        error: function (xhr, status, error) {
            showNotification('error', 'Có lỗi xảy ra. Vui lòng thử lại!');
            console.error('Lỗi:', error);
        }
    });
}

// Cập nhật số lượng
function updateCartQuantity(cartItemId, newQuantity) {
    if (newQuantity < 1) {
        removeCartItem(cartItemId);
        return;
    }

    $.ajax({
        url: '/Cart/UpdateQuantity',
        type: 'POST',
        data: {
            cartItemId: cartItemId,
            quantity: newQuantity
        },
        success: function (response) {
            if (response.success) {
                loadCartItems();
                showNotification('success', response.message);
            } else {
                showNotification('error', response.message);
            }
        },
        error: function (xhr, status, error) {
            showNotification('error', 'Có lỗi xảy ra!');
        }
    });
}

// Xóa sản phẩm
function removeCartItem(cartItemId) {
    if (!confirm('Bạn có chắc muốn xóa sản phẩm này?')) {
        return;
    }

    $.ajax({
        url: '/Cart/RemoveItem',
        type: 'POST',
        data: { cartItemId: cartItemId },
        success: function (response) {
            if (response.success) {
                loadCartItems();
                showNotification('success', response.message);
            } else {
                showNotification('error', response.message);
            }
        },
        error: function (xhr, status, error) {
            showNotification('error', 'Có lỗi xảy ra!');
        }
    });
}

// Thanh toán
function checkout() {
    window.location.href = '/Order/Checkout';
}

// Update cart count in header
function updateCartCount(count) {
    $('.cartShopping').attr('data-count', count);

    // Nếu có badge trong header
    var badge = $('.cart-badge');
    if (badge.length) {
        badge.text(count);
    }
}

// Format giá
function formatPrice(price) {
    return new Intl.NumberFormat('vi-VN').format(price);
}

// Hiển thị thông báo
function showNotification(type, message) {
    // Sử dụng alert tạm thời, sau này có thể dùng Toast notification
    if (type === 'success') {
        // Success notification
        var notification = $('<div class="notification success">' + message + '</div>');
    } else {
        // Error notification
        var notification = $('<div class="notification error">' + message + '</div>');
    }

    $('body').append(notification);

    setTimeout(function () {
        notification.fadeOut(function () {
            $(this).remove();
        });
    }, 3000);
}

// CSS cho notification
var notificationStyle = `
<style>
.notification {
    position: fixed;
    top: 20px;
    right: 20px;
    padding: 15px 25px;
    border-radius: 8px;
    color: white;
    font-weight: 600;
    z-index: 99999;
    animation: slideInRight 0.3s ease;
    box-shadow: 0 5px 20px rgba(0,0,0,0.3);
}

.notification.success {
    background: linear-gradient(135deg, #28a745 0%, #218838 100%);
}

.notification.error {
    background: linear-gradient(135deg, #dc3545 0%, #c82333 100%);
}

@keyframes slideInRight {
    from { transform: translateX(400px); opacity: 0; }
    to { transform: translateX(0); opacity: 1; }
}
</style>
`;

$('head').append(notificationStyle);

// Load cart count khi trang load
$(document).ready(function () {
    loadCartItems();

    // Bind event cho icon giỏ hàng trong header
    $('.cartShopping').click(function (e) {
        e.preventDefault();
        openCartPopup();
    });
});