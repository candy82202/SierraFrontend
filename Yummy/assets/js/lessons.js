const lessons = {
  template: `
    <section>
    <div class="container mt-5">
        <div class="row">
            <div class="col-12 mb-4">課程類別選擇:
                <select v-model="selectedCategory" @change="filterLessons" name="" id="">
                    <option value="">All</option>
                    <option v-for="lessonCategory in lessonCategories" :value="lessonCategory.lessonCategoryName">
                    {{lessonCategory.lessonCategoryName}}
                    </option>
                </select>
            </div>
            <div  v-for="lesson in lessons" :key="lesson.lessonId" class="col-4">
                <div  class="card card1 border-5 " style="width: 18rem;margin-bottom:10px;overflow:hidden">
                    <div id="carouselExampleSlidesOnly" class="carousel slide" data-bs-ride="carousel">
                        <carousel :interval="500">
                            <div v-for="(lessonImage,imageIndex) in lesson.lessonImageName" :key="imageIndex"
                            class="carousel-item active">
                            <img :src="PhotoPath+lessonImage" class="d-block w-100 " style=" width: 100%;
                            height: 300px;" alt="...">
                            </div>
                        </carousel>
                    </div>
                    <div class="card-body">
                        <h5 class="card-title">課程名稱:
                            <p>{{lesson.lessonTitle}}</p>
                        </h5>
                    </div>
                    <span class="card-text">{{formatDate(lesson.lessonTime)}}開課</span>
                    <ul class="list-group list-group-flush">
                        <li class="list-group-item">課程簡介:{{lesson.lessonInfo}}</li>
                        <li class="list-group-item">成品:{{lesson.lessonDessert}}</li>
                    </ul>
                    <div class="card-footer">
                        <router-link :to="'/lesson/' + lesson.lessonId" class="btn">
                            <i class="fa-solid fa-circle-info fa-bounce fa-2xl" style="color: #d185ca;"></i> 
                        </router-link>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </section>
    `,
  data() {
    return {
      lessons: [],
      //lessonImages: [],
      PhotoPath: "/assets/img/",
      lessonCategories: [],
      selectedCategory: null,
    };
  },

  methods: {
    async filterLessons() {
      let _this = this;
      var request = {};
      if (this.selectedCategory) {
        request.params = { categoryName: this.selectedCategory };
      }
      const response1 = axios
        .get(variables.API_URL + "Lesson/category", request)
        .then((response) => {
          //console.log(response.data);
          _this.lessonCategories = response.data;
        })
        .catch((err) => {
          alert(err);
        });
      const response = axios
        .get(variables.API_URL + "Lesson/lesson", request)
        .then((response) => {
          
          _this.lessons = response.data.filter(lesson =>!lesson.isLessonExpired);
          //console.log(_this.lessons.isLessonExpired);
          console.log(_this.lessons);
          //_this.lessonImages = response.data;
          
        })
        .catch((err) => {
          alert(err);
        });

    },
    formatDate: function (value) {
      //console.log(value);
      const options = {
        year: "numeric",
        month: "2-digit",
        day: "2-digit",
        hour: "2-digit",
        minute: "2-digit",
        second: "2-digit",
      };
      return new Date(value).toLocaleDateString("en-US", options);
    },
    async getCartItems() {
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
                  height: 50px; width: 50px;   "/>
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
  },
  mounted: function () {
    //this.filterLessonCategories();
    this.filterLessons();
    this.getCartItems();
  },
};
