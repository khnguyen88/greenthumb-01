import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { AuthService } from '../../../services/auth-service';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-logout',
  imports: [ButtonModule],
  templateUrl: './logout.html',
  styleUrl: './logout.css',
})
export class Logout implements OnDestroy {
  subscription: Subscription = new Subscription();

  constructor(private authService: AuthService, private cd: ChangeDetectorRef) {}

  logout() {
    this.subscription.add(
      this.authService.logout().subscribe(() => {
        this.authService.currentUserSig.set(undefined);
        this.authService.isLoginSubmitSuccessful.set(false);
      })
    );
    this.cd.detectChanges();
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
