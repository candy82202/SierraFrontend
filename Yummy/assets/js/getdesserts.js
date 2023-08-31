const getdesserts = {
  template: `
  <div class="row gy-5">
  <div class="col-12">
  <input
  type="search"
  v-model="keyword"
  class="form-control mb-3"
  placeholder="請輸入甜點名稱"
  style="width:200px ; float:right"
  @input="inputHandler"
/>
</div> 
  <div
    class="col-lg-3 menu-item"
    v-for="dessert in products"
    :key="dessert.dessertId"
  >
    <div class="card card1 border-0 mb-3">
    <div class="card-img h200px overflow-hidden object-fit-cover">
    <router-link :to="getDessertLink(dessert.dessertId)" class="cardlink text-decoration-none text-dark stretched-link">
    <img class="menu-img img-fluid" :src="PhotoPath + dessert.imageName" />
    </router-link>
      </div> 
      <div class="card-body">
        <a
        :href="dessertLinkURL + dessert.dessertId"
          class="cardlink text-decoration-none text-dark stretched-link"
        >
          <h4 class="card-title">{{ dessert.dessertName }}</h4>
           <h4 class="card-text price" v-if="dessert.unitPrice == dessert.discountedPrice">$ {{ dessert.unitPrice }}</h4>
           <h4 class="text-danger" v-else> <span class="text-dark text-decoration-line-through">  $ {{ dessert.unitPrice }} </span>$ {{dessert.discountedPrice}}</h4>
     
        </a> 
        <div class="card-footer bg-white border-top-0 d-flex">

          <a class="button js-add-product" :to=this.router title="Add to cart" style="z-index:1" @click.stop="addProduct(dessert)"   >
          加入購物車
        </a>
            </div>   
      </div>
    </div>
  </div>
</div>
<a
  href="#"
  class="scroll-top d-flex align-items-center justify-content-center"
  ><i class="bi bi-arrow-up-short"></i
></a>
<div class="col-12">
<nav aria-label="Page navigation example"  style=" float:right">
  <ul class="pagination">
      <li class="page-item " v-for="(value,index) in totalPages" :key="index" @click="clickHandler(value)"><a :class="{ 'dessertPage': true,'page-link':true, 'currentPage': thePage === value }">{{value}}</a>
      </li>     
  </ul>
</nav></div>
`,
  data() {
    return {
      keyword: this.$route.params.keyword || "", // 從路由參數獲取關鍵字，如果沒有則為空字串
      products: [], // 儲存甜點資料
      totalPages: 0,
      PhotoPath: "/assets/img/",
      dessertLinkURL: "dessertProducts.html#/desserts/",
      thePage: 1, //第幾頁
    };
  },
  watch: {
    keyword: {
      handler: function (newKeyword) {
        console.log(newKeyword);
        this.loadProducts(newKeyword);
      },
      immediate: true, // 如果希望在初始化時也觸發 watch 事件，請加上這個選項
    },
  },
  methods: {
    async loadProducts(keyword) {
      const response = await fetch(
        `https://localhost:520/api/Desserts?keyword=${keyword}&page=${this.thePage}&pageSize=8`
      );
      const data = await response.json();
      this.products = data;
      // console.log(this.products);
      this.totalPages = data[0].totalPages;
      // console.log(data[0].totalPages);
      this.getCartItems();
    },
    async reloadProducts() {
      let apiUrl = `https://localhost:520/api/Desserts?page=${this.thePage}&pageSize=8`;
      if (this.keyword) {
        apiUrl += `&keyword=${this.keyword}`;
      }
      const response = await fetch(apiUrl);
      const data = await response.json();
      this.products = data;
      console.log(this.products);
      this.totalPages = data[0].totalPages;
      console.log(data[0].totalPages);
    },
    async getDessertLink(dessertId) {
      try {
        const response = await axios.get(
          variables.API_URL + `Desserts/${dessertId}`
        );
        const dessertLink = `dessertProducts.html#/desserts/${dessertId}`;
        console.log(dessertLink);
        return dessertLink;
        // const dessertData = response.data;
        // // Now you can use the dessertData to display the information on dessertProducts.html
        // console.log(dessertData);
      } catch (error) {
        console.error("Error fetching dessert data:", error);
      }
    },
    async inputHandler() {
      this.loadProducts(value);
    },
    async clickHandler(page) {
      this.thePage = page;
      console.log(this.thePage);
      this.reloadProducts();
    },
    async addProduct(dessert) {
      this.GetToken();

      //商品名稱是甜點的名稱
      // const dessert = this.dvm.find((item) => item.dessertId === dessertId);
      const productName = dessert.dessertName;
      const dessertPrice = dessert.unitPrice;
      const specificationId = dessert.specificationId;
      const cartProducts = $(".js-cart-product h1");
      let productExists = false;

      // 檢查購物車的商品名稱，是否已存在於購物車中，已存在就不在重複添加
      cartProducts.each(function () {
        if ($(this).text() === productName) {
          productExists = true;
          const qtyElement = $(this).siblings(".qty");
          const newQty = parseInt(qtyElement.text()) + 1;
          qtyElement.text(newQty);

          //return false;
        }
      });
      const newQty = 1;
      this.addToCart(dessert.dessertId, specificationId, newQty);
      // Update the number of products in the cart
      numberOfProducts++;
      $(".cart__footer .button").text("結帳 " + numberOfProducts);
      // Fetch the updated cart items and display them
      if (!cartOpen) {
        openCart();
      }
    },
    async addToCart(dessertId, specificationId, newQty) {
      const username = localStorage.getItem("username");

      const apiUrl = `${variables.API_URL}DessertCarts/AddToCart`;
      const params = new URLSearchParams({
        username: username,
        dessertId: dessertId,
        specificationId: specificationId,
        quantity: newQty,
      });

      await axios.post(apiUrl + "?" + params.toString(), null, config);
      await this.getCartItems();
      console.log("Cart item added successfully!");
    },
    async getCartItems() {
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
            <img class="menu-img img-fluid" src="/assets/img/${
              item.dessertImage
            }" style="
            height: 70px; width: 70px;  margin-right:15px  "/>
            <h1 class="product_name ">${item.dessertName}</h1>
            ${
              item.flavor !== null
                ? `<h1 class="product_name"> -${item.flavor} </h1>`
                : ""
            }
            ${
              item.size !== null
                ? `<h1 class="product_name">-${item.size}</h1>`
                : ""
            }
            </div>
              <div class="cart__product__qty">
              數量: <span class="qty">${item.count}</span> * NT
              <span class="dessertPrice">${item.price}</span>         
                <a class="js-remove-product" href="#" title="Delete product" onclick="handleDelete(${
                  item.dessertCartItemId
                })" >
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
    },
    async handleDelete(item) {
      try {
        await axios.delete(
          `${variables.API_URL}DessertCartItems/${dessertCartItemId}`
        );
        console.log("Cart item deleted successfully!");
      } catch (error) {
        console.error("Error deleting cart item:", error);
      }
      this.getCartItems();
    },

    GetToken() {
      const storedToken = localStorage.getItem("jwtToken");
      if (storedToken == null) {
        // this._router.navigate(["回首頁"]);
        window.location.href = "LogIn.html"; // 刷新頁面，並導至首頁
      } else {
        // Parse the token to check if it's expired
        const tokenExpired = isTokenExpired(storedToken);
        //如果token過期刷新重新抓取token
        // if (tokenExpired) {
        //   this.refreshData();
        // }
      }
      return { Authorization: "Bearer " + storedToken };
    },

    isTokenExpired(token) {
      // Parse the token to get the expiration date (replace this with your actual parsing logic)
      const decodedToken = this.parseToken(token);
      const expirationDate = new Date(decodedToken.exp * 1000);
      const currentDate = new Date();

      return currentDate > expirationDate;
    },
  },
  mounted() {
    this.loadProducts(this.keyword);
  },
};
