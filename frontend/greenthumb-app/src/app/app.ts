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
import { AuthCaGuard } from './guards/auth-ca-guard';
import { UserInterface } from './interfaces/login-interface';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ButtonModule, Menubar, TopMenubar],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App implements OnInit {
  constructor(
    private authService: AuthService,
    private cd: ChangeDetectorRef,
    private authGuard: AuthCaGuard
  ) {
    this.authGuard.authState.set(localStorage.getItem('user') !== null);
    this.authService.isLoginSubmitSuccessful.set(localStorage.getItem('user') !== null);
  }

  ngOnInit(): void {
    this.authService.user$.subscribe((user) => {
      if (user) {
        let userInfo: UserInterface = {
          email: user.email!,
          displayName: user.displayName!,
          uid: user.uid,
        };
        this.authService.currentUserSig.set(userInfo);
        localStorage.setItem('user', JSON.stringify(userInfo));
        this.authService.isLoginSubmitSuccessful.set(true);
        this.authGuard.authState.set(true);
        this.cd.detectChanges();
      } else {
        this.authService.currentUserSig.set(null);
        localStorage.removeItem('user');
        this.authService.isLoginSubmitSuccessful.set(false);
        this.authGuard.authState.set(false);
        this.cd.detectChanges();
      }
    });
  }

  checkLogStatus(): boolean {
    return this.authService.checkLogStatus();
  }
}
