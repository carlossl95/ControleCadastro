using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleCadastro.Domain.Entities;

namespace ControleCadastro.Infra.Data.EntitieConfiguration
{
    internal class AutorizationConfiguration : IEntityTypeConfiguration<Autorization>
    {
        public void Configure(EntityTypeBuilder<Autorization> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.ClientId)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.ClientSecret)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.IsAdmin)
                .IsRequired();
        }
    }
}
