using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using ZLib.DLib;


namespace LibSQL.DIdentity
{
    /// <summary>
    /// Table AspNetRoles
    /// </summary>
    public class RoleTable
    {
        private ZSqlClient mDB;

        public RoleTable(ZSqlClient db1)
        {
            mDB = db1;
        }

        public int Delete(string roleId)
        {
            string commandText = "Delete from AspNetRoles where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", roleId);

            return mDB.Execute(commandText, parameters);
        }

        public int Insert(Role role)
        {
            string commandText = "Insert into AspNetRoles (Id, Name) values (@id, @name)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@name", role.Name);
            parameters.Add("@id", role.Id);

            return mDB.Execute(commandText, parameters);
        }

        public string GetRoleName(string roleId)
        {
            string commandText = "Select Name from AspNetRoles where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", roleId);

            return mDB.QueryString(commandText, parameters);
        }

        public string GetRoleId(string roleName)
        {
            string roleId = null;
            string commandText = "Select Id from AspNetRoles where Name = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@name", roleName } };

            var result = mDB.QueryValue(commandText, parameters);
            if (result != null)
            {
                return Convert.ToString(result);
            }

            return roleId;
        }

        public Role GetRoleById(string roleId)
        {
            var roleName = GetRoleName(roleId);
            Role role = null;

            if (roleName != null)
            {
                role = new Role(roleName, roleId);
            }

            return role;

        }

        public Role GetRoleByName(string roleName)
        {
            var roleId = GetRoleId(roleName);
            Role role = null;

            if (roleId != null)
            {
                role = new Role(roleName, roleId);
            }

            return role;
        }

        public int Update(Role role)
        {
            string commandText = "Update AspNetRoles set Name = @name where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", role.Id);

            return mDB.Execute(commandText, parameters);
        }

    }
}
