using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Args;

namespace ServiceBusThroughputTester
{
    public class AppArgs
    {
        public bool IsSender { get; set; }
        public bool IsReciver { get; set; }
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
        public int MessageSize { get; set; }
        public int TaskCount { get; set; }
        public bool Express { get; set; }
        public bool Partioned { get; set; }
        public int MaxConcurrentCalls { get; set; }
    }
}
