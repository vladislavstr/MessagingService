//import { defineConfig } from "vite";
import { ConfigEnv, defineConfig, loadEnv } from 'vite';
import react from "@vitejs/plugin-react";

// import plugin from "@vitejs/plugin-react";

// https://vitejs.dev/config/
// export default defineConfig({
//  plugins: [plugin()],
//  server: {
//    port: 60718,
//  },
// });

export default defineConfig(({ command, mode }: ConfigEnv) => {
    console.log(`configuring vite with command: ${command}, mode: ${mode}`);
    // suppress eslint warning that process isn't defined (it is)
    // eslint-disable-next-line
    const cwd = process.cwd();
    console.log(`loading envs from ${cwd} ...`);
    const env = { ...loadEnv(mode, cwd, 'VITE_') };
    console.log(`loaded env: ${JSON.stringify(env)}`);

    const port = Number(env.VITE_PORT);
    if (isNaN(port)) {
        
        throw new Error(`Invalid port number: ${env.VITE_PORT}`);
    }

    // reusable config for both server and preview
    const serverConfig = {
        host: true,
        port: port,
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
