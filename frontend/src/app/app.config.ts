import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(
      withInterceptors([
        (req, next) => {
          // Base URL del backend + versionado
          const baseUrl = 'http://localhost:5259/api/v1.0';
          const isAbsolute = /^https?:\/\//i.test(req.url);
          const url = isAbsolute ? req.url : `${baseUrl}/${req.url.replace(/^\//, '')}`;
          return next(req.clone({ url }));
        }
      ])
    )
  ]
};
