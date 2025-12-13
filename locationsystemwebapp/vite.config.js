import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-vue';

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [plugin()],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src')
    }
  },
    server: {
      port: 14091,
      proxy: {
        '/api': {
          target: 'http://localhost:5003',
          changeOrigin: true,
          rewrite: (path) => path.replace(/^\/api/, '')
        }
      }
    }
})
