import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { AnomaliesService, Anomaly } from '../../../services/anomalies.service';
import { Observable, switchMap } from 'rxjs';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-anomaly-detail',
  imports: [CommonModule, RouterLink, MatCardModule, MatButton],
  templateUrl: './anomaly-detail.html',
  styleUrl: './anomaly-detail.scss',
})
export class AnomalyDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private anomaliesService = inject(AnomaliesService);

  anomaly$!: Observable<Anomaly>;

  ngOnInit(): void {
    this.anomaly$ = this.route.paramMap.pipe(
      switchMap((params) =>
        this.anomaliesService.getAnomalyById(params.get('id')!)
      )
    );
  }
}
