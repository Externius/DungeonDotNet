﻿using System;

namespace RDMG.Core.Abstractions.Services.Models;

public class EditModel
{
    public int Id { get; set; }
    public byte[] Timestamp { get; set; }
    public string CreatedBy { get; set; }

    public DateTime Created { get; set; }

    public string LastModifiedBy { get; set; }

    public DateTime LastModified { get; set; }
}