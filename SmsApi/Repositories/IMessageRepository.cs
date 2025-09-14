using SmsApi.DTOs;
using SmsApi.Entities;

namespace SmsApi.Repositories;

public interface IMessageRepository
{
    Task<Message> AddAsync(Message message);
    Task<Message> GetByIdAsync(int id);
    Task UpdateAsync(Message message);
}
