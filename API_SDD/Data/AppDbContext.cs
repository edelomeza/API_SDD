using Microsoft.EntityFrameworkCore;

namespace ApiSDD.Data;

/// <summary>
/// DbContext base nv-01: registra SQL externo con retry + timeout 60s.
/// Sin entidades aún (llegan con nv-02); el modelo vacío es válido y verificable.
/// </summary>
public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}
