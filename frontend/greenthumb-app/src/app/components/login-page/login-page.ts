import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';
import { ButtonModule } from 'primeng/button';
@Component({
  selector: 'app-login-page',
  imports: [FormsModule, ButtonModule, CheckboxModule, InputTextModule],
  templateUrl: './login-page.html',
  styleUrl: './login-page.css',
})
export class LoginPage {}
