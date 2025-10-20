import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { DashboardTemperature } from '../dashboard/dashboard-temperature/dashboard-temperature';
import { DashboardHumidity } from '../dashboard/dashboard-humidity/dashboard-humidity';
import { DashboardLight } from '../dashboard/dashboard-light/dashboard-light';

@Component({
  selector: 'app-main-page',
  imports: [RouterOutlet, DashboardTemperature, DashboardHumidity, DashboardLight],
  templateUrl: './main-page.html',
  styleUrl: './main-page.css',
})
export class MainPage {}
