using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothes_Project
{
    internal class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string UserPassword { get; set; }
        public char Gender { get; set; }
        public int Age { get; set; }

        public static short UserCount { get; private set; }

        public User()
        {
            UserCount++;
        }

        public User(string FirstName, string LastName, string Username, string UserPassword, char Gender, int Age)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Username = Username;
            this.UserPassword = UserPassword;
            this.Gender = Gender;
            this.Age = Age;
            UserCount++;
        }

        public int ReadInt(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine();
                if (int.TryParse(input, out int result))
                    return result;

                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }

        public char ReadGender(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine();
                if (char.TryParse(input, out char result))
                    return result;

                Console.WriteLine("Invalid input. Please enter a valid character.");
            }
        }

        public float ReadFloat(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine();
                if (float.TryParse(input, out float result))
                    return result;

                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }
    }

}
