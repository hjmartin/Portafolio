import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { API_BASE_URL } from '../../../core/config/app.tokens';
import { TechnologyVm } from './technologies.models';

@Injectable({ providedIn: 'root' })
export class TechnologiesApi {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = inject(API_BASE_URL);

  getAll() {
    return this.http.get<TechnologyVm[]>(`${this.apiBaseUrl}/Technologies`);
  }
}
