using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net;
using System.Configuration;
using System.IO;

namespace GeneratorApp
{
    class Program
    {
        static private List<int> numbers = new List<int>();
        static void Main(string[] args)
        {
            Generator(); //Запуск генерации диапазона чисел
            Console.WriteLine(ListOutput(numbers)); //Вывод первоначального списка рандомных чисел в диапазоне
            string method = GenerateMethodSort(); //Получение результата сортировки
            Console.WriteLine(method);
            ConnectPOST(method); //Отправка результата сортировки
        }

        static public void ConnectPOST(string data) //Метод отправки на сервер
        {
            WebRequest request = WebRequest.Create(ConfigurationManager.AppSettings["baseUrl"]);
            request.Method = "POST"; //метод передачи данных POST
            request.Timeout = 120000; //Таймаут соединения
            byte[] sentData = Encoding.UTF8.GetBytes(data);
            request.ContentType = "application/x-www-form-urlencoded"; //Тип контента
            request.ContentLength = sentData.Length;
            using(Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(sentData, 0, sentData.Length);
            }
        }
        static private string GenerateMethodSort() //Случайный выбор метода сортировки
        {
            string method = null;
            Random random = new Random();
            int methodID = random.Next(1, 4);
            switch (methodID)
            {
                case 1: 
                    { 
                        method = ListOutput(BubbleSort(numbers));
                        //Console.WriteLine("Сортировка пузырьками!");
                        break; 
                    } //Вывод сортировки пузырьком
                case 2: 
                    { 
                        method = ListOutput(InsertionSort(numbers));
                        //Console.WriteLine("Сортировка вставками!");
                        break; 
                    } //Вывод сортировки вставками
                case 3: 
                    { 
                        method = ListOutput(SelectionSort(numbers));
                        //Console.WriteLine("Сортировка выбора!");
                        break; 
                    } //Вывод сортировки выбора
            }
            return method;
        }

        static public void Generator() //Генерация диапазона чисел
        {
            Random random = new Random();
            int countN = random.Next(20, 101); //Генерация количества чисел
            for (int i = 0; i < countN; i++)
            {
                int num = random.Next(-100, 101); //Генерация случайных чисел
                if (!numbers.Contains(num))
                    numbers.Add(num);
            }
        }

        static public string ListOutput(List<int> list) //Вывод списка чисел в строку
        {
            StringBuilder sb = new StringBuilder();
            foreach (int i in list)
            {
                sb.Append(i + " ");
            }
            //Console.WriteLine(list.Count);
            //CheckListRepetition(); //Проверка на повторяемость чисел
            string str = sb.ToString();
            return str;
        }

        static private void CheckListRepetition() //Проверка чисел на повторяемость
        {
            var groups = numbers.GroupBy(s=>s);
            foreach (var group in groups)
            {
                if(group.Count() > 1)
                {
                    Console.WriteLine("{0} повторяется {1} раз(а)", group.Key, group.Count());
                }
            }   
        }

        static List<int> BubbleSort(List<int> list) //Сортировка пузырьком
        {
            int temp = 0;
            for(int i = 0; i < list.Count; i++)
            {
                for(int j = 0; j < list.Count - 1; j++)
                {
                    if(list[j] > list[i])
                    {
                        temp = list[i];
                        list[i] = list[j];
                        list[j] = temp;
                    }
                }
            }
            return list;
        }

        static List<int> InsertionSort(List<int> list) //Сортировка вставками
        {
            for(int i = 0; i < list.Count - 1; i++)
            {
                for(int j = i + 1; j > 0; j --)
                {
                    if(list[j - 1] > list[j])
                    {
                        int temp = list[j - 1];
                        list[j - 1] = list[j];
                        list[j] = temp;
                    }
                }
            }
            return list;
        }

        static List<int> SelectionSort(List<int> list) //Сортировка выбором
        {
            int temp = 0;
            for(int i = 0; i < list.Count - 1; i++)
            {
                temp = i;
                for(int j = i + 1; j < list.Count; j++)
                {
                    if(list[j] < list[temp])
                    {
                        temp = j;
                    }
                }
                Swap(i, temp, list);
            }
            return list;
        }

        static private void Swap(int first, int second, List<int> list) //Метод для сортировки выбором
        {
            int temp = list[first];
            list[first] = list[second];
            list[second] = temp;
        }
    }
}
