using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;



class Program
{
    private static  HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting downloading data...");

        Task<string> taskJsonPlaceholder = DownloadDataFromJsonPlaceholder();
        Task<string> taskHttpBin = DownloadDataFromHttpBin();
        Task<string> taskSwapi = DownloadDataFromSWAPI();
        Task<string> taskWithError  = SimulateError(); 
        List<Task<string>> downloadTasks = new List<Task<string>>
            {
                taskJsonPlaceholder,
                taskHttpBin,
                taskSwapi,
               // taskWithError // if return this string will be shown simulation of error
            };

        
        Console.WriteLine("main thread make another operations");
        double simresult = 0;
        for (int i = 0; i < 10000; i++)
        {
            simresult += i * Math.Sqrt(i);
            if(i == 9999)
            {
                await Task.Delay(1100);
                Console.WriteLine($"simulatins of making operatins is successful, result = {simresult}");
            }
        }
        await Task.Delay(1100); 
        Console.WriteLine("main thread is still active");

        try
        {
           
            string[] results = await Task.WhenAll(downloadTasks);

            Console.WriteLine("\nAll task completed successfully");
            for (int i = 0; i < results.Length; i++)
            {
                Console.WriteLine($"\nTask {i + 1}: Quantity symbols = {results[i].Length}");
                Console.WriteLine($"\n{results[i]}");
                Regex regex = new Regex(@"\bhttps?:\/\/[^\s/$.?#].[^\s]*\b");
                MatchCollection matches = regex.Matches(results[i]);

                if (matches.Count == 0)
                {
                    Console.WriteLine($"\u001b[38;2;245;55;34mNo url found \u001b[0m");
                }
                else
                {
                    Console.WriteLine($"\u001b[38;2;60;200;75mList of url in your responce:\u001b[0m");
                    foreach (Match match in matches)
                    {
                        Console.WriteLine($"\u001b[38;2;60;200;75m{match.Value}\u001b[0m");
                    }
                }
            }
        }
        catch (Exception ex)
        {
           
            Console.WriteLine("\u001b[38;2;245;55;34m\nhappend error in processing of downloading data\u001b[0m");
            Console.WriteLine($"\u001b[38;2;245;55;34mError: {ex.Message}\u001b[0m");
        }

        Console.WriteLine("\nPress any key to complete work");
        Console.ReadKey();
    }

    
    private static async Task<string> DownloadDataFromJsonPlaceholder()
    {
        Console.WriteLine("Starting downloading from JSONPlaceholder...");
        await Task.Delay(1285);
        HttpResponseMessage response = await client.GetAsync("https://jsonplaceholder.typicode.com/posts/2");
        response.EnsureSuccessStatusCode();
        string data = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Completed downloading from JSONPlaceholder.");
        return data;
    }

   
    private static async Task<string> DownloadDataFromHttpBin()
    {
        Console.WriteLine("Starting downloading from HTTPBin...");
        await Task.Delay(1783);
        HttpResponseMessage response = await client.GetAsync("https://httpbin.org/get");
        response.EnsureSuccessStatusCode();
        string data = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Completed downloading from HTTPBin.");
        return data;
    }

  
    private static async Task<string> DownloadDataFromSWAPI()
    {
        Console.WriteLine("Starting downloading from SWAPI...");
        await Task.Delay(2134);
        HttpResponseMessage response = await client.GetAsync("https://swapi.dev/api/people/6/");
        response.EnsureSuccessStatusCode();
        string data = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Completed downloading from SWAPI.");
        return data;
    }

   
    private static async Task<string> SimulateError()
    {
        Console.WriteLine("Starting downloading source with error");
        await Task.Delay(1290);
        HttpResponseMessage response = await client.GetAsync("https://swappi.dev/api/people/1/");
        response.EnsureSuccessStatusCode();
        string data = await response.Content.ReadAsStringAsync();
        Console.WriteLine("completed downloading from SWAPPI");
        return data;
    }
}

