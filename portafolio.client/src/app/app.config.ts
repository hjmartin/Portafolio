import { ApplicationConfig, ErrorHandler, provideZoneChangeDetection } from '@angular/core';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideRouter, withComponentInputBinding, withInMemoryScrolling } from '@angular/router';

import { environment } from '../environments/environment';
import { appRoutes } from './app.routes';
import { API_BASE_URL } from './core/config/app.tokens';
import { GlobalErrorHandler } from './core/errors/global-error-handler';
import { apiErrorInterceptor } from './core/interceptors/api-error.interceptor';
import { authTokenInterceptor } from './core/interceptors/auth-token.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(
      appRoutes,
      withComponentInputBinding(),
      withInMemoryScrolling({ scrollPositionRestoration: 'enabled' })
    ),
    provideHttpClient(withInterceptors([authTokenInterceptor, apiErrorInterceptor])),
    {
      provide: ErrorHandler,
      useClass: GlobalErrorHandler
    },
    {
      provide: API_BASE_URL,
      useValue: environment.apiBaseUrl
    }
  ]
};
