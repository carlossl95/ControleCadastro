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
    internal class EnderecoConfiguration : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            {
                builder.HasKey(x => x.Id);

                builder.Property(x => x.Logadouro)
                    .IsRequired()
                    .HasMaxLength(255);

                builder.Property(x => x.Numero)
                    .HasMaxLength(15);

                builder.Property(x => x.Cep)
                    .IsRequired()
                    .HasMaxLength(15);

                builder.Property(x => x.Complemento)
                    .HasMaxLength(20);

                builder.Property(x => x.Cidade)
                    .IsRequired()
                    .HasMaxLength(50);

                builder.Property(x => x.Estado)
                    .IsRequired()
                    .HasMaxLength(2);

                builder.Property(x => x.IsPrincipal)
                    .IsRequired();

                builder.HasOne(x => x.Cliente)
                    .WithMany(c => c.Enderecos)
                    .HasForeignKey(x => x.ClienteId)
                    .OnDelete(DeleteBehavior.Cascade);  
            }
        }
    }
}
