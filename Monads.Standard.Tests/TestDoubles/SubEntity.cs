using System;
using System.Collections.Generic;
using System.Linq;

namespace Monads.Tests.TestDoubles {
    public class SubEntity : EntityBase
    {
        public string Name { get; private set; }
        public DateTime OrderDueDate { get; private set; }
        public ICollection<string> Employees { get; private set; }

        private SubEntity() : base() { }

        private SubEntity(string name, DateTime orderDueDate, ICollection<string> employees)
        {
            Name = name;
            OrderDueDate = orderDueDate;
            Employees = employees;
        }

        public static Either<SubEntity, (string PropertyName, string[] ErrorMessages)[]> Create(string name, DateTime orderDueDate, ICollection<string> employees)
        {
            List<(string PropertyName, string[] ErrorMessages)> validationErrors = new List<(string PropertyName, string[] ErrorMessages)>();
            if (string.IsNullOrWhiteSpace(name))
                validationErrors.Add((nameof(Name), new[] { $"{nameof(Name)} is a required field." }));

            if (orderDueDate <= DateTime.UtcNow.Date)
                validationErrors.Add((nameof(OrderDueDate), new[] { $"{nameof(OrderDueDate)} is a required field." }));

            if (!employees.Any()) validationErrors.Add((nameof(Employees), new[] { "At least one (1) employee is required." }));

            if (validationErrors.Any())
                return Either.Left<SubEntity, (string PropertyName, string[] ErrorMessages)[]>(validationErrors.ToArray());

            var subEntity = new SubEntity(name, orderDueDate, employees);

            return Either.Right<SubEntity, (string PropertyName, string[] ErrorMessages)[]>(subEntity);
        }
    }
}