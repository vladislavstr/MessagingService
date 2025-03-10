//import { defineConfig } from "vite";
import { ConfigEnv, defineConfig, loadEnv } from 'vite';
import react from "@vitejs/plugin-react";

//// https://vite.dev/config/
//export default defineConfig({
//  plugins: [react()],
//  server: {
//    port: 60719,
//  },
//});


export default defineConfig(({ command, mode }: ConfigEnv) => {
    console.log(`configuring vite with command: ${command}, mode: ${mode}`);
    // suppress eslint warning that process isn't defined (it is)
    // eslint-disable-next-line
    const cwd = process.cwd();
    console.log(`loading envs from ${cwd} ...`);
    const env = { ...loadEnv(mode, cwd, 'VITE_') };
    console.log(`loaded env: ${JSON.stringify(env)}`);

    // reusable config for both server and preview
    const serverConfig = {
        host: true,
        port: Number(env.VITE_PORT),
        strictPort: true,
        https: false,
    };

    return {
        base: '/',
        plugins: [react()],// basicSsl()],
        preview: serverConfig,
        server: serverConfig,
    };
});
