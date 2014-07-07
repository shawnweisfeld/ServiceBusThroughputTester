using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusThroughputTester
{
    class Program
    {
        private static AppArgs _args;
        private static PerfCounter _counter;

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            
            _args = Args.Configuration.Configure<AppArgs>().CreateAndBind(args);
            _counter = new PerfCounter();

            if (!_args.IsSender && !_args.IsReciver)
            {
                Setup();
            }
            else
            {
                for (int taskId = 0; taskId < _args.TaskCount; taskId++)
                {
                    if (_args.IsSender)
                    {
                        SendMessage(taskId);
                    }
                    else if (_args.IsReciver)
                    {
                        ReciveMessages(taskId);
                    }
                }
            }
            Console.ReadKey();
        }

        private static void Setup()
        {
            Console.WriteLine("Setup Environment");

            try
            {
                NamespaceManager ns = NamespaceManager.CreateFromConnectionString(_args.ConnectionString);

                if (!ns.QueueExists(_args.QueueName))
                {
                    ns.CreateQueue(new QueueDescription(_args.QueueName)
                    {
                        EnableExpress = _args.Express,
                        EnablePartitioning = _args.Partioned,
                    });
                }

                _counter.Reset();
                
            }
            catch (Exception ex)
            {
                WriteException(ex);
            }

            Console.WriteLine("Environment Setup Complete!");
        }

        private static void ReciveMessages(int taskId)
        {
            Console.WriteLine("Reciving task {0}", taskId);
            try
            {
                var client = QueueClient.CreateFromConnectionString(_args.ConnectionString, _args.QueueName);
                var opts = new OnMessageOptions()
                {
                    AutoComplete = true,
                    MaxConcurrentCalls = _args.MaxConcurrentCalls,
                };

                opts.ExceptionReceived += (sender, e) => { WriteException(e.Exception); };

                client.OnMessageAsync(msg =>
                {
                    return Task.Factory.StartNew(() =>
                        {
                            _counter.Increment();
                            var body = msg.GetBody<MyMessage>();
                            _counter.Increment();
                            Console.WriteLine("Reciever {0} Message {1} Sender {2}", taskId, body.Id, body.TaskName);
                        });
                }, opts);
            }
            catch (Exception ex)
            {
                WriteException(ex);
            }
        }

        public static async void SendMessage(int taskId)
        {
            Console.WriteLine("Sending Task {0}", taskId);
            try
            {
                var client = QueueClient.CreateFromConnectionString(_args.ConnectionString, _args.QueueName);
                var message = RandomString(_args.MessageSize * 1024);
                int iteration = 0;

                while (true)
                {
                    await client.SendAsync(new BrokeredMessage(new MyMessage(iteration++, message, taskId.ToString())));
                    _counter.Increment();
                    Console.WriteLine("Sender {0} Message {1}", taskId, iteration);
                }
            }
            catch (Exception ex)
            {
                WriteException(ex);
            }
        }

        private static void WriteException(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: {0}\r\n{1}", ex.Message, ex.StackTrace);
        }

        private static Random random = new Random((int)DateTime.Now.Ticks);
        private static string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

    }
}
