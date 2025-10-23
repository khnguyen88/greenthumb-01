import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { DashboardBase } from '../dashboard-base/dashboard-base';
import { AdafruitService } from '../../../services/adafruit-service';
import { AdafruitData } from '../../../interfaces/adafruit-interface';

@Component({
  selector: 'app-dashboard-humidity',
  imports: [DashboardBase],
  templateUrl: './dashboard-humidity.html',
  styleUrl: './dashboard-humidity.css',
})
export class DashboardHumidity implements OnInit {
  data!: AdafruitData[];
  feedName: string = 'Humidity';

  constructor(private cd: ChangeDetectorRef, private adafruitService: AdafruitService) {}

  ngOnInit(): void {
    this.adafruitService.getHumidityData().subscribe((result) => {
      this.data = result;
      console.log(result);
      this.cd.detectChanges();
    });
  }
}
