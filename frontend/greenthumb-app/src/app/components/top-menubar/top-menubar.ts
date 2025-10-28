import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { ButtonModule } from 'primeng/button';
import { MenuItem } from 'primeng/api';
import { Menubar } from 'primeng/menubar';
import { SharedService } from '../../services/shared-service';

@Component({
  selector: 'app-top-menubar',
  imports: [CommonModule, ButtonModule, Menubar],
  templateUrl: './top-menubar.html',
  styleUrl: './top-menubar.css',
})
export class TopMenubar implements OnInit {
  items: MenuItem[] | undefined;
  protected readonly title = signal('greenthumb-app');
  lightDarkMode = signal('Light');
  lightDarkModeLabel = signal(this.lightDarkMode() === 'Light' ? 'Dark' : 'Light');
  buttonLabel = signal(`Toogle to Dark mode!`);
  pIconClass = signal('pi pi-moon');
  currentRoute = signal('');

  constructor(private router: Router, private sharedService: SharedService) {
    this.currentRoute.set(this.router.url);
  }

  ngOnInit(): void {
    this.items = [
      {
        label: 'Home',
        icon: 'pi pi-chart-line',
        items: [
          {
            label: 'Temperature Dashboard',
            routerLink: '/dashboard/temperature',
          },
          {
            label: 'Humidity Dashboard',
            routerLink: '/dashboard/humidity',
          },
        ],
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

    this.sharedService.updateThemeMode(this.lightDarkMode());
  }

  toggleDarkMode() {
    const element = document.querySelector('html');
    const element2 = document.querySelector('html');
    element?.classList.toggle('my-app-dark');
    element2?.classList.toggle('dark');

    this.lightDarkMode.set(this.lightDarkMode() === 'Light' ? 'Dark' : 'Light');
    this.sharedService.updateThemeMode(this.lightDarkMode());

    this.lightDarkModeLabel.set(this.lightDarkModeLabel() === 'Light' ? 'Dark' : 'Light');
    this.pIconClass.set(this.lightDarkModeLabel() === 'Light' ? 'pi pi-sun' : 'pi pi-moon');

    this.buttonLabel.set(`Toogle to ${this.lightDarkModeLabel()} mode!`);
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
