import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'destinations' },
  {
    path: 'destinations',
    loadComponent: () => import('./destinations/destinations-page.component').then(m => m.DestinationsPageComponent)
  },
  {
    path: 'destinations/:id',
    loadComponent: () => import('./destinations/destination-detail-page.component').then(m => m.DestinationDetailPageComponent)
  },
  { path: '**', redirectTo: 'destinations' }
];
