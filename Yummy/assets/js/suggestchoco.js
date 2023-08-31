const suggestChoco = {
  template: `   
    <div  v-for="dessert in dessertDiscountList" :key="dessert.dessertId">
    <a class="product-box">
      <span class="img">
        <span :style="{ backgroundImage: 'url(' + PhotoPath + dessert.dessertImageName + ')' }" class="i first"></span>
        <span class="i second" style="background-image: url('assets/img/blueberry.jpg');"></span>
      </span>
      <span class="text">
        <strong>{{ dessert.dessertName }}</strong>
        <span> $ {{ dessert.unitPrice }} </span>
        <div class="variants">
          <div class="variant">
            <div class="var available">
              <div>{{ dessert.specification.size }}</div>
            </div>      
          </div>
          <div class="variant">
            <div class="var available">
              <div>{{ dessert.specification.flavor }}</div>
            </div>    
          </div>
        </div>
      </span>
    </a>
  </div>`,
  data() {
    return {
      dessertDiscountList: [],
      dessertId: 0,
      dessertName: "",
      dessertImageName: "",
      unitPrice: 0,
      flavor: "",
      size: "",
      PhotoPath: "/assets/img/",
      specification: [],
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
    getDessertUrl(dessertId) {
      return variables.API_URL + "Desserts/ChocoDiscountGroups";
    },
    goToDessertDetails(dessertId) {
      window.location.href = this.getDessertUrl(dessertId);
    },

    refreshData() {
      axios
        .get(variables.API_URL + "Desserts/ChocoDiscountGroups")
        .then((response) => {
          //console.log(response.data);
          this.dessertDiscountList = response.data; // Assign the fetched desserts to the longCake data property
        });
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
        <img class="menu-img img-fluid" src="/assets/img/${
          item.dessertImage
        }" style="
        height: 70px; width: 70px;   margin-right:15px "/>
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
  },
  mounted: function () {
    this.refreshData();
  },
};
