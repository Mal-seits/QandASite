using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace QASite.data
{
    public class QADbContext : DbContext
    {
        private readonly string _connectionString;

        public QADbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<QuestionsTags> QuestionsTags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Likes> Likes { get; set; }
        public DbSet<Answer> Answers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuestionsTags>().HasKey(qt => new { qt.QuestionId, qt.TagId });
            modelBuilder.Entity<QuestionsTags>()
                .HasOne(qt => qt.Question)
                .WithMany(q => q.QuestionsTags)
                .HasForeignKey(qt => qt.QuestionId);
            modelBuilder.Entity<QuestionsTags>()
                  .HasOne(qt => qt.Tag)
                  .WithMany(t => t.QuestionsTags)
                  .HasForeignKey(qt => qt.TagId);
            modelBuilder.Entity<Likes>().HasKey(l => new { l.QuestionId, l.UserId });
            modelBuilder.Entity<Likes>()
                .HasOne(l => l.Question)
                .WithMany(q => q.Likes)
                .HasForeignKey(l => l.QuestionId);
            modelBuilder.Entity<Likes>()
              .HasOne(l => l.User)
              .WithMany(u => u.Likes)
              .HasForeignKey(l => l.UserId);
        }
    }
}
