import { authService } from "@/services/auth.service";
import { createApp } from "vue";
import { createPinia } from "pinia";
import Notifications, { notify } from "@kyvg/vue3-notification";
import App from "./App.vue";
import router from "./router";
import * as fetchIntercept from "fetch-intercept";
import { StatusCodes } from "http-status-codes";
import "./assets/main.css";
import { ConfigService } from "./services/config.service";

const app = createApp(App);
const pinia = createPinia();

const configService: ConfigService = new ConfigService();
configService.load();

const unregister = fetchIntercept.register({
  request: async function (url, config) {
    return [url, config];
  },

  requestError: function (error) {
    return Promise.reject(error);
  },

  response: function (response) {
    if (response.status === StatusCodes.UNAUTHORIZED) {
      notify("Sorry you are not logged in. Login to re-perform this action.");
      authService.logout();
    }
    const clonedResponse = response.clone();

    const json = () =>
      clonedResponse
        .json()
        .then((data) => ({ ...data, title: `Intercepted: ${data.title}` }));

    response.json = json;
    return response;
  },

  responseError: function (error) {
    return Promise.reject(error);
  },
});
app.use(pinia);
app.use(router);
app.use(Notifications);
app.mount("#app");
