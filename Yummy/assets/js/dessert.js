const desserts = {
  template: `
  <section  v-for="des in dessert"
              :key="des.dessertId">
  <div class="container" data-aos="fade-up">
  <h3>{{des.categoryName}}</h3>
</div>
          <div class="container">
            <section
              aria-label="Main content"
              role="main"
              class="product-detail" >
              <div itemscope itemtype="" >
                <meta itemprop="url" />
                <meta itemprop="image" />
                <div class="shadow">
                  <div class="_cont detail-top">
                    <div class="cols">
                      <div class="left-col">
                        <div class="thumbs">
                          <a  v-for="(dimg, index) in des.dessertImages" :key="index"
                            class="thumb-image active"                     
                            @click="imgModal(dimg)"
                            data-index="0"
                          >
                            <span
                              ><img
                              :src="PhotoPath + dimg.dessertImageName"
                              v-bind:title="des.dessertName"
                            /></span>
                          </a>
                        </div>
                        <div class="big" >                      
                          <span
                            id="big-image"
                            class="img"
                            quickbeam="image"
                            :style="{ backgroundImage: 'url(' + PhotoPath + des.dessertImages[0].dessertImageName + ')' }"
                            v-bind:title="des.dessertName"
                          ></span>
                          <div id="banner-gallery" class="swipe">
                            <div class="swipe-wrap">
                              <div
                              :style="{ backgroundImage: 'url(' + PhotoPath + des.dessertImages[0].dessertImageName + ')' }"></div>
                           </div>
                          </div>
                          <div class="detail-socials">
                            <div class="social-sharing" data-permalink="#">
                              <a
                                target="_blank"
                                class="share-facebook"
                                title="Share"
                              ></a>
                              <a
                                target="_blank"
                                class="share-twitter"
                                title="Tweet"
                              ></a>
                              <a
                                target="_blank"
                                class="share-pinterest"
                                title="Pin it"
                              ></a>
                            </div>
                          </div>
                        </div>
                      </div>
                      <div class="right-col" >
                      <h1 itemprop="name">{{ des.dessertName }}</h1>
                      <div itemprop="offers" >
                        <meta itemprop="priceCurrency" content="USD" />
                        <link itemprop="availability"/>
                        <div class="price-shipping">
                          <div class="price" id="price-preview" quickbeam="price" quickbeam-price="800">
                        $  {{ des.unitPrice }}
                          <div style="display:none">  {{  selectedUnitPrice}}</div>                         
                          </div>
                        </div>
                      </div>
                      <form>            
                      <label class="form-label" for="productSelect">規格 </label>
                      <select class="form-select" name="desserts" v-model="selectedUnitPrice"  >
                      <option v-for="(spec, index) in des.specifications" :key="spec.specificationId" :value="spec.specificationId">
                      {{ spec.flavor }} {{ spec.size }}
                      <span v-if="des.discountedPrice == 0"> $ {{ spec.unitPrice }}</span>
                      <span v-else class="text-danger">
                        <span class="text-dark text-decoration-line-through">原價 $ {{ spec.unitPrice }}</span>
                       特價 $ {{ des.discountedPrice }}
                      </span>
                    </option>
                     
                    </select>
                  </form>

                      <form id="AddToCartForm">
                      <div class="btn-and-quantity-wrap">
                        <div class="btn-and-quantity">
                          <div class="spinner">
                            <span class="btn minus"  @click="decrement"></span>
                            <input
                              type="text"
                              id="inputQuantity"
                              name="quantity"
                              v-model="quantity"
                              class="quantity-selector"
                            />
                            <input
                              type="hidden"
                              id="product_id"
                              name="product_id"
                          
                            />
                            <span class="q">數量</span>
                            <span class="btn plus"  @click="increment"></span>
                          </div>
                          <div id="AddToCart" quickbeam="add-to-cart">
                            <a
                              class="button js-add-product"
                              href="#"
                              title="Add to cart"
                              style="z-index: 1"
                              @click.stop="addProduct(des)"
                            >
                              <span class="text-light">加到購物車</span>
                            </a>                            
                          </div>
                        </div>
                      </div>
                    </form>  
                      <ul class="nav nav-tabs tabs">
                        <li class="nav-item">
                          <a
                            class="nav-link active tab-labels"
                            id="profile-tab"
                            data-bs-toggle="tab"
                            href="#profile"
                            role="tab"
                            aria-current="page"
                            style="color: var(--color-text)"
                            ><span>詳細內容</span></a
                          >
                        </li>
                        <li class="nav-item">
                          <a
                            class="nav-link  tab-labels"
                            id="delivery-tab"
                            data-bs-toggle="tab"
                            href="#delivery"
                            role="tab"
                            style="color: var(--color-text) "
                            ><span>送貨及付款方式</span></a
                          >
                        </li>
                      </ul>
                      <div class="tab-content" id="myTabContent">
                        <!-- 第一個 Tab 頁面：詳細內容 -->
                        <div
                          class="tab-pane fade show active"
                          id="profile"
                          role="tabpanel"
                          aria-labelledby="profile-tab"
                        >
                          <p>     {{des.description}}</p>
                        </div>
                        <!-- 第二個 Tab 頁面：送貨及付款方式 -->
                        <div
                          class="tab-pane fade"
                          id="delivery"
                          role="tabpanel"
                          aria-labelledby="delivery-tab"
                        >
                          <p>送貨方式
                          【黑貓】冷藏宅配 (建議提早配送)
<br/>
                          付款方式
【推薦】信用卡線上刷卡</p>
                        </div>
                      </div>
                    </div>
                      <div class="social-sharing-btn-wrapper">
                        <span id="social_sharing_btn">Share</span>
                      </div>
                    </div>
                  </div>
                </div>
              
                <aside class="related">
                  <div class="_cont">
                   
                      <h2>推薦商品</h2>
                    <div
                      class="collection-list cols-4"
                      id="collection-list"
                      data-products-per-page="4"
                    >
                    <div  v-for="dessert in dessertDiscountList" :key="dessert.dessertId">
                    <a  :href="dessertLinkURL + dessert.dessertId" class="product-box" @click="refreshData"
                    >
                      <span class="img">
                        <span :style="{ backgroundImage: 'url(' + PhotoPath + dessert.dessertImageName + ')' }" class="i first"></span>
                      </span>
                      <span class="text">
                        <strong>{{ dessert.dessertName }}</strong>
                        <span> $ {{ dessert.unitPrice }} </span>
                        <div class="variants">
                          <div class="variant" >
                            <div class="var available">
                              <div>{{ dessert.size }}</div>
                            </div>      
                          </div>
                          <div class="variant">
                            <div class="var available">
                              <div>{{ dessert.flavor }}</div>
                            </div>    
                          </div>
                        </div>
                      </span>
                    </a>
                  </div>
                    </div>
                    <div class="more-products" id="more-products-wrap">
                      <span id="more-products" data-rows_per_page="1"
                        >
                        <a href="desserts.html#/choco">更多商品</a></span
                      >
                    </div>
                  </div>
                </aside>
              </div>
            </section>
          </div>
        </section>
  `,
  data() {
    return {
      dessert: [], // Initialize dessert as an empty object
      dessertDiscountList: [],
      dessertId: 0,
      dessertName: "",
      dessertImageName: "",
      unitPrice: 0,
      selectedUnitPrice: "",
      flavor: "",
      size: "",
      PhotoPath: "/assets/img/",
      specifications: [],
      dessertLinkURL: "dessertProducts.html#/desserts/",
      cartItems: [],
      specificationId: 0,
      specification: [],
      dessertImages: [],
      selectedImageIndex: 0, // 初始选择的图片索引为0
      categoryName: "",
      specifications: [],
      discountedPrice: 0,
      quantity: 1,
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
      const dessertId = this.$route.params.id; // Access the dessert ID from the route params
      const dessertDiscountList = this.dessertDiscountList;
      let config = {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("jwtToken")}`,
        },
      };

      axios
        .get(variables.API_URL + "Desserts/" + dessertId)
        .then((response) => {
          console.log(response.data);
          this.dessert = response.data; // Assign the fetched dessert to the dessert data property
          if (
            response.data.length > 0 &&
            response.data[0].specifications.length > 0
          ) {
            this.specifications = response.data[0].specifications;
            this.discountedPrice = response.data[0].dessertDiscountPrice;
            this.selectedUnitPrice = "$ " + this.specifications[0].unitPrice;
          } else {
            console.error("No specifications available in the response.");
          }
        });

      axios
        .get(
          variables.API_URL +
            "Desserts/SuggestDiscountGroups?dessertId=" +
            dessertId
        )
        .then((response) => {
          console.log(response.data);
          this.dessertDiscountList = response.data; // Assign the fetched desserts to the longCake data property
        });
      // this.getCartItems();
    },
    increment() {
      this.quantity++;
    },
    decrement() {
      if (this.quantity > 1) {
        this.quantity--;
      }
    },
    imgModal(selectedImage) {
      console.log(selectedImage);
      Swal.fire({
        title: selectedImage.dessert.dessertName,
        text: selectedImage.dessert.description,
        imageUrl: this.PhotoPath + selectedImage.dessertImageName,
        imageWidth: 400,
        imageHeight: 400,
        imageAlt: "Custom image",
      });
    },
    // getDessertprodUrl(dessertId) {
    //   return dessertLinkURL + "dessertId";
    // },
    // toDessertProducts(dessertId) {
    //   window.location.href = this.getDessertprodUrl(dessertId);
    // },
    // getDessertUrl(dessertId) {
    //   return variables.API_URL + "Desserts/ChocoDiscountGroups";
    // },
    // goToDessertDetails(dessertId) {
    //   window.location.href = this.getDessertUrl(dessertId);
    // },

    async addProduct(des) {
      // cartOpen如果是false那就打開購物車
      this.GetToken();
      const selectedSpecId = this.selectedUnitPrice; // Use selectedUnitPrice directly
      console.log(selectedSpecId);
      const dessertId = des.dessertId;

      console.log(des);
      const newQty = this.quantity; // Use this.quantity instead of trying to get value from #inputQuantity
      // if (!cartOpen) {
      //   openCart();
      //   await this.getCartItems();
      // }
      this.addToCart(dessertId, selectedSpecId, newQty);

      numberOfProducts++;
      $(".cart__footer .button").text("結帳 " + numberOfProducts);
      $(".shoppingCart").text(numberOfProducts);
      // Fetch the updated cart items and display them
      if (!cartOpen) {
        openCart();
      }
    },

    async addToCart(dessertId, specificationId, newQty) {
      let config = {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("jwtToken")}`,
        },
      };
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
      let config = {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("jwtToken")}`,
        },
      };
      const username = localStorage.getItem("username"); // Replace this with the actual username
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
          cartItems.forEach((item) => {
            const cartProductTemplate = `
            <div class="cart__product"  v-for="item in cartItems" :key="item.id">
            <article class="js-cart-product">
            <div class="d-flex">
            <img class="menu-img img-fluid" src="/assets/img/${item.dessertImage}" style="
            height: 70px; width: 70px;  margin-right:15px  "/>
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
  computed: {
    selectedImageURL() {
      if (this.des.dessertImages.length > 0) {
        return (
          this.PhotoPath +
          this.des.dessertImages[this.selectedImageIndex].dessertImageName
        );
      }
      return ""; // 返回默认或者空URL
    },
    selectedUnitPriceText() {
      const selectedSpec = this.specifications.find(
        (spec) => spec.specificationId === this.selectedUnitPrice
      );
      if (selectedSpec) {
        if (this.des.discountedPrice === 0) {
          return `$ ${selectedSpec.unitPrice}`;
        } else {
          return `原價 $ ${selectedSpec.unitPrice} 特價 $ ${this.des.discountedPrice}`;
        }
      }
      return ""; // Return an empty string if no matching specification is found
    },
  },
  mounted: function () {
    this.refreshData();
    this.getCartItems();
  },
};
