using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Monads.Extensions;
using NUnit.Framework;

namespace Monads.Tests.TestDoubles
{
    [TestFixture]
    public class TestService
    {
        public Either<EntityDto, string[]> AddOrUpdate(EntityDto dto)
        {
            var existing = Get(dto.RowGuid);
            return (existing.HasValue
                       ? Either.Right<EntityDto, string[]>(dto)
                       : Either.Left<EntityDto, string[]>(new[] {"The specified entity was not found"}))
                  .Chain(ConvertSubEntities)
                  .Chain(right => ConvertEntity(right.dto, right.subEntities))
                  .Chain(right => Either.Right<EntityDto, string[]>(dto));
        }

        private Either<Entity, string[]> ConvertEntity(EntityDto dto, ICollection<SubEntity> subEntities)
        {
            var entity = Entity.Create(dto.Balance, dto.Notes, subEntities.ToList());
            if (entity.IsLeft)
                return Either.Left<Entity, string[]>(entity.Left.Select(x => $"{x.PropertyName}: {string.Join(Environment.NewLine, x.ErrorMessages)}").ToArray());

            return Either.Right<Entity, string[]>(entity.Right);
        }

        public async Task<Either<EntityDto, string[]>> AddOrUpdateAsync(EntityDto dto)
        {
            var existing = await GetAsync(dto.RowGuid);
            return await (existing.HasValue
                             ? Either.Right<EntityDto, string[]>(dto)
                             : Either.Left<EntityDto, string[]>(new[] {"The specified entity was not found"}))
                        .ChainAsync(async right => ConvertSubEntities(right))
                        .ChainAsync(async right => ConvertEntity(right.dto, right.subEntities))
                        .ChainAsync(async right => Either.Right<EntityDto, string[]>(dto));
        }

        private Either<(EntityDto dto, SubEntity[] subEntities), string[]> ConvertSubEntities(EntityDto dto)
        {
            var subEntities = dto.SubEntities.Select(x => SubEntity.Create(x.Name, x.OrderDueDate, x.Employees)).ToList();
            var errors = subEntities.Where(x => x.IsLeft).ToList();
            if (errors.Any())
                return Either.Left<(EntityDto dto, SubEntity[] subEntities), string[]>(errors.SelectMany(x => x.Left).Select(x => $"{x.PropertyName}: {string.Join(Environment.NewLine, x.ErrorMessages)}").ToArray());

            return Either.Right<(EntityDto dto, SubEntity[] subEntities), string[]>((dto, subEntities.Select(x => x.Right).ToArray()));
        }

        private Maybe<Entity> Get(Guid rowGuid)
        {
            if (rowGuid == Guid.Empty)
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

        private async Task<Maybe<Entity>> GetAsync(Guid rowGuid)
        {
            if (rowGuid == Guid.Empty)
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

        [Test]
        public void SmokeTest()
        {
            var result = AddOrUpdate(new EntityDto());
        }

        [Test]
        public async Task SmokeTestAsync()
        {
            var result = await AddOrUpdateAsync(new EntityDto());
        }
    }
}