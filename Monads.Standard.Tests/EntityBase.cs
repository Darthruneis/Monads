using System;
using Bogus;

namespace Monads.Tests {
    public abstract class EntityBase
    {
        public long Id { get; protected set; }
        public Guid RowGuid { get; protected set; }
        public DateTime CreatedUtc { get; protected set; }
        public DateTime? ModifiedUtc { get; protected set; }

        protected EntityBase()
        {
            RowGuid = Guid.NewGuid();
            CreatedUtc = DateTime.UtcNow;
        }
    }

    public static class FakerExtensions
    {
        public static Faker<TEntity> RulesForEntityBase <TEntity>(this Faker<TEntity> faker)
            where TEntity : EntityBase
        {
            return faker.RuleFor(x => x.RowGuid, x => x.Random.Guid())
                        .RuleFor(x => x.Id, x => x.Random.Long());
        }
        
        public static Faker<TEntity> RulesForDtoBase<TEntity>(this Faker<TEntity> faker)
            where TEntity : DtoBase
        {
            return faker.RuleFor(x => x.RowGuid, x => x.Random.Guid());
        }
    }
}