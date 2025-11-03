import {
  ApplicationConfig,
  importProvidersFrom,
  provideBrowserGlobalErrorListeners,
  provideZonelessChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { providePrimeNG } from 'primeng/config';
import { provideHttpClient } from '@angular/common/http';
import { routes } from './app.routes';
import { MyPreset } from './themes/mypresets';
import { provideFirebaseApp, initializeApp } from '@angular/fire/app';
import { provideAuth, getAuth } from '@angular/fire/auth';
import { provideFirestore, getFirestore } from '@angular/fire/firestore';
import { environment_firebase } from './environments/environment';
import { AuthCaGuard } from './guards/auth-ca-guard';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideRouter(routes),
    provideHttpClient(),
    providePrimeNG({
      theme: {
        preset: MyPreset,
        options: {
          darkModeSelector: '.my-app-dark',
          // cssLayer: {
          //   name: 'primeng',
          //   order: 'primeng',
          // },
        },
      },
    }),
    provideFirebaseApp(() => initializeApp(environment_firebase.firebaseConfig)),
    provideAuth(() => getAuth()),
    provideFirestore(() => getFirestore()),
    AuthCaGuard,
  ],
};
