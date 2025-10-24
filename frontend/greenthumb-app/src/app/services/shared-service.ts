import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SharedService {
  private _themeMode = new BehaviorSubject<string>('Light'); 
  public themeMode$: Observable<string> = this._themeMode.asObservable();

  constructor() {}

  updateThemeMode(newMode: string) {
    this._themeMode.next(newMode);
  }
}
