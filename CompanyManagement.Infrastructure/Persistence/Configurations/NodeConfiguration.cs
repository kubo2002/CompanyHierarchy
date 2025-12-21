using CompanyManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyManagement.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Entity Framework Core configuration for the Node entity.
    /// Defines property constraints and relationships within the organizational hierarchy.
    /// </summary>
    public class NodeConfiguration : IEntityTypeConfiguration<Domain.Entities.Node>
    {
        /// <summary>
        /// Konfiguruje entitu Node pomocou Fluent API.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<Node> builder)
        {
            // Primarny kluc entity Node
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(n => n.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(n => n.Type)
                .IsRequired();

            /// <summary>
            /// Kofiguruje vztah odkazujuci na seba, ktori reprezentuje nadradeny uzol v hierarchii
            /// V pripade vymazania nadradeneho uzla sa nic nedeje (Restrict).
            /// </summary>
            builder.HasOne<Node>()
                .WithMany()
                .HasForeignKey(n => n.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            /// <summary>
            /// Konfiguruje volitelny vztah medzi uzlom a jeho veducim zamestnancom.
            /// Po odstraneni veduceho sa odkaz nastavi na null.
            /// </summary>
            builder
                .HasOne(n => n.Leader)
                .WithMany() // z Employee strany ziadna navigacia
                .HasForeignKey(n => n.LeaderEmployeeId)
                .OnDelete(DeleteBehavior.SetNull);


            /// <summary>
            /// Konfiguruje vztah typu N:M medzi uzlom a zamestnancami.
            /// Vyuziva tabulku DepartmentEmployees ako spojovaciu tabulku.
            /// </summary>
            builder
                .HasMany(n => n.Employees)
                .WithMany(e => e.MemberOfNodes)
                .UsingEntity(j => j.ToTable("DepartmentEmployees"));

        }
    }
}
