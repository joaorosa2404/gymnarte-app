import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
    return {
        plugins: [react()],
        server: {
            port: 5174,
            strictPort: true,
            hmr: {
                protocol: 'ws',
                host: 'localhost'
            }
        },
        base: mode === 'production' ? '/admin/' : '/',
    }
})
