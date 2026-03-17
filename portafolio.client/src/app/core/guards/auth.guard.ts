import { inject } from '@angular/core';
import { CanActivateFn, Router, UrlTree } from '@angular/router';

import { AuthFacade } from '../auth/auth.facade';

export const authGuard: CanActivateFn = (): boolean | UrlTree => {
  const authFacade = inject(AuthFacade);
  const router = inject(Router);

  return authFacade.isAuthenticated() ? true : router.createUrlTree(['/login']);
};
