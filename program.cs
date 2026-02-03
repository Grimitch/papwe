using System;
using System.Linq;

namespace SimpleECommerce
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new AppDbContext())
            {
                Console.WriteLine("=== ПРОСТОЙ ИНТЕРНЕТ-МАГАЗИН ===");
                Console.WriteLine("Создаем базу данных...");
                db.Database.EnsureCreated();
                if (!db.Users.Any())
                {
                    Console.WriteLine("Добавляем тестовые данные...");
                    AddTestData(db);
                }
                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("\n--- МЕНЮ ---");
                    Console.WriteLine("1. Показать всех пользователей");
                    Console.WriteLine("2. Добавить пользователя");
                    Console.WriteLine("3. Показать все товары");
                    Console.WriteLine("4. Добавить товар");
                    Console.WriteLine("5. Создать заказ");
                    Console.WriteLine("6. Выйти");
                    Console.Write("Выберите действие: ");
                    
                    string choice = Console.ReadLine();
                    
                    switch (choice)
                    {
                        case "1":
                            ShowUsers(db);
                            break;
                        case "2":
                            AddUser(db);
                            break;
                        case "3":
                            ShowProducts(db);
                            break;
                        case "4":
                            AddProduct(db);
                            break;
                        case "5":
                            CreateOrder(db);
                            break;
                        case "6":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Неверный выбор!");
                            break;
                    }
                }
                
                Console.WriteLine("Программа завершена!");
            }
        }
        static void ShowUsers(AppDbContext db)
        {
            Console.WriteLine("\n=== ПОЛЬЗОВАТЕЛИ ===");
            var users = db.Users.ToList();
            
            if (users.Count == 0)
            {
                Console.WriteLine("Пользователей нет.");
                return;
            }
            
            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.Id}, Имя: {user.Name}, Email: {user.Email}");
            }
        }
        
        static void AddUser(AppDbContext db)
        {
            Console.WriteLine("\n=== ДОБАВЛЕНИЕ ПОЛЬЗОВАТЕЛЯ ===");
            
            Console.Write("Введите имя: ");
            string name = Console.ReadLine();
            
            Console.Write("Введите email: ");
            string email = Console.ReadLine();
            
            var user = new User
            {
                Name = name,
                Email = email
            };
            
            db.Users.Add(user);
            db.SaveChanges();
            
            Console.WriteLine("Пользователь добавлен!");
        }
        
        static void ShowProducts(AppDbContext db)
        {
            Console.WriteLine("\n=== ТОВАРЫ ===");
            var products = db.Products.Include(p => p.Category).ToList();
            
            if (products.Count == 0)
            {
                Console.WriteLine("Товаров нет.");
                return;
            }
            
            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.Id}, Название: {product.Name}, Цена: {productPrice}, Категория: {product.Category?.Name}");
            }
        }
        
        static void AddProduct(AppDbContext db)
        {
            Console.WriteLine("\n=== ДОБАВЛЕНИЕ ТОВАРА ===");

            var categories = db.Categories.ToList();
            Console.WriteLine("Доступные категории:");
            foreach (var cat in categories)
            {
                Console.WriteLine($"ID: {cat.Id}, Название: {cat.Name}");
            }
            
            Console.Write("Введите название товара: ");
            string name = Console.ReadLine();
            
            Console.Write("Введите цену: ");
            decimal price = decimal.Parse(Console.ReadLine());
            
            Console.Write("Введите количество на складе: ");
            int stock = int.Parse(Console.ReadLine());
            
            Console.Write("Введите ID категории: ");
            int categoryId = int.Parse(Console.ReadLine());
            
            var product = new Product
            {
                Name = name,
                Price = price,
                Stock = stock,
                CategoryId = categoryId
            };
            
            db.Products.Add(product);
            db.SaveChanges();
            
            Console.WriteLine("Товар добавлен!");
        }
        
        static void CreateOrder(AppDbContext db)
        {
            Console.WriteLine("\n=== СОЗДАНИЕ ЗАКАЗА ===");

            Console.WriteLine("Пользователи:");
            ShowUsers(db);
            Console.Write("Введите ID пользователя: ");
            int userId = int.Parse(Console.ReadLine());

            Console.WriteLine("Товары:");
            ShowProducts(db);
            Console.Write("Введите ID товара: ");
            int productId = int.Parse(Console.ReadLine());
            
            Console.Write("Введите количество: ");
            int quantity = int.Parse(Console.ReadLine());

            var product = db.Products.Find(productId);
            if (product == null)
            {
                Console.WriteLine("Товар не найден!");
                return;
            }
            
            var order = new Order
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity,
                TotalPrice = product.Price * quantity
            };
            
            db.Orders.Add(order);
            db.SaveChanges();
            
            Console.WriteLine($"Заказ создан! Сумма: {order.TotalPrice}");
        }
        
        static void AddTestData(AppDbContext db)
        {
            var cat1 = new Category { Name = "Электроника", Description = "Техника" };
            var cat2 = new Category { Name = "Одежда", Description = "Модная одежда" };
            db.Categories.AddRange(cat1, cat2);
            db.SaveChanges();

            var user1 = new User { Name = "Иван Иванов", Email = "ivan@mail.com" };
            var user2 = new User { Name = "Мария Петрова", Email = "maria@mail.com" };
            db.Users.AddRange(user1, user2);
            db.SaveChanges();

            var prod1 = new Product { Name = "Телефон", Price = 15000, Stock = 10, CategoryId = 1 };
            var prod2 = new Product { Name = "Ноутбук", Price = 30000, Stock = 5, CategoryId = 1 };
            var prod3 = new Product { Name = "Футболка", Price = 500, Stock = 50, CategoryId = 2 };
            db.Products.AddRange(prod1, prod2, prod3);
            db.SaveChanges();
          
            var review1 = new Review { UserId = 1, ProductId = 1, Rating = 5, Comment = "Отличный телефон!" };
            var review2 = new Review { UserId = 2, ProductId = 3, Rating = 4, Comment = "Хорошая футболка" };
            db.Reviews.AddRange(review1, review2);
            db.SaveChanges();
            
            Console.WriteLine("Тестовые данные добавлены!");
        }
    }
}
