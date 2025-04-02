using System;
using System.Collections.Generic;

namespace ATMApplication
{
    class Program
    {
        static Dictionary<string, (string Pin, decimal Balance)> accounts = new Dictionary<string, (string, decimal)>();
        static string currentUser = null;

        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в банкомат!");

            bool isRunning = true;

            while (isRunning)
            {
                if (currentUser == null)
                {
                    ShowAuthMenu();
                }
                else
                {
                    ShowMainMenu();
                }

                string choice = Console.ReadLine();

                if (currentUser == null)
                {
                    switch (choice)
                    {
                        case "1":
                            RegisterAccount();
                            break;
                        case "2":
                            Login();
                            break;
                        case "3":
                            isRunning = false;
                            Console.WriteLine("Спасибо за использование нашего банкомата. До свидания!");
                            break;
                        default:
                            Console.WriteLine("Неверный выбор. Пожалуйста, попробуйте снова.");
                            break;
                    }
                }
                else
                {
                    switch (choice)
                    {
                        case "1":
                            CheckBalance();
                            break;
                        case "2":
                            DepositMoney();
                            break;
                        case "3":
                            WithdrawMoney();
                            break;
                        case "4":
                            Console.WriteLine($"До свидания, {currentUser}!");
                            currentUser = null;
                            break;
                        default:
                            Console.WriteLine("Неверный выбор. Пожалуйста, попробуйте снова.");
                            break;
                    }
                }
            }
        }

        static void ShowAuthMenu()
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1. Регистрация");
            Console.WriteLine("2. Вход");
            Console.WriteLine("3. Выход");
            Console.Write("Ваш выбор: ");
        }

        static void ShowMainMenu()
        {
            Console.WriteLine($"\nПользователь: {currentUser}");
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Проверить баланс");
            Console.WriteLine("2. Пополнить счет");
            Console.WriteLine("3. Снять деньги");
            Console.WriteLine("4. Выйти из аккаунта");
            Console.Write("Ваш выбор: ");
        }

        static void RegisterAccount()
        {
            Console.Write("\nВведите имя пользователя: ");
            string username = Console.ReadLine();

            if (accounts.ContainsKey(username))
            {
                Console.WriteLine("Пользователь с таким именем уже существует.");
                return;
            }

            Console.Write("Придумайте PIN-код (4 цифры): ");
            string pin = Console.ReadLine();

            if (pin.Length != 4 || !int.TryParse(pin, out _))
            {
                Console.WriteLine("PIN-код должен состоять из 4 цифр.");
                return;
            }

            accounts[username] = (pin, 0m);
            Console.WriteLine("Регистрация прошла успешно! Теперь вы можете войти в систему.");
        }

        static void Login()
        {
            Console.Write("\nВведите имя пользователя: ");
            string username = Console.ReadLine();

            if (!accounts.ContainsKey(username))
            {
                Console.WriteLine("Пользователь не найден.");
                return;
            }

            Console.Write("Введите PIN-код: ");
            string pin = Console.ReadLine();

            if (accounts[username].Pin == pin)
            {
                currentUser = username;
                Console.WriteLine($"Вход выполнен успешно, {username}!");
            }
            else
            {
                Console.WriteLine("Неверный PIN-код.");
            }
        }

        static void CheckBalance()
        {
            Console.WriteLine($"\nВаш текущий баланс: {accounts[currentUser].Balance:C}");
        }

        static void DepositMoney()
        {
            Console.Write("\nВведите сумму для пополнения: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                var account = accounts[currentUser];
                account.Balance += amount;
                accounts[currentUser] = account;
                Console.WriteLine($"Вы успешно пополнили счет на {amount:C}. Новый баланс: {account.Balance:C}");
            }
            else
            {
                Console.WriteLine("Неверная сумма. Пожалуйста, введите положительное число.");
            }
        }

        static void WithdrawMoney()
        {
            Console.Write("\nВведите сумму для снятия: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                var account = accounts[currentUser];

                if (amount <= account.Balance)
                {
                    account.Balance -= amount;
                    accounts[currentUser] = account;
                    Console.WriteLine($"Вы успешно сняли {amount:C}. Новый баланс: {account.Balance:C}");
                }
                else
                {
                    Console.WriteLine("Недостаточно средств на счете.");
                }
            }
            else
            {
                Console.WriteLine("Неверная сумма. Пожалуйста, введите положительное число.");
            }
        }
    }
}