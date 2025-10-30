import {
  Component,
  Output,
  EventEmitter,
  ChangeDetectorRef,
  OnInit,
  OnDestroy,
  AfterViewChecked,
  AfterViewInit,
  signal,
} from '@angular/core';
import { TextareaModule } from 'primeng/textarea';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { ChatService } from '../../../services/chat-service';
import { Subscription } from 'rxjs';
import { ChatHistoryDto, ChatMessageDto, ChatRequestDto } from '../../../interfaces/chat-interface';
import { SharedService } from '../../../services/shared-service';

@Component({
  selector: 'app-chat-form',
  imports: [TextareaModule, ButtonModule, FormsModule],
  templateUrl: './chat-form.html',
  styleUrl: './chat-form.css',
})
export class ChatForm implements OnInit, OnDestroy, AfterViewChecked, AfterViewInit {
  @Output() recieveChatMessage = new EventEmitter<ChatMessageDto>();
  subscription: Subscription = new Subscription();
  value: string = '';
  loading: boolean = false;
  hasChat = signal(false);

  constructor(
    private cd: ChangeDetectorRef,
    private chatService: ChatService,
    private sharedService: SharedService
  ) {}

  ngOnInit(): void {
    this.checkChat();
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  ngAfterViewChecked(): void {
    this.checkChat();
  }

  ngAfterViewInit(): void {
    this.checkChat();
  }

  submitPrompt(value: string) {
    this.loading = true;
    this.recieveChatMessage.emit(this.buildChatMessage(value, 'user'));

    let payload: ChatRequestDto = { prompt: value };

    this.subscription.add(
      this.chatService.postSingleChatResponse(payload).subscribe((result: any) => {
        if (this.value !== '' && result) {
          this.recieveChatMessage.emit(result);
        }

        this.loading = false;
        this.value = '';
      })
    );

    this.cd.detectChanges();
  }

  buildChatMessage(message: string, role: 'user' | 'assistant'): ChatMessageDto {
    let chatMessage: ChatMessageDto = {
      role: role,
      message: message,
    };

    return chatMessage;
  }

  checkChat() {
    this.subscription.add(
      this.sharedService.chatHistory$.subscribe((result) => {
        this.hasChat.set(result?.chatMessages?.length > 0 ? true : false);
        this.cd.detectChanges();
      })
    );
  }

  clearPrompt() {
    this.value = '';
    this.cd.detectChanges();
  }

  clearChat() {
    this.sharedService.updateChatHistory({ chatMessages: [] });
    this.cd.detectChanges();
  }
}
