import { Routes } from '@angular/router';

export const PROJECTS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/projects-page/projects-page.component').then(m => m.ProjectsPageComponent)
  }
];
