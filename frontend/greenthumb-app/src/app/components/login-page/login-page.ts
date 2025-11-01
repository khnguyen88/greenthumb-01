import { Component, signal } from '@angular/core';
import { Register } from '../login/register/register';
import { Login } from '../login/login/login';
import { Logout } from '../login/logout/logout';
import { AuthService } from '../../services/auth-service';
@Component({
  selector: 'app-login-page',
  imports: [Register, Login, Logout],
  templateUrl: './login-page.html',
  styleUrl: './login-page.css',
})
export class LoginPage {
  isNewMember = signal(false);

  constructor(public authService: AuthService) {}

  toggleRegister(flagState: boolean) {
    this.isNewMember.set(flagState);
  }
}
