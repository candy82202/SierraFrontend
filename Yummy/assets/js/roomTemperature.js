const roomTemperature = {
  template: `
  <div class="row gy-5">
  <div
    class="col-lg-3 menu-item"
    v-for="dessert in dvm"
    :key="dessert.dessertId"
  >
    <div class="card card1 border-0 mb-3">
    <div class="card-img h200px overflow-hidden object-fit-cover">
    <router-link :to="getDessertLink(dessert.dessertId)" class="cardlink text-decoration-none text-dark stretched-link">
    <img class="menu-img img-fluid" :src="PhotoPath + dessert.dessertImageName" />
    </router-link>
      </div> 
      <div class="card-body">
        <a
        :href="dessertLinkURL + dessert.dessertId"
          class="cardlink text-decoration-none text-dark stretched-link"
        >
          <h4 class="card-title">{{ dessert.dessertName }}</h4>
          <h4 class="card-text price" v-if="dessert.unitPrice == dessert.dessertDiscountPrice">$ {{ dessert.unitPrice }}</h4>
          <h4 class="text-danger" v-else> <span class="text-dark text-decoration-line-through">  $ {{ dessert.unitPrice }} </span>$ {{dessert.dessertDiscountPrice}}</h4>
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
            `,
  data() {
    return {
      dvm: [],
      dessertName: "",
      dessertImageName: "",
      unitPrice: 0,
      flavor: "",
      size: "",
      specification: [],
      PhotoPath: "/assets/img/",
      dessertLinkURL: "dessertProducts.html#/desserts/",
      cartItems: [],
      specificationId: 0,
      selectedSpecificationId: 0,
      dessertDiscountPrice: 0,
    };
  },
  computed: {
    config() {
      return {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("jwtToken")}`,
        },
      };
    },
  },
  methods: {
    refreshData() {
      axios
        .get(variables.API_URL + "Desserts/roomTemperature")
        .then((response) => {
          // console.log(response.data);
          this.dvm = response.data; // Assign the fetched desserts to the longCake data property
        });
      this.getCartItems();
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
        // console.log(dessertData);
      } catch (error) {
        console.error("Error fetching dessert data:", error);
      }

      // Return the link to be used in the anchor tag's href attribute
    },
    async addProduct(dessert) {
      this.GetToken();

      //商品名稱是甜點的名稱
      // const dessert = this.dvm.find((item) => item.dessertId === dessertId);
      const productName = dessert.dessertName;
      const dessertPrice = dessert.unitPrice;
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

      await axios.post(apiUrl + "?" + params.toString(), null, this.config);
      await this.getCartItems();
      console.log("Cart item added successfully!");
    },

    async getCartItems() {
      const username = localStorage.getItem("username"); // Replace this with the actual username
      try {
        const response = await axios.get(
          `${variables.API_URL}DessertCarts?username=${username}`,
          this.config
        );

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
        height: 70px; width: 70px;   margin-right:15px "/>
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
    },
    async handleDelete(item) {
      try {
        await axios.delete(
          `${variables.API_URL}DessertCartItems/${dessertCartItemId}`,
          this.config
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
      const decodedToken = parseToken(token);
      const expirationDate = new Date(decodedToken.exp * 1000);
      const currentDate = new Date();

      return currentDate > expirationDate;
    },

    parseToken(token) {
      // Replace this with your actual token parsing logic
      // This function should decode the token and return its contents
      // For example, using JWT library: jwt_decode(token);
      // For the purpose of this example, let's assume it returns an object with 'exp' property
      return { exp: 1679027400 }; // Replace with actual decoded token data
    },
  },
  mounted: function () {
    this.refreshData();
    // this.getCartItems();
  },
};
