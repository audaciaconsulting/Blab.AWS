import type { UpdateMessagesToReadResponse } from "@/models/messages/update-messages-to-read-response.model";
import type { ApiResponseWithType } from "@/models/api/api-response-with-type.model";
import type { ApiResponse } from "@/models/api/api-response.model";
import type { AddMessage } from "@/models/messages/add-message.model";
import baseService from "@/services/base.service";

export class MessagesService {
  public getMessages(chatId: number): Promise<ApiResponse> {
    //calls the endpoint to get a the messages of a user in the specified chat.
    return baseService.get(`/api/chat/${chatId}/messages`);
  }

  public addMessage(chatId: number, model: AddMessage): Promise<ApiResponse> {
    //calls the post request to add a message to a chat
    return baseService.post(`/api/chat/${chatId}/message`, model);
  }

  public updateMessagesToRead(
    chatId: number
  ): Promise<ApiResponseWithType<UpdateMessagesToReadResponse>> {
    //calls the endpoint that updates the unread messages to read

    return baseService.put(`/api/chat/${chatId}/messages/read`, {});
  }
}
