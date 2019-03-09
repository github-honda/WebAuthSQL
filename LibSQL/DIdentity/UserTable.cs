using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using System.Data;
using ZLib;
using ZLib.DLib;

namespace LibSQL.DIdentity
{
    public class UserTable<TUser> where TUser:User
    {
        private ZSqlClient mDB;

        public UserTable(ZSqlClient db1)
        {
            mDB = db1;
        }

        public string GetUserName(string userId)
        {
            string commandText = "Select Name from AspNetUsers where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };

            return mDB.QueryString(commandText, parameters);
        }

        public string GetUserId(string userName)
        {
            string commandText = "Select Id from AspNetUsers where UserName = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", userName } };

            return mDB.QueryString(commandText, parameters);
        }

        public TUser GetUserById(string userId)
        {
            TUser user = null;
            string commandText = "Select * from AspNetUsers where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };

            DataTable dt1 = mDB.QueryDataSet(commandText, parameters).ZGetTable(0);
            if (dt1.ZGetRowsCount() == 1)
            {
                DataRow dr1 = dt1.Rows[0];
                user = (TUser)Activator.CreateInstance(typeof(TUser));
                user.Id = dr1.ZFetchString("Id");
                user.UserName = dr1.ZFetchString("UserName");
                user.PasswordHash = dr1.ZFetchString("PasswordHash");
                user.SecurityStamp = dr1.ZFetchString("SecurityStamp");
                user.Email = dr1.ZFetchString("Email");
                user.EmailConfirmed = dr1.ZFetchBoolean("EmailConfirmed", false);
                user.PhoneNumber = dr1.ZFetchString("PhoneNumber");
                user.PhoneNumberConfirmed = dr1.ZFetchBoolean("PhoneNumberConfirmed", false);
                user.LockoutEnabled = dr1.ZFetchBoolean("LockoutEnabled", false);
                user.LockoutEndDateUtc = dr1.ZFetchDateTime("LockoutEndDateUtc", DateTime.UtcNow);
                user.AccessFailedCount = dr1.ZFetchInt("AccessFailedCount", 0);
            }
            return user;
        }

        public List<TUser> GetUserByName(string userName)
        {
            List<TUser> users = new List<TUser>();
            string commandText = "Select * from AspNetUsers where UserName = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", userName } };

            var dt1 = mDB.QueryDataSet(commandText, parameters).ZGetTable(0);
            foreach (DataRow row in dt1.Rows)
            {
                TUser user = (TUser)Activator.CreateInstance(typeof(TUser));
                user.Id = row.ZFetchString("Id");
                user.UserName = row.ZFetchString("UserName");
                user.PasswordHash = row.ZFetchString("PasswordHash"); 
                user.SecurityStamp = row.ZFetchString("SecurityStamp"); 
                user.Email = row.ZFetchString("Email"); 
                user.EmailConfirmed = row.ZFetchBoolean("EmailConfirmed", false);
                user.PhoneNumber = row.ZFetchString("PhoneNumber");
                user.PhoneNumberConfirmed = row.ZFetchBoolean("PhoneNumberConfirmed", false);
                user.LockoutEnabled = row.ZFetchBoolean("LockoutEnabled", false);
                user.TwoFactorEnabled = row.ZFetchBoolean("TwoFactorEnabled", false);
                user.LockoutEndDateUtc = row.ZFetchDateTime("LockoutEndDateUtc", DateTime.UtcNow);
                user.AccessFailedCount = row.ZFetchInt("AccessFailedCount", 0);

                users.Add(user);
            }
            return users;
        }

        public List<TUser> GetUserByEmail(string email)
        {
            return null;
        }

        public string GetPasswordHash(string userId)
        {
            string commandText = "Select PasswordHash from AspNetUsers where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", userId);

            var passHash = mDB.QueryString(commandText, parameters);
            if (string.IsNullOrEmpty(passHash))
            {
                return null;
            }
            return passHash;
        }

        public int SetPasswordHash(string userId, string passwordHash)
        {
            string commandText = "Update AspNetUsers set PasswordHash = @pwdHash where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@pwdHash", passwordHash);
            parameters.Add("@id", userId);

            return mDB.Execute(commandText, parameters);
        }

        public string GetSecurityStamp(string userId)
        {
            string commandText = "Select SecurityStamp from AspNetUsers where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@id", userId } };
            var result = mDB.QueryString(commandText, parameters);
            return result;
        }

        public int Insert(TUser user)
        {
            string commandText = @"Insert into AspNetUsers (UserName, Id, PasswordHash, SecurityStamp,Email,EmailConfirmed,PhoneNumber,PhoneNumberConfirmed, AccessFailedCount,LockoutEnabled,LockoutEndDateUtc,TwoFactorEnabled) values (@name, @id, @pwdHash, @SecStamp,@email,@emailconfirmed,@phonenumber,@phonenumberconfirmed,@accesscount,@lockoutenabled,@lockoutenddate,@twofactorenabled)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@name", user.UserName);
            parameters.Add("@id", user.Id);
            parameters.Add("@pwdHash", user.PasswordHash);
            parameters.Add("@SecStamp", user.SecurityStamp);
            parameters.Add("@email", user.Email);
            parameters.Add("@emailconfirmed", user.EmailConfirmed);
            parameters.Add("@phonenumber", user.PhoneNumber);
            parameters.Add("@phonenumberconfirmed", user.PhoneNumberConfirmed);
            parameters.Add("@accesscount", user.AccessFailedCount);
            parameters.Add("@lockoutenabled", user.LockoutEnabled);
            parameters.Add("@lockoutenddate", user.LockoutEndDateUtc);
            parameters.Add("@twofactorenabled", user.TwoFactorEnabled);

            return mDB.Execute(commandText, parameters);
        }

        private int Delete(string userId)
        {
            string commandText = "Delete from AspNetUsers where Id = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userId", userId);

            return mDB.Execute(commandText, parameters);
        }

        public int Delete(TUser user)
        {
            return Delete(user.Id);
        }

        public int Update(TUser user)
        {
            string commandText = @"Update AspNetUsers set UserName = @userName, PasswordHash = @pswHash, SecurityStamp = @secStamp, 
                Email=@email, EmailConfirmed=@emailconfirmed, PhoneNumber=@phonenumber, PhoneNumberConfirmed=@phonenumberconfirmed,
                AccessFailedCount=@accesscount, LockoutEnabled=@lockoutenabled, LockoutEndDateUtc=@lockoutenddate, TwoFactorEnabled=@twofactorenabled  
                WHERE Id = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userName", user.UserName);
            parameters.Add("@pswHash", user.PasswordHash);
            parameters.Add("@secStamp", user.SecurityStamp);
            parameters.Add("@userId", user.Id);
            parameters.Add("@email", user.Email);
            parameters.Add("@emailconfirmed", user.EmailConfirmed);
            parameters.Add("@phonenumber", user.PhoneNumber);
            parameters.Add("@phonenumberconfirmed", user.PhoneNumberConfirmed);
            parameters.Add("@accesscount", user.AccessFailedCount);
            parameters.Add("@lockoutenabled", user.LockoutEnabled);
            parameters.Add("@lockoutenddate", user.LockoutEndDateUtc);
            parameters.Add("@twofactorenabled", user.TwoFactorEnabled);

            return mDB.Execute(commandText, parameters);
        }

    }
}
