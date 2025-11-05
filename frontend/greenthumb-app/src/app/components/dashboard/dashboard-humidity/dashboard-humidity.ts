import { Component, OnInit, ChangeDetectorRef, signal, OnDestroy } from '@angular/core';
import { DashboardBase } from '../dashboard-base/dashboard-base';
import { AdafruitService } from '../../../services/adafruit-service';
import { AdafruitData } from '../../../interfaces/adafruit-interface';
import { Subscription } from 'rxjs';
import { SharedService } from '../../../services/shared-service';
@Component({
  selector: 'app-dashboard-humidity',
  imports: [DashboardBase],
  templateUrl: './dashboard-humidity.html',
  styleUrl: './dashboard-humidity.css',
})
export class DashboardHumidity implements OnInit, OnDestroy {
  data!: AdafruitData[];
  subscription: Subscription = new Subscription();
  lightDarkMode = signal('Light');

  constructor(
    private cd: ChangeDetectorRef,
    private adafruitService: AdafruitService,
    private sharedService: SharedService
  ) {}

  ngOnInit(): void {
    this.subscription.add(
      this.adafruitService.getHumidityData().subscribe((result) => {
        this.data = result;
        console.log(result);
        this.cd.detectChanges();
      })
    );

    this.subscription.add(
      this.sharedService.themeMode$.subscribe((result) => {
        this.lightDarkMode.set(result);
        this.cd.detectChanges();
      })
    );
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
