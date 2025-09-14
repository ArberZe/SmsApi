namespace SmsApi.Entities;

public record SendMessageDto(string RecipientNumber, string Text);