using System.Collections.Generic;

namespace DistroLabCommunity.Models {
    public interface IMessageHandler {
        List<Message> GetMessagesByReceiverId(string userId);
        List<Message> GetMessagesBySenderId(string userId);
        List<Message> GetMessagesByMessageId(int messageId);
        bool AddMessage(string title, string text, User senderUser, User receiverUser);
        bool SetMessageAsOpened(int messageId);
        bool removeMessageByMid(int messageId);
    }
}
