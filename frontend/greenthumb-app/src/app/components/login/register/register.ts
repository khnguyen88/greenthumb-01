import { Component, Output, EventEmitter, ChangeDetectorRef, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormsModule, Validators } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';
import { ButtonModule } from 'primeng/button';
import { AuthService } from '../../../services/auth-service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, FormsModule, ButtonModule, CheckboxModule, InputTextModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  @Output() setNewMemberFlagEvent = new EventEmitter<boolean>();

  registerForm: any;
  errorMessage = signal<string | null>(null);

  constructor(
    private cd: ChangeDetectorRef,
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private http: HttpClient,
    private router: Router
  ) {
    this.registerForm = this.formBuilder.nonNullable.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  onLinkClick(): void {
    this.setNewMemberFlagEvent.emit(false);
    this.cd.detectChanges();
  }

  onSubmit(): void {
    const rawFormData = this.registerForm.getRawValue();
    this.authService
      .register(
        rawFormData.firstName,
        rawFormData.lastName,
        rawFormData.email,
        rawFormData.password
      )
      .subscribe({
        next: () => {
          this.router.navigateByUrl('/');
          this.errorMessage.set(null);
          this.registerForm.reset();
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
