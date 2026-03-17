import { Routes } from '@angular/router';

export const HOME_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/landing-page/landing-page.component').then(m => m.LandingPageComponent)
  }
];
