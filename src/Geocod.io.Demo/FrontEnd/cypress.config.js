import { defineConfig } from "cypress";

export default defineConfig({
  component: {
    video: false,
    screenshotOnRunFailure: false,
    devServer: {
      framework: "react",
      bundler: "vite",
    },
  },
});
