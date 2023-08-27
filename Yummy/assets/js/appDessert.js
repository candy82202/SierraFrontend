const app = Vue.createApp({});

app.component("desserts", desserts);
const router = VueRouter.createRouter({
  history: VueRouter.createWebHashHistory(),
  routes: [
    { path: "/", component: desserts },
    { path: "/desserts/:id", component: desserts }, // Add named route for desserts
    { path: "/desserts", component: desserts },
  ],
});

app.use(router);
app.mount("#app");
