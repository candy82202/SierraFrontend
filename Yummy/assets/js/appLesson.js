const index = {
  template: ``,
};

const app = Vue.createApp({});
app.component("lessonDetail", lessonDetail);
app.component("lessons", lessons);
// app.component("suggest", suggest);

const router = VueRouter.createRouter({
  history: VueRouter.createWebHashHistory(),
  routes: [
    { path: "/", component: lessons },
    { path: "/lessons", component: lessons },
    { path: "/lesson/:lessonId", component: lessonDetail },
    // { path: "/suggest", component: suggest },
  ],
});
app.use(router);
app.mount("#app");
