const lessonDetail = {
  template: `
    <section>
    <div class="container">
        <div v-if="selectedLesson">
            <h2>{{selectedLesson.lessonTitle}}</h2>
            <br/>
            <div class="panel-body">
                <ul>
                    <li>
                        <i class="far fa-calendar"></i> 開課時間: {{formatDate(selectedLesson.lessonTime)}}
                    </li>
                    <br/>
                    <li>
                    <i class="fas fa-dollar"></i>
                         課堂價格: {{selectedLesson.lessonPrice}}
                    </li>
                </ul>
            </div>
            <div class="lesson-info-table">
            <table class="table table striped">
        <thead>
            <tr>
                <th>時間</th>
                <th>剩餘人數</th>
                <th>報名人數</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>
                <span>{{formatTimeRange(selectedLesson.lessonTime, selectedLesson.lessonHours)}}</span>
                </td>
                <td>{{countPerson(selectedLesson.maximumCapacity, selectedLesson.actualCapacity)}}</td>
                <td>
                    <select v-model="selectedRegistrationCount" id="registrationCount" style="width:100%"> 
                        <option v-for="count in availableRegistrationCounts" :key="count">{{count}}</option>
                    </select>
                </td>
            </tr>
        </tbody>
    </table>
    </div>
            
            <br/>
        <section>
            <button class="btn btn-danger" style="float:right" @click="checkRegister">確認報名</button>
        </section>
            <br/>
            <h3>詳細介紹</h3>
            <br/>
            <h5>
            <div>
                <p>課堂成品:{{selectedLesson.lessonDessert}}</p>
            </div>
            <div>
                <p>{{selectedLesson.lessonInfo}}</p>
            </div>
            <div>
                <p>{{selectedLesson.lessonDetail}}</p>
            </div>
            </h5>
            <div v-for="lessonImage in selectedLesson.lessonImages" :key="lessonImage.lessonImageId">
                <img :src="PhotoPath + lessonImage.lessonImageName" alt="Lesson Image" style=" width: 400px;
                height: 300px;" />
            </div>
        </div>
    </div>
        </section>
    `,

  data() {
    return {
      selectedLesson: [],
      lessonImages: [],
      PhotoPath: "./assets/img/",
      lessonCategories: [],
      lessonLinkURL: "#/lessons/",
      selectedRegistrationCount: 0,
      lessonId: "",
    };
  },
  methods: {
    async filterLessonById(lessonId) {
      let _this = this;
      const response = await axios
        .get(variables.API_URL + "Lesson/lessonId", {
          params: { lessonId: lessonId },
        }) // Pass lessonId as a query parameter
        .then((response) => {
          //console.log(response.data);
          _this.selectedLesson = response.data;
          _this.lessonImages = response.data.lessonImages;

          //console.log("圖片URL:",PhotoPath+lessonImage.lessonImageName);
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
      return new Date(value).toLocaleDateString("zh-TW", options);
    },
    formatTimeRange: function (startTime, lessonHours) {
      const options = {
        hour: "2-digit",
        minute: "2-digit",
        hour12: false,
      };
      const startDate = new Date(startTime);
      const endDate = new Date(startTime);
      endDate.setHours(startDate.getHours() + lessonHours);
      return `${startDate.toLocaleTimeString(
        "zh-TW",
        options
      )} - ${endDate.toLocaleTimeString("zh-TW", options)}`;
    },
    checkRegister: function () {
      const token = this.GetToken();
      if (!token) {
        window.location.href = "Login.html";
        return;
      }
      //if (token)
      else {
        const tokenExpired = this.isTokenExpired(token);
        if (tokenExpired) {
          //this.filterLessonById(lessonId);
          localStorage.setItem(
            "selectedLesson",
            JSON.stringify(this.selectedLesson)
          );
          localStorage.setItem(
            "selectedRegistrationCount",
            this.selectedRegistrationCount
          );
          if (this.selectedRegistrationCount == "") {
            Swal.fire({
              icon: "error",
              title: "歐不!",
              text: "還沒填選報名人數唷!",
            });
            return;
          }

          // 添加一个 console.log 来输出存储的值
          console.log("selectedLesson stored:", this.selectedLesson);
          console.log(
            "selectedRegistrationCount stored:",
            this.selectedRegistrationCount
          );

          // 然后再进行页面跳转
          window.location.href = "LessonCheck.html";
        }
      }
    },
    countPerson: function (maxCapacity, actualCapacity) {
      const realCapacity = maxCapacity - actualCapacity;
      return realCapacity;
    },
    GetToken() {
      const storedToken = localStorage.getItem("jwtToken");
      if (storedToken == null) {
        // this._router.navigate(["回首頁"]);
        //window.location.href = "LogIn.html"; // 刷新頁面，並導至首頁
        return null;
      } else {
        // Parse the token to check if it's expired
        const tokenExpired = isTokenExpired(storedToken);
        if (tokenExpired) {
          //this.filterLessonById(lessonId);
        }
      }
      return { Authorization: "bearer " + storedToken };
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
  computed: {
    availableRegistrationCounts() {
      const maximumCapacity = this.selectedLesson.maximumCapacity;
      const actualCapacity = this.selectedLesson.actualCapacity;
      const remainingCapacity = maximumCapacity - actualCapacity;
      const counts = [];
      for (let i = 1; i <= remainingCapacity; i++) {
        counts.push(i);
      }

      return counts;
    },
  },
  mounted: function () {
    //this.filterLessonCategories();
    const lessonId = this.$route.params.lessonId;
    this.filterLessonById(lessonId);
    //this.lessonId = this.$route.params.lessonId;
    //this.filterLessonById(this.lessonId);
  },
};
