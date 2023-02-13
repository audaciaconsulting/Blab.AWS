import type { Chat } from "@/models/user-chats/chat.model";
import type { ApiResponse } from "@/models/api/api-response.model";
import baseService from "@/services/base.service";
import type { GetChatInfoResponse } from "@/models/user-chats/getChatInfoApiResponse.model";
import type { ApiResponseWithType } from "@/models/api/api-response-with-type.model";

export class ChatService {
  public createNewChat(chat: Chat): Promise<ApiResponse> {
    // calls endpoint to create a new chat between users, if successful, will return the ChatId of the new created chat.
    return baseService.post("/api/chats/add", chat);
  }
  public getChatID(chat: Chat): Promise<ApiResponse> {
    //calls the endpoint to get a chatId between twos users if a chat exists between them, the chatID will be returned.
    return baseService.get(
      `/api/chats/${chat.loggedInUserId}/${chat.otherUserId}`
    );
  }
  public getChatInfo(): Promise<ApiResponseWithType<GetChatInfoResponse[]>> {
    // Calls endpoint to get all the chats the loggedInUser is apart of and its necessary info for the chat navigation.
    //Returns information in as an array of objects, each with type getChatInfoResponse.
    return baseService.get(`/api/chats/all`);
  }
}
