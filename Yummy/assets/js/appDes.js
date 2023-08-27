const app = Vue.createApp({});

app.component("longCake", longCake);
app.component("snack", snack);
app.component("presents", presents);
app.component("moldCake", moldCake);
app.component("roomTemperature", roomTemperature);
app.component("getdesserts", getdesserts);

app.component("choco", choco);

const router = VueRouter.createRouter({
  history: VueRouter.createWebHashHistory(),
  routes: [
    { path: "/", component: getdesserts },
    { path: "/longCake", component: longCake },
    { path: "/snack", component: snack },
    { path: "/presents", component: presents },
    { path: "/moldCake", component: moldCake },
    { path: "/roomTemperature", component: roomTemperature },
    { path: "/getdesserts", component: getdesserts },
    { path: "/choco", component: choco },
  ],
});

app.use(router);
app.mount("#app");
