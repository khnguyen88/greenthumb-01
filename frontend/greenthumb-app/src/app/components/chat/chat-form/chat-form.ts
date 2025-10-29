import {
  Component,
  Output,
  EventEmitter,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  OnDestroy,
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
export class ChatForm implements OnDestroy {
  @Output() recieveChatMessage = new EventEmitter<ChatMessageDto>();
  subscription: Subscription = new Subscription();
  value: string = '';
  loading: boolean = false;

  constructor(
    private cd: ChangeDetectorRef,
    private chatService: ChatService,
    private sharedService: SharedService
  ) {}

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

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  buildChatMessage(message: string, role: 'user' | 'assistant'): ChatMessageDto {
    let chatMessage: ChatMessageDto = {
      role: role,
      message: message,
    };

    return chatMessage;
  }

  clearPronpt() {
    this.value = '';
    this.cd.detectChanges();
  }

  clearChat() {
    this.sharedService.updateChatHistory({ chatMessages: [] });
    this.cd.detectChanges();
  }
}
