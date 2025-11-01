import { inject, Injectable, signal, computed } from '@angular/core';
import {
  Auth,
  createUserWithEmailAndPassword,
  updateProfile,
  signInWithEmailAndPassword,
  user,
  signOut,
} from '@angular/fire/auth';
import { Observable, from } from 'rxjs';
import { UserInterface } from '../interfaces/login-interface';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  //Need to look into when to use the inject method, and when to simple DI via constructor.
  //constructor(firebaseAuth: Auth){} should do the same thing as well, but I guess, inject is cleaner and modern.

  firebaseAuth = inject(Auth);
  user$ = user(this.firebaseAuth);
  currentUserSig = signal<UserInterface | null | undefined>(undefined);
  isUserLogin = computed(() => {
    return this.checkLogStatus();
  });

  register(fName: string, lastName: string, email: string, password: string): Observable<void> {
    const promise = createUserWithEmailAndPassword(this.firebaseAuth, email, password).then(
      (response) => updateProfile(response.user, { displayName: `${fName} ${lastName}` })
    );

    return from(promise);
  }

  login(email: string, password: string): Observable<void> {
    const promise = signInWithEmailAndPassword(this.firebaseAuth, email, password).then(() => {});

    return from(promise);
  }

  logout(): Observable<void> {
    const promise = signOut(this.firebaseAuth);
    return from(promise);
  }

  checkLogStatus(): boolean {
    const user = this.currentUserSig();
    return user !== null && user !== undefined;
  }
}
