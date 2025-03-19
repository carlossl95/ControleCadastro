using ControleCadastro.Domain.Exception;
using Konscious.Security.Cryptography;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ControleCadastro.Domain.Entities
{
    public class Cliente
    { 
        public int Id { get; private set; }
        public int AutorizationId { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string SenhaHash { get; set; }
        public byte[]? Logotipo { get; set; }
        public virtual ICollection<Endereco> Enderecos { get; set; }
        public virtual Autorization Autorization { get; set; }

        public Cliente()
        {
        }

        public Cliente(int autorizationId, string nome, string email, string senhaHash)
        {
            AutorizationId = autorizationId;
            Nome = nome;
            Email = email;
            SenhaHash = senhaHash;
        }

        public Cliente CriarUsusario(int autorizationId, string nome, string email, string senhaHash)
        {
            AutorizationId = autorizationId;
            Nome = nome;
            Email = email;
            SenhaHash = GerarSenhaHash(senhaHash);
            return this;
        }

        public string AtualizaSenha(string senhaHash)
        {
            return SenhaHash = GerarSenhaHash(senhaHash);
        }

        public string Senha
        {
            set => SenhaHash = GerarSenhaHash(value);
        }

        // Método para gerar o hash da senha usando Argon2
        private string GerarSenhaHash(string senha)
        {
            // Gerar um salt aleatório
            var salt = GerarSalt();

            // Configurações do Argon2
            using (var hasher = new Argon2id(Encoding.UTF8.GetBytes(senha)))
            {
                // Configurações do Argon2
                hasher.Salt = salt; // Definindo o salt
                hasher.DegreeOfParallelism = 8; // A quantidade de threads
                hasher.MemorySize = 1024 * 64; // 64MB de memória
                hasher.Iterations = 4; // Número de iterações

                // Gerar o hash
                var hash = hasher.GetBytes(32);  // 32 bytes é o tamanho do hash gerado

                // Combina o salt com o hash gerado
                var resultado = new byte[salt.Length + hash.Length];
                Buffer.BlockCopy(salt, 0, resultado, 0, salt.Length);
                Buffer.BlockCopy(hash, 0, resultado, salt.Length, hash.Length);

                return Convert.ToBase64String(resultado); // Converte para string Base64
            }
        }

        // Método para validar a senha informada
        public bool ValidarSenha(string senhaInformada)
        {
            // Decodifica o hash armazenado
            var hashComSalt = Convert.FromBase64String(SenhaHash);

            // O salt está no início do hash
            var salt = new byte[16]; // Salt de 16 bytes
            Buffer.BlockCopy(hashComSalt, 0, salt, 0, salt.Length);

            // O restante é o hash
            var hashArmazenado = new byte[hashComSalt.Length - salt.Length];
            Buffer.BlockCopy(hashComSalt, salt.Length, hashArmazenado, 0, hashArmazenado.Length);

            // Recalcula o hash da senha informada usando o mesmo salt
            using (var hasher = new Argon2id(Encoding.UTF8.GetBytes(senhaInformada)))
            {
                hasher.Salt = salt;
                hasher.DegreeOfParallelism = 8;
                hasher.MemorySize = 1024 * 64;
                hasher.Iterations = 4;

                var hashInformado = hasher.GetBytes(32);

                // Compara o hash informado com o armazenado
                for (int i = 0; i < hashArmazenado.Length; i++)
                {
                    if (hashArmazenado[i] != hashInformado[i])
                    {
                        return false; // Senha incorreta
                    }
                }
                return true; // Senha correta
            }
        }

        // Método para gerar salt aleatório
        private static byte[] GerarSalt()
        {
            var salt = new byte[16]; // 16 bytes de salt
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
