using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Utilities.Common
{
    public class PageResult<T>
    {
        public List<T> Data { get; set; }
        public int Total { get; set; }
    }
}
