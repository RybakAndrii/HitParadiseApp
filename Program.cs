using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

class Program
{
    static string fileName = "hit_parade.json";

    static void Main()
    {
        List<HitParade> hitParadeList = LoadData();
        while (true)
        {
            Console.WriteLine("Меню:");
            Console.WriteLine("1. Створити новий об’єкт");
            Console.WriteLine("2. Вивести вміст файлу");
            Console.WriteLine("3. Вивести звіт");
            Console.WriteLine("4. Шукати об’єкт");
            Console.WriteLine("5. Вихід");
            Console.Write("Ваш вибір: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateNewObject(hitParadeList);
                    break;
                case "2":
                    DisplayFileContents();
                    break;
                case "3":
                    DisplayReport(hitParadeList);
                    break;
                case "4":
                    SearchObject(hitParadeList);
                    break;
                case "5":
                    SaveData(hitParadeList);
                    return;
                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }
        }
    }

    static void CreateNewObject(List<HitParade> hitParadeList)
    {
        Console.Write("Введіть прізвище співака: ");
        string surname = Console.ReadLine();

        Console.Write("Введіть назву твору: ");
        string song = Console.ReadLine();

        Console.Write("Введіть країну: ");
        string country = Console.ReadLine();

        int year;
        while (true)
        {
            Console.Write("Введіть рік: ");
            if (int.TryParse(Console.ReadLine(), out year))
                break;
            else
                Console.WriteLine("Некоректний рік.");
        }

        int votes;
        while (true)
        {
            Console.Write("Введіть кількість голосів: ");
            if (int.TryParse(Console.ReadLine(), out votes))
                break;
            else
                Console.WriteLine("Некоректна кількість голосів.");
        }

        hitParadeList.Add(new HitParade(surname, song, country, year, votes));
        Console.WriteLine("Об’єкт додано.");
    }

    static void DisplayFileContents()
    {
        if (File.Exists(fileName))
        {
            string fileContents = File.ReadAllText(fileName);
            Console.WriteLine("Вміст файлу:");
            Console.WriteLine(fileContents);
        }
        else
        {
            Console.WriteLine("Файл не знайдено.");
        }
    }

    static void DisplayReport(List<HitParade> hitParadeList)
    {
        Console.WriteLine("Звіт про об’єкти:");
        for (int i = 0; i < hitParadeList.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {hitParadeList[i]}");
        }

        int totalVotes = hitParadeList.Sum(x => x.Votes);
        Console.WriteLine($"Загальна кількість голосів: {totalVotes}");
    }

    static void SearchObject(List<HitParade> hitParadeList)
    {
        Console.Write("Введіть рік для пошуку: ");
        int year = int.Parse(Console.ReadLine());

        Console.Write("Введіть країну для пошуку: ");
        string country = Console.ReadLine();

        var results = hitParadeList.Where(x => x.Year == year && x.Country.ToLower() == country.ToLower()).ToList();

        if (results.Any())
        {
            Console.WriteLine("Знайдено об’єкти:");
            foreach (var item in results)
            {
                Console.WriteLine(item);
            }
        }
        else
        {
            Console.WriteLine("Об’єкти не знайдені.");
        }
    }

    static List<HitParade> LoadData()
    {
        if (File.Exists(fileName))
        {
            string fileContents = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<List<HitParade>>(fileContents) ?? new List<HitParade>();
        }
        return new List<HitParade>();
    }

    static void SaveData(List<HitParade> hitParadeList)
    {
        string jsonData = JsonConvert.SerializeObject(hitParadeList, Formatting.Indented);
        File.WriteAllText(fileName, jsonData);
        Console.WriteLine("Дані збережено.");
    }
}

class HitParade
{
    public string Surname { get; set; }
    public string Song { get; set; }
    public string Country { get; set; }
    public int Year { get; set; }
    public int Votes { get; set; }

    public HitParade(string surname, string song, string country, int year, int votes)
    {
        Surname = surname;
        Song = song;
        Country = country;
        Year = year;
        Votes = votes;
    }

    public override string ToString()
    {
        return $"Прізвище: {Surname}, Твір: {Song}, Країна: {Country}, Рік: {Year}, Голоси: {Votes}";
    }
}
