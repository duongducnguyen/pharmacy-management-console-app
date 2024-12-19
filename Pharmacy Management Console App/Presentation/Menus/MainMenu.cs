using System;
using PharmacyManagement.Core.Models;
using PharmacyManagement.Infrastructure.Data;
using PharmacyManagement.Core.Interfaces;  // Cho IAuthService và IDataService
using PharmacyManagement.Infrastructure.Services; // Cho các service
using System.Linq;

namespace PharmacyManagement.Presentation.Menus
{
    public class MainMenu
    {
        private readonly IAuthService _authService;
        private readonly IDataService _dataService;
        private User _currentUser;
        private bool _isLoggedOut = false;

        public MainMenu(IAuthService authService, IDataService dataService)
        {
            _authService = authService;
            _dataService = dataService;
        }

        public void Show(User currentUser)
        {
            _currentUser = currentUser;
            _isLoggedOut = false;

            while (!_isLoggedOut)
            {
                DisplayMenu();
                ProcessMenuChoice();
            }
        }

        private void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine($"=== MENU CHÍNH === | User: {_currentUser.Username}");
            Console.WriteLine("1. Quản lý Thuốc");
            Console.WriteLine("2. Quản lý Danh mục");
            Console.WriteLine("3. Quản lý Khách hàng");
            Console.WriteLine("4. Quản lý Nhà cung cấp");
            Console.WriteLine("5. Quản lý Đơn hàng");
            Console.WriteLine("6. Quản lý Nhập hàng");
            Console.WriteLine("7. Đăng xuất");
            Console.WriteLine("8. Thoát");
            Console.Write("Chọn chức năng: ");
        }

