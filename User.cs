using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }

        public User(string FirstName, string LastName, string Password, string Username)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Password = Password;
            this.Username = Username;
        }

        public void EditUser(string firstName, string lastName, string password, string userName)
        {
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Username = userName;
        }
    }
}
