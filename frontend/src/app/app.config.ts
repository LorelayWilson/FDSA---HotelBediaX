import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors, HTTP_INTERCEPTORS } from '@angular/common/http';

import { routes } from './app.routes';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { API_BASE_URL } from './services/api-client';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(
      withInterceptors([
        (req, next) => {
          const baseUrl = 'http://localhost:5259';
          const isAbsolute = /^https?:\/\//i.test(req.url);
          const url = isAbsolute ? req.url : `${baseUrl}/${req.url.replace(/^\//, '')}`;
          return next(req.clone({ url }));
        }
      ])
    ),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true
    },
    {
      provide: API_BASE_URL,
      useValue: 'http://localhost:5259'
    }
  ]
};
