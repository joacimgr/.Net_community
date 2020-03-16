using System.Collections.Generic;

namespace DistroLabCommunity.Models {
    public interface IUserHandler {
        List<User> GetUserByIdList(string userId);
        List<User> GetUserByUsernameList(string username);
        List<User> GetAllUsersList();
        bool AddUser(string userId, string username);
        bool UserIDExists(string userId);
        bool UsernameExists(string username);
        bool AddUserLogin(string userId);
        List<UserLogin> GetUserLoginsById(string userId);
    }
}
