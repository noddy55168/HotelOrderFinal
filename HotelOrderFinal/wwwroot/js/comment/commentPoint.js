console.log("point");
console.log("xyz1")
//製作一個星星評分
var divStars = document.getElementById('stars');
var score = document.getElementById('score');
const commentPoint = document.getElementById('CommentPoint');
var attitude = [1, 2, 3, 4, 5];
var starNum = -1; //記錄當前第幾顆星星被點擊
var starArray = Array.from(divStars.children); //星星數組
console.log("start1");
//滑鼠移入
divStars.onmouseover = function (e) {
    console.log("over1");
    if (e.target.tagName === "IMG") { //事件源是圖片
        //把滑鼠移動到的星星替換圖片
        e.target.src = "../image/comment/shining.png";
        //把滑鼠移動到的星星之前的星星替換圖片
        var prev = e.target.previousElementSibling;
        while (prev) {
            prev.src = "../image/comment/shining.png";
            prev = prev.previousElementSibling;
        }
        //把滑鼠移動到的星星之後的星星替換圖片
        var next = e.target.nextElementSibling;
        while (next) { //把滑鼠移動到的星星之後的星星替換圖片
            next.src = "../image/comment/empty.png";
            next = next.nextElementSibling;
        }
        var index = starArray.indexOf(e.target); //找到滑鼠移動到的星星的序號
        score.innerHTML = attitude[index]; //顯示對應的點數
        commentPoint.value = attitude[index]; //給對應的點數值
    }
}
//滑鼠點擊
divStars.onclick = function (e) {
    if (e.target.tagName === "IMG") {
        //記錄當前點擊的星星序號
        starNum = starArray.indexOf(e.target);
    }
}

//滑鼠移出
divStars.onmouseout = function (e) {
    if (starNum !== -1) { //滑鼠點擊事件發生，將評分固定在點擊的星星上
        for (var i = 0; i < divStars.children.length; i++) {
            if (i <= starNum) {
                divStars.children[i].src = "../image/comment/shining.png"
            } else {
                divStars.children[i].src = "../image/comment/empty.png";
            }
        }
        score.innerHTML = attitude[starNum]; //顯示對應的點數
        commentPoint.value = attitude[starNum]; //給對應的點數值
    } else {
        for (var i = 0; i < divStars.children.length; i++) {
            divStars.children[i].src = "../image/comment/empty.png";
        }
        score.innerHTML = 0; //不顯示點數
        commentPoint.value = 0; //給對應的點數值
    }
}
