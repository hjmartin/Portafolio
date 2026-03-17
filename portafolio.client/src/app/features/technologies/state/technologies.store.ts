import { Injectable, signal } from '@angular/core';

import { TechnologyVm } from '../data-access/technologies.models';

@Injectable({ providedIn: 'root' })
export class TechnologiesStore {
  readonly items = signal<TechnologyVm[]>([]);
}
