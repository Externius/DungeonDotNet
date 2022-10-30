using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace RDMG.Infrastructure.Extensions;

public static class ModelBuilderExtensions
{
    public static void UseEnumStringConverter(this ModelBuilder modelBuilder)
    {
        var properties = modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties())
            .Where(p => (Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType).IsEnum);

        foreach (var property in properties)
            property.SetProviderClrType(typeof(string));
    }
}