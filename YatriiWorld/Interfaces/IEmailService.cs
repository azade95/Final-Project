﻿namespace YatriiWorld.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string email, string subject, string body, bool isHtml = false);
    }
}
