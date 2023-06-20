using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CalibrationTesting
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Messenger messenger = new Messenger();
            Jasper jasper = new Jasper();

            while (true)
            {
                messenger.DoTask();
            }
        }
    }

    internal class Messenger
    {
        private int counter = 0;

        public async void DoTask()
        {
            while (true)
            {
                await SendNewMessage();
                Console.WriteLine($"{counter}");
                Thread.Sleep(1000);
                counter++;
            }
        }

        public Task<bool> SendNewMessage()
        {
            bool i = WeakReferenceMessenger.Default.Send<SomethingRequest>();
            return Task.FromResult(i);
        }
    }
}