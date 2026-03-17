import { Injectable, signal } from '@angular/core';

import { PortfolioProfileVm } from '../data-access/portfolio.models';

@Injectable({ providedIn: 'root' })
export class PortfolioStore {
  readonly profile = signal<PortfolioProfileVm | null>(null);
}
