let cartItemsDiv;


//頁面載入時載入購物車內容
$(document).ready(function () {
    loadCartFromSession();
    loadActivitySession();
    loadDiscount();
});

function loadCartFromSession() {
    fetch('/Order/GetShopCartOrderDetailSession', {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' },
    })
        .then(response => response.json())
        .then(cartData => {
            const cartItemsDiv = $('#SessionRoomList');
            cartItemsDiv.empty();

            if (cartData !== null) {
                let totalPrice = 0;
                const headerRow = $('<tr></tr>').css('background-color', '#f0f0f0');
                headerRow.append($('<th></th>').attr('width', '20%').text('住宿時間'));
                headerRow.append($('<th></th>').attr('width', '20%').text('飯店名稱'));
                headerRow.append($('<th></th>').attr('width', '20%').text('房間類型'));
                headerRow.append($('<th></th>').attr('width', '15%').text('住房人數'));
                headerRow.append($('<th></th>').attr('width', '15%').text('小計'));
                headerRow.append($('<th></th>').attr('width', '10%').text('刪除'));

                cartItemsDiv.append(headerRow);
                cartData.forEach((item, index) => {

                    const itemRow = $('<tr></tr>');
                    const dateCell = $('<td></td>');
                    dateCell.append($('<p></p>').text('入住日期'));
                    const formattedCheckInDate = new Date(item.checkInDate).toLocaleDateString('zh-TW', { year: 'numeric', month: '2-digit', day: '2-digit' });
                    dateCell.append($('<p></p>').text(formattedCheckInDate));
                    dateCell.append($('<p></p>').text('退房日期'));
                    const formattedCheckOutDate = new Date(item.checkOutDate).toLocaleDateString('zh-TW', { year: 'numeric', month: '2-digit', day: '2-digit' });
                    dateCell.append($('<p></p>').text(formattedCheckOutDate));
                    itemRow.append(dateCell);

                    itemRow.append($('<td></td>').text(item.hotelName));
                    itemRow.append($('<td></td>').text(item.roomClassName));
                    //住宿人數為下拉選單, 上限+2
                    const selectroomPeople = $('<td></td>');
                    const select = $('<select></select>');
                    //const select = $('<select></select>').attr('id', 'roomPeople').attr('name', 'roomPeople');


                    // 建立下拉式選單選項
                    for (let i = item.roomClassPeople; i <= item.roomClassPeople + 2; i++) {
                        const option = $('<option></option>').text(i);

                        // 設定預設選項
                        if (i === item.roomClassPeople) {
                            option.attr('selected', 'selected');
                        }

                        select.append(option);
                    }
                    // 設定唯一的id和name屬性值
                    //const uniqueId = 'roomPeople-' + index;
                    //select.attr('id', uniqueId).attr('name', uniqueId);

                    // 監聽下拉選單變更事件
                    select.change(() => {
                        const selectedPeople = parseInt(select.val());
                        let price;

                        if (selectedPeople <= item.roomClassPeople) {
                            price = item.weekdayPrice;
                        } else {
                            const additionalPeople = selectedPeople - item.roomClassPeople;
                            price = item.weekdayPrice + item.addPrice * additionalPeople;
                        }

                        priceCell.text('NT$ ' + price);

                        //更新總金額
                        calculateTotalPrice();
                    });
                    //顯示入住人數
                    selectroomPeople.append(select);
                    itemRow.append(selectroomPeople);

                    //顯示房間金額
                    const priceCell = $('<td></td>').text('NT$ ' + item.weekdayPrice);
                    itemRow.append(priceCell);


                    //偶數列底色變色
                    if (index % 2 === 1) {
                        itemRow.addClass('even-row');
                    }

                    const deleteCell = $('<td></td>');
                    const deleteButton = $('<button></button>').text('刪除').addClass('delete-button');
                    const itemId = 'item-' + item.fId;
                    itemRow.attr('data-item-id', itemId);

                    //點擊刪除按鈕
                    deleteButton.click(() => {

                        //跳出確認框
                        if (confirm('確定要刪除此項目嗎？')) {

                            //抓取刪除列的FId
                            const rowToDelete = cartItemsDiv.find('[data-item-id="' + itemId + '"]');

                            //刪除價格
                            const itemPrice = item.weekdayPrice;
                            totalPrice -= itemPrice;

                            rowToDelete.remove();
                            const extractedFId = itemId.split('-')[1];
                            //執行刪除function
                            deleteSessionData(extractedFId);

                            //更新總金額
                            //const totalPriceCell = $('<td></td>').addClass('text-right').text('NT$ ' + totalPrice);
                            //$('#OrderDetailTotalPrice').empty().append(totalPriceCell);
                            //const totalPriceCell = $('#OrderDetailTotalPrice');
                            //totalPriceCell.text('NT$ ' + totalPrice);
                            //showDiscount(totalPrice);

                            calculateTotalPrice();
                            console.log("刪除房間有執行到嗎");

                            // 購物車內容為空，回到訂房首頁(目前無作用)
                            //if (totalPriceCell === 0) {
                            //    window.location.href = "/Order/List";
                            //    return;
                            //}
                        }
                    });
                    deleteCell.append(deleteButton);
                    itemRow.append(deleteCell);

                    cartItemsDiv.append(itemRow);

                    totalPrice += item.weekdayPrice


                });
                //const totalPriceCell = $('<td></td>').addClass('text-right').text('NT$ ' + totalPrice);
                //$('#OrderDetailTotalPrice').empty().append(totalPriceCell);

                //一開始的房間列表的總額
                const totalPriceCell = $('#OrderDetailTotalPrice');
                totalPriceCell.text('NT$ ' + totalPrice);
                showDiscount(totalPrice);

                ////呼叫總額計算
                //calculateTotalPrice()
            }
        });
}

