using SmsApi.DTOs;
using SmsApi.Entities;

namespace SmsApi.Services;

public interface IMessageService
{
    Task<MessageStatusDto> SendMessageAsync(SendMessageDto messageDto);
    Task<MessageStatusDto> GetMessageStatusAsync(int id);
}
