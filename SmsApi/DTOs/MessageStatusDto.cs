using SmsApi.Entities;

namespace SmsApi.DTOs;

public record MessageStatusDto(int Id, MessageStatus Status, string MessageContent);