import { Routes } from '@angular/router';
import { AuthCaGuard } from './guards/auth-ca-guard';
import { MainPage } from './components/main-page/main-page';
import { LandingPage } from './components/landing-page/landing-page';
import { ChatPage } from './components/chat-page/chat-page';
import { LoginPage } from './components/login-page/login-page';
import { DashboardTemperature } from './components/dashboard/dashboard-temperature/dashboard-temperature';
import { DashboardHumidity } from './components/dashboard/dashboard-humidity/dashboard-humidity';
import { DashboardLight } from './components/dashboard/dashboard-light/dashboard-light';
import { DashboardSoilMoisture } from './components/dashboard/dashboard-soil-moisture/dashboard-soil-moisture';
import { DashboardPlantHeight } from './components/dashboard/dashboard-plant-height/dashboard-plant-height';
import { DashboardWaterLevel } from './components/dashboard/dashboard-water-level/dashboard-water-level';
import { DashboardLightTrigger } from './components/dashboard/dashboard-light-trigger/dashboard-light-trigger';
import { DashboardPumpTrigger } from './components/dashboard/dashboard-pump-trigger/dashboard-pump-trigger';

export const routes: Routes = [
  { path: '', redirectTo: 'landing', pathMatch: 'full' },
  { path: 'landing', component: LandingPage, canActivate: [AuthCaGuard] },
  {
    path: 'dashboard',
    component: MainPage,
    children: [
      { path: 'temperature', component: DashboardTemperature },
      { path: 'humidity', component: DashboardHumidity },
      { path: 'light-intensity', component: DashboardLight },
      { path: 'soil-moisture', component: DashboardSoilMoisture },
      { path: 'plant-height', component: DashboardPlantHeight },
      { path: 'water-depth', component: DashboardWaterLevel },
      { path: 'growlight-trigger', component: DashboardLightTrigger },
      { path: 'pump-trigger', component: DashboardPumpTrigger },
    ],
    canActivateChild: [AuthCaGuard],
  },
  { path: 'chat', component: ChatPage, canActivate: [AuthCaGuard] },
  { path: 'login', component: LoginPage },
];
