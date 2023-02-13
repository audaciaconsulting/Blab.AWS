export class GetChatInfoResponse {
  chatId!: number;
  otherUserDisplayName!: string;
  otherUserId!: number;
  userIdOfLastMessageSender!: number | null;
  timeOfLatestMessage!: Date | null;
  contentOfLastMessage!: string | null;
  hasLastMessageBeenRead!: boolean | null;
  profilePhotoBlob!: string;
}
