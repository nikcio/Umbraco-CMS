using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Umbraco.Cms.Core.Models.Entities;
using Umbraco.Cms.Persistence.EFCore.Models;

namespace Umbraco.Cms.Persistence.EFCore.Configurations
{
    internal class UmbracoCacheInstructionConfiguration : IEntityTypeConfiguration<UmbracoCacheInstruction>
    {
        public void Configure(EntityTypeBuilder<UmbracoCacheInstruction> builder)
        {
            builder.ToTable("umbracoCacheInstruction");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.InstructionCount)
                .HasDefaultValueSql("('1')")
                .HasColumnName("instructionCount");
            builder.Property(e => e.JsonInstruction).HasColumnName("jsonInstruction");
            builder.Property(e => e.Originated)
                .HasMaxLength(500)
                .HasColumnName("originated");
            builder.Property(e => e.UtcStamp)
                .HasColumnType("datetime")
                .HasColumnName("utcStamp");
        }
    }
}
