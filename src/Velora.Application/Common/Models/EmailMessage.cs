namespace Velora.Application.Common.Models;

public sealed record EmailMessage(string To, string Subject, string Body);
