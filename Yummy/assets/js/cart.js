var cartOpen = false;
var numberOfProducts = 0;

$("body").on("click", ".js-toggle-cart", toggleCart);
// $("body").on("click", ".js-remove-product", handleDelete);

function toggleCart(element) {
  element.preventDefault();
  if (cartOpen) {
    closeCart();
    return;
  }
  // Otherwise, open the cart and add the "open" class
  openCart();
  getCartItems();
}

function openCart() {
  cartOpen = true;
  $("body").addClass("open");
}

function closeCart() {
  cartOpen = false;
  $("body").removeClass("open");
}
function GetToken() {
  const storedToken = localStorage.getItem("jwtToken");
  if (storedToken == null) {
    // this._router.navigate(["回首頁"]);
    window.location.href = "LogIn.html"; // 刷新頁面，並導至首頁
  } else {
    // Parse the token to check if it's expired
    const tokenExpired = isTokenExpired(storedToken);
    // if (tokenExpired) {
    //   this.refreshData();
    // }
  }
  return { Authorization: "bearer " + storedToken };
}

function isTokenExpired(token) {
  const decodedToken = parseToken(token);
  const expirationDate = new Date(decodedToken.exp * 1000);
  const currentDate = new Date();

  return currentDate > expirationDate;
}

function parseToken(token) {
  return { exp: 1679027400 }; // Replace with actual decoded token data
}
// function removeProduct(itemId) {
//   //   // numberOfProducts--;
//   //   // $(this).closest(".js-cart-product").remove();
//   //   // $(".cart__footer .button").text("結帳 " + numberOfProducts);
//   //   // if (numberOfProducts === 0) {
//   //   //   $(".js-cart-empty").removeClass("hide");
//   //   // }
//   //   console.log(itemId);
//   getCartItems();
//   handleDelete(itemId);
// }
const addProduct = async (dessert) => {
  // cartOpen如果是false那就打開購物車
  if (!cartOpen) {
    openCart();
    await this.getCartItems();
  }

  //商品名稱是甜點的名稱
  // const dessert = this.dvm.find((item) => item.dessertId === dessertId);
  const productName = dessertId.dessertName;
  const dessertPrice = dessertId.unitPrice;
  const specificationId = dessert.specification.specificationId;
  const cartProducts = $(".js-cart-product h1");
  let productExists = false;

  // 檢查購物車的商品名稱，是否已存在於購物車中，已存在就不在重複添加
  cartProducts.each(function () {
    if ($(this).text() === productName) {
      productExists = true;
      const qtyElement = $(this).siblings(".qty");
      const newQty = parseInt(qtyElement.text()) + 1;
      qtyElement.text(newQty);
    }
  });
  const newQty = 1;
  this.addToCart(dessert.dessertId, specificationId, newQty);
  // Update the number of products in the cart
  numberOfProducts++;
  // $(".cart__footer .button").text("結帳 " + numberOfProducts);
  // Fetch the updated cart items and display them
};
const addToCart = async (dessertId, specificationId, newQty) => {
  const username = localStorage.getItem("username");
  const apiUrl = `${variables.API_URL}DessertCarts/AddToCart`;
  const params = new URLSearchParams({
    username: username,
    dessertId: dessertId,
    specificationId: specificationId,
    quantity: newQty,
  });

  await axios.post(apiUrl + "?" + params.toString(), config);
  console.log("Cart item added successfully!");
};
async function handleDelete(dessertCartItemId) {
  //var id = this.data("id");
  // console.log(dessertCartItemId);
  try {
    await axios.delete(
      `${variables.API_URL}DessertCartItems/${dessertCartItemId}`,
      config
    );
    console.log("Cart item deleted successfully!");
  } catch (error) {
    console.error("Error deleting cart item:", error);
  }
  getCartItems();
}
async function getCartItems() {
  const PhotoPath = "/assets/img/";
  const username = localStorage.getItem("username"); // Replace this with the actual username
  try {
    const response = await axios.get(
      `${variables.API_URL}DessertCarts?username=${username}`,
      config
    );
    console.log(response.data);
    const cartItems = response.data;
    const cartProductsContainer = $(".js-cart-products");
    cartProductsContainer.empty(); // Clear the existing cart items

    if (cartItems.length === 0) {
      // Display the cart empty message if there are no items
      cartProductsContainer.append(
        '<p class="cart__empty js-cart-empty">新增商品到購物車吧~</p>'
      );
    } else {
      cartItems.forEach((item) => {
        const cartProductTemplate = `
        <div class="cart__product"  v-for="item in cartItems" :key="item.id">
        <article class="js-cart-product">
        <div class="d-flex">
        <img class="menu-img img-fluid" src="/assets/img/${item.dessertImage}" style="
        height: 70px; width: 70px;  margin-right:15px "/>
         <h1 class="product_name ">${item.dessertName}</h1> </div>
          <div class="cart__product__qty">
          數量: <span class="qty">${item.count}</span> * NT
          <span class="dessertPrice">${item.price}</span>         
            <a class="js-remove-product" href="#" title="Delete product" onclick="handleDelete(${item.dessertCartItemId})" >
              刪除
            </a>
           </div>
        </article>
      </div>
        `;
        cartProductsContainer.append(cartProductTemplate);
      });
    }
    numberOfProducts = cartItems.length;

    $(".cart__footer .button").text("結帳 " + numberOfProducts);
    $(".shoppingCart").text(numberOfProducts);
  } catch (error) {
    console.error("Error getting cart items:", error);
  }
}
