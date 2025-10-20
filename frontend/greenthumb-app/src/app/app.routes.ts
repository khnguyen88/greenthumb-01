import { Routes } from '@angular/router';
import { MainPage } from './components/main-page/main-page';
import { ChatPage } from './components/chat-page/chat-page';
import { LoginPage } from './components/login-page/login-page';
import { DashboardTemperature } from './components/dashboard/dashboard-temperature/dashboard-temperature';
import { DashboardHumidity } from './components/dashboard/dashboard-humidity/dashboard-humidity';
import { DashboardLight } from './components/dashboard/dashboard-light/dashboard-light';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard/temperature', pathMatch: 'full' },
  {
    path: 'dashboard',
    component: MainPage,
    children: [
      { path: 'temperature', component: DashboardTemperature },
      { path: 'humidity', component: DashboardHumidity },
      { path: 'light', component: DashboardLight },
    ],
  },
  { path: 'chat', component: ChatPage },
  { path: 'login', component: LoginPage },
];
