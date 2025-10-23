import { isPlatformBrowser } from '@angular/common';
import {
  Component,
  OnInit,
  PLATFORM_ID,
  ChangeDetectorRef,
  inject,
  signal,
  effect,
  Input,
} from '@angular/core';

import { ChartModule } from 'primeng/chart';

import {
  ChartData,
  ChartDataset,
  ChartOptions,
  AxisOptions,
} from '../../../interfaces/chart-interface';
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
  @Input() data: AdafruitData[] = [];
  @Input() feedName!: string; //WIP
  @Input() chartType: 'bar' | 'line' = 'line';
  @Input() borderColor: string = 'black';
  @Input() borderWidth: number = 2;
  @Input() isLineDash: boolean = false;
  @Input() isFill: boolean = false;
  @Input() tension: number = 0.4;

  title!: string;
  lightDarkMode = signal('Light');

  chartData: ChartData = {
    labels: [],
    datasets: [],
  };

  options!: ChartOptions;

  platformId = inject(PLATFORM_ID);

  constructor(private cd: ChangeDetectorRef) {}

  ngOnInit() {
    this.initChart();
  }

  chartLabelBuilder(data: AdafruitData[]): string[] {
    return data.map((d) => d.createdAt);
  }

  chartDataSetBuilder(
    chartType: 'bar' | 'line' | null,
    label: string,
    borderColor: string,
    data: AdafruitData[],
    fill: boolean = false,
    borderDash: number[] | null = this.isLineDash ? [5, 5] : null,
    borderWidth: number = 2,
    tension: number = 0.4,
    documentStyle: CSSStyleDeclaration | null = null
  ): ChartDataset {
    return {
      type: chartType,
      label: `${label}, ${data[0].unit}`,
      borderColor: documentStyle?.getPropertyValue?.(borderColor)
        ? documentStyle.getPropertyValue(borderColor)
        : borderColor,
      borderDash: borderDash,
      borderWidth: borderWidth,
      fill: fill,
      tension: tension,
      data: data.map((d) => d.value),
    };
  }

  chartOptionBuilder(
    labelTextColor: string,
    tickSecondaryColor: string,
    gridBorderColor: string,
    maintainAspectRatio: boolean = false,
    aspectRatio: number = 0.6
  ): ChartOptions {
    return {
      maintainAspectRatio: maintainAspectRatio,
      aspectRatio: aspectRatio,
      plugins: {
        legend: {
          labels: {
            color: labelTextColor,
          },
        },
      },
      scales: {
        x: {
          ticks: {
            color: tickSecondaryColor,
          },
          grid: {
            color: gridBorderColor,
          },
        },
        y: {
          ticks: {
            color: gridBorderColor,
          },
          grid: {
            color: gridBorderColor,
          },
        },
      },
    };
  }

  initChart() {
    if (isPlatformBrowser(this.platformId)) {
      const documentStyle = getComputedStyle(document.documentElement);
      const textColor = documentStyle.getPropertyValue('--p-text-color');
      const textColorSecondary = documentStyle.getPropertyValue('--p-text-muted-color');
      const surfaceBorder = documentStyle.getPropertyValue('--p-content-border-color');

      // this.chartData.labels = this.chartLabelBuilder(this.data);

      // var chartDataSet: ChartDataset = this.chartDataSetBuilder(
      //   this.chartType,
      //   this.feedName,
      //   this.borderColor,
      //   this.data,
      // );

      // this.chartData.datasets.push(chartDataSet);

      // this.options = this.chartOptionBuilder(textColor, textColorSecondary, surfaceBorder);

      this.chartData = {
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
