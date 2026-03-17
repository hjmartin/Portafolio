import { Injectable, signal } from '@angular/core';

import { ProjectVm } from '../data-access/projects.models';

@Injectable({ providedIn: 'root' })
export class ProjectsStore {
  readonly items = signal<ProjectVm[]>([]);
}
