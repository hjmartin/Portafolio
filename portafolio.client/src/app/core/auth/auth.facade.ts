import { Injectable, computed, inject } from '@angular/core';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';

import { LoginCredentials } from './auth.models';
import { AuthService } from './auth.service';

@Injectable({ providedIn: 'root' })
export class AuthFacade {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  readonly session = this.authService.session;
  readonly user = this.authService.user;
  readonly isAuthenticated = this.authService.isAuthenticated;
  readonly isAdmin = this.authService.isAdmin;
  readonly accessToken = this.authService.accessToken;
  readonly displayName = computed(() => this.user()?.email ?? 'Invitado');

  async login(credentials: LoginCredentials): Promise<void> {
    await firstValueFrom(this.authService.login(credentials));
    await this.router.navigate(['/admin/portfolio']);
  }

  async logout(): Promise<void> {
    this.authService.logout();
    await this.router.navigate(['/login']);
  }
}
