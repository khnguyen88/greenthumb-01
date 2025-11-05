import { Component } from '@angular/core';
import { DashboardBase } from '../dashboard-base/dashboard-base';

@Component({
  selector: 'app-dashboard-humidity',
  imports: [DashboardBase],
  templateUrl: './dashboard-humidity.html',
  styleUrl: './dashboard-humidity.css',
})
export class DashboardHumidity {
  constructor() {}
}
