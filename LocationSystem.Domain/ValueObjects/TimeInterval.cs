using LocationSystem.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Domain.ValueObjects
{
    public class TimeInterval
    {
        private TimeInterval() { }
        public TimeInterval(DateTime start,DateTime end)
        {
            if (start > end)
                throw new BussinessRuleException("开始时间不能大于结束时间");
            Start = start;
            End = end;
        }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
