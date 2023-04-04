using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TatBlog.Data.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.UrlSlug)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.ShowOnMenu)
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}
