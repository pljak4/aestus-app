import { Component, OnInit, inject } from '@angular/core';
import {
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexDataLabels,
  ApexTitleSubtitle,
  NgApexchartsModule,
} from 'ng-apexcharts';
import {
  AnomaliesService,
  StatsItem,
} from '../../../services/anomalies.service';
import { MatCardModule } from '@angular/material/card';

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  dataLabels: ApexDataLabels;
  title: ApexTitleSubtitle;
};

@Component({
  selector: 'app-anomaly-stats',
  imports: [NgApexchartsModule, MatCardModule],
  templateUrl: './anomaly-stats.html',
  styleUrl: './anomaly-stats.scss',
})
export class AnomalyStatsComponent implements OnInit {
  private svc = inject(AnomaliesService);

  public chartOptions: ChartOptions | undefined;

  ngOnInit(): void {
    this.loadChartData();
  }

  private loadChartData(): void {
    this.svc.getStats('1').subscribe((data: StatsItem[]) => {
      this.chartOptions = {
        series: [{ name: 'Sumnjive', data: data.map((d) => d.count) }],
        chart: { type: 'line', height: 350 },
        xaxis: { categories: data.map((d) => d.date) },
        dataLabels: { enabled: false },
        title: { text: 'Sumnjive transakcije po datumu' },
      };
    });
  }
}
