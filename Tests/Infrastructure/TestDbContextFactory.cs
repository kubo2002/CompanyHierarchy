using CompanyManagement.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;


namespace Tests.Infrastructure
{
    public static class TestDbContextFactory
    {
        /// <summary>
        /// Vytvori a inicializuje databazovy kontext pre testovacie ucely.
        /// Pouziva in-memory SQLite databazu, ktora existuje len pocas zivotnosti otvoreneho spojenia.
        /// </summary>
        /// <returns>
        /// Inicializovany ManagementDbContext pripraveny na pouzitie v testoch.
        /// </returns>
        public static ManagementDbContext Create()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ManagementDbContext>()
                .UseSqlite(connection)
                .Options;

            /// <summary>
            /// Vytvori databazovu schemu podla aktualneho modelu.
            /// V testovacom prostredi sa nepouzivaju migracie.
            /// </summary>
            var context = new ManagementDbContext(options);


            context.Database.EnsureCreated();

            return context;
        }
    }
}
