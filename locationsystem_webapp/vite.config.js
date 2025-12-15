import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import path from 'path'

// https://vite.dev/config/
export default defineConfig({
    plugins: [vue()],
    resolve: {
        alias: {
            // eslint-disable-next-line no-undef
            '@': path.resolve(__dirname, './src')
        }
    },
    server: {
        proxy: {
            '/api': {
                target: 'http://localhost:5231',
                changeOrigin: true
                // 如果后端API不包含/api前缀，需要使用rewrite规则
                // rewrite: (path) => path.replace(/^\/api/, '')
            }
        }
    }
})
