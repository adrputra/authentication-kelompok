using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication
{
    public class Program
    {
        public static List<User> users = new List<User>();

        public static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"====================================================");
                Console.WriteLine($"{"AUTHENTICATION BASIC",37}");
                Console.WriteLine($"====================================================\n");

                Console.WriteLine($"[1] Create User");
                Console.WriteLine($"[2] Show User");
                Console.WriteLine($"[3] Edit User");
                Console.WriteLine($"[4] Delete User");
                Console.WriteLine($"[5] Search");
                Console.WriteLine($"[6] Login");
                Console.WriteLine($"[7] Forget Password");
                Console.WriteLine($"[8] Exit");

                int input;
                Console.Write("Input : ");
                while (!Int32.TryParse(Console.ReadLine(),out input))
                {
                    Console.Write("Invalid input! Masukkan Input berupa angka : ");
                }

                switch (input)
                {
                    case 1:
                        CreateUser();
                        break;
                    case 2:
                        ShowUser();
                        break;
                    case 3:
                        EditUser();
                        break;
                    case 4:
                        DeleteUser();
                        break;
                    case 5:
                        Search();
                        break;
                    case 6:
                        Login();
                        break;
                    case 7:
                        ForgetPassword();
                        break;
                    case 8:
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
                Console.ReadKey();
            }
        }

        public static void CreateUser()
        {
            Console.Clear();
            Console.WriteLine("============\nCreate User\n============\n");
            Console.Write("Firstname\t: ");
            string firstName = Console.ReadLine().Trim();
            Console.Write("Lastname\t: ");
            string lastName = Console.ReadLine().Trim();
            Console.Write("Password\t: ");
            string password = Console.ReadLine().Trim();
            if (ValidateUser(firstName,lastName,password))
            {
                users.Add(new User(firstName, lastName, BCrypt.Net.BCrypt.HashPassword(password), generateUsername(firstName, lastName)));
                Console.WriteLine("User Berhasil Dibuat");
            }
        }

        public static bool ValidateUser(string firstName, string lastName, string password)
        {
            if (firstName != "" && lastName != "" && password != "")
            {
                if (firstName.Length >= 3 && lastName.Length >= 3 && password.Length >= 3)
                {
                    if (password.Any(char.IsUpper) && password.Any(char.IsLower) && password.Any(char.IsDigit))
                    {
                        return true;
                        Console.WriteLine("User Berhasil Dibuat");
                    }
                    else
                    {
                        Console.WriteLine("!! Password must contain number, upper case and lower case letter !!");
                        return false;
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("!! Input should be at least 3 characters !! ");
                    return false;
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("!! Input should not be empty !!");
                return false;
                Console.ReadKey();
            }
        }

        public static void ShowUser()
        {
            Console.Clear();
            if (users.Count == 0)
            {
                Console.WriteLine($"============= DATA USER TIDAK DITEMUKAN =============");
            }
            else
            {
                Console.WriteLine($"============= DATA USER =============\n");
                Console.WriteLine($"| {"Firstname",-17}| {"Lastname",-17}| { "USERNAME",-15}| {"PASSWORD",-15}|");
                Console.WriteLine("-------------------------------------------------------------------------");
                foreach (var user in users)
                {
                    Console.WriteLine($"| {user.FirstName,-17}| {user.LastName,-17}| {user.Username,-15}| {user.Password,-15}|");
                }
            }
        }

        public static void EditUser()
        {
            ShowUser();

            Console.Write("\nMasukkan Username yang ingin di edit : ");
            string input = Console.ReadLine();

            int index = users.FindIndex(User => User.Username == input);

            if (index!=-1)
            {
                Console.Write("\nFirstname\t: ");
                string firstName = Console.ReadLine().Trim();
                Console.Write("Lastname\t: ");
                string lastName = Console.ReadLine().Trim();
                Console.Write("Password\t: ");
                string password = Console.ReadLine().Trim();
                if (ValidateUser(firstName, lastName, password))
                {
                    users[index].EditUser(firstName, lastName, BCrypt.Net.BCrypt.HashPassword(password), generateUsername(firstName, lastName));
                    Console.WriteLine("User Berhasil Diubah");
                }
            }
            else
            {
                Console.WriteLine($"!! Username tidak ditemukan !!");
            }
        }

        public static void DeleteUser()
        {
            ShowUser();
            Console.Write("\nMasukkan Username yang ingin dihapus : ");
            string input = Console.ReadLine();

            int index = users.FindIndex(User => User.Username == input);

            if (index != -1)
            {
                users.RemoveAt(index);
                Console.WriteLine("Username berhasil dihapus");

            }
            else
            {
                Console.WriteLine("Username Tidak Ditemukan");
            }
        }

        public static void Search()
        {
            Console.Clear();
            Console.WriteLine("==============\nSearch\n==============\n");
            Console.Write("Masukkan kata kunci : ");
            string input = Console.ReadLine();
            List<User> findUsers = users.FindAll(
            user => user.FirstName.ToLower().Contains(input.ToLower()) || user.LastName.ToLower().Contains(input.ToLower()) || user.Username.ToLower().Contains(input.ToLower()));

            if (findUsers.Count == 0)
            {
                Console.WriteLine($"!! TIDAK ADA USER YANG COCOK !!");
            }
            else
            {
                Console.WriteLine($"| {"Firstname",-17}| {"Lastname",-17}| { "USERNAME",-15}| {"PASSWORD",-15}|");
                foreach (var user in findUsers)
                {
                    Console.WriteLine($"| {user.FirstName,-17}| {user.LastName,-17}| {user.Username,-15}| {user.Password,-15}|");
                }
            }
        }

        public static string generateUsername(string a, string b)
        {
            Random rnd = new Random();
            string c = $"{a.ToLower()}{b.ToLower()}";
            int index = users.FindIndex(user => user.Username == c);
            if (index!=-1)
            {
                c += rnd.Next(100,1000);
            }
            return c;
        }

        public static void Login()
        {
            Console.Clear();
            Console.WriteLine("==LOGIN==");
            Console.Write("USERNAME : ");
            string username = Console.ReadLine();
            Console.Write("PASSWORD : ");
            string password = Console.ReadLine();
            ValidateLogin(username, password);
        }

        public static void ValidateLogin(string username,string password)
        {
            int index = users.FindIndex(user => user.Username == username);
            if (index != -1)
            {
                // jika ada cocokan password user 
                if (BCrypt.Net.BCrypt.Verify(password,users[index].Password))
                {
                    Console.WriteLine($"Login Successfully. Logged in as {users[index].Username}");
                }
                else
                {
                    Console.WriteLine($"!! Wrong Username or Password !!");
                }
            }
            else
            {
                Console.WriteLine($"!! Username Not Found !!");
            }
        }

        public static void ForgetPassword()
        {
            ShowUser();
            Console.Write("\nMasukkan Username : ");
            string input = Console.ReadLine();

            int index = users.FindIndex(User => User.Username == input);

            if (index != -1)
            {
                Console.Write("\nMasukkan Password baru : ");
                string newPassword = Console.ReadLine();
                if (ValidateUser(users[index].FirstName, users[index].LastName, newPassword))
                {
                    users[index].EditUser(users[index].FirstName, users[index].LastName, BCrypt.Net.BCrypt.HashPassword(newPassword), users[index].Username);
                    Console.WriteLine("Password berhasil diubah");
                }

            }
            else
            {
                Console.WriteLine("Username Tidak Ditemukan");
            }
        }
    }
}
