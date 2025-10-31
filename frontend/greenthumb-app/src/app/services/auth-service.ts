import { inject, Injectable } from '@angular/core';
import { Auth, createUserWithEmailAndPassword, updateProfile } from '@angular/fire/auth';
import { Observable, from } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  //Need to look into when to use the inject method, and when to simple DI via constructor.
  //constructor(firebaseAuth: Auth){} should do the same thing as well. It is cleaner.

  firebaseAuth = inject(Auth);

  register(fName: string, lastName: string, email: string, password: string): Observable<void> {
    const promise = createUserWithEmailAndPassword(this.firebaseAuth, email, password).then(
      (response) => updateProfile(response.user, { displayName: `${fName} ${lastName}` })
    );

    return from(promise);
  }
}
