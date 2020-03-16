using DistroLabCommunity.Data;
using System.Collections.Generic;
using System.Linq;

namespace DistroLabCommunity.Models {

    //https://stackoverflow.com/questions/41058142/injecting-dbcontext-into-service-layer

    /// <summary>
    /// Class for handling CommunityContext users.
    /// Contains methods for adding, updating, fetching and checking data
    /// in the user and userlogin tables.
    /// </summary>
    public class UserHandler : IUserHandler{

        private CommunityContext _communityContext;

        public UserHandler(CommunityContext communityContext) {
            _communityContext = communityContext;
        }

        /// <summary>
        /// Returns all users from the database with a matching UserID
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns>A list of users</returns>
        public List<User> GetUserByIdList(string userId) {
            var user = _communityContext.Users.Where(b => b.UserID.Equals(userId));
            List<User> u = user.Select(a => new User {
                UserID = a.UserID,
                Username = a.Username,
                Created = a.Created
            }).ToList();
            return u;
        }

        /// <summary>
        /// Returns all users from the database with a matching username
        /// </summary>
        /// <param name="username">Username of the user</param>
        /// <returns>A list of users</returns>
        public List<User> GetUserByUsernameList(string username) {
            var user = _communityContext.Users.Where(b => b.Username.Equals(username));
            List<User> u = user.Select(a => new User {
                UserID = a.UserID,
                Username = a.Username,
                Created = a.Created
            }).ToList();
            return u;
        }

        public List<User> GetAllUsersList() {
            List<User> users = _communityContext.Users.OrderBy(u => u.Username).ToList();
            return users;
        }

        /// <summary>
        /// Adds a new user to the database
        /// </summary>
        /// <param name="userId">UserID matching the Identity generated UserID</param>
        /// <param name="username">Name of the user</param>
        /// <returns>True or false depending on if the operation was successful</returns>
        public bool AddUser(string userId, string username) {
            try {
                User newUser = new User();
                newUser.UserID = userId;
                newUser.Username = username;
                _communityContext.Add(newUser);
                _communityContext.SaveChanges();
                return true;
            }
            catch{
                return false;
            }
        }

        /// <summary>
        /// Checks whether a user with the corresponding userID exists in the database
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>True or false depending on if the userID was found</returns>
        public bool UserIDExists(string userID) {
            return _communityContext.Users.Any(u => u.UserID == userID);
        }

        /// <summary>
        /// Checks whether a user with the corresponding username exists in the database
        /// </summary>
        /// <param name="username"></param>
        /// <returns>True or false depending on if the username was found</returns>
        public bool UsernameExists(string username) {
            return _communityContext.Users.Any(u => u.Username == username);
        }

        /// <summary>
        /// Saves the current time and date of the given userID
        /// Is used for updating login history data table
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>True or false depending on successful update</returns>
        public bool AddUserLogin(string userId) {
            List<User> user = GetUserByIdList(userId);
            if(user.Count < 1) {
                return false;
            }
            else {
                try {
                    _communityContext.Attach(user[0]);
                    UserLogin userLogin = new UserLogin();
                    userLogin.User = user[0];
                    _communityContext.Add(userLogin);
                    _communityContext.SaveChanges();
                    return true;
                }
                catch {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the login history for the given user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of login dates</returns>
        public List<UserLogin> GetUserLoginsById(string userId) {
            var userLogins = _communityContext.UserLogins.Where(b => b.User.UserID.Equals(userId));
            List<UserLogin> ul = userLogins.Select(a => new UserLogin {
                Login = a.Login,
                User = a.User
            }).ToList();
            return ul;
        }
    }
}
