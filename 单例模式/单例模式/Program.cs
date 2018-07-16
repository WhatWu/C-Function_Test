using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//单例模式的目的：单例模式的使用自然是当我们的系统中某个对象只需要一个实例的情况
//例如:操作系统中只能有一个任务管理器,操作文件时,同一时间内只允许一个实例对其操作等

//实现思路：1、确保一个类只有一个实例
//          2、提供一个访问它的全局访问点

//资料来源：https://blog.csdn.net/qq373591361/article/details/81033564

namespace 单例模式
{
    class Program
    {
        static void Main(string[] args)
        {
            Task task1 = Task.Factory.StartNew(Singleton.GetInstance().PrintTest, "1");
            Task task2 = Task.Factory.StartNew(Singleton.GetInstance().PrintTest, "2");
            Console.ReadKey();
        }
    }

    /// <summary>
    /// 单例模式的实现
    /// </summary>
    public class Singleton
    {
        // 定义一个静态变量来保存类的实例
        private static Singleton uniqueInstance;

        // 定义一个标识确保线程同步
        private static readonly object locker = new object();

        // 定义私有构造函数，使外界不能创建该类实例
        private Singleton()
        {
        }

        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static Singleton GetInstance()
        {
            // 当第一个线程运行到这里时，此时会对locker对象 "加锁"，
            // 当第二个线程运行该方法时，首先检测到locker对象为"加锁"状态，该线程就会挂起等待第一个线程解锁
            // lock语句运行完之后（即线程运行完之后）会对该对象"解锁"
            // 双重锁定只需要一句判断就可以了
            if (uniqueInstance == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (uniqueInstance == null)
                    {
                        uniqueInstance = new Singleton();
                    }
                }
            }
            return uniqueInstance;
        }

        public void PrintTest(object obj)
        {
            while (true)
            {
                Console.WriteLine("当前线程：" + obj + "  " + DateTime.Now);
                Thread.Sleep(5000);
            }
        }

        //protected的Dispose方法，保证不会被外部调用。
        //传入bool值disposing以确定是否释放托管资源
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                ///TODO:在这里加入清理"托管资源"的代码，应该是xxx.Dispose();
            }
            ///TODO:在这里加入清理"非托管资源"的代码
        }

        //供程序员显式调用的Dispose方法
        public void Dispose()
        {
            //调用带参数的Dispose方法，释放托管和非托管资源
            Dispose(true);
            //手动调用了Dispose释放资源，那么析构函数就是不必要的了，这里阻止GC调用析构函数
            System.GC.SuppressFinalize(this);
        }

        //供GC调用的析构函数，防止未手动调用Dispose()方法
        ~Singleton()
        {
            Dispose(false);//释放非托管资源
        }
    }
}
