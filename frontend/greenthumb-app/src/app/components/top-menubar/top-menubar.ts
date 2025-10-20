import { Component, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { ButtonModule } from 'primeng/button';
import { MenuItem } from 'primeng/api';
import { Menubar } from 'primeng/menubar';

@Component({
  selector: 'app-top-menubar',
  imports: [ButtonModule, Menubar],
  templateUrl: './top-menubar.html',
  styleUrl: './top-menubar.css',
})
export class TopMenubar implements OnInit {
  items: MenuItem[] | undefined;
  protected readonly title = signal('greenthumb-app');
  lightDarkMode = signal('Dark');
  buttonLabel = signal(`Toogle to Dark mode!`);

  ngOnInit(): void {
    this.items = [
      {
        label: 'Home',
        icon: 'pi pi-home',
      },
      {
        label: 'Features',
        icon: 'pi pi-star',
      },
    ];
  }

  toggleDarkMode() {
    const element = document.querySelector('html');
    element?.classList.toggle('my-app-dark');

    this.lightDarkMode.set(this.lightDarkMode() === 'Light' ? 'Dark' : 'Light');
    this.buttonLabel.set(`Toogle to ${this.lightDarkMode()} mode!`);
  }
}
