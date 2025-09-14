using Microsoft.EntityFrameworkCore;
using SmsApi.Data;
using SmsApi.Entities;

namespace SmsApi.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _context;

    public MessageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Message> AddAsync(Message message)
    {
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<Message> GetByIdAsync(int id) =>
        await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);

    public async Task UpdateAsync(Message message)
    {
        _context.Messages.Update(message);
        await _context.SaveChangesAsync();
    }
}
