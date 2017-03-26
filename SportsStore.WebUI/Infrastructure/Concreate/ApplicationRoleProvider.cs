using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Data;

namespace SportsStore.WebUI.Infrastructure
{
    public class ApplicationRoleProvider: RoleProvider
    {
        public override bool IsUserInRole(string email, string roleName)
        {
            var user = GetUser(email);
            if (user == null) return false;

            return user.Role.Name == roleName;
        }

        User GetUser(string email)
        {
            var context = new Entities();
            return context.Users.FirstOrDefault(u => u.Email == email);
        }

        public override string[] GetRolesForUser(string email)
        {
           var user = GetUser(email);
			return new[] { user.Role.Name };
        }

        public override void CreateRole(string roleName)
        {
            using (var context = new Entities())
                context.Roles.Add(new Role { Name = roleName });
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName) {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            using (var context = new Entities())
                return context.Roles.Select(r => r.Name).ToArray();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch) {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }
    }
}