using ControleCadastro.Infra.Data.Contexto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ControleCadastro.Infra.Data.Procedure
{
    public class ProcedureService
    {
        private readonly ApplicationDbContext _context;

        public ProcedureService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task ExecutarProceduresAsync()
        {
            await ClienteProceduresAsync();
            await EnderecoProceduresAsync();
            await AutorizationProceduresAsync();
        }

        internal async Task ClienteProceduresAsync()
        {
            var procedures = new[]
        {
            new
            {
                Name = "InserirCliente",
                Script = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'InserirCliente') AND type = 'P')
                    BEGIN
                        EXEC('
                        CREATE PROCEDURE InserirCliente
                            @AutorizationId INT,
                            @Nome NVARCHAR(255),
                            @Email NVARCHAR(255),
                            @SenhaHash NVARCHAR(255)
                        AS
                        BEGIN
                            INSERT INTO Cliente (AutorizationId, Nome, Email, SenhaHash)
                            VALUES (@AutorizationId, @Nome, @Email, @SenhaHash);
                        END
                        ')
                    END
                "
            },
            new
            {
                Name = "AlterarSenhaClientePorId",
                Script = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'AlterarSenhaClientePorId') AND type = 'P')
                    BEGIN
                        EXEC('
                        CREATE PROCEDURE AlterarSenhaClientePorId
                            @Id INT,
                            @SenhaHash NVARCHAR(255)
                        AS
                        BEGIN
                            UPDATE Cliente
                            SET SenhaHash = @SenhaHash
                            WHERE Id = @Id;                          
                            
                        END
                        ')
                    END
                "
            },
            new
            {
                Name = "EditarClientePorId",
                Script = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'EditarClientePorId') AND type = 'P')
                    BEGIN
                        EXEC('
                        CREATE PROCEDURE EditarClientePorId
                            @Id INT,
                            @AutorizationId INT,
                            @Nome NVARCHAR(255),
                            @Email NVARCHAR(255),
                            @Logotipo VARBINARY(MAX)
                        AS
                        BEGIN
                            UPDATE Cliente
                            SET 
                                AutorizationId = @AutorizationId,
                                Nome = @Nome,
                                Email = @Email,
                                Logotipo = @Logotipo
                            WHERE Id = @Id;
                        END
                        ')
                    END
                "
            },
            new
                {
                Name = "AtualizarLogotipoPorId",
                Script = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'AtualizarLogotipoPorId') AND type = 'P')
                    BEGIN
                        EXEC('
                        CREATE PROCEDURE AtualizarLogotipoPorId
                            @Id INT,
                            @Logotipo VARBINARY(MAX)   -- Logotipo para ser atualizado
                        AS
                        BEGIN
                            UPDATE Cliente
                            SET 
                                Logotipo = @Logotipo   -- Atualiza o campo Logotipo
                            WHERE Id = @Id;  -- Onde o Id do cliente corresponde
                        END
                        ')
                    END
                "
            },
            new
            {
                Name = "ExcluirClientePorId",
                Script = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'ExcluirClientePorId') AND type = 'P')
                    BEGIN
                        EXEC('
                        CREATE PROCEDURE ExcluirClientePorId
                            @Id INT
                        AS
                        BEGIN    
                            DELETE FROM Endereco WHERE ClienteId = @Id;
                            DELETE FROM Cliente WHERE Id = @Id; 
                        END
                        ')
                    END
                "
            }
        };

            foreach (var procedure in procedures)
            {
                try
                {
                    await _context.Database.ExecuteSqlRawAsync(procedure.Script);
                    Console.WriteLine($"Stored Procedure {procedure.Name} criada com sucesso.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao criar a stored procedure {procedure.Name}: {ex.Message}");
                }
            }
        }

        internal async Task EnderecoProceduresAsync()
        {
            var procedures = new[]
        {
            new
            {
                Name = "InserirEndereco",
                Script = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'InserirEndereco') AND type = 'P')
                    BEGIN
                        EXEC('
                        CREATE PROCEDURE InserirEndereco
                            @ClienteId INT,
                            @Logadouro NVARCHAR(255),
                            @Numero NVARCHAR(15),
                            @Cep NVARCHAR(15),
                            @Complemento NVARCHAR(20),
                            @Cidade NVARCHAR(50),
                            @Estado NVARCHAR(2),
                            @IsPrincipal BIT
                        AS
                        BEGIN
                            INSERT INTO Endereco (ClienteId, Logadouro, Numero, Cep, Complemento, Cidade, Estado, IsPrincipal)
                            VALUES (@ClienteId, @Logadouro, @Numero, @Cep, @Complemento, @Cidade, @Estado, @IsPrincipal);
                        END
                        ')
                    END
                "
            },
            new
            {
                Name = "AtualizarEndereco",
                Script = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'AtualizarEndereco') AND type = 'P')
                    BEGIN
                        EXEC('
                        CREATE PROCEDURE AtualizarEndereco
                            @Id INT,
                            @ClienteId INT,
                            @Logadouro NVARCHAR(255),
                            @Numero NVARCHAR(15),
                            @Cep NVARCHAR(15),
                            @Complemento NVARCHAR(20),
                            @Cidade NVARCHAR(50),
                            @Estado NVARCHAR(2),
                            @IsPrincipal BIT
                        AS
                        BEGIN
                            UPDATE Endereco
                            SET 
                                ClienteId = @ClienteId,
                                Logadouro = @Logadouro,
                                Numero = @Numero,
                                Cep = @Cep,
                                Complemento = @Complemento,
                                Cidade = @Cidade,
                                Estado = @Estado,
                                IsPrincipal = @IsPrincipal
                            WHERE Id = @Id;
                        END
                        ')
                    END
                "
            },
            new
            {
                Name = "ExcluirEndereco",
                Script = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'ExcluirEndereco') AND type = 'P')
                    BEGIN
                        EXEC('
                        CREATE PROCEDURE ExcluirEndereco
                            @Id INT
                        AS
                        BEGIN
                            DELETE FROM Endereco
                            WHERE Id = @Id;
                        END
                        ')
                    END
                "
            },
            new
            {
                Name = "AtualizarEnderecoPrincipal",
                Script = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'AtualizarEnderecoPrincipal') AND type = 'P')
                    BEGIN
                        EXEC('
                        CREATE PROCEDURE AtualizarEnderecoPrincipal
                            @ClienteId INT,
                            @EnderecoId INT
                        AS
                        BEGIN
                            -- Atualiza todos os endereços do cliente para IsPrincipal = 0
                            UPDATE Endereco
                            SET IsPrincipal = 0
                            WHERE ClienteId = @ClienteId;
                            
                            -- Define o novo endereço como principal
                            UPDATE Endereco
                            SET IsPrincipal = 1
                            WHERE Id = @EnderecoId;
                        END
                        ')
                    END
                "
            }
        };

            // Executa os scripts para criar as procedures
            foreach (var procedure in procedures)
            {
                try
                {
                    await _context.Database.ExecuteSqlRawAsync(procedure.Script);
                    Console.WriteLine($"Stored Procedure {procedure.Name} criada com sucesso.");
                }
                catch (Exception ex)
                {
                    // Log ou tratamento de exceções
                    Console.WriteLine($"Erro ao criar a stored procedure {procedure.Name}: {ex.Message}");
                }
            }
        }

        internal async Task AutorizationProceduresAsync()
        {
            var procedures = new[]
    {
        new
        {
            Name = "InserirAutorization",
            Script = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'InserirAutorization') AND type = 'P')
                BEGIN
                    EXEC('
                    CREATE PROCEDURE InserirAutorization
                        @ClientId NVARCHAR(255),
                        @ClientSecret NVARCHAR(255),
                        @IsAdmin BIT
                    AS
                    BEGIN
                        INSERT INTO Autorization (ClientId, ClientSecret, IsAdmin)
                        VALUES (@ClientId, @ClientSecret, @IsAdmin);
                    END
                    ')
                END
            "
        },
        new
        {
            Name = "EditarAutorization",
            Script = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'EditarAutorization') AND type = 'P')
                BEGIN
                    EXEC('
                    CREATE PROCEDURE EditarAutorization
                        @Id INT,
                        @ClientId NVARCHAR(255),
                        @ClientSecret NVARCHAR(255),
                        @IsAdmin BIT
                    AS
                    BEGIN
                        UPDATE Autorization
                        SET 
                            ClientId = @ClientId,
                            ClientSecret = @ClientSecret,
                            IsAdmin = @IsAdmin
                        WHERE Id = @Id;
                    END
                    ')
                END
            "
        },
        new
        {
            Name = "ExcluirAutorization",
            Script = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'ExcluirAutorization') AND type = 'P')
                BEGIN
                    EXEC('
                    CREATE PROCEDURE ExcluirAutorization
                        @Id INT
                    AS
                    BEGIN
                        -- Excluir os endereços do cliente
                        DELETE FROM Endereco
                        WHERE ClienteId IN (SELECT Id FROM Cliente WHERE AutorizationId = @Id);
        
                        -- Excluir o cliente
                        DELETE FROM Cliente
                        WHERE AutorizationId = @Id;
        
                        -- Excluir a autorização
                        DELETE FROM Autorization
                        WHERE Id = @Id;
                    END
                    ')
                END
            "
        }
    };

            foreach (var procedure in procedures)
            {
                try
                {
                    await _context.Database.ExecuteSqlRawAsync(procedure.Script);
                    Console.WriteLine($"Stored Procedure {procedure.Name} criada com sucesso.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao criar a stored procedure {procedure.Name}: {ex.Message}");
                }
            }
        }
    }
}
