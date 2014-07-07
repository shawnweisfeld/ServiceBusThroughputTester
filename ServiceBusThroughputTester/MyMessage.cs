using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceBusThroughputTester
{
    public class MyMessage
    {
        public MyMessage()
        {

        }
        public MyMessage(int id, string message, string taskName)
        {
            this.Id = id;
            this.Message = message;
            this.TaskName = taskName;
        }

        public int Id { get; set; }
        public string Message { get; set; }
        public string TaskName { get; set; }

    }
}
