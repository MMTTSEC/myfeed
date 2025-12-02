using System;

namespace MyFeed.Domain.Entities;

public abstract class Entity
{
    public int Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }

    protected Entity()
    {
        CreatedAt = DateTime.UtcNow;
    }

    protected Entity(int id) : this()
    {
        Id = id;
    }
}

