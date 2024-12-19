using System;
using PharmacyManagement.Core.Interfaces;
using PharmacyManagement.Core.Models;
using PharmacyManagement.Infrastructure.Data;

namespace PharmacyManagement.Presentation.Menus
{
    public class LoginMenu
    {
        private readonly IAuthService _authService;
        private User _currentUser;

        public LoginMenu(IAuthService authService)
        {
            _authService = authService;
        }

        public void Show()
        {
            Console.Clear();
            Console.WriteLine("=== PHARMACY MANAGEMENT SYSTEM ===");
            Console.WriteLine("1. Đăng nhập");
            Console.WriteLine("2. Thoát");
            Console.Write("Chọn chức năng: ");

            string choice = Console.ReadLine();
            ProcessChoice(choice);
        }

        private void ProcessChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    ProcessLogin();
                    break;
                case "2":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Lựa chọn không hợp lệ!");
                    Console.ReadKey();
                    break;
            }
        }

        private void ProcessLogin()
        {
            Console.Clear();
            Console.WriteLine("=== ĐĂNG NHẬP ===");
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();

            var user = _authService.Login(username, password);

            if (user != null)
            {
                _currentUser = user;
                Console.WriteLine("Đăng nhập thành công!");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Sai tên đăng nhập hoặc mật khẩu!");
                Console.ReadKey();
            }
        }

        public User GetCurrentUser() => _currentUser;
    }

}
