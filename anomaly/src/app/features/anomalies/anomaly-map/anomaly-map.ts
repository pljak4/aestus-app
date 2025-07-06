import { Component, inject } from '@angular/core';
import { AnomaliesService, Anomaly } from '../../../services/anomalies.service';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { LeafletModule } from '@asymmetrik/ngx-leaflet';
import { tileLayer, marker, icon, Map as LeafletMap } from 'leaflet';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';

const CITY_COORDS: Record<string, [number, number]> = {
  Zagreb: [45.815, 15.9819],
  Split: [43.5081, 16.4402],
  Rijeka: [45.3271, 14.4422],
  Osijek: [45.554, 18.6939],
  Zadar: [44.1194, 15.2314],
  Sarajevo: [43.8563, 18.4131],
  Beograd: [44.7866, 20.4489],
};

@Component({
  selector: 'app-anomaly-map',
  imports: [
    CommonModule,
    LeafletModule,
    RouterLink,
    MatCardModule,
    MatButtonModule,
  ],
  templateUrl: './anomaly-map.html',
  styleUrl: './anomaly-map.scss',
})
export class AnomalyMapComponent {
  private anomaliesService = inject(AnomaliesService);
  private map!: LeafletMap;

  options = {
    layers: [
      tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 18,
      }),
    ],
    zoom: 6,
    center: [43.8563, 18.4131] as [number, number],
  };

  private customIcon = icon({
    iconUrl: 'https://cdn-icons-png.flaticon.com/512/684/684908.png',
    iconSize: [32, 32],
    iconAnchor: [16, 32],
    popupAnchor: [0, -32],
  });

  onMapReady(map: LeafletMap): void {
    this.map = map;
    const userId = '1';

    this.anomaliesService.getAnomalies(userId).subscribe((data: Anomaly[]) => {
      const list = data.filter((a) => CITY_COORDS.hasOwnProperty(a.location));

      const groups = list.reduce((acc, a) => {
        (acc[a.location] = acc[a.location] || []).push(a);
        return acc;
      }, {} as Record<string, Anomaly[]>);

      Object.entries(groups).forEach(([city, items]) => {
        const base = CITY_COORDS[city]!;
        const radius = 0.02;
        const count = items.length;

        items.forEach((a, i) => {
          const angle = ((2 * Math.PI) / count) * i;
          const lat = base[0] + radius * Math.cos(angle);
          const lng = base[1] + radius * Math.sin(angle);

          marker([lat, lng], { icon: this.customIcon })
            .bindTooltip(`€${a.amount.toFixed(2)}`, { direction: 'bottom' })
            .bindPopup(
              `
            <b>Iznos:</b> €${a.amount}<br>
            <b>Grad:</b> ${a.location}<br>
            <b>Vrijeme:</b> ${new Date(a.utcTimestamp).toLocaleString()}
          `
            )
            .addTo(this.map);
        });
      });
    });
  }
}
