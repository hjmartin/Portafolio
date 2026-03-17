import { inject } from '@angular/core';
import { CanActivateFn, Router, UrlTree } from '@angular/router';

import { AuthFacade } from '../auth/auth.facade';

export const adminGuard: CanActivateFn = (): boolean | UrlTree => {
  const authFacade = inject(AuthFacade);
  const router = inject(Router);

  return authFacade.isAdmin() ? true : router.createUrlTree(['/']);
};
