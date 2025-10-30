import { Component, OnChanges, OnDestroy, OnInit, signal } from '@angular/core';
import { ChatContainer } from '../chat/chat-container/chat-container';
import { ChatForm } from '../chat/chat-form/chat-form';
import { SharedService } from '../../services/shared-service';
import { Subscription } from 'rxjs';
import { ChatHistoryDto, ChatMessageDto } from '../../interfaces/chat-interface';

@Component({
  selector: 'app-chat-page',
  imports: [ChatContainer, ChatForm],
  templateUrl: './chat-page.html',
  styleUrl: './chat-page.css',
})
export class ChatPage implements OnInit, OnDestroy {
  subscription: Subscription = new Subscription();
  lightDarkMode = signal('Light');
  chatHistory = signal<ChatHistoryDto>({ chatMessages: [] });

  constructor(private sharedService: SharedService) {}

  ngOnInit(): void {
    this.subscription.add(
      this.sharedService.themeMode$.subscribe((result) => {
        this.lightDarkMode.set(result);
      })
    );

    this.subscription.add(
      this.sharedService.chatHistory$.subscribe((result) => {
        this.chatHistory.set(result);
        console.log(JSON.stringify(result));
      })
    );
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  handleChatResponse(message: ChatMessageDto) {
    let updatedChatHistory: ChatHistoryDto = this.chatHistory();
    updatedChatHistory.chatMessages.push(message);
    this.sharedService.updateChatHistory(updatedChatHistory);
    this.chatHistory.set(updatedChatHistory);
  }
}
