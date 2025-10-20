import { Component, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { ButtonModule } from 'primeng/button';
import { MenuItem } from 'primeng/api';
import { Menubar } from 'primeng/menubar';

import { TopMenubar } from './components/top-menubar/top-menubar';
import { LoginPage } from './components/login-page/login-page';
import { MainPage } from './components/main-page/main-page';
import { ChatPage } from './components/chat-page/chat-page';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ButtonModule, Menubar, TopMenubar],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App implements OnInit {
  ngOnInit(): void {}
}
