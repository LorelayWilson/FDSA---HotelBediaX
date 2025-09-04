import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'destinations' },
  {
    path: 'destinations',
    loadComponent: () => import('./destinations/destinations-page.component').then(m => m.DestinationsPageComponent)
  },
  { path: '**', redirectTo: 'destinations' }
];
