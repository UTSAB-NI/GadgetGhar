﻿

using Microsoft.AspNetCore.Identity.UI.Services;

namespace Eccommerce.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //to send email
            return Task.CompletedTask;
        }
    }
}
