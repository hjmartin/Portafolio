import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { API_BASE_URL } from '../../../core/config/app.tokens';
import { PortfolioProfileVm } from './portfolio.models';

@Injectable({ providedIn: 'root' })
export class PortfolioApi {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = inject(API_BASE_URL);

  getProfile() {
    return this.http.get<PortfolioProfileVm>(`${this.apiBaseUrl}/PortfolioProfiles`);
  }
}
