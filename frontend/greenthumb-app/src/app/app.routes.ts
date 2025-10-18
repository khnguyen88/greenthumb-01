import { Routes } from '@angular/router';
import { MainPage } from './components/main-page/main-page';
import { ChatPage } from './components/chat-page/chat-page';
import { LoginPage } from './components/login-page/login-page';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: MainPage },
  { path: 'chat', component: ChatPage },
  { path: 'login', component: LoginPage },
];
