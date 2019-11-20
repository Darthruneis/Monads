using System;
using System.Linq;
using Bogus;

namespace Monads.Tests {
    public class TestService
    {
        public Either<EntityDto, string[]> AddOrUpdate(EntityDto dto)
        {
            var existing = Get(dto.RowGuid);
            return existing.HasValue
                ? Either.Right<EntityDto, string[]>(dto)
                : Either.Left<EntityDto, string[]>(new[] {"The specified entity was not found"})
                        .Chain(right =>
                               {
                                   var subEntities = right.SubEntities.Select(x => SubEntity.Create(x.Name, x.OrderDueDate, x.Employees)).ToList();
                                   var errors = subEntities.Where(x => x.IsLeft).ToList();
                                   if (errors.Any())
                                       return Either.Left<(EntityDto dto, SubEntity[] subEntities), string[]>(errors.SelectMany(x => x.Left).Select(x => $"{x.PropertyName}: {string.Join(Environment.NewLine, x.ErrorMessages)}").ToArray());

                                   return Either.Right<(EntityDto dto, SubEntity[] subEntities), string[]>((right, subEntities.Select(x => x.Right).ToArray()));
                               })
                        .Chain(right =>
                               {
                                   var entity = Entity.Create(right.dto.Balance, right.dto.Notes, right.subEntities.ToList());
                                   if (entity.IsLeft)
                                       return Either.Left<EntityDto, string[]>(entity.Left.Select(x => $"{x.PropertyName}: {string.Join(Environment.NewLine, x.ErrorMessages)}").ToArray());

                                   return Either.Right<EntityDto, string[]>(right.dto);
                               });
        }

        private Maybe<Entity> Get(Guid rowGuid)
        {
            if(rowGuid == Guid.Empty)
                return Maybe<Entity>.Empty();

            Randomizer.Seed = new Random(8675309);

            var subEntities = new Faker<SubEntity>()
               .RulesForEntityBase()
                             .RuleFor(x => x.Name, x => x.Random.String())
                             .RuleFor(x => x.OrderDueDate, x => DateTime.UtcNow.Date.AddDays(x.Random.Double(2, 366)))
                             .RuleFor(x => x.Employees, x => x.Random.WordsArray(3, 500).ToList());

            var entity = new Faker<Entity>()
                        .RulesForEntityBase()
                        .RuleFor(x => x.Balance, x => x.Random.Decimal(.01m, 1000m))
                        .RuleFor(x => x.Notes, x => x.Random.String())
                        .RuleFor(x => x.SubEntities, x => subEntities.Generate(x.Random.Int()));

            return entity.Generate();
        }
    }
}