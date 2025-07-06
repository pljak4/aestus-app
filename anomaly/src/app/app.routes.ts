import { Routes } from '@angular/router';
import { AnomalyListComponent } from './features/anomalies/anomaly-list/anomaly-list';
import { AnomalyMapComponent } from './features/anomalies/anomaly-map/anomaly-map';
import { AnomalyDetailComponent } from './features/anomalies/anomaly-detail/anomaly-detail';
import { AnomalyStatsComponent } from './features/anomalies/anomaly-stats/anomaly-stats';

export const routes: Routes = [
  { path: '', redirectTo: 'anomalies', pathMatch: 'full' },
  { path: 'anomalies', component: AnomalyListComponent },
  { path: 'anomalies/map', component: AnomalyMapComponent },
  { path: 'anomalies/stats', component: AnomalyStatsComponent },
  { path: 'anomalies/detail/:id', component: AnomalyDetailComponent },
  { path: '**', redirectTo: 'anomalies' },
];
