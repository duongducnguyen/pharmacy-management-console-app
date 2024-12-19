using System;
using System.Linq;
using System.Data.Entity;
using System.Text;
using PharmacyManagement.Core.Models;
using PharmacyManagement.Presentation.Menus;
using PharmacyManagement.Infrastructure.Data;
using PharmacyManagement.Infrastructure.Services;
using PharmacyManagement.Core.Interfaces;

namespace PharmacyManagement
{
    class Program
    {
        private static ApplicationDbContext _context;
        private static IAuthService _authService;
        private static IDataService _dataService;
        private static MainMenu _mainMenu;
        private static User _currentUser;

        static void Main(string[] args)
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            InitializeDependencies();
            RunApplication();
        }

        private static void InitializeDependencies()
        {
            try
            {
                _context = new ApplicationDbContext();
                _authService = new AuthService(_context);
                _dataService = new DataService(_context);
                _mainMenu = new MainMenu(_authService, _dataService);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khởi tạo: {ex.Message}");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }

        private static void RunApplication()
        {
            while (true)
            {
                if (_currentUser == null)
                {
                    ShowLoginMenu();
                }
                else
                {
                    _mainMenu.Show(_currentUser);
                    if (_mainMenu.IsLoggedOut())
                    {
                        _currentUser = null;
                    }
                }
            }
        }

        private static void ShowLoginMenu()
        {
            Console.Clear();
            Console.WriteLine("=== ĐĂNG NHẬP ===");
            Console.Write("Tên đăng nhập: ");
            string username = Console.ReadLine();
            Console.Write("Mật khẩu: ");
            string password = Console.ReadLine();

            try
            {
                _currentUser = _authService.Login(username, password);

                if (_currentUser == null)
                {
                    Console.WriteLine("Đăng nhập thất bại!");
                    Console.WriteLine("Nhấn phím bất kỳ để thử lại...");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi đăng nhập: {ex.Message}");
                Console.WriteLine("Nhấn phím bất kỳ để thử lại...");
                Console.ReadKey();
            }
        }

        private static void HandleExit()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
    }

}