        private void ProcessMenuChoice()
        {
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ShowMedicineMenu();
                    break;
                case "2":
                    ShowCategoryMenu();
                    break;
                case "3":
                    ShowCustomerMenu();
                    break;
                case "4":
                    ShowSupplierMenu();
                    break;
                case "5":
                    ShowOrderMenu();
                    break;
                case "6":
                    ShowPurchaseMenu();
                    break;
                case "7":
                    Logout();
                    break;
                case "8":
                    Environment.Exit(0);
                    break;
                default:
                    ShowInvalidChoice();
                    break;
            }
        }

        private void ShowMedicineMenu()
        {
            Console.Clear();
            Console.WriteLine("=== QUẢN LÝ THUỐC ===");
            Console.WriteLine("1. Xem danh sách thuốc");
            Console.WriteLine("2. Thêm thuốc mới");
            Console.WriteLine("3. Cập nhật thuốc");
            Console.WriteLine("4. Xóa thuốc");
            Console.WriteLine("5. Quay lại");

            ProcessMedicineChoice();
        }

        private void ProcessMedicineChoice()
        {
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    // Xem danh sách thuốc
                    var medicines = _dataService.GetAll<Medicine>();
                    Console.WriteLine("\nDanh sách thuốc:");
                    Console.WriteLine("ID\tTên thuốc\tĐơn vị\tGiá\tTồn kho\tHạn dùng\tDanh mục");
                    Console.WriteLine("--------------------------------------------------------------------------------");
                    foreach (var medicine in medicines)
                    {
                        Console.WriteLine($"{medicine.Id}\t{medicine.Name}\t{medicine.Unit}\t" +
                                        $"{medicine.Price:C}\t{medicine.Stock}\t" +
                                        $"{medicine.ExpiryDate:dd/MM/yyyy}\t{medicine.Category?.Name}");
                    }
                    WaitForKey();
                    break;

                case "2":
                    // Thêm thuốc mới
                    Console.WriteLine("\nThêm thuốc mới:");
                    Console.Write("Nhập tên thuốc: ");
                    string name = Console.ReadLine();

                    Console.Write("Nhập mô tả: ");
                    string description = Console.ReadLine();

                    Console.Write("Nhập đơn vị: ");
                    string unit = Console.ReadLine();

                    Console.Write("Nhập giá: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal price))
                    {
                        Console.WriteLine("Giá không hợp lệ!");
                        WaitForKey();
                        break;
                    }

                    Console.Write("Nhập số lượng tồn kho: ");
                    if (!int.TryParse(Console.ReadLine(), out int stock))
                    {
                        Console.WriteLine("Số lượng không hợp lệ!");
                        WaitForKey();
                        break;
                    }

                    Console.Write("Nhập hạn sử dụng (dd/MM/yyyy): ");
                    if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null,
                        System.Globalization.DateTimeStyles.None, out DateTime expiryDate))
                    {
                        Console.WriteLine("Ngày không hợp lệ!");
                        WaitForKey();
                        break;
                    }

                    Console.Write("Nhập ID danh mục: ");
                    if (!int.TryParse(Console.ReadLine(), out int categoryId))
                    {
                        Console.WriteLine("ID danh mục không hợp lệ!");
                        WaitForKey();
                        break;
                    }

                    var newMedicine = new Medicine
                    {
                        Name = name,
                        Description = description,
                        Price = price,
                        Stock = stock,
                        Unit = unit,
                        ExpiryDate = expiryDate,
                        CategoryId = categoryId,
                        CreatedAt = DateTime.Now
                    };

                    try
                    {
                        _dataService.Add(newMedicine);
                        _dataService.SaveChanges();
                        Console.WriteLine("Thêm thuốc thành công!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi: {ex.Message}");
                    }
                    WaitForKey();
                    break;

                case "3":

                    // Xem danh sách thuốc
                    medicines = _dataService.GetAll<Medicine>();
                    Console.WriteLine("\nDanh sách thuốc:");
                    Console.WriteLine("ID\tTên thuốc\tĐơn vị\tGiá\tTồn kho\tHạn dùng\tDanh mục");
                    Console.WriteLine("--------------------------------------------------------------------------------");
                    foreach (var medicine in medicines)
                    {
                        Console.WriteLine($"{medicine.Id}\t{medicine.Name}\t{medicine.Unit}\t" +
                                        $"{medicine.Price:C}\t{medicine.Stock}\t" +
                                        $"{medicine.ExpiryDate:dd/MM/yyyy}\t{medicine.Category?.Name}");
                    }

                    // Cập nhật thuốc
                    Console.Write("\nNhập ID thuốc cần cập nhật: ");
                    if (int.TryParse(Console.ReadLine(), out int updateId))
                    {
                        var medicineToUpdate = _dataService.GetById<Medicine>(updateId);
                        if (medicineToUpdate != null)
                        {
                            Console.Write("Nhập tên mới (để trống để giữ nguyên): ");
                            string newName = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newName))
                                medicineToUpdate.Name = newName;

                            Console.Write("Nhập mô tả mới (để trống để giữ nguyên): ");
                            string newDescription = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newDescription))
                                medicineToUpdate.Description = newDescription;

                            Console.Write("Nhập đơn vị mới (để trống để giữ nguyên): ");
                            string newUnit = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newUnit))
                                medicineToUpdate.Unit = newUnit;

                            Console.Write("Nhập giá mới (để trống để giữ nguyên): ");
                            string newPriceStr = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newPriceStr) && decimal.TryParse(newPriceStr, out decimal newPrice))
                                medicineToUpdate.Price = newPrice;

                            Console.Write("Nhập số lượng tồn kho mới (để trống để giữ nguyên): ");
                            string newStockStr = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newStockStr) && int.TryParse(newStockStr, out int newStock))
                                medicineToUpdate.Stock = newStock;

                            Console.Write("Nhập hạn sử dụng mới (dd/MM/yyyy) (để trống để giữ nguyên): ");
                            string newExpiryDateStr = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newExpiryDateStr) &&
                                DateTime.TryParseExact(newExpiryDateStr, "dd/MM/yyyy", null,
                                System.Globalization.DateTimeStyles.None, out DateTime newExpiryDate))
                                medicineToUpdate.ExpiryDate = newExpiryDate;

                            Console.Write("Nhập ID danh mục mới (để trống để giữ nguyên): ");
                            string newCategoryIdStr = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newCategoryIdStr) && int.TryParse(newCategoryIdStr, out int newCategoryId))
                                medicineToUpdate.CategoryId = newCategoryId;

                            medicineToUpdate.UpdatedAt = DateTime.Now;

                            try
                            {
                                _dataService.Update(medicineToUpdate);
                                _dataService.SaveChanges();
                                Console.WriteLine("Cập nhật thuốc thành công!");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Lỗi: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Không tìm thấy thuốc!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID không hợp lệ!");
                    }
                    WaitForKey();
                    break;

                case "4":


                    // Xem danh sách thuốc
                    medicines = _dataService.GetAll<Medicine>();
                    Console.WriteLine("\nDanh sách thuốc:");
                    Console.WriteLine("ID\tTên thuốc\tĐơn vị\tGiá\tTồn kho\tHạn dùng\tDanh mục");
                    Console.WriteLine("--------------------------------------------------------------------------------");
                    foreach (var medicine in medicines)
                    {
                        Console.WriteLine($"{medicine.Id}\t{medicine.Name}\t{medicine.Unit}\t" +
                                        $"{medicine.Price:C}\t{medicine.Stock}\t" +
                                        $"{medicine.ExpiryDate:dd/MM/yyyy}\t{medicine.Category?.Name}");
                    }

                    // Xóa thuốc
                    Console.Write("\nNhập ID thuốc cần xóa: ");
                    if (int.TryParse(Console.ReadLine(), out int deleteId))
                    {
                        var medicineToDelete = _dataService.GetById<Medicine>(deleteId);
                        if (medicineToDelete != null)
                        {
                            Console.Write("Bạn có chắc chắn muốn xóa thuốc này? (Y/N): ");
                            if (Console.ReadLine().Trim().ToUpper() == "Y")
                            {
                                try
                                {
                                    _dataService.Delete(medicineToDelete);
                                    _dataService.SaveChanges();
                                    Console.WriteLine("Xóa thuốc thành công!");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Lỗi: {ex.Message}");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Không tìm thấy thuốc!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID không hợp lệ!");
                    }
                    WaitForKey();
                    break;

                case "5":
                    // Quay lại menu chính
                    return;

                default:
                    ShowInvalidChoice();
                    break;
            }
        }

        private void ShowCategoryMenu()
        {
            Console.Clear();
            Console.WriteLine("=== QUẢN LÝ DANH MỤC ===");
            Console.WriteLine("1. Xem danh sách danh mục");
            Console.WriteLine("2. Thêm danh mục mới");
            Console.WriteLine("3. Cập nhật danh mục");
            Console.WriteLine("4. Xóa danh mục");
            Console.WriteLine("5. Quay lại");
            Console.Write("Chọn chức năng: ");

            ProcessCategoryChoice();
        }

        private void ProcessCategoryChoice()
        {
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":

                    // Xem danh sách danh mục
                    var categories = _dataService.GetAll<Category>();
                    Console.WriteLine("\nDanh sách danh mục:");
                    Console.WriteLine("ID\tTên danh mục\t\tMô tả");
                    Console.WriteLine("----------------------------------------");
                    foreach (var category in categories)
                    {
                        Console.WriteLine($"{category.Id}\t{category.Name}\t\t{category.Description}");
                    }

                    WaitForKey();
                    break;

                case "2":
                    // Thêm danh mục mới
                    Console.WriteLine("\nThêm danh mục mới:");
                    Console.Write("Nhập tên danh mục: ");
                    string name = Console.ReadLine();
                    Console.Write("Nhập mô tả: ");
                    string description = Console.ReadLine();

                    var newCategory = new Category
                    {
                        Name = name,
                        Description = description
                    };

                    try
                    {
                        _dataService.Add(newCategory);
                        _dataService.SaveChanges();
                        Console.WriteLine("Thêm danh mục thành công!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi: {ex.Message}");
                    }
                    WaitForKey();
                    break;

                case "3":

                    // Xem danh sách danh mục
                    categories = _dataService.GetAll<Category>();
                    Console.WriteLine("\nDanh sách danh mục:");
                    Console.WriteLine("ID\tTên danh mục\t\tMô tả");
                    Console.WriteLine("----------------------------------------");
                    foreach (var category in categories)
                    {
                        Console.WriteLine($"{category.Id}\t{category.Name}\t\t{category.Description}");
                    }

                    // Cập nhật danh mục
                    Console.Write("\nNhập ID danh mục cần cập nhật: ");
                    if (int.TryParse(Console.ReadLine(), out int updateId))
                    {
                        var categoryToUpdate = _dataService.GetById<Category>(updateId);
                        if (categoryToUpdate != null)
                        {
                            Console.Write("Nhập tên mới (để trống để giữ nguyên): ");
                            string newName = Console.ReadLine();
                            Console.Write("Nhập mô tả mới (để trống để giữ nguyên): ");
                            string newDescription = Console.ReadLine();

                            if (!string.IsNullOrWhiteSpace(newName))
                                categoryToUpdate.Name = newName;
                            if (!string.IsNullOrWhiteSpace(newDescription))
                                categoryToUpdate.Description = newDescription;

                            try
                            {
                                _dataService.Update(categoryToUpdate);
                                _dataService.SaveChanges();
                                Console.WriteLine("Cập nhật danh mục thành công!");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Lỗi: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Không tìm thấy danh mục!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID không hợp lệ!");
                    }
                    WaitForKey();
                    break;

                case "4":

                    // Xem danh sách danh mục
                    categories = _dataService.GetAll<Category>();
                    Console.WriteLine("\nDanh sách danh mục:");
                    Console.WriteLine("ID\tTên danh mục\t\tMô tả");
                    Console.WriteLine("----------------------------------------");
                    foreach (var category in categories)
                    {
                        Console.WriteLine($"{category.Id}\t{category.Name}\t\t{category.Description}");
                    }

                    // Xóa danh mục
                    Console.Write("\nNhập ID danh mục cần xóa: ");
                    if (int.TryParse(Console.ReadLine(), out int deleteId))
                    {
                        var categoryToDelete = _dataService.GetById<Category>(deleteId);
                        if (categoryToDelete != null)
                        {
                            Console.Write("Bạn có chắc chắn muốn xóa danh mục này? (Y/N): ");
                            if (Console.ReadLine().Trim().ToUpper() == "Y")
                            {
                                try
                                {
                                    _dataService.Delete(categoryToDelete);
                                    _dataService.SaveChanges();
                                    Console.WriteLine("Xóa danh mục thành công!");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Lỗi: {ex.Message}");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Không tìm thấy danh mục!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID không hợp lệ!");
                    }
                    WaitForKey();
                    break;

                case "5":
                    // Quay lại menu chính
                    return;

                default:
                    ShowInvalidChoice();
                    break;
            }
        }

        private void ShowCustomerMenu()
        {
            Console.Clear();
            Console.WriteLine("=== QUẢN LÝ KHÁCH HÀNG ===");
            Console.WriteLine("1. Xem danh sách khách hàng");
            Console.WriteLine("2. Thêm khách hàng mới");
            Console.WriteLine("3. Cập nhật khách hàng");
            Console.WriteLine("4. Xóa khách hàng");
            Console.WriteLine("5. Quay lại");
            Console.Write("Chọn chức năng: ");

            ProcessCustomerChoice();
        }

        private void ProcessCustomerChoice()
        {
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    // Xem danh sách khách hàng
                    var customers = _dataService.GetAll<Customer>();
                    Console.WriteLine("\nDanh sách khách hàng:");
                    Console.WriteLine("ID\tHọ tên\t\tSố điện thoại\tEmail\t\tĐịa chỉ");
                    Console.WriteLine("--------------------------------------------------------------------------------");
                    foreach (var customer in customers)
                    {
                        Console.WriteLine($"{customer.Id}\t{customer.Name}\t{customer.Phone}\t" +
                                        $"{customer.Email}\t{customer.Address}");
                    }
                    WaitForKey();
                    break;

                case "2":
                    // Thêm khách hàng mới
                    Console.WriteLine("\nThêm khách hàng mới:");
                    Console.Write("Nhập họ tên: ");
                    string name = Console.ReadLine();

                    Console.Write("Nhập số điện thoại: ");
                    string phone = Console.ReadLine();

                    Console.Write("Nhập email: ");
                    string email = Console.ReadLine();

                    Console.Write("Nhập địa chỉ: ");
                    string address = Console.ReadLine();

                    var newCustomer = new Customer
                    {
                        Name = name,
                        Phone = phone,
                        Email = email,
                        Address = address,
                        CreatedAt = DateTime.Now
                    };

                    try
                    {
                        _dataService.Add(newCustomer);
                        _dataService.SaveChanges();
                        Console.WriteLine("Thêm khách hàng thành công!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi: {ex.Message}");
                    }
                    WaitForKey();
                    break;

                case "3":

                    // Xem danh sách khách hàng
                    customers = _dataService.GetAll<Customer>();
                    Console.WriteLine("\nDanh sách khách hàng:");
                    Console.WriteLine("ID\tHọ tên\t\tSố điện thoại\tEmail\t\tĐịa chỉ");
                    Console.WriteLine("--------------------------------------------------------------------------------");
                    foreach (var customer in customers)
                    {
                        Console.WriteLine($"{customer.Id}\t{customer.Name}\t{customer.Phone}\t" +
                                        $"{customer.Email}\t{customer.Address}");
                    }

                    // Cập nhật khách hàng
                    Console.Write("\nNhập ID khách hàng cần cập nhật: ");
                    if (int.TryParse(Console.ReadLine(), out int updateId))
                    {
                        var customerToUpdate = _dataService.GetById<Customer>(updateId);
                        if (customerToUpdate != null)
                        {
                            Console.Write("Nhập họ tên mới (để trống để giữ nguyên): ");
                            string newName = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newName))
                                customerToUpdate.Name = newName;

                            Console.Write("Nhập số điện thoại mới (để trống để giữ nguyên): ");
                            string newPhone = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newPhone))
                                customerToUpdate.Phone = newPhone;

                            Console.Write("Nhập email mới (để trống để giữ nguyên): ");
                            string newEmail = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newEmail))
                                customerToUpdate.Email = newEmail;

                            Console.Write("Nhập địa chỉ mới (để trống để giữ nguyên): ");
                            string newAddress = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newAddress))
                                customerToUpdate.Address = newAddress;

                            customerToUpdate.UpdatedAt = DateTime.Now;

                            try
                            {
                                _dataService.Update(customerToUpdate);
                                _dataService.SaveChanges();
                                Console.WriteLine("Cập nhật khách hàng thành công!");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Lỗi: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Không tìm thấy khách hàng!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID không hợp lệ!");
                    }
                    WaitForKey();
                    break;

                case "4":

                    // Xem danh sách khách hàng
                    customers = _dataService.GetAll<Customer>();
                    Console.WriteLine("\nDanh sách khách hàng:");
                    Console.WriteLine("ID\tHọ tên\t\tSố điện thoại\tEmail\t\tĐịa chỉ");
                    Console.WriteLine("--------------------------------------------------------------------------------");
                    foreach (var customer in customers)
                    {
                        Console.WriteLine($"{customer.Id}\t{customer.Name}\t{customer.Phone}\t" +
                                        $"{customer.Email}\t{customer.Address}");
                    }

                    // Xóa khách hàng
                    Console.Write("\nNhập ID khách hàng cần xóa: ");
                    if (int.TryParse(Console.ReadLine(), out int deleteId))
                    {
                        var customerToDelete = _dataService.GetById<Customer>(deleteId);
                        if (customerToDelete != null)
                        {
                            // Kiểm tra xem khách hàng có đơn hàng không
                            if (customerToDelete.Orders != null && customerToDelete.Orders.Count > 0)
                            {
                                Console.WriteLine("Không thể xóa khách hàng này vì đã có đơn hàng!");
                                WaitForKey();
                                break;
                            }

                            Console.Write("Bạn có chắc chắn muốn xóa khách hàng này? (Y/N): ");
                            if (Console.ReadLine().Trim().ToUpper() == "Y")
                            {
                                try
                                {
                                    _dataService.Delete(customerToDelete);
                                    _dataService.SaveChanges();
                                    Console.WriteLine("Xóa khách hàng thành công!");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Lỗi: {ex.Message}");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Không tìm thấy khách hàng!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID không hợp lệ!");
                    }
                    WaitForKey();
                    break;

                case "5":
                    // Quay lại menu chính
                    return;

                default:
                    ShowInvalidChoice();
                    break;
            }
        }

        private void ShowSupplierMenu()
        {
            Console.Clear();
            Console.WriteLine("=== QUẢN LÝ NHÀ CUNG CẤP ===");
            Console.WriteLine("1. Xem danh sách nhà cung cấp");
            Console.WriteLine("2. Thêm nhà cung cấp mới");
            Console.WriteLine("3. Cập nhật nhà cung cấp");
            Console.WriteLine("4. Xóa nhà cung cấp");
            Console.WriteLine("5. Quay lại");
            Console.Write("Chọn chức năng: ");

            ProcessSupplierChoice();
        }

        private void ProcessSupplierChoice()
        {
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    // Xem danh sách nhà cung cấp
                    var suppliers = _dataService.GetAll<Supplier>();
                    Console.WriteLine("\nDanh sách nhà cung cấp:");
                    Console.WriteLine("ID\tTên nhà cung cấp\tSố điện thoại\tEmail\t\tĐịa chỉ");
                    Console.WriteLine("--------------------------------------------------------------------------------");
                    foreach (var supplier in suppliers)
                    {
                        Console.WriteLine($"{supplier.Id}\t{supplier.Name}\t{supplier.Phone}\t" +
                                        $"{supplier.Email}\t{supplier.Address}");
                    }
                    WaitForKey();
                    break;

                case "2":
                    // Thêm nhà cung cấp mới
                    Console.WriteLine("\nThêm nhà cung cấp mới:");

                    Console.Write("Nhập tên nhà cung cấp: ");
                    string name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        Console.WriteLine("Tên nhà cung cấp không được để trống!");
                        WaitForKey();
                        break;
                    }

                    Console.Write("Nhập số điện thoại: ");
                    string phone = Console.ReadLine();
                    if (phone?.Length > 20)
                    {
                        Console.WriteLine("Số điện thoại không được vượt quá 20 ký tự!");
                        WaitForKey();
                        break;
                    }

                    Console.Write("Nhập email: ");
                    string email = Console.ReadLine();
                    if (email?.Length > 255)
                    {
                        Console.WriteLine("Email không được vượt quá 255 ký tự!");
                        WaitForKey();
                        break;
                    }

                    Console.Write("Nhập địa chỉ: ");
                    string address = Console.ReadLine();
                    if (address?.Length > 255)
                    {
                        Console.WriteLine("Địa chỉ không được vượt quá 255 ký tự!");
                        WaitForKey();
                        break;
                    }

                    var newSupplier = new Supplier
                    {
                        Name = name,
                        Phone = phone,
                        Email = email,
                        Address = address,
                        CreatedAt = DateTime.Now
                    };

                    try
                    {
                        _dataService.Add(newSupplier);
                        _dataService.SaveChanges();
                        Console.WriteLine("Thêm nhà cung cấp thành công!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi: {ex.Message}");
                    }
                    WaitForKey();
                    break;

                case "3":

                    // Xem danh sách nhà cung cấp
                    suppliers = _dataService.GetAll<Supplier>();
                    Console.WriteLine("\nDanh sách nhà cung cấp:");
                    Console.WriteLine("ID\tTên nhà cung cấp\tSố điện thoại\tEmail\t\tĐịa chỉ");
                    Console.WriteLine("--------------------------------------------------------------------------------");
                    foreach (var supplier in suppliers)
                    {
                        Console.WriteLine($"{supplier.Id}\t{supplier.Name}\t{supplier.Phone}\t" +
                                        $"{supplier.Email}\t{supplier.Address}");
                    }

                    // Cập nhật nhà cung cấp
                    Console.Write("\nNhập ID nhà cung cấp cần cập nhật: ");
                    if (int.TryParse(Console.ReadLine(), out int updateId))
                    {
                        var supplierToUpdate = _dataService.GetById<Supplier>(updateId);
                        if (supplierToUpdate != null)
                        {
                            Console.Write("Nhập tên mới (để trống để giữ nguyên): ");
                            string newName = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newName))
                            {
                                if (newName.Length <= 100)
                                    supplierToUpdate.Name = newName;
                                else
                                {
                                    Console.WriteLine("Tên không được vượt quá 100 ký tự!");
                                    WaitForKey();
                                    break;
                                }
                            }

                            Console.Write("Nhập số điện thoại mới (để trống để giữ nguyên): ");
                            string newPhone = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newPhone))
                            {
                                if (newPhone.Length <= 20)
                                    supplierToUpdate.Phone = newPhone;
                                else
                                {
                                    Console.WriteLine("Số điện thoại không được vượt quá 20 ký tự!");
                                    WaitForKey();
                                    break;
                                }
                            }

                            Console.Write("Nhập email mới (để trống để giữ nguyên): ");
                            string newEmail = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newEmail))
                            {
                                if (newEmail.Length <= 255)
                                    supplierToUpdate.Email = newEmail;
                                else
                                {
                                    Console.WriteLine("Email không được vượt quá 255 ký tự!");
                                    WaitForKey();
                                    break;
                                }
                            }

                            Console.Write("Nhập địa chỉ mới (để trống để giữ nguyên): ");
                            string newAddress = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(newAddress))
                            {
                                if (newAddress.Length <= 255)
                                    supplierToUpdate.Address = newAddress;
                                else
                                {
                                    Console.WriteLine("Địa chỉ không được vượt quá 255 ký tự!");
                                    WaitForKey();
                                    break;
                                }
                            }

                            supplierToUpdate.UpdatedAt = DateTime.Now;

                            try
                            {
                                _dataService.Update(supplierToUpdate);
                                _dataService.SaveChanges();
                                Console.WriteLine("Cập nhật nhà cung cấp thành công!");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Lỗi: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Không tìm thấy nhà cung cấp!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID không hợp lệ!");
                    }
                    WaitForKey();
                    break;

                case "4":

                    // Xem danh sách nhà cung cấp
                    suppliers = _dataService.GetAll<Supplier>();
                    Console.WriteLine("\nDanh sách nhà cung cấp:");
                    Console.WriteLine("ID\tTên nhà cung cấp\tSố điện thoại\tEmail\t\tĐịa chỉ");
                    Console.WriteLine("--------------------------------------------------------------------------------");
                    foreach (var supplier in suppliers)
                    {
                        Console.WriteLine($"{supplier.Id}\t{supplier.Name}\t{supplier.Phone}\t" +
                                        $"{supplier.Email}\t{supplier.Address}");
                    }

                    // Xóa nhà cung cấp
                    Console.Write("\nNhập ID nhà cung cấp cần xóa: ");
                    if (int.TryParse(Console.ReadLine(), out int deleteId))
                    {
                        var supplierToDelete = _dataService.GetById<Supplier>(deleteId);
                        if (supplierToDelete != null)
                        {
                            // Kiểm tra xem nhà cung cấp có đơn nhập hàng không
                            if (supplierToDelete.Purchases != null && supplierToDelete.Purchases.Count > 0)
                            {
                                Console.WriteLine("Không thể xóa nhà cung cấp này vì đã có đơn nhập hàng!");
                                WaitForKey();
                                break;
                            }

                            Console.Write("Bạn có chắc chắn muốn xóa nhà cung cấp này? (Y/N): ");
                            if (Console.ReadLine().Trim().ToUpper() == "Y")
                            {
                                try
                                {
                                    _dataService.Delete(supplierToDelete);
                                    _dataService.SaveChanges();
                                    Console.WriteLine("Xóa nhà cung cấp thành công!");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Lỗi: {ex.Message}");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Không tìm thấy nhà cung cấp!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID không hợp lệ!");
                    }
                    WaitForKey();
                    break;

                case "5":
                    // Quay lại menu chính
                    return;

                default:
                    ShowInvalidChoice();
                    break;
            }
        }

        private void ShowOrderMenu()
        {
            Console.Clear();
            Console.WriteLine("=== QUẢN LÝ ĐƠN HÀNG ===");
            Console.WriteLine("1. Xem danh sách đơn hàng");
            Console.WriteLine("2. Tạo đơn hàng mới");
            Console.WriteLine("3. Xem chi tiết đơn hàng");
            Console.WriteLine("4. Xóa đơn hàng");
            Console.WriteLine("5. Quay lại");
            Console.Write("Chọn chức năng: ");

            ProcessOrderChoice();
        }

        private void ProcessOrderChoice()
        {
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    // Xem danh sách đơn hàng
                    var orders = _dataService.GetAll<Order>();
                    Console.WriteLine("\nDanh sách đơn hàng:");
                    Console.WriteLine("ID\tNgày đặt\t\tKhách hàng\t\tTổng tiền");
                    Console.WriteLine("--------------------------------------------------------------------------------");
                    foreach (var order in orders)
                    {
                        Console.WriteLine($"{order.Id}\t{order.OrderDate:dd/MM/yyyy HH:mm}\t" +
                                        $"{order.Customer?.Name,-20}\t{order.TotalAmount:N0} VNĐ");
                    }
                    WaitForKey();
                    break;

                case "2":
                    // Tạo đơn hàng mới
                    Console.WriteLine("\nTạo đơn hàng mới:");

                    // Hiển thị và chọn khách hàng
                    Console.WriteLine("\nDanh sách khách hàng:");
                    var customers = _dataService.GetAll<Customer>();
                    foreach (var customer in customers)
                    {
                        Console.WriteLine($"{customer.Id}. {customer.Name} - SĐT: {customer.Phone}");
                    }

                    Console.Write("\nChọn ID khách hàng: ");
                    if (!int.TryParse(Console.ReadLine(), out int customerId))
                    {
                        Console.WriteLine("ID khách hàng không hợp lệ!");
                        WaitForKey();
                        return;
                    }

                    var selectedCustomer = _dataService.GetById<Customer>(customerId);
                    if (selectedCustomer == null)
                    {
                        Console.WriteLine("Không tìm thấy khách hàng!");
                        WaitForKey();
                        return;
                    }

                    var newOrder = new Order
                    {
                        OrderDate = DateTime.Now,
                        CustomerId = customerId,
                        CreatedAt = DateTime.Now,
                        TotalAmount = 0
                    };

                    try
                    {
                        _dataService.Add(newOrder);
                        _dataService.SaveChanges();

                        // Thêm chi tiết đơn hàng
                        bool addingItems = true;
                        while (addingItems)
                        {
                            Console.WriteLine("\nDanh sách thuốc:");
                            var medicines = _dataService.GetAll<Medicine>();
                            foreach (var medicine in medicines)
                            {
                                Console.WriteLine($"{medicine.Id}. {medicine.Name,-30} - Giá: {medicine.Price:N0} VNĐ - Tồn kho: {medicine.Stock}");
                            }

                            Console.Write("\nChọn ID thuốc (0 để kết thúc): ");
                            if (!int.TryParse(Console.ReadLine(), out int medicineId) || medicineId == 0)
                            {
                                addingItems = false;
                                continue;
                            }

                            var selectedMedicine = _dataService.GetById<Medicine>(medicineId);
                            if (selectedMedicine == null)
                            {
                                Console.WriteLine("Không tìm thấy thuốc!");
                                continue;
                            }

                            Console.Write("Nhập số lượng: ");
                            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
                            {
                                Console.WriteLine("Số lượng không hợp lệ!");
                                continue;
                            }

                            if (quantity > selectedMedicine.Stock)
                            {
                                Console.WriteLine("Số lượng vượt quá tồn kho!");
                                continue;
                            }

                            var orderDetail = new OrderDetail
                            {
                                OrderId = newOrder.Id,
                                MedicineId = medicineId,
                                Quantity = quantity,
                                UnitPrice = selectedMedicine.Price,
                                Subtotal = quantity * selectedMedicine.Price,
                                CreatedAt = DateTime.Now
                            };

                            _dataService.Add(orderDetail);

                            // Cập nhật tồn kho
                            selectedMedicine.Stock -= quantity;
                            selectedMedicine.UpdatedAt = DateTime.Now;
                            _dataService.Update(selectedMedicine);

                            // Cập nhật tổng tiền đơn hàng
                            newOrder.TotalAmount += orderDetail.Subtotal;
                            newOrder.UpdatedAt = DateTime.Now;
                            _dataService.Update(newOrder);

                            _dataService.SaveChanges();
                            Console.WriteLine($"Đã thêm {quantity} {selectedMedicine.Name} vào đơn hàng.");
                        }

                        if (newOrder.TotalAmount > 0)
                        {
                            Console.WriteLine($"\nTạo đơn hàng thành công! Tổng tiền: {newOrder.TotalAmount:N0} VNĐ");
                        }
                        else
                        {
                            _dataService.Delete(newOrder);
                            _dataService.SaveChanges();
                            Console.WriteLine("\nĐơn hàng đã bị hủy do không có sản phẩm nào được thêm.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi: {ex.Message}");
                    }
                    WaitForKey();
                    break;

                case "3":

                    // Xem danh sách đơn hàng
                    orders = _dataService.GetAll<Order>();
                    Console.WriteLine("\nDanh sách đơn hàng:");
                    Console.WriteLine("ID\tNgày đặt\t\tKhách hàng\t\tTổng tiền");
                    Console.WriteLine("--------------------------------------------------------------------------------");
                    foreach (var order in orders)
                    {
                        Console.WriteLine($"{order.Id}\t{order.OrderDate:dd/MM/yyyy HH:mm}\t" +
                                        $"{order.Customer?.Name,-20}\t{order.TotalAmount:N0} VNĐ");
                    }

                    // Xem chi tiết đơn hàng
                    Console.Write("\nNhập ID đơn hàng cần xem: ");
                    if (int.TryParse(Console.ReadLine(), out int viewOrderId))
                    {
                        var orderToView = _dataService.GetById<Order>(viewOrderId);
                        if (orderToView != null)
                        {
                            Console.WriteLine($"\nChi tiết đơn hàng #{orderToView.Id}");
                            Console.WriteLine($"Ngày đặt: {orderToView.OrderDate:dd/MM/yyyy HH:mm}");
                            Console.WriteLine($"Khách hàng: {orderToView.Customer?.Name}");
                            Console.WriteLine($"Số điện thoại: {orderToView.Customer?.Phone}");
                            Console.WriteLine("\nDanh sách sản phẩm:");
                            Console.WriteLine("STT\tTên thuốc\t\t\tSố lượng\tĐơn giá\t\tThành tiền");
                            Console.WriteLine("--------------------------------------------------------------------------------");

                            int stt = 1;
                            foreach (var detail in orderToView.OrderDetails)
                            {
                                Console.WriteLine($"{stt}\t{detail.Medicine?.Name,-30}\t" +
                                                $"{detail.Quantity}\t\t" +
                                                $"{detail.UnitPrice:N0}\t\t" +
                                                $"{detail.Subtotal:N0} VNĐ");
                                stt++;
                            }

                            Console.WriteLine("--------------------------------------------------------------------------------");
                            Console.WriteLine($"Tổng tiền: {orderToView.TotalAmount:N0} VNĐ");
                            Console.WriteLine($"Ngày tạo: {orderToView.CreatedAt:dd/MM/yyyy HH:mm}");
                            if (orderToView.UpdatedAt.HasValue)
                            {
                                Console.WriteLine($"Cập nhật lần cuối: {orderToView.UpdatedAt:dd/MM/yyyy HH:mm}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Không tìm thấy đơn hàng!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID không hợp lệ!");
                    }
                    WaitForKey();
                    break;

                case "4":

                    // Xem danh sách đơn hàng

                    orders = _dataService.GetAll<Order>();
                    Console.WriteLine("\nDanh sách đơn hàng:");
                    Console.WriteLine("ID\tNgày đặt\t\tKhách hàng\t\tTổng tiền");
                    Console.WriteLine("--------------------------------------------------------------------------------");
                    foreach (var order in orders)
                    {
                        Console.WriteLine($"{order.Id}\t{order.OrderDate:dd/MM/yyyy HH:mm}\t" +
                                        $"{order.Customer?.Name,-20}\t{order.TotalAmount:N0} VNĐ");
                    }

                    // Xóa đơn hàng
                    Console.Write("\nNhập ID đơn hàng cần xóa: ");
                    if (int.TryParse(Console.ReadLine(), out int deleteId))
                    {
                        var orderToDelete = _dataService.GetById<Order>(deleteId);
                        if (orderToDelete != null)
                        {
                            // Hiển thị thông tin đơn hàng trước khi xóa
                            Console.WriteLine($"\nThông tin đơn hàng #{orderToDelete.Id}:");
                            Console.WriteLine($"Ngày đặt: {orderToDelete.OrderDate:dd/MM/yyyy HH:mm}");
                            Console.WriteLine($"Khách hàng: {orderToDelete.Customer?.Name}");
                            Console.WriteLine($"Tổng tiền: {orderToDelete.TotalAmount:N0} VNĐ");

                            Console.Write("\nBạn có chắc chắn muốn xóa đơn hàng này? (Y/N): ");
                            if (Console.ReadLine().Trim().ToUpper() == "Y")
                            {
                                try
                                {
                                    // Hoàn lại số lượng tồn kho
                                    foreach (var detail in orderToDelete.OrderDetails)
                                    {
                                        var medicine = _dataService.GetById<Medicine>(detail.MedicineId);
                                        if (medicine != null)
                                        {
                                            medicine.Stock += detail.Quantity;
                                            medicine.UpdatedAt = DateTime.Now;
                                            _dataService.Update(medicine);
                                        }
                                    }

                                    // Xóa chi tiết đơn hàng trước
                                    foreach (var detail in orderToDelete.OrderDetails.ToList())
                                    {
                                        _dataService.Delete(detail);
                                    }

                                    // Sau đó xóa đơn hàng
                                    _dataService.Delete(orderToDelete);
                                    _dataService.SaveChanges();
                                    Console.WriteLine("Xóa đơn hàng thành công!");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Lỗi: {ex.Message}");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Không tìm thấy đơn hàng!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID không hợp lệ!");
                    }
                    WaitForKey();
                    break;

                case "5":
                    // Quay lại menu chính
                    return;

                default:
                    ShowInvalidChoice();
                    break;
            }
        }



        private void ShowPurchaseMenu()
        {
            Console.Clear();
            Console.WriteLine("=== QUẢN LÝ NHẬP HÀNG ===");
            Console.WriteLine("1. Xem danh sách đơn nhập hàng");
            Console.WriteLine("2. Tạo đơn nhập hàng mới");
            Console.WriteLine("3. Xem chi tiết đơn nhập hàng");
            Console.WriteLine("4. Xóa đơn nhập hàng");
            Console.WriteLine("5. Quay lại");
            Console.Write("Chọn chức năng: ");

            ProcessPurchaseChoice();
        }

        private void ProcessPurchaseChoice()
        {
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    // Xem danh sách đơn nhập hàng
                    var purchases = _dataService.GetAll<Purchase>();
                    Console.WriteLine("\nDanh sách đơn nhập hàng:");
                    Console.WriteLine("ID\tNgày nhập\t\tNhà cung cấp\t\tTổng tiền");
                    Console.WriteLine("--------------------------------------------------------------------------------");
                    foreach (var purchase in purchases)
                    {
                        Console.WriteLine($"{purchase.Id}\t{purchase.PurchaseDate:dd/MM/yyyy HH:mm}\t" +
                                        $"{purchase.Supplier?.Name,-20}\t{purchase.TotalAmount:N0} VNĐ");
                    }
                    WaitForKey();
                    break;

                case "2":
                    Console.WriteLine("\nTạo đơn nhập hàng mới:");

                    // Hiển thị và chọn nhà cung cấp
                    Console.WriteLine("\nDanh sách nhà cung cấp:");
                    var suppliers = _dataService.GetAll<Supplier>();
                    foreach (var supplier in suppliers)
                    {
                        Console.WriteLine($"{supplier.Id}. {supplier.Name} - SĐT: {supplier.Phone}");
                    }

                    Console.Write("\nChọn ID nhà cung cấp: ");
                    if (!int.TryParse(Console.ReadLine(), out int supplierId))
                    {
                        Console.WriteLine("ID nhà cung cấp không hợp lệ!");
                        WaitForKey();
                        return;
                    }

                    var selectedSupplier = _dataService.GetById<Supplier>(supplierId);
                    if (selectedSupplier == null)
                    {
                        Console.WriteLine("Không tìm thấy nhà cung cấp!");
                        WaitForKey();
                        return;
                    }

                    var newPurchase = new Purchase
                    {
                        PurchaseDate = DateTime.Now,
                        SupplierId = supplierId,
                        CreatedAt = DateTime.Now,
                        TotalAmount = 0
                    };

                    try
                    {
                        _dataService.Add(newPurchase);
                        _dataService.SaveChanges();

                        // Thêm chi tiết đơn nhập hàng
                        bool addingItems = true;
                        while (addingItems)
                        {
                            Console.WriteLine("\nDanh sách thuốc:");
                            var medicines = _dataService.GetAll<Medicine>();
                            foreach (var medicine in medicines)
                            {
                                Console.WriteLine($"{medicine.Id}. {medicine.Name,-30} - Giá hiện tại: {medicine.Price:N0} VNĐ");
                            }

                            Console.Write("\nChọn ID thuốc (0 để kết thúc): ");
                            if (!int.TryParse(Console.ReadLine(), out int medicineId) || medicineId == 0)
                            {
                                addingItems = false;
                                continue;
                            }

                            var selectedMedicine = _dataService.GetById<Medicine>(medicineId);
                            if (selectedMedicine == null)
                            {
                                Console.WriteLine("Không tìm thấy thuốc!");
                                continue;
                            }

                            Console.Write("Nhập số lượng: ");
                            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
                            {
                                Console.WriteLine("Số lượng không hợp lệ!");
                                continue;
                            }

                            Console.Write("Nhập giá nhập: ");
                            if (!decimal.TryParse(Console.ReadLine(), out decimal unitPrice) || unitPrice <= 0)
                            {
                                Console.WriteLine("Giá không hợp lệ!");
                                continue;
                            }

                            var purchaseDetail = new PurchaseDetail
                            {
                                PurchaseId = newPurchase.Id,
                                MedicineId = medicineId,
                                Quantity = quantity,
                                UnitPrice = unitPrice,
                                Subtotal = quantity * unitPrice,
                                CreatedAt = DateTime.Now
                            };

                            _dataService.Add(purchaseDetail);

                            // Cập nhật tồn kho và giá mới cho thuốc
                            selectedMedicine.Stock += quantity;
                            selectedMedicine.Price = unitPrice * 1.1m; // Giá bán = giá nhập + 10%
                            selectedMedicine.UpdatedAt = DateTime.Now;
                            _dataService.Update(selectedMedicine);

                            // Cập nhật tổng tiền đơn nhập hàng
                            newPurchase.TotalAmount += purchaseDetail.Subtotal;
                            newPurchase.UpdatedAt = DateTime.Now;
                            _dataService.Update(newPurchase);

                            _dataService.SaveChanges();
                            Console.WriteLine($"Đã thêm {quantity} {selectedMedicine.Name} vào đơn nhập hàng.");
                        }

                        if (newPurchase.TotalAmount > 0)
                        {
                            Console.WriteLine($"\nTạo đơn nhập hàng thành công! Tổng tiền: {newPurchase.TotalAmount:N0} VNĐ");
                        }
                        else
                        {
                            _dataService.Delete(newPurchase);
                            _dataService.SaveChanges();
                            Console.WriteLine("\nĐơn nhập hàng đã bị hủy do không có sản phẩm nào được thêm.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi: {ex.Message}");
                    }
                    WaitForKey();
                    break;

                case "3":

                    // Xem danh sách đơn nhập hàng
                    purchases = _dataService.GetAll<Purchase>();
                    Console.WriteLine("\nDanh sách đơn nhập hàng:");
                    Console.WriteLine("ID\tNgày nhập\t\tNhà cung cấp\t\tTổng tiền");
                    Console.WriteLine("--------------------------------------------------------------------------------");
                    foreach (var purchase in purchases)
                    {
                        Console.WriteLine($"{purchase.Id}\t{purchase.PurchaseDate:dd/MM/yyyy HH:mm}\t" +
                                        $"{purchase.Supplier?.Name,-20}\t{purchase.TotalAmount:N0} VNĐ");
                    }

                    Console.Write("\nNhập ID đơn nhập hàng cần xem: ");
                    if (int.TryParse(Console.ReadLine(), out int viewPurchaseId))
                    {
                        var purchaseToView = _dataService.GetById<Purchase>(viewPurchaseId);
                        if (purchaseToView != null)
                        {
                            Console.WriteLine($"\nChi tiết đơn nhập hàng #{purchaseToView.Id}");
                            Console.WriteLine($"Ngày nhập: {purchaseToView.PurchaseDate:dd/MM/yyyy HH:mm}");
                            Console.WriteLine($"Nhà cung cấp: {purchaseToView.Supplier?.Name}");
                            Console.WriteLine($"Số điện thoại: {purchaseToView.Supplier?.Phone}");
                            Console.WriteLine("\nDanh sách sản phẩm:");
                            Console.WriteLine("STT\tTên thuốc\t\t\tSố lượng\tĐơn giá\t\tThành tiền");
                            Console.WriteLine("--------------------------------------------------------------------------------");

                            int stt = 1;
                            foreach (var detail in purchaseToView.PurchaseDetails)
                            {
                                Console.WriteLine($"{stt}\t{detail.Medicine?.Name,-30}\t" +
                                                $"{detail.Quantity}\t\t" +
                                                $"{detail.UnitPrice:N0}\t\t" +
                                                $"{detail.Subtotal:N0} VNĐ");
                                stt++;
                            }

                            Console.WriteLine("--------------------------------------------------------------------------------");
                            Console.WriteLine($"Tổng tiền: {purchaseToView.TotalAmount:N0} VNĐ");
                            Console.WriteLine($"Ngày tạo: {purchaseToView.CreatedAt:dd/MM/yyyy HH:mm}");
                            if (purchaseToView.UpdatedAt.HasValue)
                            {
                                Console.WriteLine($"Cập nhật lần cuối: {purchaseToView.UpdatedAt:dd/MM/yyyy HH:mm}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Không tìm thấy đơn nhập hàng!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID không hợp lệ!");
                    }
                    WaitForKey();
                    break;

                case "4":

                    // Xem danh sách đơn nhập hàng
                    purchases = _dataService.GetAll<Purchase>();
                    Console.WriteLine("\nDanh sách đơn nhập hàng:");
                    Console.WriteLine("ID\tNgày nhập\t\tNhà cung cấp\t\tTổng tiền");
                    Console.WriteLine("--------------------------------------------------------------------------------");
                    foreach (var purchase in purchases)
                    {
                        Console.WriteLine($"{purchase.Id}\t{purchase.PurchaseDate:dd/MM/yyyy HH:mm}\t" +
                                        $"{purchase.Supplier?.Name,-20}\t{purchase.TotalAmount:N0} VNĐ");
                    }

                    Console.Write("\nNhập ID đơn nhập hàng cần xóa: ");
                    if (int.TryParse(Console.ReadLine(), out int deleteId))
                    {
                        var purchaseToDelete = _dataService.GetById<Purchase>(deleteId);
                        if (purchaseToDelete != null)
                        {
                            // Hiển thị thông tin đơn nhập hàng trước khi xóa
                            Console.WriteLine($"\nThông tin đơn nhập hàng #{purchaseToDelete.Id}:");
                            Console.WriteLine($"Ngày nhập: {purchaseToDelete.PurchaseDate:dd/MM/yyyy HH:mm}");
                            Console.WriteLine($"Nhà cung cấp: {purchaseToDelete.Supplier?.Name}");
                            Console.WriteLine($"Tổng tiền: {purchaseToDelete.TotalAmount:N0} VNĐ");

                            Console.Write("\nBạn có chắc chắn muốn xóa đơn nhập hàng này? (Y/N): ");
                            if (Console.ReadLine().Trim().ToUpper() == "Y")
                            {
                                try
                                {
                                    // Giảm số lượng tồn kho
                                    foreach (var detail in purchaseToDelete.PurchaseDetails)
                                    {
                                        var medicine = _dataService.GetById<Medicine>(detail.MedicineId);
                                        if (medicine != null)
                                        {
                                            if (medicine.Stock >= detail.Quantity)
                                            {
                                                medicine.Stock -= detail.Quantity;
                                                medicine.UpdatedAt = DateTime.Now;
                                                _dataService.Update(medicine);
                                            }
                                            else
                                            {
                                                Console.WriteLine($"Không thể xóa vì số lượng tồn kho của {medicine.Name} không đủ!");
                                                WaitForKey();
                                                return;
                                            }
                                        }
                                    }

                                    // Xóa chi tiết đơn nhập hàng trước
                                    foreach (var detail in purchaseToDelete.PurchaseDetails.ToList())
                                    {
                                        _dataService.Delete(detail);
                                    }

                                    // Sau đó xóa đơn nhập hàng
                                    _dataService.Delete(purchaseToDelete);
                                    _dataService.SaveChanges();
                                    Console.WriteLine("Xóa đơn nhập hàng thành công!");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Lỗi: {ex.Message}");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Không tìm thấy đơn nhập hàng!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID không hợp lệ!");
                    }
                    WaitForKey();
                    break;

                case "5":
                    return;

                default:
                    ShowInvalidChoice();
                    break;
            }
        }



        private void Logout()
        {
            _authService.Logout();
            _isLoggedOut = true;
            _currentUser = null;
            Console.WriteLine("Đăng xuất thành công!");
            WaitForKey();
        }

        private void ShowInvalidChoice()
        {
            Console.WriteLine("Lựa chọn không hợp lệ!");
            WaitForKey();
        }

        private void WaitForKey()
        {
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
        }

        public bool IsLoggedOut() => _isLoggedOut;
    }

}
