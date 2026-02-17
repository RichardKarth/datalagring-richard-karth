using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Application.CourseInstances.PersistanceModels
{
    public class CourseInstance
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int TeacherId { get; set; }

        public DateOnly StartDateUtc { get; set; }
        public DateOnly EndDateUtc { get; set; }
        public string Location { get; set; } = string.Empty;
        public int Capacity { get; set; }

        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
