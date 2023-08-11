using System.Net.Mail;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Security.Policy;
using Microsoft.AspNetCore.Mvc;
using SIERRA_Server.Models.EFModels;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Mvc.Routing;

namespace SIERRA_Server.Models.Infra
{
	public class EmailHelper
	{
		//private string senderEmail = "g01.webapp@gmail.com"; // 寄件者
		private readonly IConfiguration _config;
		//private readonly IUrlHelper _urlHelper;
		public EmailHelper(IConfiguration config)//, IUrlHelper urlHelper)
		{
			_config = config;
			//_urlHelper = urlHelper;
		}
		public void SendForgotPasswordEmail(string email, string url, string name)
		{
			var to = email;
			var subject = "[Sierra重設密碼通知信]";
			var body = $@"Hi {name},
<br />
請點擊此連結 [<a href='{url}' target='_blank'>我要重設密碼</a>], 以進行重設密碼, 如果您沒有提出申請, 請忽略本信, 謝謝"; // target='_blank' 代表打開新視窗

			SendFromGmail(to, subject, body);
		}

		public void SendConfirmRegisterEmail(string email, string url, string name)
		{
			var to = email;
			var subject = "[Sierra會員註冊確認信]";
			var body = $@"Hi {name},
<br />
請點擊此連結 [<a href='{url}' target='_blank'>的確是我申請會員</a>], 如果您沒有提出申請, 請忽略本信, 謝謝"; // target='_blank' 代表打開新視窗

			SendFromGmail(to, subject, body);
		}

		public void SendFromGmail(string recipientEmail, string subject, string body)
		{
			var senderEmail = _config["Gmail:Email"];
			var password = _config["Gmail:ApplicationPassword"];

			var mms = new MailMessage();
			mms.From = new MailAddress(senderEmail);
			mms.To.Add(recipientEmail);
			mms.Subject = subject;
			mms.Body = body;
			mms.IsBodyHtml = true;
			mms.SubjectEncoding = Encoding.UTF8;

			// 設定郵件主機
			SmtpClient client = new SmtpClient("smtp.gmail.com");
			client.Port = 587;
			client.Credentials = new NetworkCredential(senderEmail, password);
			client.EnableSsl = true;

			// 寄出郵件
			try
			{
				client.Send(mms);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
		public void SendVerificationEmail(string recipientEmail, string confirmUrl)
		{
			var senderEmail = _config["Gmail:Email"];
			var password = _config["Gmail:ApplicationPassword"];

			MailMessage mms = new MailMessage();
			mms.From = new MailAddress(senderEmail);
			mms.To.Add(recipientEmail);
			mms.Subject = "Sierra會員註冊信";
			mms.Body = $"歡迎您成為Sierra的會員，請點擊以下連結進行驗證：{confirmUrl}";

			// 設定郵件主機
			SmtpClient client = new SmtpClient("smtp.gmail.com");
			client.Port = 587;
			client.Credentials = new NetworkCredential(senderEmail, password);
			client.EnableSsl = true;

			// 寄出郵件
			try
			{
				client.Send(mms);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
	}
}
