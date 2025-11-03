import {
  Component,
  OnInit,
  signal,
  Input,
  AfterViewInit,
  OnChanges,
  SimpleChanges,
  ChangeDetectorRef,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { ButtonModule } from 'primeng/button';
import { MenuItem } from 'primeng/api';
import { Menubar } from 'primeng/menubar';
import { SharedService } from '../../services/shared-service';
import { AuthService } from '../../services/auth-service';

@Component({
  selector: 'app-top-menubar',
  imports: [CommonModule, ButtonModule, Menubar],
  templateUrl: './top-menubar.html',
  styleUrl: './top-menubar.css',
})
export class TopMenubar implements OnInit, OnChanges, AfterViewInit {
  items: MenuItem[] = [];
  protected readonly title = signal('greenthumb-app');
  lightDarkMode = signal('Light');
  lightDarkModeLabel = signal(this.lightDarkMode() === 'Light' ? 'Dark' : 'Light');
  buttonLabel = signal(`Toogle to Dark mode!`);
  pIconClass = signal('pi pi-moon');
  currentRoute = signal('');
  @Input() isLogin!: boolean;

  constructor(
    private router: Router,
    private sharedService: SharedService,
    private authService: AuthService,
    private cd: ChangeDetectorRef
  ) {
    this.currentRoute.set(this.router.url);
  }

  ngOnInit(): void {
    this.isLogin = this.authService.isUserLogin();
    let initialLabel = this.isLogin ? 'Logout' : 'Login';
    this.items = [
      {
        label: 'Home',
        icon: 'pi pi-comments',
        routerLink: '/landing',
        command: () => {
          this.checkRoute();
        },
      },
      {
        label: 'Dashboard',
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
        },
      },
      {
        label: 'AI Chat',
        icon: 'pi pi-comments',
        routerLink: '/chat',
        command: () => {
          this.checkRoute();
        },
      },
      {
        label: initialLabel,
        icon: 'pi pi-sign-in',
        routerLink: '/login',
        command: () => {
          this.checkRoute();
        },
      },
    ];

    this.sharedService.updateThemeMode(this.lightDarkMode());
    this.cd.detectChanges();
  }

  ngAfterViewInit(): void {
    this.updateLogStatus();
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.updateLogStatus();
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
    const isLogin: boolean = this.authService.checkLogStatus();

    if (this.isLogin !== isLogin) {
      this.isLogin = isLogin;
    }

    const item = this.items?.find((i) => i.label === 'Login' || i.label === 'Logout');
    if (item) {
      item.label = this.isLogin ? 'Logout' : 'Login';
    }

    const updatedItems: MenuItem[] = this.items?.map((i) => {
      if (i.label === 'Login' || i.label === 'Logout') {
        return { ...i, label: this.isLogin ? 'Logout' : 'Login' };
      } else {
        return i;
      }
    });

    this.items = [...updatedItems];

    this.cd.detectChanges();
  }
}
