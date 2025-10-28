export interface ChatMessageDto {
  role: string;
  message: string;
}

export interface ChatHistoryDto {
  chatMessages: ChatMessageDto[];
}
