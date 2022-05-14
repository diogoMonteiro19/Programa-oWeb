using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TP.Models;

namespace TP.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
       
        public DbSet<Imovel> Imoveis { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<AvalicaoCliente> AvalicoesClientes { get; set; }
        public DbSet<Utilizador> Utilizadores { get; set; }
    }
}
