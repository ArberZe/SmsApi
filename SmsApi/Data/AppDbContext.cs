using Microsoft.EntityFrameworkCore;
using SmsApi.Entities;

namespace SmsApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Message> Messages => Set<Message>();
}