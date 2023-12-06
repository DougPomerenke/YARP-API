using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RetirementPlanning.DbMonteCarloSimulations;

public partial class MonteCarloSimulationsContext : DbContext
{
    public MonteCarloSimulationsContext()
    {
    }

    public MonteCarloSimulationsContext(DbContextOptions<MonteCarloSimulationsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountInfo> AccountInfos { get; set; }

    public virtual DbSet<HistoricalDatum> HistoricalData { get; set; }

    public virtual DbSet<MonteCarloType> MonteCarloTypes { get; set; }

    public virtual DbSet<RetirementExpense> RetirementExpenses { get; set; }

    public virtual DbSet<RetirementExpenseType> RetirementExpenseTypes { get; set; }

    public virtual DbSet<Simulation> Simulations { get; set; }

    public virtual DbSet<SocialSecurity> SocialSecurities { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-ETA94LG;Database=MonteCarloSimulations;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountInfo>(entity =>
        {
            entity.ToTable("AccountInfo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StartingBalance).HasColumnType("money");

            entity.HasOne(d => d.User).WithMany(p => p.AccountInfos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountInfo_User");
        });

        modelBuilder.Entity<HistoricalDatum>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<MonteCarloType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Scenario");

            entity.ToTable("MonteCarloType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RetirementExpense>(entity =>
        {
            entity.ToTable("RetirementExpense");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.MonthlyAmount).HasColumnType("money");

            entity.HasOne(d => d.RetirementExpenseType).WithMany(p => p.RetirementExpenses)
                .HasForeignKey(d => d.RetirementExpenseTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RetirementExpense_RetirementExpenseType");
        });

        modelBuilder.Entity<RetirementExpenseType>(entity =>
        {
            entity.ToTable("RetirementExpenseType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Simulation>(entity =>
        {
            entity.ToTable("Simulation");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AnnualContributionAmount).HasColumnType("money");
            entity.Property(e => e.AnnualContributionEndingYear).HasColumnType("money");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SocialSecurityMonthlyPayout).HasColumnType("money");
            entity.Property(e => e.TargetIncome).HasColumnType("money");

            entity.HasOne(d => d.AccountInfo).WithMany(p => p.Simulations)
                .HasForeignKey(d => d.AccountInfoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Simulation_AccountInfo");

            entity.HasOne(d => d.MonteCarloType).WithMany(p => p.Simulations)
                .HasForeignKey(d => d.MonteCarloTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Simulation_MonteCarloType");
        });

        modelBuilder.Entity<SocialSecurity>(entity =>
        {
            entity.ToTable("SocialSecurity");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.MonthlyPayout).HasColumnType("money");

            entity.HasOne(d => d.User).WithMany(p => p.SocialSecurities)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SocialSecurity_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
