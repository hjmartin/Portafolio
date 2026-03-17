const PROXY_CONFIG = [
  {
    context: [
      '/api',
      '/secure',
      '/healthz'
    ],
    target: 'https://localhost:7125',
    secure: false,
    changeOrigin: true,
    logLevel: 'debug'
  }
];

module.exports = PROXY_CONFIG;
