﻿using System.ComponentModel.DataAnnotations;
using Domain.Primitives;

namespace Domain.Entities;
public class StoredData : BaseEntity
{
    public required string Content { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
