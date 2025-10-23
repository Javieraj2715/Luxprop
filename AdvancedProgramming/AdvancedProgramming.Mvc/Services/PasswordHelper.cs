using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace AdvancedProgramming.Mvc.Services
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create()){
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password)); 
                StringBuilder builder = new StringBuilder();
                foreach(var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}