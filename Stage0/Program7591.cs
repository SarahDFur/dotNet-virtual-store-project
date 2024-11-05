// See https://aka.ms/new-console-template for more information

namespace Stage0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome7591();
            Console.ReadKey();
        }

        static partial void welcome6627();//Collaborator 2

        private static void Welcome7591()//Collaborator 1 
        {
            Console.Write("Enter your name: ");
            string? userName = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my console application", userName);
        }
    }
}