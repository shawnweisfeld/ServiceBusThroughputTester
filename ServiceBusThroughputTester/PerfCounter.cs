using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusThroughputTester
{
    public class PerfCounter
    {
        /// <summary>
        /// Counter for counting number of operations per second
        /// </summary>
        private PerformanceCounter _OperationsPerSecond;

        private static string _category = "ServiceBusTestCategory";

        public PerfCounter()
        {
            if (!PerformanceCounterCategory.Exists(_category))
            {
                CounterCreationDataCollection counters = new CounterCreationDataCollection();

                // 2. counter for counting operations per second: PerformanceCounterType.RateOfCountsPerSecond32
                CounterCreationData opsPerSecond = new CounterCreationData();
                opsPerSecond.CounterName = "# operations / sec";
                opsPerSecond.CounterHelp = "Number of operations executed per second";
                opsPerSecond.CounterType = PerformanceCounterType.RateOfCountsPerSecond32;
                counters.Add(opsPerSecond);

                // create new category with the counters above
                PerformanceCounterCategory.Create(_category, "Sample category for Codeproject",PerformanceCounterCategoryType.Unknown, counters);
            }

            _OperationsPerSecond = new PerformanceCounter();
            _OperationsPerSecond.CategoryName = _category;
            _OperationsPerSecond.CounterName = "# operations / sec";
            _OperationsPerSecond.MachineName = ".";
            _OperationsPerSecond.ReadOnly = false;
        }

        public void Reset()
        {
            _OperationsPerSecond.RawValue = 0;
        }

        public void Increment()
        {
            _OperationsPerSecond.Increment();
        }
    }
}
