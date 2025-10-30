import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';
import { ButtonModule } from 'primeng/button';
@Component({
  selector: 'app-register',
  imports: [FormsModule, ButtonModule, CheckboxModule, InputTextModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {}
