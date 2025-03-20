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

        private string GerarSenhaHash(string senha)
        {
            var salt = GerarSalt();

            // Configurações do Argon2
            using (var hasher = new Argon2id(Encoding.UTF8.GetBytes(senha)))
            {
                hasher.Salt = salt;
                hasher.DegreeOfParallelism = 8;
                hasher.MemorySize = 1024 * 64;
                hasher.Iterations = 4;

                var hash = hasher.GetBytes(32); 
                // Combina o salt com o hash gerado
                var resultado = new byte[salt.Length + hash.Length];
                Buffer.BlockCopy(salt, 0, resultado, 0, salt.Length);
                Buffer.BlockCopy(hash, 0, resultado, salt.Length, hash.Length);

                return Convert.ToBase64String(resultado);
            }
        }

        public bool ValidarSenha(string senhaInformada)
        {
            var hashComSalt = Convert.FromBase64String(SenhaHash);

            var salt = new byte[16];
            Buffer.BlockCopy(hashComSalt, 0, salt, 0, salt.Length);

            var hashArmazenado = new byte[hashComSalt.Length - salt.Length];
            Buffer.BlockCopy(hashComSalt, salt.Length, hashArmazenado, 0, hashArmazenado.Length);

            using (var hasher = new Argon2id(Encoding.UTF8.GetBytes(senhaInformada)))
            {
                hasher.Salt = salt;
                hasher.DegreeOfParallelism = 8;
                hasher.MemorySize = 1024 * 64;
                hasher.Iterations = 4;

                var hashInformado = hasher.GetBytes(32);

                for (int i = 0; i < hashArmazenado.Length; i++)
                {
                    if (hashArmazenado[i] != hashInformado[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private static byte[] GerarSalt()
        {
            var salt = new byte[16];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
