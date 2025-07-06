import { Component, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { AnomaliesService, Anomaly } from '../../../services/anomalies.service';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';

@Component({
  selector: 'app-anomaly-list',
  imports: [
    CommonModule,
    RouterLink,
    MatButtonModule,
    MatCardModule,
    MatTableModule,
  ],
  templateUrl: './anomaly-list.html',
  styleUrl: './anomaly-list.scss',
})
export class AnomalyListComponent {
  private anomaliesService = inject(AnomaliesService);

  displayedColumns: string[] = [
    'id',
    'city',
    'amount',
    'utcTimestamp',
    'actions',
  ];
  anomalies$!: Observable<Anomaly[]>;

  ngOnInit(): void {
    const userId = '1';
    this.anomalies$ = this.anomaliesService.getAnomalies(userId);
  }
}
