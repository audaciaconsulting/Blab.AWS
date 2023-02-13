import HomeView from "@/views/HomeView.vue";
import UserProfileView from "@/views/UserProfileView.vue";
import ChatView from "@/views/ChatView.vue";
import ChatsView from "@/views/ChatsView.vue";
import SinglePostView from "@/views/SinglePostView.vue";

const routes = [
  {
    path: "/",
    name: "home",
    component: HomeView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/user/:handle",
    name: "UserProfile",
    component: UserProfileView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/chat/:chatId",
    name: "Chat",
    component: ChatView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/chats",
    name: "Chats",
    component: ChatsView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/blab/:id",
    name: "SinglePost",
    component: SinglePostView,
    meta: {
      requiresAuth: true,
    },
  },
];

export default routes;
