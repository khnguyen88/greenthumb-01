import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatHistoryDto } from '../../../interfaces/chat-interface';

@Component({
  selector: 'app-chat-container',
  imports: [CommonModule],
  templateUrl: './chat-container.html',
  styleUrl: './chat-container.css',
})
export class ChatContainer {
  @Input() chatHistory: ChatHistoryDto = {
    chatMessages: [
      {
        role: 'assistant',
        message: 'hi',
      },
      {
        role: 'user',
        message: 'hello',
      },
      {
        role: 'assistant',
        message: 'how can I help you?',
      },
      {
        role: 'user',
        message: 'can you tell me the meaning of life?',
      },
    ],
  };
}
