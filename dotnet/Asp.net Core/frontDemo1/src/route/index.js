import { createRouter, createWebHashHistory } from "vue-router";
import Login from "../views/Login.vue";
const routes = [
  {path: "/", redirect: "/login"},
  {path:"/login", name:"Login", component:Login},
]

const router = createRouter({history: createWebHashHistory(), routes:routes});

export default router;