using Microsoft.EntityFrameworkCore;

namespace Api_para_login.Dados
{
    public class BancoDados:DbContext
    {
        public DbSet<Data> login { get; set; }
        public BancoDados(DbContextOptions<BancoDados> options) : base(options) { }
    }
}
