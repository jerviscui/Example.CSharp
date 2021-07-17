using System;

namespace PostSharpTest
{
    public interface IService
    {
        public void Do();
    }
    
    public class ServiceA : IService
    {
        [UowMethodAspect]
        public void Do()
        {
            Console.WriteLine("A.Do");
        }
    }

    public class ServiceB : IService
    {
        [UowBoundaryAspect]
        public void Do()
        {
            Console.WriteLine("B.Do");
        }
    }

    public class ServiceC : IService
    {
        [UowBoundaryAspect]
        [UowBoundaryAspect2]
        public void Do()
        {
            Console.WriteLine("C.Do");
        }
    }

    public class ServiceD : IService
    {
        [UowMethodAspect]
        [UowMethodAspect2]
        public void Do()
        {
            Console.WriteLine("D.Do");
        }
    }

    public class ServiceE : IService
    {
        [UowMethodAspect]
        [UowBoundaryAspect2]
        public void Do()
        {
            Console.WriteLine("E.Do");
        }
    }
}