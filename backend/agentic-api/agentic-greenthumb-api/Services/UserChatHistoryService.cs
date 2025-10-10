using AgenticGreenthumbApi.Domain;
using Azure.Messaging;
using Elastic.Clients.Elasticsearch;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using StackExchange.Redis;
using System.Collections.Concurrent;

namespace AgenticGreenthumbApi.Services
{
    public class UserChatHistoryService
    {
        public ConcurrentDictionary<string, UserChatHistory> InMemUserChatHistory { get; set; }

        public UserChatHistoryService()
        {
            InMemUserChatHistory = new();
        }

        public bool IsUserChatHistoryRecordExpired(string username)
        {
            InMemUserChatHistory.TryGetValue(username, out UserChatHistory? userChatHistoryRecord);

            if (userChatHistoryRecord == null)
            {
                return true;
            }
            else {
                int recordExpirationDay = 1;
                DateTime currentDateTime = DateTime.UtcNow;
                DateTime expirationDateTime = GetUserChatHistory(username).SessionDateTime.AddDays(recordExpirationDay);

                int compareResult = DateTime.Compare(expirationDateTime, currentDateTime);

                return compareResult >= 0;
            }
        }

        public UserChatHistory GetUserChatHistory(string username)
        {
            InMemUserChatHistory.TryGetValue(username, out UserChatHistory? userChatHistoryRecord);

            if (userChatHistoryRecord == null)
            {
                return new UserChatHistory()
                {
                    Username = username,
                    SessionDateTime = DateTime.UtcNow,
                    ChatHistory = new ChatHistory()
                };
            }
            else
            {
                return userChatHistoryRecord;
            }
        }

        public void AddUpdateUserChatHistory(string username, ChatHistory latestChatHistory)
        {
            UserChatHistory userChatHistory = GetUserChatHistory(username);

            userChatHistory.ChatHistory = latestChatHistory;

            InMemUserChatHistory.AddOrUpdate(username, userChatHistory, (username, userChatHistory) => userChatHistory);
        }


        public void DeleteUserChatHistory(string username)
        {
            InMemUserChatHistory.TryRemove(username, out _);
        }

        public void PopulateAgentThread(ChatHistoryAgentThread agentThread, ChatHistory userChatHistory)
        {
            foreach (ChatMessageContent chatMessage in userChatHistory)
            {
                agentThread.ChatHistory.Add(chatMessage);
            }
        }
    }
}
