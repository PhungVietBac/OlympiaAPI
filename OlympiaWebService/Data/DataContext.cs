using Microsoft.EntityFrameworkCore;
using OlympiaWebService.Models;
using System.Data;

namespace OlympiaWebService.Data {
    public class DataContext : DbContext {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {
            
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<GroupQuestion> GroupQuestions { get; set; }
        public DbSet<Answer> Answers { get; set; } 

        // Configure primary and foreign key
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            #region Matches
            modelBuilder.Entity<Match>()
                .HasKey(m => new { m.IDPlayer, m.IDRoom, m.Time });
            modelBuilder.Entity<Match>()
                .HasOne(r => r.Room)
                .WithMany(m => m.Matches)
                .HasForeignKey(r => r.IDRoom);
            modelBuilder.Entity<Match>()
                .HasOne(p => p.Player)
                .WithMany(m => m.Matches)
                .HasForeignKey(p => p.IDPlayer);
            modelBuilder.Entity<Match>()
                .ToTable("Matches", ins => ins.HasTrigger("TRG_INS_Matches"));
            modelBuilder.Entity<Match>()
                .ToTable("Matches", del => del.HasTrigger("TRG_DEL_Matches"));
            #endregion

            #region Friends
            modelBuilder.Entity<Friend>()
                .HasKey(f => new { f.IDSelf, f.IDFriend });
            modelBuilder.Entity<Friend>()
                .HasOne(p => p.Player)
                .WithMany(f => f.Friends)
                .HasForeignKey(p => p.IDSelf);
            modelBuilder.Entity<Friend>()
                .HasOne(p => p.FriendPlayer)
                .WithMany(f => f.FriendOfs)
                .HasForeignKey(p => p.IDFriend);
            #endregion

            #region Players
            modelBuilder.Entity<Player>()
                .HasKey(p => p.IDPlayer);
            #endregion

            #region Rooms
            modelBuilder.Entity<Room>()
                .HasKey(r => r.IDRoom);
            #endregion

            #region Ratings
            modelBuilder.Entity<Rating>()
                .HasKey(r => new { r.IDPlayer, r.Time });
            modelBuilder.Entity<Rating>()
                .HasOne(p => p.Player)
                .WithMany(r => r.Ratings)
                .HasForeignKey(p => p.IDPlayer);
            #endregion

            #region Questions
            modelBuilder.Entity<Question>()
                .HasKey(q => q.IDQuestion);
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Answer)
                .WithMany(a => a.Questions)
                .HasForeignKey(q => q.IDAnswer);
            #endregion

            #region GroupQuestions
            modelBuilder.Entity<GroupQuestion>()
                .HasKey(g => new { g.IDQuestion, g.IDGroup });
            modelBuilder.Entity<GroupQuestion>()
                .HasOne(q => q.Main)
                .WithMany(g => g.MemberQuestions)
                .HasForeignKey(q => q.IDQuestion);
            modelBuilder.Entity<GroupQuestion>()
                .HasOne(q => q.Member)
                .WithMany(g => g.QuestionOfs)
                .HasForeignKey(q => q.IDGroup);
            #endregion

            #region Answers
            modelBuilder.Entity<Answer>()
                .HasKey(a => a.IDAnswer);
            #endregion
        }

    }
}
