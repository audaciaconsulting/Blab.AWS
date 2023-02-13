import { fileURLToPath, URL } from "node:url";
import { defineConfig, loadEnv, type UserConfigExport } from "vite";
import type { IEnvironmentConfig } from "./src/models/config/environment.config";
import vue from "@vitejs/plugin-vue";
import { readFileSync } from "fs";

// https://vitejs.dev/config/
export default ({ mode }: IEnvironmentConfig) => {
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
