import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject, catchError, of, throwError } from 'rxjs';
import { environment } from '../environments/environment';
import { AdafruitData } from '../interfaces/adafruit-interface';

@Injectable({
  providedIn: 'root',
})
export class AdafruitService {
  constructor(private http: HttpClient) {}
  getGrowLightTriggerData() {
    return this.http
      .get<AdafruitData[]>(
        environment.baseUrl +
          environment.subPath.adafruit +
          environment.adafruitData.growlightTrigger,
        {
          headers: {
            'Content-Type': 'application/json',
          },
        }
      )
      .pipe(
        catchError((err) => {
          return this.handleError(err);
        })
      );
  }

  getTemperatureData() {
    return this.http
      .get<AdafruitData[]>(
        environment.baseUrl + environment.subPath.adafruit + environment.adafruitData.temperature,
        {
          headers: {
            'Content-Type': 'application/json',
          },
        }
      )
      .pipe(
        catchError((err) => {
          return this.handleError(err);
        })
      );
  }

  getHumidityData() {
    return this.http
      .get<AdafruitData[]>(
        environment.baseUrl + environment.subPath.adafruit + environment.adafruitData.humidity,
        {
          headers: {
            'Content-Type': 'application/json',
          },
        }
      )
      .pipe(
        catchError((err) => {
          return this.handleError(err);
        })
      );
  }

  getLightIntensityData() {
    return this.http
      .get<AdafruitData[]>(
        environment.baseUrl +
          environment.subPath.adafruit +
          environment.adafruitData.lightIntensity,
        {
          headers: {
            'Content-Type': 'application/json',
          },
        }
      )
      .pipe(
        catchError((err) => {
          return this.handleError(err);
        })
      );
  }

  getSoilMoistureData() {
    return this.http
      .get<AdafruitData[]>(
        environment.baseUrl + environment.subPath.adafruit + environment.adafruitData.soilMoisture,
        {
          headers: {
            'Content-Type': 'application/json',
          },
        }
      )
      .pipe(
        catchError((err) => {
          return this.handleError(err);
        })
      );
  }

  getPumpTriggerData() {
    return this.http
      .get<AdafruitData[]>(
        environment.baseUrl + environment.subPath.adafruit + environment.adafruitData.pumpTrigger,
        {
          headers: {
            'Content-Type': 'application/json',
          },
        }
      )
      .pipe(
        catchError((err) => {
          return this.handleError(err);
        })
      );
  }

  getWaterLevelData() {
    return this.http
      .get<AdafruitData[]>(
        environment.baseUrl + environment.subPath.adafruit + environment.adafruitData.waterLevel,
        {
          headers: {
            'Content-Type': 'application/json',
          },
        }
      )
      .pipe(
        catchError((err) => {
          return this.handleError(err);
        })
      );
  }

  getPlantHeightData() {
    return this.http
      .get<AdafruitData[]>(
        environment.baseUrl + environment.subPath.adafruit + environment.adafruitData.plantHeight,
        {
          headers: {
            'Content-Type': 'application/json',
          },
        }
      )
      .pipe(
        catchError((err) => {
          return this.handleError(err);
        })
      );
  }

  getFeedDataByName(feedName: string): Observable<AdafruitData[]> {
    let santizedFeedName = feedName.toLowerCase();
    console.log(santizedFeedName);
    switch (santizedFeedName) {
      case 'temperature':
        return this.getTemperatureData();
      case 'humidity':
        return this.getHumidityData();
      case 'growlight trigger':
        return this.getGrowLightTriggerData();
      case 'light intensity':
        return this.getLightIntensityData();
      case 'soil moisture':
        return this.getSoilMoistureData();
      case 'water level':
        return this.getWaterLevelData();
      case 'plant height':
        return this.getPlantHeightData();
      case 'pump trigger':
        return this.getPumpTriggerData();
      default:
        console.warn(`Unknown feed type: ${feedName}`);
        return new Observable<AdafruitData[]>();
    }
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
