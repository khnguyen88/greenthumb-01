import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { ButtonModule } from 'primeng/button';
import { MenuItem } from 'primeng/api';
import { Menubar } from 'primeng/menubar';

@Component({
  selector: 'app-top-menubar',
  imports: [CommonModule, ButtonModule, Menubar],
  templateUrl: './top-menubar.html',
  styleUrl: './top-menubar.css',
})
export class TopMenubar implements OnInit {
  items: MenuItem[] | undefined;
  protected readonly title = signal('greenthumb-app');
  lightDarkMode = signal('Dark');
  buttonLabel = signal(`Toogle to Dark mode!`);
  pIconClass = signal('pi pi-moon');
  currentRoute = signal('');

  constructor(private router: Router) {
    this.currentRoute.set(this.router.url);
  }

  ngOnInit(): void {
    this.items = [
      {
        label: 'Home',
        icon: 'pi pi-chart-line',
        routerLink: '/home',
        command: () => {
          this.checkRoute();
          this.updateLogStatus();
        },
      },
      {
        label: 'AI Chat',
        icon: 'pi pi-comments',
        routerLink: '/chat',
        command: () => {
          this.checkRoute();
          this.updateLogStatus();
        },
      },
      {
        label: 'Login',
        icon: 'pi pi-sign-in',
        routerLink: '/login',
        command: () => {
          this.checkRoute();
          this.updateLogStatus();
        },
      },
    ];
  }

  toggleDarkMode() {
    const element = document.querySelector('html');
    element?.classList.toggle('my-app-dark');

    this.lightDarkMode.set(this.lightDarkMode() === 'Light' ? 'Dark' : 'Light');
    this.pIconClass.set(this.lightDarkMode() === 'Light' ? 'pi pi-sun' : 'pi pi-moon');

    this.buttonLabel.set(`Toogle to ${this.lightDarkMode()} mode!`);
  }

  checkRoute() {
    this.currentRoute.set(this.router.url);
  }

  updateLogStatus() {
    //REPLACE THIS ONCE WE FIGUREOUT SIGNIN/SIGNOUT IF ANY
    var boolCheck: number = Math.round(Math.random());

    const item = this.items?.find((i) => i.label === 'Login' || i.label === 'Logout');
    if (item) {
      item.label = boolCheck ? 'Logout' : 'Login';
    }
  }
}
