import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { API_BASE_URL } from '../../../core/config/app.tokens';
import { ProjectVm } from './projects.models';

@Injectable({ providedIn: 'root' })
export class ProjectsApi {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = inject(API_BASE_URL);

  getByProfile(profileId: string) {
    return this.http.get<ProjectVm[]>(`${this.apiBaseUrl}/PortfolioProfiles/${profileId}/Projects`);
  }
}
