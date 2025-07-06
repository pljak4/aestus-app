import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Anomaly {
  id: string;
  userId: string;
  amount: number;
  utcTimestamp: string;
  location: string;
  isSuspicious: boolean;
  comment?: string;
}

export interface StatsItem {
  date: string;
  count: number;
}

@Injectable({ providedIn: 'root' })
export class AnomaliesService {
  private http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7156';

  getAnomalies(userId: string): Observable<Anomaly[]> {
    return this.http.get<Anomaly[]>(`${this.apiUrl}/users/${userId}/anomalies`);
  }

  getStats(userId: string): Observable<StatsItem[]> {
    return this.http.get<StatsItem[]>(
      `${this.apiUrl}/users/${userId}/anomalies/stats`
    );
  }

  getAnomalyById(id: string): Observable<Anomaly> {
    return this.http.get<Anomaly>(`${this.apiUrl}/transactions/${id}`);
  }
}
