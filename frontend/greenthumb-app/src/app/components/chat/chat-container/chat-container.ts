import {
  Component,
  Input,
  AfterViewChecked,
  ElementRef,
  ViewChild,
  OnInit,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatHistoryDto } from '../../../interfaces/chat-interface';

@Component({
  selector: 'app-chat-container',
  imports: [CommonModule],
  templateUrl: './chat-container.html',
  styleUrl: './chat-container.css',
})
export class ChatContainer implements OnInit, AfterViewChecked, OnChanges {
  @ViewChild('scrollMe') private myScrollContainer!: ElementRef;
  @Input() lightDarkMode!: string;
  @Input() chatHistory: ChatHistoryDto = {
    chatMessages: [
      {
        role: 'user',
        message: 'hi',
      },
      {
        role: 'assistant',
        message: 'hello',
      },
      {
        role: 'user',
        message: 'can you tell me the meaning of life?',
      },
      {
        role: 'assistant',
        message: '42',
      },
      {
        role: 'user',
        message: 'what is love?',
      },
      {
        role: 'assistant',
        message: 'a construct of the mind',
      },
      {
        role: 'user',
        message: 'what is my purpose?',
      },
      {
        role: 'assistant',
        message: 'magician',
      },
    ],
  };

  ngOnInit() {
    this.scrollToBottom();
  }

  ngOnChanges(changes: SimpleChanges): void {}

  ngAfterViewChecked(): void {
    this.scrollToBottom();
  }

  //https://stackoverflow.com/questions/35232731/angular-2-scroll-to-bottom-chat-style
  scrollToBottom(): void {
    try {
      this.myScrollContainer.nativeElement.scrollTop =
        this.myScrollContainer.nativeElement.scrollHeight;
    } catch (err) {
      console.error('Scroll error:', err);
    }
  }
}
