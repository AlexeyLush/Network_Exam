
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            const string GIVE = "GIVE";
            const string ADD = "ADD";
            const string UPDATE = "UPDATE";
            const string REMOVE = "REMOVE";

            while (true)
            {
                var request = new Request();

                Console.WriteLine("1. Получить данные\n2. Добавить данные \n3. Обновить данные \n4. Удалить данные");
                string action = Console.ReadLine();
                switch (action)
                {

                    case "1":
                        request.Action = GIVE;
                        request.Path = "user";
                        HttpWebRequest getAllRequest = (HttpWebRequest)WebRequest.Create("http://localhost/users/get/");
                        getAllRequest.Method = "POST";

                        var getAllRequestJson = JsonSerializer.Serialize(request);

                        var dataAll = Encoding.UTF8.GetBytes(getAllRequestJson);


                        using (var stream = getAllRequest.GetRequestStream())
                        {
                            stream.Write(dataAll, 0, dataAll.Length);
                        }


                        string result = string.Empty;
                        var responseGetAll = getAllRequest.GetResponse();
                        using (Stream stream = responseGetAll.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                result = reader.ReadToEnd();
                            }
                        }
                        responseGetAll.Close();


                        Console.WriteLine("---------------------------------\n");
                        Console.WriteLine("---------------------------------");
                        string[] options = result.Split(new char[] { '\n' });
                        bool isHeader = false;
                        bool isData = false;
                        bool isNewData = false;
                        foreach (var item in options)
                        {

                            if (item == "\t[header]")
                            {
                                isHeader = true;
                                continue;
                            }
                            else if (item == "\t[/header]")
                            {
                                isHeader = false;
                                Console.WriteLine("\n---------------------------------");
                            }
                            else if (item == "\t[data]")
                            {
                                isData = true;
                                continue;
                            }
                            else if (item == "\t[/data]")
                            {
                                isData = false;
                                Console.WriteLine("\n---------------------------------");
                            }

                            if (isHeader)
                            {
                                var header = item.Remove(0, 5);
                                header = header.Remove(header.Length - 4, 4);
                                Console.Write($"\t{header}\t|");
                            }
                            else if (isData)
                            {
                                var dataServer = item.Remove(0, 5);
                                dataServer = dataServer.Remove(dataServer.Length - 4, 4);
                                Console.Write($"\t{dataServer}\t|");
                                if (isNewData)
                                {
                                    Console.WriteLine("\n---------------------------------");
                                    isNewData = false;
                                }

                                else
                                    isNewData = true;
                            }

                        }

                        break;
                    case "2":
                        request.Action = ADD;
                        request.Path = "user";
                        Console.Write("Введите имя пользователя: ");
                        request.Value = Console.ReadLine();
                        bool isTrueName = false;
                        foreach (var item in request.Value)
                        {
                            if (char.IsLetterOrDigit(item))
                            {
                                isTrueName = true;
                            }
                        }
                        if (isTrueName)
                        {
                            HttpWebRequest addRequest = (HttpWebRequest)WebRequest.Create("http://localhost/users/add/");
                            addRequest.Method = "POST";

                            var addRequestJson = JsonSerializer.Serialize(request);

                            var dataAdd = Encoding.UTF8.GetBytes(addRequestJson);


                            using (var stream = addRequest.GetRequestStream())
                            {
                                stream.Write(dataAdd, 0, dataAdd.Length);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Вы ввели некоректное имя");
                        }
                        break;

                    case "3":
                        request.Action = UPDATE;
                        request.Path = "user";
                        Console.Write("Введите ID пользователя: ");
                        request.Value = Console.ReadLine();
                        Console.Write("Введите новое имя пользователя: ");
                        request.NewData = Console.ReadLine();
                        bool isTrueNameUpdate = false;
                        foreach (var item in request.NewData)
                        {
                            if (char.IsLetterOrDigit(item))
                            {
                                isTrueNameUpdate = true;
                            }
                        }
                        if (isTrueNameUpdate)
                        {
                            HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create("http://localhost/users/update/");
                            updateRequest.Method = "POST";

                            var updateRequestJson = JsonSerializer.Serialize(request);

                            var dataUpdate = Encoding.UTF8.GetBytes(updateRequestJson);


                            using (var stream = updateRequest.GetRequestStream())
                            {
                                stream.Write(dataUpdate, 0, dataUpdate.Length);
                            }

                            string resultUpdate = string.Empty;
                            var responseUpdate = updateRequest.GetResponse();
                            using (Stream stream = responseUpdate.GetResponseStream())
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    resultUpdate = reader.ReadToEnd();
                                }
                            }
                            if (resultUpdate == "Вы ввели неверный ID")
                            {
                                Console.WriteLine(resultUpdate);
                            }
                            else
                            {
                                Console.WriteLine("Успешно обновлён");
                            }
                            responseUpdate.Close();
                        }
                        else
                        {
                            Console.WriteLine("Вы ввели некоректное имя");
                        }
                        break;
                    case "4":
                        request.Action = REMOVE;
                        request.Path = "user";
                        Console.Write("Введите ID пользователя: ");
                        request.Value = Console.ReadLine();
                        HttpWebRequest deleteRequest = (HttpWebRequest)WebRequest.Create("http://localhost/users/delete/");
                        deleteRequest.Method = "POST";

                        var deleteRequestJson = JsonSerializer.Serialize(request);

                        var dataDelete = Encoding.UTF8.GetBytes(deleteRequestJson);


                        using (var stream = deleteRequest.GetRequestStream())
                        {
                            stream.Write(dataDelete, 0, dataDelete.Length);
                        }

                        string resultDelete = string.Empty;
                        var responseDelete = deleteRequest.GetResponse();
                        using (Stream stream = responseDelete.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                resultDelete = reader.ReadToEnd();
                            }
                        }
                        if (resultDelete == "Вы ввели неверный ID")
                        {
                            Console.WriteLine(resultDelete);
                        }
                        else
                        {
                            Console.WriteLine("Успешно обновлён");
                        }
                        responseDelete.Close();

                        break;
                    default:
                        Console.WriteLine("Вы ошиблись");
                        break;
                }


            }
        }
    }
}
