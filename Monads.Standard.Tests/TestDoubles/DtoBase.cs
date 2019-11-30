using System;

namespace Monads.Tests.TestDoubles {
    public abstract class DtoBase
    {
        public Guid RowGuid { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? ModifiedUtc { get; set; }
    }
}