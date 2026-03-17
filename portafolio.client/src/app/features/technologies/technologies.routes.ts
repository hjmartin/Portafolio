import { Routes } from '@angular/router';

export const TECHNOLOGIES_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/technologies-page/technologies-page.component').then(m => m.TechnologiesPageComponent)
  }
];
