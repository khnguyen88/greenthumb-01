import { isPlatformBrowser } from '@angular/common';
import {
  Component,
  OnInit,
  PLATFORM_ID,
  ChangeDetectorRef,
  inject,
  effect,
  Input,
} from '@angular/core';
import { ChartModule } from 'primeng/chart';
import { ChartData } from '../../../interfaces/chart-interface';
import { AdafruitData } from '../../../interfaces/adafruit-interface';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-dashboard-base',
  imports: [ButtonModule, TableModule, ChartModule],
  templateUrl: './dashboard-base.html',
  styleUrl: './dashboard-base.css',
})
export class DashboardBase implements OnInit {
  @Input() feedName!: string; //WIP

  title!: string;

  @Input() data!: AdafruitData[];

  chartData!: ChartData;

  options: any;

  platformId = inject(PLATFORM_ID);

  constructor(private cd: ChangeDetectorRef) {}

  ngOnInit() {
    this.initChart();
    console.log(JSON.stringify(this.data));
  }

  initChart() {
    if (isPlatformBrowser(this.platformId)) {
      const documentStyle = getComputedStyle(document.documentElement);
      const textColor = documentStyle.getPropertyValue('--p-text-color');
      const textColorSecondary = documentStyle.getPropertyValue('--p-text-muted-color');
      const surfaceBorder = documentStyle.getPropertyValue('--p-content-border-color');

      var data = {
        labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
        datasets: [
          {
            type: 'line',
            label: 'Dataset 1',
            borderColor: documentStyle.getPropertyValue('--p-orange-500'),
            borderWidth: 2,
            fill: false,
            tension: 0.4,
            data: [50, 25, 12, 48, 56, 76, 42],
          },
          {
            type: 'bar',
            label: 'Dataset 2',
            backgroundColor: documentStyle.getPropertyValue('--p-gray-500'),
            data: [21, 84, 24, 75, 37, 65, 34],
            borderColor: 'white',
            borderWidth: 2,
          },
          {
            type: 'bar',
            label: 'Dataset 3',
            backgroundColor: documentStyle.getPropertyValue('--p-cyan-500'),
            data: [41, 52, 24, 74, 23, 21, 32],
          },
        ],
      };

      this.options = {
        maintainAspectRatio: false,
        aspectRatio: 0.6,
        plugins: {
          legend: {
            labels: {
              color: textColor,
            },
          },
        },
        scales: {
          x: {
            ticks: {
              color: textColorSecondary,
            },
            grid: {
              color: surfaceBorder,
            },
          },
          y: {
            ticks: {
              color: textColorSecondary,
            },
            grid: {
              color: surfaceBorder,
            },
          },
        },
      };

      this.cd.markForCheck();
    }
  }
}
