using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kavenegar;
using System.Net.Mail;
using System.Web;
using System.Net;
using DigiStore.Core.Interfaces;
using DigiStore.Core.Services;
using DigiStore.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DigiStore.Core.Classes
{
    public class MessageSender
    {
        private readonly IAdmin _admin;

        public MessageSender(IAdmin admin)
        {
            _admin = admin;
        }

        public void SMS(string to, string body)
        {
            var setting = _admin.GetSetting();
            var sender = setting.SmsSender;
            var receptor = to;
            var message = body;
            var api = new Kavenegar.KavenegarApi(setting.SmsApi);
            api.Send(sender, receptor, message);
        }

        public void Email(string to, string subject, string body)
        {
            Setting setting = _admin.GetSetting();
            using(var message = new MailMessage())
            {
                message.To.Add(new MailAddress(to, "دیجی استور"));
                message.From = new MailAddress(setting.MailAddress, "دیجی استور");
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                using(var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.UseDefaultCredentials = false;
                    client.Port = 587;
                    client.Credentials = new NetworkCredential(setting.MailAddress, setting.MailPassword);
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
        }
    }
}
