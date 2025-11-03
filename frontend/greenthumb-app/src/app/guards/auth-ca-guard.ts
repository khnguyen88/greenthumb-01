import { Injectable, Signal, signal } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanActivateChild,
  GuardResult,
  MaybeAsync,
  RouterStateSnapshot,
  Router,
} from '@angular/router';
import { AuthService } from '../services/auth-service';

@Injectable({
  providedIn: 'root',
})
export class AuthCaGuard implements CanActivate, CanActivateChild {
  authState = signal(sessionStorage.getItem('user') !== null);
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): MaybeAsync<GuardResult> {
    return this.isAuth();
  }

  canActivateChild(
    childRoute: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): MaybeAsync<GuardResult> {
    return this.isAuth();
  }

  private isAuth(): boolean {
    console.log(this.authService.isUserLogin() || this.authState());
    if (this.authService.isUserLogin()) {
      return true;
    } else {
      this.router.navigate(['/login']);
      return false;
    }
  }
}
