import { Component, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { ButtonModule } from 'primeng/button';
import { MenuItem } from 'primeng/api';
import { Menubar } from 'primeng/menubar';

import { TopMenubar } from './components/top-menubar/top-menubar';
import { LoginPage } from './components/login-page/login-page';
import { MainPage } from './components/main-page/main-page';
import { ChatPage } from './components/chat-page/chat-page';
import { AuthService } from './services/auth-service';
import { UserInterface } from './interfaces/login-interface';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ButtonModule, Menubar, TopMenubar],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App implements OnInit {
  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.authService.user$.subscribe((user) => {
      if (user) {
        let userInfo: UserInterface = {
          email: user.email!,
          displayName: user.displayName!,
          uid: user.uid,
        };

        this.authService.currentUserSig.set(userInfo);
      } else {
        this.authService.currentUserSig.set(null);
      }
    });
  }

  checkLogStatus(): boolean {
    return this.authService.checkLogStatus();
  }
}
