using System;

namespace RecordTest
{
    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            Equal_Test();
        }

        public static void CreateRecord_Test()
        {
            var person = new Person("is a");
            //CS8852
            //person.Name = "";
        }

        public static void CreateRecord_WithInit_Test()
        {
            var person = new Person2 { Name = "is b" };
            //CS8852
            //person.Name = "";
        }

        public static void CanSetProperty_Test()
        {
            var person = new Person3 { Name = "is c" };
            person.Name = "";

            Console.WriteLine(person.Name);
        }

        public static void CreateRecord_With_Test()
        {
            var person = new Person("is a");

            var pb = person with { Name = "is b" };
            Console.WriteLine(pb.Name);
        }

        public static void Equal_Test()
        {
            var p1 = new Person("is a");
            var p2 = new Person("is a");

            Console.WriteLine(p1.Equals(p2));
        }

        private record Person2Para(string FirstName, string LastName);

        public static void Deconstruct_Test()
        {
            var p1 = new Person("is a");
            p1.Deconstruct(out var abc);

            Person2Para p2 = new("is a", "is b");
            var (a, b) = p2;
        }
    }
}
