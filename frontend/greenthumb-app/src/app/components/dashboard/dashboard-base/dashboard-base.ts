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
  OnDestroy,
  AfterViewInit,
  EffectRef,
} from '@angular/core';

import { toSignal } from '@angular/core/rxjs-interop';

import { ChartModule } from 'primeng/chart';

import { Observable, Subscription } from 'rxjs';

import {
  ChartData,
  ChartDataset,
  ChartOptions,
  AxisOptions,
} from '../../../interfaces/chart-interface';
import { AdafruitData } from '../../../interfaces/adafruit-interface';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';

import { AdafruitService } from '../../../services/adafruit-service';
import { SharedService } from '../../../services/shared-service';

@Component({
  selector: 'app-dashboard-base',
  imports: [ButtonModule, TableModule, ChartModule],
  templateUrl: './dashboard-base.html',
  styleUrl: './dashboard-base.css',
})
export class DashboardBase implements OnInit, OnChanges, OnDestroy, AfterViewInit {
  @Input() data: AdafruitData[] = [];
  @Input() feedName!: string; //WIP
  @Input() chartType: 'bar' | 'line' = 'line';
  @Input() borderColor: string = 'black';
  @Input() borderWidth: number = 2;
  @Input() isLineDash: boolean = false;
  @Input() isFill: boolean = false;
  @Input() tension: number = 0.4;
  @Input() simplifyYAxis: boolean = false;
  lightDarkMode = signal('Light');
  lightDarkEffect!: EffectRef;

  title!: string;
  simplifyYAxisSignal = signal(false);

  chartData: ChartData = {
    labels: [],
    datasets: [],
  };

  options!: ChartOptions;

  platformId = inject(PLATFORM_ID);

  windowWidth = signal(window.innerWidth);

  subscription = new Subscription();

  //Consider replacing constructor with using inject();
  constructor(
    private adafruitService: AdafruitService,
    private sharedService: SharedService,
    private cd: ChangeDetectorRef
  ) {
    let lightDarkObservableSignal = toSignal(this.sharedService.themeMode$, {
      initialValue: 'Light',
    });

    this.lightDarkEffect = effect(() => {
      this.lightDarkMode.set(lightDarkObservableSignal());
    });
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.windowWidth.set(event.target.innerWidth);
    this.cd.detectChanges();
  }

  ngOnInit() {
    this.windowWidth.set(window.innerWidth);
    this.loadFeedData(this.feedName, () => {
      this.initChart();
    });
    this.cd.detectChanges();
  }

  ngOnChanges(changes: SimpleChanges) {
    this.simplifyYAxisSignal.set(this.simplifyYAxis);

    if (changes['data'] && this.data?.length) {
      this.initChart();
    }

    this.cd.detectChanges();
  }

  ngAfterViewInit(): void {
    this.loadFeedData(this.feedName, () => {
      this.initChart();
    });
    this.cd.detectChanges();
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
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
        '--p-green-500', // CSS variable name
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

  private loadFeedData(feedName: string = 'empty', afterLoad?: () => void): void {
    let dataObservable: Observable<AdafruitData[]> =
      this.adafruitService.getFeedDataByName(feedName);

    this.subscription.add(
      dataObservable.subscribe((result) => {
        this.data = result;
        console.log(result);
        this.cd.detectChanges();
        if (afterLoad) {
          afterLoad?.();
          this.cd.detectChanges();
        }
      })
    );
  }
}
