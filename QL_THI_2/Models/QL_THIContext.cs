﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace QL_THI_2.Models
{
    public partial class QL_THIContext : DbContext
    {
        public QL_THIContext()
        {
        }

        public QL_THIContext(DbContextOptions<QL_THIContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CHINH_SUA_DIEM> CHINH_SUA_DIEMs { get; set; }
        public virtual DbSet<CHI_TIET_DIEM> CHI_TIET_DIEMs { get; set; }
        public virtual DbSet<HINH_THUC_THI> HINH_THUC_THIs { get; set; }
        public virtual DbSet<HOC_PHAN_THI> HOC_PHAN_THIs { get; set; }
        public virtual DbSet<MA_HOC_PHAN> MA_HOC_PHANs { get; set; }
        public virtual DbSet<NHOM_THI> NHOM_THIs { get; set; }
        public virtual DbSet<TAI_KHOAN> TAI_KHOANs { get; set; }
        public virtual DbSet<THONG_BAO> THONG_BAOs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-EHEN21F\\SQLEXPRESS;database=QL_THI;Trusted_Connection=True;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<CHINH_SUA_DIEM>(entity =>
            {
                entity.HasKey(e => new { e.ID_N, e.LANCHINHSUA_V });

                entity.ToTable("CHINH_SUA_DIEM");

                entity.Property(e => e.ID_N)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.THOIGIAN_V).HasColumnType("datetime");

                entity.HasOne(d => d.ID_NNavigation)
                    .WithMany(p => p.CHINH_SUA_DIEMs)
                    .HasForeignKey(d => d.ID_N)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CHINH_SUA_DIEM_NHOM_THI");
            });

            modelBuilder.Entity<CHI_TIET_DIEM>(entity =>
            {
                entity.HasKey(e => new { e.ID_N, e.MSSV_CTBT, e.SOCHINHSUA_CTBT })
                    .HasName("PK_CHI_TIET_BAI_THI")
                    .IsClustered(false);

                entity.ToTable("CHI_TIET_DIEM");

                entity.HasIndex(e => e.ID_N, "CO_FK");

                entity.Property(e => e.ID_N)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.MSSV_CTBT)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.DIEM_CTBT)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.ID_NNavigation)
                    .WithMany(p => p.CHI_TIET_DIEMs)
                    .HasForeignKey(d => d.ID_N)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CHI_TIET_CO_NHOM_THI");
            });

            modelBuilder.Entity<HINH_THUC_THI>(entity =>
            {
                entity.HasKey(e => e.ID_HT)
                    .IsClustered(false);

                entity.ToTable("HINH_THUC_THI");

                entity.Property(e => e.ID_HT).ValueGeneratedNever();

                entity.Property(e => e.TEN_HT).HasMaxLength(50);
            });

            modelBuilder.Entity<HOC_PHAN_THI>(entity =>
            {
                entity.HasKey(e => e.ID_HP)
                    .IsClustered(false);

                entity.ToTable("HOC_PHAN_THI");

                entity.HasIndex(e => e.ID_TK, "TAO_FK");

                entity.HasIndex(e => e.ID_MHP, "THUOC_VE_FK");

                entity.Property(e => e.ID_HP)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.DIEMTHANHPHAN_HP).HasMaxLength(2000);

                entity.Property(e => e.HANNOP_HP).HasColumnType("datetime");

                entity.Property(e => e.ID_TK)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.NAMHOCB_HP)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.NAMHOCK_HP)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.HasOne(d => d.ID_MHPNavigation)
                    .WithMany(p => p.HOC_PHAN_THIs)
                    .HasForeignKey(d => d.ID_MHP)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HOC_PHAN_THUOC_VE_MA_HOC_P");

                entity.HasOne(d => d.ID_TKNavigation)
                    .WithMany(p => p.HOC_PHAN_THIs)
                    .HasForeignKey(d => d.ID_TK)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HOC_PHAN_TAO_TAI_KHOA");
            });

            modelBuilder.Entity<MA_HOC_PHAN>(entity =>
            {
                entity.HasKey(e => e.ID_MHP)
                    .IsClustered(false);

                entity.ToTable("MA_HOC_PHAN");

                entity.Property(e => e.MA_MHP)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TEN_MHP).HasMaxLength(300);
            });

            modelBuilder.Entity<NHOM_THI>(entity =>
            {
                entity.HasKey(e => e.ID_N)
                    .IsClustered(false);

                entity.ToTable("NHOM_THI");

                entity.HasIndex(e => e.ID_HP, "CUA_FK");

                entity.HasIndex(e => e.ID_TK, "PHU_TRACH_FK");

                entity.HasIndex(e => e.ID_HT, "THUOC_FK");

                entity.Property(e => e.ID_N)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.ID_HP)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ID_TK)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.LINKELEARNING_N)
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.LINKEXCELDIEM_N)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.LINKPDFDE_N)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.LINKPDFDIEM_N)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.LINKZIPBAI_N)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.NGAYTHI_N).HasColumnType("datetime");

                entity.HasOne(d => d.ID_HPNavigation)
                    .WithMany(p => p.NHOM_THIs)
                    .HasForeignKey(d => d.ID_HP)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NHOM_THI_CUA_HOC_PHAN");

                entity.HasOne(d => d.ID_HTNavigation)
                    .WithMany(p => p.NHOM_THIs)
                    .HasForeignKey(d => d.ID_HT)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NHOM_THI_TO_CHUC_HINH_THU");

                entity.HasOne(d => d.ID_TKNavigation)
                    .WithMany(p => p.NHOM_THIs)
                    .HasForeignKey(d => d.ID_TK)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NHOM_THI_PHU_TRACH_TAI_KHOA");
            });

            modelBuilder.Entity<TAI_KHOAN>(entity =>
            {
                entity.HasKey(e => e.ID_TK)
                    .IsClustered(false);

                entity.ToTable("TAI_KHOAN");

                entity.Property(e => e.ID_TK)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ANHDAIDIEN_TK)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DN_TK)
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.EMAIL_TK).HasMaxLength(500);

                entity.Property(e => e.HOTEN_TK).HasMaxLength(100);

                entity.Property(e => e.LANHDCUOI_TK).HasColumnType("datetime");

                entity.Property(e => e.MK_TK).IsUnicode(false);

                entity.Property(e => e.NGAYTAO_TK).HasColumnType("datetime");
            });

            modelBuilder.Entity<THONG_BAO>(entity =>
            {
                entity.HasKey(e => e.ID_TB)
                    .IsClustered(false);

                entity.ToTable("THONG_BAO");

                entity.HasIndex(e => e.ID_TK, "DANG_FK");

                entity.Property(e => e.ID_TB)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.ID_TK)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.THOIGIAN_TB).HasColumnType("datetime");

                entity.HasOne(d => d.ID_TKNavigation)
                    .WithMany(p => p.THONG_BAOs)
                    .HasForeignKey(d => d.ID_TK)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_THONG_BA_DANG_TAI_KHOA");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
