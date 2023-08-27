const choco = {
  template: `
  <div class="row gy-5">
  <div
    class="col-lg-3 menu-item"
    v-for="dessert in dessertDiscountList"
    :key="dessert.dessertId"
  >
    <div class="card card1 border-0 mb-3">
      <div class="card-img h200px overflow-hidden object-fit-cover">
      <router-link :to="{ path: '/desserts/' + dessert.dessertId }">
      <img
      class="menu-img img-fluid"
      :src="PhotoPath+dessert.dessertImageName"
    />
    </router-link>
      </div>
      <div class="card-body">
      <a
      :href="dessertLinkURL + dessert.dessertId"
        class="cardlink text-decoration-none text-dark stretched-link">
      <h4 class="card-title">{{ dessert.dessertName }}</h4>
      <h4 class="card-text price">$ {{ dessert.unitPrice }}</h4>
   </a>
        <div class="card-footer bg-white border-top-0 d-flex">
      
        <a class="button js-add-product" href="#" title="Add to cart" style="z-index:1" @click.stop="addProduct(dessert)"   >
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
      dessertDiscountList: [],
      dessertId: 0,
      dessertName: "",
      dessertImageName: "",
      unitPrice: 0,
      flavor: "",
      size: "",
      specification: [],
      PhotoPath: "/assets/img/",
      dessertLinkURL: "dessertProducts.html#/desserts/",
      discount: 0.8,
      cartItems: [],
      specificationId: 0,
      selectedSpecificationId: 0,
      dessertDiscountPrice: 0,
    };
  },
  computed: {
    // Compute the final discounted price for each dessert
    dessertWithFinalPrice() {
      return this.dessertDiscountList.map((dessert) => ({
        final_price: parseInt(dessert.unitPrice * this.discount),
      }));
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
          console.log(response.data);
        });
    },
    async addProduct(dessert) {
      // cartOpen如果是false那就打開購物車
      if (!cartOpen) {
        openCart();
        await this.getCartItems();
      }

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
        }
      });
      const newQty = 1;
      this.addToCart(dessert.dessertId, specificationId, newQty);
      // if (!productExists) {
      //   $(".js-cart-empty").addClass("hide");
      //   var product = $(".js-cart-product-template").html();
      //   $(".js-cart-products").prepend(product);
      //   $(".js-cart-product h1").first().text(productName);
      //   $(".js-cart-product .dessertPrice").first().text(dessertPrice);
      //   const newQty = 1;
      //   $(".js-cart-product span").first().text(newQty);

      //   this.addToCart(dessert.dessertId, specificationId, newQty);
      // }
      // Update the number of products in the cart
      numberOfProducts++;
      $(".cart__footer .button").text("結帳 " + numberOfProducts);
      // Fetch the updated cart items and display them
      await this.getCartItems();
      // Update the number of products in the cart
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

      await axios.post(apiUrl + "?" + params.toString());
      console.log("Cart item added successfully!");
    },

    async getCartItems() {
      const username = "danny"; // Replace this with the actual username
      try {
        const response = await axios.get(
          `${variables.API_URL}DessertCarts?username=${username}`,
          config
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
          // Display the cart items
          cartItems.forEach((item) => {
            const cartProductTemplate = `
              <div class="cart__product js-cart-product-template">
                <article class="js-cart-product">
                  <h1 class="product_name">${item.dessertName}</h1>
                  數量: <span class="qty">${item.quantity}</span> * NT
                  <span class="dessertPrice">${item.unitPrice}</span>
                  <p>
                    <a class="js-remove-product" href="#" title="Delete product" @click="removeProduct(item.id)">
                      刪除
                    </a>
                  </p>
                </article>
              </div>
            `;
            cartProductsContainer.append(cartProductTemplate);
          });
        }

        numberOfProducts = cartItems.length;
        $(".cart__footer .button").text("結帳 " + numberOfProducts);
      } catch (error) {
        console.error("Error getting cart items:", error);
      }
    },
  },
  mounted: function () {
    this.refreshData();
  },
};
