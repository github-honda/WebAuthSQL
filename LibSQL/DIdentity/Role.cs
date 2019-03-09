using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using Microsoft.AspNet.Identity;

namespace LibSQL.DIdentity
{
    // Implements the ASP.NET Identity IRole interface.
    public class Role: IRole
    {
        public Role()
        {
            Id = Guid.NewGuid().ToString();
        }
        public Role(string name) : this()
        {
            Name = name;
        }

        public Role(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public string Id { get; set; }

        public string Name { get; set; }

    }
}
