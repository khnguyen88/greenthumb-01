import { Component, OnChanges, OnDestroy, OnInit, signal } from '@angular/core';
import { ChatContainer } from '../chat/chat-container/chat-container';
import { ChatForm } from '../chat/chat-form/chat-form';
import { SharedService } from '../../services/shared-service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-chat-page',
  imports: [ChatContainer, ChatForm],
  templateUrl: './chat-page.html',
  styleUrl: './chat-page.css',
})
export class ChatPage implements OnInit, OnDestroy {
  subscription!: Subscription;
  lightDarkMode = signal('Light');

  constructor(private sharedService: SharedService) {}

  ngOnInit(): void {
    this.subscription = this.sharedService.themeMode$.subscribe((result) => {
      this.lightDarkMode.set(result);
    });
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
