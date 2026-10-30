import { loadEnv } from 'vite'
import { defineConfig } from 'vitest/config'
import react from '@vitejs/plugin-react'
// import basicSsl from '@vitejs/plugin-basic-ssl' // Uncomment this line if you want to use https in development

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, '.', 'VITE_')

  return {
    plugins: [react()], // basicSsl() - Add this plugin if you want to use https in development
    build: {
      sourcemap: env.VITE_SOURCEMAP === 'true',
    },
    test: {
      environment: 'jsdom',
      setupFiles: './src/test/setup.ts',
      clearMocks: true,
      pool: 'threads',
      maxWorkers: 1,
    },
  }
})
