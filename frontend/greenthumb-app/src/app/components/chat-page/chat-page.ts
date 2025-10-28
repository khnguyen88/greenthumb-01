import { Component } from '@angular/core';
import { ChatContainer } from '../chat/chat-container/chat-container';
import { ChatForm } from '../chat/chat-form/chat-form';
import { SharedService } from '../../services/shared-service';

@Component({
  selector: 'app-chat-page',
  imports: [ChatContainer, ChatForm],
  templateUrl: './chat-page.html',
  styleUrl: './chat-page.css',
})
export class ChatPage {
  constructor(private sharedService: SharedService) {}
}
