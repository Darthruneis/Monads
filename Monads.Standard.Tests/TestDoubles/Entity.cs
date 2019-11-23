using System.Collections.Generic;
using System.Linq;

namespace Monads.Tests.TestDoubles {
    public class Entity : EntityBase
    {
        public ICollection<SubEntity> SubEntities { get; private set; }

        public decimal Balance { get; private set; }
        public string Notes { get; private set; }

        private Entity() : base()
        {
            SubEntities = new List<SubEntity>();
        }

        private Entity(decimal balance, string notes) : this()
        {
            Balance = balance;
            Notes = notes;
        }

        public static Either<Entity, (string PropertyName, string[] ErrorMessages)[]> Create(decimal balance, string notes, ICollection<SubEntity> ownedEntities)
        {
            List<(string PropertyName, string[] ErrorMessages)> validationErrors = new List<(string PropertyName, string[] ErrorMessages)>();
            if (string.IsNullOrWhiteSpace(notes))
                validationErrors.Add((nameof(Notes), new[] { $"{nameof(Notes)} is a required field." }));

            if (balance == decimal.Zero)
                validationErrors.Add((nameof(Balance), new[] { $"{nameof(Balance)} is a required field." }));

            if (validationErrors.Any())
                return Either.Left<Entity, (string PropertyName, string[] ErrorMessages)[]>(validationErrors.ToArray());

            var entity = new Entity(balance, notes);
            foreach (var ownedEntity in ownedEntities)
                entity.SubEntities.Add(ownedEntity);

            return Either.Right<Entity, (string PropertyName, string[] ErrorMessages)[]>(entity);
        }
    }
}