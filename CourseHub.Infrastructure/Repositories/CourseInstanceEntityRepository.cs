using CourseHub.Application.Abstractions.Persistence.Repositories;
using CourseHub.Application.CourseInstances.PersistanceModels;
using CourseHub.Infrastructure.Data;
using CourseHub.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Infrastructure.Repositories
{
    public class CourseInstanceEntityRepository(CHDbContext context) : RepositoryBase<CourseInstanceEntity, int, CourseInstance>(context), ICourseInstanceRepository
    {
        public override async Task AddAsync(CourseInstance model, CancellationToken ct = default)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            var entity = new CourseInstanceEntity
            {
                CourseId = model.CourseId,
                TeacherId = model.TeacherId,
                StartDateUtc = model.StartDateUtc,
                EndDateUtc = model.EndDateUtc,
                Location = model.Location,
                Capacity = model.Capacity
            };

            await Set.AddAsync(entity, ct);
        }

        public override CourseInstance ToPersistanceModel(CourseInstanceEntity entity) => new()
        {
            Id = entity.Id,
            CourseId = entity.CourseId,
            TeacherId = entity.TeacherId,
            StartDateUtc = entity.StartDateUtc,
            EndDateUtc = entity.EndDateUtc,
            Location = entity.Location,
            Capacity = entity.Capacity
        };

        public override async Task UpdateAsync(CourseInstance model, CancellationToken ct = default)
        {
            var entity = await Set.SingleOrDefaultAsync(x => x.Id == model.Id, ct)
                ?? throw new ArgumentException($"CourseInstance with id {model.Id} not found");

            entity.CourseId = model.CourseId;
            entity.TeacherId = model.TeacherId;
            entity.StartDateUtc = model.StartDateUtc;
            entity.EndDateUtc = model.EndDateUtc;
            entity.Location = model.Location.Trim();
            entity.Capacity = model.Capacity;
        }
    }
}
