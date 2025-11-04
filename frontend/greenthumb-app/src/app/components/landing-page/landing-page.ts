import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { Router } from '@angular/router';

@Component({
  selector: 'app-landing-page',
  imports: [CommonModule, ButtonModule],
  templateUrl: './landing-page.html',
  styleUrl: './landing-page.css',
})
export class LandingPage {
  constructor(private router: Router) {}

  routeToDashboard(): void {
    this.router.navigateByUrl('/dashboard/humidity');
  }
  routeToChat(): void {
    this.router.navigateByUrl('/chat');
  }
}
