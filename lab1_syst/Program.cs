using System;
using System.Threading;

class Program
{
    static Semaphore[] semaphores;
    static int numberOfProcesses; 

    static void Main(string[] args)
    {
        numberOfProcesses = 2; 
        int numberOfResources = 3; 
        string[] resourceUsage = { "1,3", "1,2,3" }; 

        semaphores = new Semaphore[numberOfResources];

        for (int i = 0; i < numberOfResources; i++)
        {
            semaphores[i] = new Semaphore(1, 1);
        }

        for (int i = 0; i < numberOfProcesses; i++)
        {
            Thread processThread = new Thread(Process);
            processThread.Start(new ProcessInfo(i + 1, resourceUsage[i]));
        }
    }

    static void Process(object processInfoObj)
    {
        ProcessInfo processInfo = (ProcessInfo)processInfoObj;
        int processNumber = processInfo.ProcessNumber;
        string[] resourceUsage = processInfo.ResourceUsage.Split(','); 
        int[] resources = Array.ConvertAll(resourceUsage, int.Parse); 
        foreach (int resource in resources)
        {
            Console.WriteLine($"Процесс {processNumber} чекає на ресурс {resource}");
            semaphores[resource - 1].WaitOne(); 

            Console.WriteLine($"Процесс {processNumber} бере ресурс {resource}");

            Thread.Sleep(500); 

            Console.WriteLine($"Процесс {processNumber} відпускає ресурс {resource}");

            semaphores[resource - 1].Release();
        }

        Console.WriteLine($"Процесс {processNumber} закінчує роботу");
    }

    class ProcessInfo
    {
        public int ProcessNumber { get; }
        public string ResourceUsage { get; } 

        public ProcessInfo(int processNumber, string resourceUsage)
        {
            ProcessNumber = processNumber;
            ResourceUsage = resourceUsage;
        }
    }
}