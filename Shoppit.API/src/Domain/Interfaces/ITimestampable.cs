using System;

namespace Domain.Interfaces
{
    public interface ITimestampable
    {
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset UpdatedAt { get; set; }
    }
}