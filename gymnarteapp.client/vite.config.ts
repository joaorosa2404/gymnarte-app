import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
    plugins: [react()],
    server: {
        port: 5173, // Lembra-te: 5173 num e 5174 no outro
        strictPort: true,
        hmr: {
            protocol: 'ws',
            host: 'localhost'
        }
    }
})