using AgenticGreenthumbApi.Domain;
using Azure.Messaging;
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

        public bool IsUserChatHistoryRecordExpired(string userId)
        {
            UserChatHistory userChatHistoryRecord = GetUserChatHistory(userId);
            int recordExpirationDay = 1;
            DateTime currentDateTime = DateTime.UtcNow;
            DateTime expirationDateTime = userChatHistoryRecord.SessionDateTime.AddDays(recordExpirationDay);

            int compareResult = DateTime.Compare(expirationDateTime, currentDateTime);

            return compareResult >= 0;
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


        public void AddUpdateUserChatHistory(string username, ChatHistory currentChatHistory)
        {

            UserChatHistory userChatHistory = GetUserChatHistory(username);

            if (IsUserChatHistoryRecordExpired(username))
            {
                userChatHistory.ChatHistory = new ChatHistory();
            }
            else
            {
                userChatHistory.ChatHistory = currentChatHistory;
            }
        }

        public void DeleteUserChatHistory(string username)
        {
            InMemUserChatHistory.TryRemove(username, out _);
        }
    }
}
