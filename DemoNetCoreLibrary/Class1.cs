using Newtonsoft.Json;
using System;

namespace Demo
{
    public class Class1
    {

    }
}

namespace DemoNetCoreLibrary
{
    public class Utils
    {
        public void GenerateGUID()
        {
            string test = "Test";
            string output = JsonConvert.SerializeObject(test);
            Console.WriteLine($"{Guid.NewGuid().ToString()}-{output}");
        }
    }

    public class Class1
    {
        public Class1()
        {

        }

        public Class1(string value)
        {

        }

        public void Method1(string arg1, int arg2 = 28)
        {
            Console.WriteLine($"Hello World!! -> {arg1}, {arg2}");
        }
    }

    public class Class2
    {
    }

    public class Class3
    {
    }

    public interface IInterface1
    {

    }

    public enum EnumType : int
    {

    }
}
