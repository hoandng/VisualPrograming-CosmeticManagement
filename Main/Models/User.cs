using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main.Models
{
    public class User
    {
        private string _username;
        private string _password;
        private int _isAdmin;
        private string _picture;
        private string _EmployeeID;

        public User()
        {
            
        }
        public User(User user)
        {
            _username = user.Username;
            _password = user.Password;
            _isAdmin = user.IsAdmin;
            _picture = user.Picture;
            _EmployeeID = user.EmployeeID;
        }
        public void Assign(User other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            this.Username = other.Username;
            this.Password = other.Password;
            this.Picture = other.Picture;
            this.IsAdmin = other.IsAdmin;
            this.EmployeeID = other.EmployeeID;
        }

        public string Username { get => _username; set => _username = value; }
        public string Password { get => _password; set => _password = value; }
        public int IsAdmin { get => _isAdmin; set => _isAdmin = value; }
        public string Picture { get => _picture; set => _picture = value; }
        public string EmployeeID { get => _EmployeeID; set => _EmployeeID = value; }
    }
}
