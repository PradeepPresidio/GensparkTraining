using BankApp.Models;
using Microsoft.EntityFrameworkCore;
namespace BankApp.Contexts
{
	public class BankAppContext : dbContext
	{
		public BankAppContext(DbContextOptions options) : base(options) {
		
		}
		public DbSet<User> Users { get; set; }
		public DbSet<TransactionLog> TransactionLogs { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().HasKey(user=>user.Id);
			modelBuilder.Entity<TransactionLog>()
				.HasOne(t => t.Sender)
				.WithMany(t => t.SentTransactions)
				.HasForeignKey(t => t.SenderId)
				.HasConstraintName("FK_TransactionLogs_Sender")
				.OnDelete(DeleteBehaviour.Restrict);

			modelBuilder.Entity<TransactionLog>()
				.HasOne(t => t.Receiver)
				.WithMany(t => t.ReceivedTransactions)
				.HasForeignKey(t => t.ReceiverId)
				.HasConstraintName("FK_TransactionLogs_Receiver")
				.OnDelete(DeleteBehaviour.Restrict);
		}
	}
}