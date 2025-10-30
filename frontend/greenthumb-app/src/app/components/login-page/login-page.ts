import { Component, signal } from '@angular/core';
import { Register } from '../login/register/register';
import { Login } from '../login/login/login';
@Component({
  selector: 'app-login-page',
  imports: [Register, Login],
  templateUrl: './login-page.html',
  styleUrl: './login-page.css',
})
export class LoginPage {
  isRegister = signal(false);

  toggleRegister(flagState: boolean) {
    this.isRegister.set(flagState);
  }
}
