import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject, catchError, of, throwError } from 'rxjs';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AdafruitService {
  constructor(private http: HttpClient) {}

  getTemperatureData() {
    return this.http.get(environment.baseUrl + 'Adafruit/GetPhotoResistorFeedData').pipe(
      catchError((err) => {
        return this.handleError(err);
      })
    );
  }

  handleError(err: Error) {
    if (err instanceof HttpErrorResponse) {
      if (err.status == 0) {
        console.log('This service could not be reached!');
      } else if (err.status != 403) {
        console.log('Some issue other than authorization has occurred!');
      } else {
        console.log(err.status);
      }
    }

    return throwError(() => err);
  }
}
