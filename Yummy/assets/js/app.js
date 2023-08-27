const index = {
  template: ``,
};

const app = Vue.createApp({});

app.component("longCake", longCake);
app.component("snack", snack);
app.component("presents", presents);
app.component("moldCake", moldCake);
app.component("roomTemperature", roomTemperature);
app.component("desserts", desserts);
app.component("choco", choco);
// app.component("promotionApp", promotionApp);

// app.component("suggest", suggest);

const router = VueRouter.createRouter({
  history: VueRouter.createWebHashHistory(),
  routes: [
    { path: "/", component: menu },
    { path: "/menu", component: menu },
    { path: "/longCake", component: longCake },
    { path: "/snack", component: snack },
    { path: "/presents", component: presents },
    { path: "/moldCake", component: moldCake },
    { path: "/roomTemperature", component: roomTemperature },
    { path: "/desserts/:id", component: desserts }, // Add named route for desserts
    { path: "/choco", component: choco },
    // { path: "/promotion", component: promotionApp },

    // { path: "/suggest", component: suggest },
  ],
});
app.use(router);
app.mount("#app");
