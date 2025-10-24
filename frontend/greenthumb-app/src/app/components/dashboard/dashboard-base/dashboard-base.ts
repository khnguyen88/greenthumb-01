import { isPlatformBrowser } from '@angular/common';
import {
  Component,
  OnInit,
  OnChanges,
  SimpleChanges,
  PLATFORM_ID,
  ChangeDetectorRef,
  inject,
  signal,
  effect,
  Input,
  HostListener,
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
export class DashboardBase implements OnInit, OnChanges {
  @Input() data: AdafruitData[] = [];
  @Input() feedName!: string; //WIP
  @Input() chartType: 'bar' | 'line' = 'line';
  @Input() borderColor: string = 'black';
  @Input() borderWidth: number = 2;
  @Input() isLineDash: boolean = false;
  @Input() isFill: boolean = false;
  @Input() tension: number = 0.4;
  @Input() simplifyYAxis: boolean = false;
  @Input() lightDarkMode!: string;

  title!: string;
  simplifyYAxisSignal = signal(false);

  chartData: ChartData = {
    labels: [],
    datasets: [],
  };

  options!: ChartOptions;

  platformId = inject(PLATFORM_ID);

  windowWidth = signal(window.innerWidth);

  constructor(private cd: ChangeDetectorRef) {}

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.windowWidth.set(event.target.innerWidth);
    this.initChart();
    this.cd.detectChanges();
  }

  ngOnInit() {
    this.windowWidth.set(window.innerWidth);
  }

  ngOnChanges(changes: SimpleChanges) {
    this.simplifyYAxisSignal.set(this.simplifyYAxis);

    if (changes['data'] && this.data?.length) {
      this.initChart();
    }

    if (changes['lightDarkMode']) {
      this.initChart();
    }

    this.cd.detectChanges();
  }

  chartLabelBuilder(data: AdafruitData[]): string[] {
    const dataLabel = data?.map((d) => d.createdAt);
    if (this.simplifyYAxisSignal()) {
      return this.simplifyLabel(dataLabel);
    } else {
      return dataLabel;
    }
  }

  chartDataSetBuilder(
    chartType: 'bar' | 'line' | null,
    label: string,
    borderColor: string, // expects a CSS variable like '--p-orange-500'
    data: AdafruitData[],
    fill: boolean = false,
    borderDash: number[] | null,
    borderWidth: number = 2,
    tension: number = 0.4,
    documentStyle: CSSStyleDeclaration | null = null
  ): ChartDataset {
    const unit = data[0].unit;
    return {
      type: this.chartType,
      label: `${label}, ${unit}`,
      borderColor: documentStyle?.getPropertyValue?.(borderColor)?.trim() || borderColor,
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
            color: tickSecondaryColor,
          },
          grid: {
            color: gridBorderColor,
          },
        },
      },
    };
  }

  initChart() {
    this.chartData.labels = [];
    this.chartData.datasets = [];

    if (isPlatformBrowser(this.platformId)) {
      const documentStyle = getComputedStyle(document.documentElement);
      const textColor = documentStyle.getPropertyValue('--p-text-color');
      const textColorSecondary = documentStyle.getPropertyValue('--p-text-muted-color');
      const surfaceBorder = documentStyle.getPropertyValue('--p-content-border-color');

      this.chartData.labels = this.chartLabelBuilder(this.data);

      console.log(this.data.map((d) => d.value));
      var chartDataSet: ChartDataset = this.chartDataSetBuilder(
        'line',
        this.feedName,
        '--p-purple-500', // CSS variable name
        this.data,
        false, // fill
        this.isLineDash ? [5, 5] : [5, 0],
        2, // borderWidth
        0.4, // tension
        documentStyle // CSSStyleDeclaration
      );

      this.chartData.datasets.push(chartDataSet);

      this.options = this.chartOptionBuilder(textColor, textColorSecondary, surfaceBorder);

      this.cd.markForCheck();
    }
  }

  simplifyLabel(rawLabel: string[]): string[] {
    const dateOnlyLabel = rawLabel.map((d) => d.split(' ')[0]);
    const uniqueLabel: string[] = [];
    return dateOnlyLabel.map((d, i) => {
      if (uniqueLabel?.findIndex((u) => u === d) < 0) {
        uniqueLabel.push(d);
        return rawLabel[i].split(' ')[0];
      } else {
        return this.windowWidth() > 2000 || rawLabel.length < 30
          ? rawLabel[i].split(' ')[1].slice(0, -3) + '-hr'
          : '';
      }
    });
  }
}
