import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';

import { AuthFacade } from '../auth/auth.facade';

export const authTokenInterceptor: HttpInterceptorFn = (req, next) => {
  const authFacade = inject(AuthFacade);
  const accessToken = authFacade.accessToken();

  if (!accessToken || !req.url.includes('/api')) {
    return next(req);
  }

  return next(req.clone({
    setHeaders: {
      Authorization: `Bearer ${accessToken}`
    }
  }));
};
