import { fileURLToPath, URL } from "node:url";
import { defineConfig, loadEnv, type UserConfigExport } from "vite";
import type { IEnvironmentConfig } from "./src/models/config/environment-config.interface";
import vue from "@vitejs/plugin-vue";
import { readFileSync } from "fs";
import { readFile } from "fs/promises";


// https://vitejs.dev/config/
// export default defineConfig(async ({ command, mode }) => {
//   // const configFetch = await fetch('assets/config.json');
//   // const appConfig: IEnvironmentConfig = await configFetch.json();
  
//   const test = await readFile('assets/config.json');
//   const appConfig: IEnvironmentConfig = JSON.parse(test.toString());
//   // const appConfig:  EnvironmentConfig

//   process.env = { ...process.env, ...loadEnv(mode, process.cwd()) };
//   const config: UserConfigExport = {
//     plugins: [vue()],
//     resolve: {
//       alias: {
//         "@": fileURLToPath(new URL("./src", import.meta.url)),
//       },
//     },
//     build: {
//       outDir: "dist/blab-ui",
//     },
//     assetsInclude: [ '**/config.json'],
//     define: {
//       UI_URL: appConfig.uiUrl,
//       API_URL: appConfig.apiUrl,
//       IDENTITY_URL: appConfig.identityUrl,
//       AUTH_TOKEN_LOCATION: appConfig.authTokenLocation
//     }
//   };

//   if (mode === "development") {
//     config.server = {
//       https: {
//         key: readFileSync("./ssl/server.key"),
//         cert: readFileSync("./ssl/server.crt"),
//       },
//     };
//   }

//   return defineConfig(config);
// })

export default ({ mode }: IEnvironmentConfig) => {
  const test = readFileSync('./src/config/config.json');
  const appConfig: IEnvironmentConfig = JSON.parse(test.toString());

  console.log(appConfig);

  process.env = { ...process.env, ...loadEnv(mode, process.cwd()) };
  const config: UserConfigExport = {
    plugins: [vue()],
    resolve: {
      alias: {
        "@": fileURLToPath(new URL("./src", import.meta.url)),
      },
    },
    build: {
      outDir: "dist/blab-ui",
    },
    assetsInclude: [ '**/config.json'],
    define: {
      UI_URL: appConfig.uiUrl,
      API_URL: appConfig.apiUrl,
      IDENTITY_URL: appConfig.identityUrl,
      AUTH_TOKEN_LOCATION: appConfig.authTokenLocation
    }
  };

  if (mode === "development") {
    config.server = {
      https: {
        key: readFileSync("./ssl/server.key"),
        cert: readFileSync("./ssl/server.crt"),
      },
    };
  }

  return defineConfig(config);
};
