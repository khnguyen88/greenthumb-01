import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject, catchError, of, throwError } from 'rxjs';
import { environment } from '../environments/environment';
import { AdafruitData } from '../interfaces/adafruit-interface';
import { ChatHistoryDto, ChatRequestDto } from '../interfaces/chat-interface';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  constructor(private http: HttpClient) {}

  postSingleChatResponse(prompt: ChatRequestDto) {
    return this.http
      .post<string>(
        environment.baseUrl +
          environment.subPath.chatCompletion +
          environment.chat.singleChatResponse,
        prompt,
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

  postAllChatResponses(prompt: ChatRequestDto) {
    return this.http
      .post<ChatHistoryDto>(
        environment.baseUrl +
          environment.subPath.chatCompletion +
          environment.chat.allChatResponses,
        prompt,
        {
          headers: {
            'Content-Type': 'application/json',
          },
        }
      )
      .pipe(catchError((err) => this.handleError(err)));
  }

  postPartialChatResponses(prompt: ChatRequestDto) {
    return this.http
      .post<ChatHistoryDto>(
        environment.baseUrl +
          environment.subPath.chatCompletion +
          environment.chat.partialChatResponses,
        prompt,
        {
          headers: {
            'Content-Type': 'application/json',
          },
        }
      )
      .pipe(catchError((err) => this.handleError(err)));
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
