using SmsApi.DTOs;
using SmsApi.Entities;
using SmsApi.Repositories;

namespace SmsApi.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _repository;
    private readonly HttpClient _httpClient;

    public MessageService(IMessageRepository repository, HttpClient httpClient)
    {
        _repository = repository;
        _httpClient = httpClient;
    }

    public async Task<MessageStatusDto> SendMessageAsync(SendMessageDto messageDto)
    {
        if (string.IsNullOrWhiteSpace(messageDto.Text))
            throw new ArgumentException("Message text cannot be empty.");

        if (string.IsNullOrWhiteSpace(messageDto.RecipientNumber))
            throw new ArgumentException("Recipient number cannot be empty.");

        var message = new Message
        {
            RecipientNumber = messageDto.RecipientNumber,
            Text = messageDto.Text,
            Status = MessageStatus.Pending
        };

        await _repository.AddAsync(message);

        try
        {
            var response = await _httpClient.PostAsJsonAsync("", new { to = message.RecipientNumber, text = message.Text });

            if (!response.IsSuccessStatusCode)
            {
                message.Status = MessageStatus.Failed;
                await _repository.UpdateAsync(message);

                throw new BadHttpRequestException(
                    $"Failed to send SMS. Downstream service returned {response.StatusCode}",
                    StatusCodes.Status502BadGateway
                );
            }

            message.Status = MessageStatus.Sent;
            await _repository.UpdateAsync(message);

            return new MessageStatusDto(message.Id, message.Status, message.Text);
        }
        catch (HttpRequestException ex)
        {
            message.Status = MessageStatus.Failed;
            await _repository.UpdateAsync(message);

            throw new BadHttpRequestException("Unable to reach SMS provider.", StatusCodes.Status502BadGateway, ex);
        }
    }

    public async Task<MessageStatusDto> GetMessageStatusAsync(int id)
    {
        var message = await _repository.GetByIdAsync(id);
        return message is null
            ? null
            : new MessageStatusDto(message.Id, message.Status, message.Text);
    }
}
