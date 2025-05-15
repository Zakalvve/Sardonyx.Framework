using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sardonyx.Framework.Core.CQRS.Domain
{
    internal sealed class InternalCommandTypeConfiguration : IEntityTypeConfiguration<InternalCommand>
    {
        public void Configure(EntityTypeBuilder<InternalCommand> entity)
        {
            entity.ToTable("InternalCommands");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Id)
                .HasColumnType("uniqueidentifier")
                .HasColumnName("id")
                .HasConversion<Guid>()
                .IsRequired();

            entity.Property(e => e.DateAdded)
                .HasColumnType("datetime")
                .HasColumnName("dateAdded")
                .HasDefaultValueSql("getdate()")
                .IsRequired();

            entity.Property(e => e.IsProcessed)
                .HasColumnType("bit")
                .HasColumnName("isProcessed")
                .HasDefaultValue(false)
                .IsRequired();

            entity.Property(e => e.DateProcessed)
                .HasColumnType("datetime")
                .HasColumnName("dateProcessed")
                .HasDefaultValueSql("getdate()");

            entity.Property(e => e.DateScheduled)
                .HasColumnType("datetime")
                .HasColumnName("dateScheduled")
                .IsRequired(false);

            entity.Property(e => e.MaxRetries)
                .HasColumnName("maxRetries")
                .HasColumnType("int")
                .HasDefaultValue(3)
                .IsRequired();

            entity.Property(e => e.RetryIntervalSeconds)
                .HasColumnName("retryIntervalSeconds")
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            entity.Property(e => e.Tries)
                .HasColumnName("tries")
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            entity.Property(e => e.CommandType)
                .HasColumnName("commandType")
                .IsRequired();

            entity.Property(e => e.CommandData)
                .HasColumnName("commandData")
                .IsRequired();

            entity.Property(e => e.Message)
                .HasColumnName("message")
                .IsRequired(false);
        }
    }
}
