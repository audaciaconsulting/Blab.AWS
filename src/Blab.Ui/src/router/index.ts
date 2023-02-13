import { createRouter, createWebHistory } from "vue-router";
import { authService } from "@/services/auth.service";
import { useProfileStore } from "@/stores/user-profile.store";
import authRoutes from "@/router/routes/auth.routes";
import routes from "@/router/routes/routes";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [...routes, ...authRoutes],
});

router.beforeEach((to, from, next) => {
  if (to.matched.some((record) => record.meta.requiresAuth)) {
    authService.getUser().then((user) => {
      if (user == null) {
        authService.login(to.fullPath);
      }

      const userStore = useProfileStore();
      userStore.getUserDetails();

      next();
    });
  } else {
    next();
  }
});

export default router;
