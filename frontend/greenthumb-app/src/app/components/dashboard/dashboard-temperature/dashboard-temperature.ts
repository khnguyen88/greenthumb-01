import { Component } from '@angular/core';
import { DashboardBase } from '../dashboard-base/dashboard-base';

@Component({
  selector: 'app-dashboard-temperature',
  imports: [DashboardBase],
  templateUrl: './dashboard-temperature.html',
  styleUrl: './dashboard-temperature.css',
})
export class DashboardTemperature {}
