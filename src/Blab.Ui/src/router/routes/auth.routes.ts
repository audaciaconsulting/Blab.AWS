import AuthCallbackView from "@/views/auth/AuthCallbackView.vue";
import SilentRenewCallbackView from "@/views/auth/SilentRenewCallbackView.vue";

const authRoutes = [
  {
    path: "/auth-callback",
    name: "AuthCallback",
    component: AuthCallbackView,
  },
  {
    path: "/silent-renew-callback",
    name: "SilentRenewCallback",
    component: SilentRenewCallbackView,
  },
];

export default authRoutes;
