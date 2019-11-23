using System.Collections.Generic;

namespace Monads.Tests.TestDoubles {
    public class EntityDto : DtoBase
    {
        public ICollection<SubEntityDto> SubEntities { get; set; }
        public long Id { get; set; }
        public decimal Balance { get; set; }
        public string Notes { get; set; }
    }
}