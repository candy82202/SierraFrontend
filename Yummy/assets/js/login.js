// 渲染出登入按鈕，若已登入則渲染出頭像(裡面包含使用者名稱,會員中心,重設密碼,登出)
function renderLoginAndLogoutButton() {
  let userElementsHTML = `
    <a id="login-link" class="btn-book-a-table" href="LogIn.html">登入</a>
 
    <div id="user-dropdown" class="dropdown">
      <img src="" alt="會員頭像" class="dropdown-toggle" id="user-image"
        data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
      <ul id="user-dropdown-menu" class="dropdown-menu dropdown-menu-end" aria-labelledby="user-dropdown-menu">
        <li class="dropdown-header fs-6 fw-bolder" id="user-greeting">你好, </li>
        <hr>
        <li><a class="user-dropdown-item" href="MemberCenter.html"><i class="bi bi-person"></i>會員中心</a></li>
        <li><a class="user-dropdown-item" href="Order.html"><i class="bi bi-key"></i>訂單查詢</a></li>
        <li><a class="user-dropdown-item" href="EditPassword.html"><i class="bi bi-key"></i>變更密碼</a></li>
        <hr>
        <li><a class="user-dropdown-item" href="#" onclick="logout()"><i class="bi bi-box-arrow-right"></i>登出</a></li>
      </ul>
    </div>`;

  const nav = document.querySelector(".shoppingCartDiv");
  nav.insertAdjacentHTML("afterend", userElementsHTML);
}

renderLoginAndLogoutButton();

// 檢查是否為登入狀態
function isLoggedIn() {
  return localStorage.getItem("jwtToken") !== null;
}

// 若登入，隱藏登入按鈕；顯示使用者頭像(內含登出按鈕)
// const cartLink = document.querySelector("#cart-link");
const loginLink = document.getElementById("login-link");
const shoppingCart = document.querySelector(".shoppingCartDiv");
const userGreeting = document.querySelector("#user-greeting");
const userDropdownLink = document.querySelector("#user-dropdown");
const userImage = document.querySelector("#user-image");
if (isLoggedIn()) {
  loginLink.classList.add("d-none");
  userDropdownLink.classList.remove("d-none");
  shoppingCart.classList.remove("d-none");
  // cartLink.classList.remove('d-none');
  userGreeting.innerHTML += localStorage.getItem("username");
  userImage.src = localStorage.getItem("imageUrl");
} else {
  loginLink.classList.remove("d-none");
  userDropdownLink.classList.add("d-none");
  shoppingCart.classList.add("d-none");
  // cartLink.classList.add('d-none');
  userImage.src = "";
}

// 引入Google登入相關套件
var gsiScript = document.createElement("script");
gsiScript.src = "https://accounts.google.com/gsi/client";
gsiScript.async = true;
gsiScript.defer = true;
document.head.appendChild(gsiScript);

var apiClientScript = document.createElement("script");
apiClientScript.src = "https://apis.google.com/js/api:client.js";
apiClientScript.async = true;
apiClientScript.defer = true;
document.head.appendChild(apiClientScript);

// 登出時清除localStorage的東西，以及Google登入的東西
async function logout() {
  localStorage.removeItem("jwtToken");
  localStorage.removeItem("memberId");
  localStorage.removeItem("username");
  localStorage.removeItem("imageUrl");
  // 用Google登入後，登出時要清掉的東西
  // ref:https://developers.google.com/identity/gsi/web/reference/js-reference#google.accounts.id.disableAutoSelect
  // ref:https://developers.google.com/identity/gsi/web/reference/js-reference#google.accounts.id.revoke

  let googleAccountId = localStorage.getItem("googleAccountId");
  if (googleAccountId != null) {
    google.accounts.id.disableAutoSelect();
    google.accounts.id.revoke(googleAccountId, (done) => {
      console.log(done.error);
    });
  }

  window.location.href = "index.html"; // 導至首頁
}

function showPassword(iconElement) {
  var passwordInput = iconElement.nextElementSibling;
  var toggleIcon = iconElement.querySelector("i");
  passwordInput.type = "text";
  toggleIcon.classList.replace("fa-eye-slash", "fa-eye");
}

function hidePassword(iconElement) {
  var passwordInput = iconElement.nextElementSibling;
  var toggleIcon = iconElement.querySelector("i");
  passwordInput.type = "password";
  toggleIcon.classList.replace("fa-eye", "fa-eye-slash");
}

// Testing swal
// swalLoading()
// swalSuccess("")
function swalLoading() {
  Swal.fire({
    title: "等待請求中...",
    text: "  ",
    imageUrl: "assets/img/loading.gif", // 替換成圖片的路徑
    imageAlt: "Loading Image", // 圖片的替代文字
    showCancelButton: false,
    showConfirmButton: false,
    allowOutsideClick: false,
    allowEscapeKey: false,
    allowEnterKey: false,
  });
}

function swalSuccess(message) {
  Swal.fire({
    title: "成功!",
    text: message,
    icon: "success",
    showCancelButton: false, // 不顯示取消按鈕
    confirmButtonText: "確定", // 更改確定按鈕文字
  }).then((result) => {
    if (result.isConfirmed) {
      location.reload(); // 刷新頁面
    }
  });
}

// 將 DataTime 轉成 YYYY-MM-DD HH:MM 格式
function formatToDateTime(date) {
  const DT = new Date(date);
  const year = DT.getFullYear();
  const month = String(DT.getMonth() + 1).padStart(2, "0");
  const day = String(DT.getDate()).padStart(2, "0");
  const hours = String(DT.getHours()).padStart(2, "0");
  const minutes = String(DT.getMinutes()).padStart(2, "0");

  return `${year}-${month}-${day} ${hours}:${minutes}`;
}

// 將 DateTime 轉成 YYYY-MM-DD 格式
function formatToDate(date) {
  const DT = new Date(date);
  const year = DT.getFullYear();
  const month = String(DT.getMonth() + 1).padStart(2, "0");
  const day = String(DT.getDate()).padStart(2, "0");

  return `${year}-${month}-${day}`;
}

let config = {
  headers: {
    Authorization: `Bearer ${localStorage.getItem("jwtToken")}`,
  },
};
