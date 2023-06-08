// 獲取當前日期
var today = new Date();
var tomorrow = new Date(today);
tomorrow.setDate(tomorrow.getDate() + 1); // 設定退房日為明天

// 設定入住日期欄位的屬性
var checkInDateInput = document.getElementById("checkInDate");
if (!checkInDateInput.value) {
    checkInDateInput.value = formatDate(today); // 設定起始日為今天
}
checkInDateInput.min = formatDate(today); // 設定最小可選日期
// 設定退房日期欄位的屬性
var checkOutDateInput = document.getElementById("checkOutDate");
if (!checkOutDateInput.value) {
    checkOutDateInput.value = formatDate(tomorrow)
}
checkOutDateInput.min = formatDate(tomorrow); // 設定最小可選日期

// 當入住日期發生變化時觸發
checkInDateInput.addEventListener("change", function () {
    var checkInDate = new Date(this.value);
    var checkOutDate = new Date(checkInDate);
    checkOutDate.setDate(checkOutDate.getDate() + 1); // 設定退房日期為入住日期的下一天
    checkOutDateInput.value = formatDate(checkOutDate);  // 退房日期自動變更
    // 更新退房日期欄位的屬性
    checkOutDateInput.min = formatDate(checkOutDate); // 設定最小可選日期
});

// 將日期格式化為YYYY-MM-DD的字串
function formatDate(date) {
    var year = date.getFullYear();
    var month = ("0" + (date.getMonth() + 1)).slice(-2);
    var day = ("0" + date.getDate()).slice(-2);
    return year + "-" + month + "-" + day;
}
