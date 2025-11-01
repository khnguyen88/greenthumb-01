import { Component, Output, EventEmitter, ChangeDetectorRef, signal } from '@angular/core';
import { ReactiveFormsModule, FormsModule, FormBuilder, Validators } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';
import { ButtonModule } from 'primeng/button';
import { AuthService } from '../../../services/auth-service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, FormsModule, ButtonModule, CheckboxModule, InputTextModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  @Output() setNewMemberFlagEvent = new EventEmitter<boolean>();

  loginForm: any;
  errorMessage = signal<string | null>(null);

  constructor(
    private cd: ChangeDetectorRef,
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private http: HttpClient,
    private router: Router
  ) {
    this.loginForm = this.formBuilder.nonNullable.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  onLinkClick() {
    this.setNewMemberFlagEvent.emit(true);
    this.cd.detectChanges();
  }

  onSubmit(): void {
    const rawFormData = this.loginForm.getRawValue();
    this.authService.login(rawFormData.email, rawFormData.password).subscribe({
      next: () => {
        this.errorMessage.set(null);
        this.loginForm.reset();
        this.router.navigateByUrl('/');
      },
      error: (e) => {
        this.errorMessage.set(e.code);
        console.log(this.errorMessage());
        this.cd.detectChanges();
      },
    });
    this.cd.detectChanges();
  }
}
