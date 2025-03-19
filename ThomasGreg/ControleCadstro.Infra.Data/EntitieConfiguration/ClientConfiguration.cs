using ControleCadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleCadastro.Infra.Data.EntitieConfiguration
{
    internal class ClientConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            
            builder.HasKey(x => x.Id);
            
            builder.HasOne(x => x.Autorization)
                   .WithMany()
                   .HasForeignKey(x => x.AutorizationId)
                   .IsRequired();
                        
            builder.Property(x => x.Nome)
                   .HasMaxLength(255)  
                   .IsRequired();
            
            builder.Property(x => x.Email)
                   .HasMaxLength(255)
                   .IsRequired();
            
            builder.HasIndex(x => x.Email)
                   .IsUnique();
            
            builder.Property(x => x.Logotipo)
                   .HasColumnType("VARBINARY(MAX)");
        }
    }
}
