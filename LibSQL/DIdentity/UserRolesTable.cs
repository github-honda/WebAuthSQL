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
    /// <summary>
    /// Table AspNetUserRoles.
    /// </summary>
    public class UserRolesTable
    {
        private ZSqlClient mDB;

        public UserRolesTable(ZSqlClient db1)
        {
            mDB = db1;
        }

        public List<string> FindByUserId(string userId)
        {
            List<string> roles = new List<string>();
            string commandText = "Select AspNetRoles.Name from AspNetUserRoles, AspNetRoles where AspNetUserRoles.UserId = @userId and AspNetUserRoles.RoleId = AspNetRoles.Id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userId", userId);

            DataTable dt1 = mDB.QueryDataSet(commandText, parameters).ZGetTable(0);
            foreach(DataRow dr1 in dt1.Rows)
            {
                roles.Add(dr1.ZFetchString(0));
            }
            return roles;
        }

        public int Delete(string userId)
        {
            string commandText = "Delete from AspNetUserRoles where UserId = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("UserId", userId);

            return mDB.Execute(commandText, parameters);
        }

        public int Insert(User user, string roleId)
        {
            string commandText = "Insert into AspNetUserRoles (UserId, RoleId) values (@userId, @roleId)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("userId", user.Id);
            parameters.Add("roleId", roleId);

            return mDB.Execute(commandText, parameters);
        }
    }
}
