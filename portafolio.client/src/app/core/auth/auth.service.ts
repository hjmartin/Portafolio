import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { map, tap } from 'rxjs';

import { environment } from '../../../environments/environment';
import { API_BASE_URL } from '../config/app.tokens';
import { AuthResponse, AuthSession, AuthUser, LoginCredentials } from './auth.models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = inject(API_BASE_URL);
  private readonly sessionState = signal<AuthSession | null>(this.readStoredSession());

  readonly session = this.sessionState.asReadonly();
  readonly user = computed(() => this.sessionState()?.user ?? null);
  readonly accessToken = computed(() => this.sessionState()?.accessToken ?? null);
  readonly isAuthenticated = computed(() => !!this.accessToken());
  readonly isAdmin = computed(() => this.user()?.roles.includes('Admin') ?? false);

  login(credentials: LoginCredentials) {
    return this.http.post<AuthResponse>(`${this.apiBaseUrl}/auth/login`, credentials).pipe(
      map(response => this.toSession(response)),
      tap(session => this.persistSession(session))
    );
  }

  restore(session: AuthSession): void {
    this.persistSession(session);
  }

  logout(): void {
    this.sessionState.set(null);
    localStorage.removeItem(environment.authStorageKey);
  }

  private persistSession(session: AuthSession): void {
    this.sessionState.set(session);
    localStorage.setItem(environment.authStorageKey, JSON.stringify(session));
  }

  private readStoredSession(): AuthSession | null {
    const rawSession = localStorage.getItem(environment.authStorageKey);
    if (!rawSession) {
      return null;
    }

    try {
      const parsed = JSON.parse(rawSession) as AuthSession;
      return this.isValidUser(parsed.user) && !!parsed.accessToken ? parsed : null;
    } catch {
      localStorage.removeItem(environment.authStorageKey);
      return null;
    }
  }

  private toSession(response: AuthResponse): AuthSession {
    const claims = this.readJwtClaims(response.accessToken);
    const user: AuthUser = {
      id: this.getStringClaim(claims, 'sub') ?? this.getStringClaim(claims, 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier') ?? '',
      email: this.getStringClaim(claims, 'email') ?? this.getStringClaim(claims, 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress') ?? '',
      roles: this.getRoles(claims)
    };

    return {
      accessToken: response.accessToken,
      refreshToken: response.refreshToken,
      accessTokenExpiresAtUtc: response.accessTokenExpiresAtUtc,
      refreshTokenExpiresAtUtc: response.refreshTokenExpiresAtUtc,
      user
    };
  }

  private readJwtClaims(token: string): Record<string, unknown> {
    const parts = token.split('.');
    if (parts.length < 2) {
      throw new Error('Invalid access token format.');
    }

    const normalizedPayload = parts[1].replace(/-/g, '+').replace(/_/g, '/');
    const paddedPayload = normalizedPayload.padEnd(Math.ceil(normalizedPayload.length / 4) * 4, '=');
    const jsonPayload = decodeURIComponent(
      Array.from(atob(paddedPayload))
        .map(char => `%${char.charCodeAt(0).toString(16).padStart(2, '0')}`)
        .join('')
    );

    return JSON.parse(jsonPayload) as Record<string, unknown>;
  }

  private getRoles(claims: Record<string, unknown>): string[] {
    const candidates = [
      claims['role'],
      claims['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
    ];

    return candidates.flatMap(value => {
      if (Array.isArray(value)) {
        return value.filter((item): item is string => typeof item === 'string');
      }

      return typeof value === 'string' ? [value] : [];
    }).filter((value, index, array) => array.indexOf(value) === index);
  }

  private getStringClaim(claims: Record<string, unknown>, claimName: string): string | null {
    const value = claims[claimName];
    return typeof value === 'string' ? value : null;
  }

  private isValidUser(user: AuthUser | null | undefined): user is AuthUser {
    return !!user && typeof user.email === 'string' && Array.isArray(user.roles);
  }
}
