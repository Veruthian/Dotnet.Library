﻿using System;
using Veruthian.Dotnet.Library.Collections;

namespace _Console
{
    class Program
    {
        static void Main(string[] args)
        {            
            Pause();
        }

        static void Pause()
        {
            Console.Write("Press any key to continue...");

            while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;

            Console.WriteLine();
        }
    }
}