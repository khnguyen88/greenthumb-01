import { Routes } from '@angular/router';
import { AuthCaGuard } from './guards/auth-ca-guard';
import { MainPage } from './components/main-page/main-page';
import { LandingPage } from './components/landing-page/landing-page';
import { ChatPage } from './components/chat-page/chat-page';
import { LoginPage } from './components/login-page/login-page';
import { DashboardTemperature } from './components/dashboard/dashboard-temperature/dashboard-temperature';
import { DashboardHumidity } from './components/dashboard/dashboard-humidity/dashboard-humidity';
import { DashboardLight } from './components/dashboard/dashboard-light/dashboard-light';

export const routes: Routes = [
  { path: '', redirectTo: 'landing', pathMatch: 'full' },
  { path: 'landing', component: LandingPage, canActivate: [AuthCaGuard] },
  {
    path: 'dashboard',
    component: MainPage,
    children: [
      { path: 'temperature', component: DashboardTemperature },
      { path: 'humidity', component: DashboardHumidity },
      { path: 'light', component: DashboardLight },
    ],
    canActivateChild: [AuthCaGuard],
  },
  { path: 'chat', component: ChatPage, canActivate: [AuthCaGuard] },
  { path: 'login', component: LoginPage },
];
