using Microsoft.EntityFrameworkCore;
using MusicRecords.Api.Models;

namespace MusicRecords.Api.Repositories;

public sealed class MusicRecordsDbContext(DbContextOptions<MusicRecordsDbContext> options) : DbContext(options)
{
    public DbSet<MusicRecord> MusicRecords => Set<MusicRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var records = modelBuilder.Entity<MusicRecord>();
        records.ToTable("MusicRecords");
        records.Property(record => record.Title).HasMaxLength(200);
        records.Property(record => record.Artist).HasMaxLength(200);
        records.Property<string>("SearchTitle").HasMaxLength(200).IsRequired();
        records.Property<string>("SearchArtist").HasMaxLength(200).IsRequired();
    }
}
