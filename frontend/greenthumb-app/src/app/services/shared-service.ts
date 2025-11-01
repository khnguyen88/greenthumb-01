import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { ChatHistoryDto } from '../interfaces/chat-interface';

@Injectable({
  providedIn: 'root',
})
export class SharedService {
  subscription: Subscription = new Subscription();

  // TODO: Consider refactoring with signal.

  private _themeMode = new BehaviorSubject<string>('Light');
  public themeMode$: Observable<string> = this._themeMode.asObservable();

  private _chatHistory = new BehaviorSubject<ChatHistoryDto>({ chatMessages: [] });
  public chatHistory$: Observable<ChatHistoryDto> = this._chatHistory.asObservable();

  constructor() {}

  updateThemeMode(newMode: string) {
    this._themeMode.next(newMode);
  }

  updateChatHistory(newChatHistory: ChatHistoryDto) {
    this._chatHistory.next(newChatHistory);
  }
}
