import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { AuthService } from '../../../services/auth-service';
import { Subscription } from 'rxjs';
import { AuthCaGuard } from '../../../guards/auth-ca-guard';
@Component({
  selector: 'app-logout',
  imports: [ButtonModule],
  templateUrl: './logout.html',
  styleUrl: './logout.css',
})
export class Logout implements OnDestroy {
  subscription: Subscription = new Subscription();

  constructor(
    private authService: AuthService,
    private authGuard: AuthCaGuard,
    private cd: ChangeDetectorRef
  ) {}

  logout() {
    this.subscription.add(
      this.authService.logout().subscribe(() => {
        localStorage.removeItem('user');
        this.authGuard.authState.set(false);
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
