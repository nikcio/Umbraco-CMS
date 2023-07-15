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
    internal class UmbracoDomainConfiguration : IEntityTypeConfiguration<UmbracoDomain>
    {
        public void Configure(EntityTypeBuilder<UmbracoDomain> builder)
        {
            builder.ToTable("umbracoDomain");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.DomainDefaultLanguage).HasColumnName("domainDefaultLanguage");
            builder.Property(e => e.DomainName)
                .HasMaxLength(255)
                .HasColumnName("domainName");
            builder.Property(e => e.DomainRootStructureId).HasColumnName("domainRootStructureID");
            builder.Property(e => e.SortOrder).HasColumnName("sortOrder");

            builder.HasOne(d => d.DomainRootStructure).WithMany(p => p.UmbracoDomains)
                .HasForeignKey(d => d.DomainRootStructureId)
                .HasConstraintName("FK_umbracoDomain_umbracoNode_id");
        }
    }
}