//刪除session內的房間
function deleteSessionData(FId) {
    $.ajax({
        url: '/Order/DeleteSessionData',
        method: 'POST',
        data: { FId: FId },
    });
}

// 計算總金額函數
function calculateTotalPrice() {
    let totalPrice = 0;
    let 房間列表總額 = 0;
    let 活動總額 = 0;

    //抓房間列表的小計欄位
    $('#SessionRoomList').find('tr').each((index, row) => {
        const priceText = $(row).find('td:nth-child(5)').text();//抓第5列
        const roomPrice = parseInt(priceText.substring(4)); // 去除 "NT$ "

        //扣掉標題的內容
        if (!isNaN(roomPrice)) {
            房間列表總額 += roomPrice;
            console.log("去掉標題內容", 房間列表總額);
        }
    });

    //抓活動的小計欄位
    $('#ActivityList').find('tr').each((index, row) => {
        const CostText = $(row).find('td:nth-child(4)').text();
        const actPrice = parseInt(CostText.substring(4));
        console.log("2", actPrice);
        //扣掉標題的內容
        if (!isNaN(actPrice)) {
            活動總額 += actPrice;
            console.log("2", 活動總額);
        }
    });

    totalPrice = 房間列表總額 + 活動總額;

    const totalPriceCell = $('#OrderDetailTotalPrice');
    totalPriceCell.text('NT$ ' + totalPrice);
    showDiscount(totalPrice);
}

function showDiscount(price) {
    // $('#discountRow').show().text("abcd");
    var selectedDiscount = $('#DiscountDiscount').val()
    // var totalPrice = parseFloat($('#OrderDetailTotalPrice').text().replace('NT$ ', '')); // 獲取總金額數字
    let totalPrice = price;
    //取得discountId
    //根據discountId透過Ajax去伺服器端讀取此Id的discount

    //console.log("discount",selectedDiscount);
    //console.log("total", totalPrice);
    if (selectedDiscount !== null && selectedDiscount !== "") {
        var discountAmount = Math.floor(totalPrice * selectedDiscount);
        $('#discountRow').show(); // 顯示優惠券計算內容
        $('#discountAmount').text('NT$ ' + discountAmount); // 顯示折扣金額
    } else {
        $('#discountRow').hide(); // 隱藏優惠卷計算內容
    }
}

