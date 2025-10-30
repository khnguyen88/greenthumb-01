import { Component, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';
import { ButtonModule } from 'primeng/button';
@Component({
  selector: 'app-login',
  imports: [FormsModule, ButtonModule, CheckboxModule, InputTextModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  @Output() setNewMemberFlagEvent = new EventEmitter<boolean>();

  constructor(private cd: ChangeDetectorRef) {}

  onLinkClick() {
    this.setNewMemberFlagEvent.emit(true);
    this.cd.detectChanges();
  }
}
