(function (window) {
  window.__env = window.__env || {};
  // Si se deja vacío, la aplicación usará '/api' en producción (manejado por el proxy inverso de Nginx)
  // o 'http://localhost:5050/api' en desarrollo local.
  // Puedes sobrescribir esta variable si despliegas el backend en un subdominio separado:
  // window.__env.apiUrl = 'https://api.tu-dominio.com/api';
  window.__env.apiUrl = '';
})(this);