//當優惠券選擇改變時觸發事件
$('#DiscountDiscount').change(function () {
    calculateTotalPrice();
});

//獲得活動資料
function loadActivitySession() {
    fetch('/Order/GetActivitySession', {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' },
    })
        .then(response => response.json())
        .then(item => {
            const activityListDiv = document.getElementById("ActivityList");

            console.log(item)
            var html = "<div class='area'><div><h2>活動列表</h2><div class='row'><table style='box-sizing: border-box; width:100%; text-align:center;'><tbody><tr style='background-color: #f0f0f0;'><th width='25%'>活動日期</th><th width='30%'>活動名稱</th><th width='25%'>參加人數/剩餘名額</th><th width='20%'>小計</th></tr>";
            //const datas=JSON.parse(data);

            html += "<tr>";
            //html += "<td>" + item.activityTime + "</td>";
            const formattedActivityTime = new Date(item.activityTime).toLocaleDateString('zh-TW', { year: 'numeric', month: '2-digit', day: '2-digit' });

            html += "<td>" + formattedActivityTime + "</td>";

            html += "<td>" + item.activityName + "</td>";

            html += "<td>";
            html += "<select id='ActivityPeople'name='ActivityPeople'>";
            var remainingPeople = item.activityPeople - item.活動參加總人數;

            for (var i = 0; i <= remainingPeople; i++) {
                html += "<option value='" + i + "'>" + i + "</option>";
            }
            html += "</select>";
            html += " /  " + remainingPeople + "</td>";

            html += "<td>NT$ <span id='totalCost'>" + 0 + "</span></td>";
            //html += "<td>NT$ " + totalPrice + "</td>";

            html += "</tr>";

            html += "</tbody></table></div></div>";
            activityListDiv.innerHTML = html;

            document.getElementById('ActivityPeople').addEventListener('change', calculateActSelecttotal);

            //活動人數變更計算金額
            function calculateActSelecttotal() {
                var selectedPeople = parseInt(document.getElementById('ActivityPeople').value);
                var totalCost = selectedPeople * item.activityCost;
                document.getElementById('totalCost').textContent = totalCost
                console.log("4", totalCost);
                //更新到總金額內
                //if (!isNaN(totalCost) && totalCost > 0) {
                //    calculateTotalPrice(totalCost);
                //    console.log("5");
                //}
                calculateTotalPrice();

                //const 總金額字串 = calculateTotalPrice();
                //const 總金額 = parseInt(總金額字串.substring(4));

            };

            //初始執行計算
            //calculateActSelecttotal();
        });
}

//讀取優惠卷數據
function loadDiscount() {
    fetch('/Order/GetDiscountId', {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' },
    })
        .then(response => response.json())
        .then(data => {
            var selectElement = document.getElementById("Discount");
            console.log(data);
            selectElement.innerHTML = "";

            data.forEach(item => {
                var optionHtml = "<select id='DiscountDiscount' name='DiscountDiscount'><option value='" + item.discountDiscount + "'>" + item.discountName + "</option></select> ";
                console.log(optionHtml);
                selectElement.innerHTML += optionHtml;
            });
            calculateTotalPrice()
        });
}




//獲得參數(未完成)
function getOrderDetailValue() {
    var activitypeople = document.getElementById("ActivityPeople").value;
    var discountdiscount = document.getElementById("DiscountDiscount").value;
    var txtremark = document.getElementById("txtRemark").value;
    var roomPeopleElements = document.querySelectorAll('[name="roomPeople"]');
    var roomPeopleValues = [];

    roomPeopleElements.forEach(function (element) {
        roomPeopleValues.push(element.value);
    });

    console.log('activitypeople', activitypeople);
    console.log('discountdiscount', discountdiscount);
    console.log('txtremark', txtremark);
    //console.log('roomPeople', roomPeopleValues);

    var data = {
        activitypeople: activitypeople,
        discountdiscount: discountdiscount,
        txtremark: txtremark,
        roomPeople: roomPeopleValues
    };

    fetch('/your/backend/url', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    })
}
