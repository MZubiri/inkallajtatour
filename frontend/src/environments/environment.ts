export const environment = {
  production: false,
  apiUrl: (typeof window !== 'undefined' && (window as any).__env?.apiUrl) || 'http://localhost:5050/api'
};
