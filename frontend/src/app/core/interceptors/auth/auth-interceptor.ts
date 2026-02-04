import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  // const auth = inject(Auth);

  const authRequest = req.clone({
    setHeaders: {
      Authorization: `Bearer ${""}`
    }
  });

  return next(authRequest);
};
