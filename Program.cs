using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Schedule
{
    internal class Program
    {
        static Employer CreateEmp((int, string, bool, string) _employer)
        {
            Employer emp;
            if (_employer.Item1 < 18)
                emp = new YoungEmployer(_employer.Item1, _employer.Item2, _employer.Item3);
            else if ((_employer.Item1 > 60 && _employer.Item3 == false) || (_employer.Item1 > 65 && _employer.Item3 == true))
                emp = new OldEmployer(_employer.Item1, _employer.Item2, _employer.Item3);
            else
                emp = new Employer(_employer.Item1, _employer.Item2, _employer.Item3, _employer.Item4);
            return emp;
        }
        static (int, string, bool, string) InputDateEmployer()
        {
            bool correctIn = false, sex = true;
            int age = 18;
            Console.WriteLine("\nВведите имя сотрудника");
            string nameEmp = Console.ReadLine();
            do
            {
                Console.WriteLine("Введите пол сотрудника: W - woman, M - man.");
                string input = Console.ReadLine();
                switch (input.ToLower())
                {
                    case "w": sex = false; correctIn = true; break;
                    case "m": sex = true; correctIn = true; break;
                    default: Console.WriteLine("Неверный ввод"); break;
                }
            } while (!correctIn);
            do
            {
                correctIn = false;
                Console.WriteLine("Введите дату рождения в формате ГГГГ.ММ.ДД");
                try
                {
                    string dateInput = Console.ReadLine();
                    var tmp = dateInput.Split(',','.','\\','/');
                    DateTime birthDate = new DateTime(int.Parse(tmp[0]), int.Parse(tmp[1]), int.Parse(tmp[2]));
                    DateTime now = DateTime.Now;
                    TimeSpan duration = now - birthDate;
                    age = (int)(duration.TotalDays/365);
                    if (age >= 14 && age <= 100)
                        correctIn = true;
                    else
                        Console.WriteLine("Неподходящий возраст");
                }
                catch
                {
                    Console.WriteLine("Неккоректный ввод возроста");
                }
            } while (!correctIn);
            string position = string.Empty;
            if ((age >= 18 && age <= 65 && sex == true) || (age >= 18 && age <= 60 && sex == false))
            {
                correctIn = false;
                do
                {
                    Console.WriteLine("Выберите график:\n1)2/2 по 12 часов\n2)5/2 по 8 часов\n");
                    ConsoleKey key = Console.ReadKey().Key;
                    switch (key)
                    {
                        case ConsoleKey.D1: position = "worker"; correctIn = true; break;
                        case ConsoleKey.D2: position = "manager"; correctIn = true; break;
                        default: Console.WriteLine("Неверный ввод"); break;
                    }
                } while (!correctIn);
            }
            (int, string, bool, string) tuple = (age, nameEmp, sex, position);
            return tuple;
        }

        static void Main(string[] args)
        {
            string input = string.Empty;
            DBConnect connect = new DBConnect("Schedule");
            ConsoleKey key= ConsoleKey.Enter;
            do
            {

                Console.WriteLine("1) добавить сотрудника\n2) вызвать график работников\nQ) выход");
                key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.D1:
                        connect.AddEmployer(CreateEmp(InputDateEmployer())); break;
                    case ConsoleKey.D2:
                        Console.WriteLine("\nДля ввывода графика выберите ");
                        Console.WriteLine("1) график определенного работника\n2)график всех рабочих");
                        key = Console.ReadKey().Key;
                        Console.WriteLine();
                        switch (key)
                        {
                            case ConsoleKey.D1:
                                Console.WriteLine("\nВведите имя сотрудника");
                                input = Console.ReadLine();
                                connect.SelectSchedulOne(input); break;
                            case ConsoleKey.D2: connect.SelectSchedul(); break;
                            default: Console.WriteLine("\nНеверный ввод"); break;
                        }; break;
                    case ConsoleKey.Q:
                        Console.WriteLine("\nЗавершение работы");
                        break;
                    default:
                        Console.WriteLine("\nНеверный ввод"); break;
                }
                Console.Clear();
            } while (key != ConsoleKey.Q);
            
        }
    }
}
