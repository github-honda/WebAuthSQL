using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using Microsoft.AspNet.Identity;
using System.Data;
using ZLib;
using ZLib.DLib;

namespace LibSQL.DIdentity
{
    /// <summary>
    /// Table AspNetUserLogins
    /// </summary>
    public class UserLoginsTable
    {
        private  ZSqlClient mDB;

        public UserLoginsTable(ZSqlClient db1)
        {
            mDB = db1;
        }

        public int Delete(User user, UserLoginInfo login)
        {
            string commandText = "Delete from AspNetUserLogins where UserId = @userId and LoginProvider = @loginProvider and ProviderKey = @providerKey";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("UserId", user.Id);
            parameters.Add("loginProvider", login.LoginProvider);
            parameters.Add("providerKey", login.ProviderKey);

            return mDB.Execute(commandText, parameters);
        }

        public int Delete(string userId)
        {
            string commandText = "Delete from AspNetUserLogins where UserId = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("UserId", userId);

            return mDB.Execute(commandText, parameters);
        }

        public int Insert(User user, UserLoginInfo login)
        {
            string commandText = "Insert into AspNetUserLogins (LoginProvider, ProviderKey, UserId) values (@loginProvider, @providerKey, @userId)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("loginProvider", login.LoginProvider);
            parameters.Add("providerKey", login.ProviderKey);
            parameters.Add("userId", user.Id);

            return mDB.Execute(commandText, parameters);
        }

        public string FindUserIdByLogin(UserLoginInfo userLogin)
        {
            string commandText = "Select UserId from AspNetUserLogins where LoginProvider = @loginProvider and ProviderKey = @providerKey";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("loginProvider", userLogin.LoginProvider);
            parameters.Add("providerKey", userLogin.ProviderKey);

            return mDB.QueryString(commandText, parameters);
        }

        public List<UserLoginInfo> FindByUserId(string userId)
        {
            List<UserLoginInfo> logins = new List<UserLoginInfo>();
            string commandText = "Select * from AspNetUserLogins where UserId = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@userId", userId } };

            DataTable dt1 = mDB.QueryDataSet(commandText, parameters).ZGetTable(0);
            foreach (DataRow dr1 in dt1.Rows)
            {
                var login = new UserLoginInfo(dr1.ZFetchString("LoginProvider"), dr1.ZFetchString("ProviderKey"));
                logins.Add(login);
            }
            return logins;
        }
    }
}
