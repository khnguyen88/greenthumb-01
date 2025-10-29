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
import { ChatHistoryDto, ChatRequestDto } from '../../../interfaces/chat-interface';

@Component({
  selector: 'app-chat-form',
  imports: [TextareaModule, ButtonModule, FormsModule],
  templateUrl: './chat-form.html',
  styleUrl: './chat-form.css',
})
export class ChatForm implements OnDestroy {
  @Output() newPromptEvent = new EventEmitter<ChatHistoryDto>();
  subscription!: Subscription;
  value: string = '';
  loading: boolean = false;

  constructor(private cd: ChangeDetectorRef, private chatService: ChatService) {}

  submitPrompt(value: string) {
    console.log(value);

    let payload: ChatRequestDto = { prompt: value };

    this.subscription = this.chatService
      .postPartialChatResponses(payload)
      .subscribe((result: any) => {
        console.log(JSON.stringify(result));
        if (this.value !== '' && result) {
          this.newPromptEvent.emit(result);
        }
      });

    this.value = '';
    this.load();
  }

  load() {
    this.loading = true;
    this.cd.detectChanges();

    setTimeout(() => {
      this.loading = false;

      this.cd.detectChanges();
    }, 2000);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
