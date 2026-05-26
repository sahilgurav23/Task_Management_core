import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const userJson = localStorage.getItem('user');
  const token = localStorage.getItem('token');
  
  let headers = req.headers;

  if (userJson) {
    const user = JSON.parse(userJson);
    if (user.id) {
      headers = headers.set('X-User-Guid', user.id);
    }
  }

  if (token) {
    headers = headers.set('Authorization', `Bearer ${token}`);
  }

  const authReq = req.clone({ headers });

  return next(authReq);
};
