using System;
using System.Collections.Generic;

namespace Monads.Tests.TestDoubles {
    public class SubEntityDto : DtoBase
    {
        public string Name { get; set; }
        public DateTime OrderDueDate { get; set; }
        public ICollection<string> Employees { get; set; }
    }
}