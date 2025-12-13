import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-vue';
import path from 'path'
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
          target: 'http://localhost:5231',
          changeOrigin: true,
          rewrite: (path) => path.replace(/^\/api/, '')
        }
      }
    }
})
