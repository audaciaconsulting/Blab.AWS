using Audacia.Azure.BlobStorage.AddBlob;
using Audacia.Azure.BlobStorage.DeleteBlob;
using Audacia.Azure.BlobStorage.GetBlob;
using Audacia.Azure.BlobStorage.UpdateBlob;
using Blab.Services.Services.BlabFeed;
using Blab.Services.Services.Comments.AddComment;
using Blab.Services.Services.Comments.SearchComments;
using Blab.Services.Services.Follows.AddFollow;
using Blab.Services.Services.Follows.DeleteFollow;
﻿using Blab.Services.Services.Reactions.AddReaction;
using Blab.Services.Services.Reactions.DeleteReaction;
using Blab.Services.Services.Reactions.UpdateReaction;
using Blab.Services.Services.UserChats.DeleteUserChat;
using Blab.Services.Services.UserChats.GetAllChatsForLoggedInUser;
using Blab.Services.Services.Messages.UpdateReadDateTimeOfMessages;
using Blab.Services.Services.Users.SearchForUsers;
using Blab.Services.Services.Users.UpdateUser;

namespace Blab.Api.Extensions;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adding Domain Services to the IoC container.
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    public static IServiceCollection AddDomainServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddChatServices()
            .AddReactionServices()
            .AddBlabFeedServices()
            .AddMessageService()
            .AddCommentServices()
            .AddUserServices()
            .AddFollowServices()
            .AddAzureBlobServices();
    }

    private static IServiceCollection AddChatServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddTransient<IDeleteUserChatService, DeleteUserChatService>()
                                .AddTransient<IGetAllChatsForLoggedInUserService, GetAllChatsForLoggedInUserService>();
    }

    private static IServiceCollection AddReactionServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddTransient<IAddReactionService, AddReactionService>()
            .AddTransient<IUpdateReactionService, UpdateReactionService>()
            .AddTransient<IDeleteReactionService, DeleteReactionService>();
    }

    private static IServiceCollection AddMessageService(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddTransient<IUpdateReadDateTimeOfMessagesService, UpdateReadDateTimeOfMessagesService>();
    }

    private static IServiceCollection AddUserServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddTransient<ISearchForUsersService, SearchForUsersService>()
            .AddTransient<IUpdateUserService, UpdateUserService>();
    }
    
    private static IServiceCollection AddFollowServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddTransient<IAddFollowService, AddFollowService>()
            .AddTransient<IDeleteFollowService, DeleteFollowService>();
    }

    private static IServiceCollection AddBlabFeedServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddTransient<IGetBlabFeedService, GetBlabFeedService>()
            .AddTransient<IGetProfileBlabFeedService, GetProfileBlabFeedService>();
    }

    private static IServiceCollection AddCommentServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddTransient<IAddCommentService, AddCommentService>()
            .AddTransient<ISearchCommentsService, SearchCommentsService>();
    }

    private static IServiceCollection AddAzureBlobServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddTransient<IGetAzureBlobStorageService, GetAzureBlobStorageService>()
            .AddTransient<IAddAzureBlobStorageService, AddAzureBlobStorageService>()
            .AddTransient<IUpdateAzureBlobStorageService, UpdateAzureBlobStorageService>();
    }
}
