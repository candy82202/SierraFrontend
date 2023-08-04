using System.Net.Mail;
using System.Net;
using System.Text;

namespace SIERRA_Server.Models.Infra
{
	public class EmailHelper
	{
		private string senderEmail = "g01.webapp@gmail.com"; // 寄件者

		public void SendForgetPasswordEmail(string url, string name, string email)
		{
			var subject = "[重設密碼通知]";
			var body = $@"Hi {name},
<br />
請點擊此連結 [<a href='{url}' target='_blank'>我要重設密碼</a>], 以進行重設密碼, 如果您沒有提出申請, 請忽略本信, 謝謝";

			var from = senderEmail;
			var to = email;

			SendFromGmail(from, to, subject, body);
		}

		public void SendConfirmRegisterEmail(string url, string name, string email)
		{
			var subject = "[新會員確認信]";
			var body = $@"Hi {name},
<br />
請點擊此連結 [<a href='{url}' target='_blank'>的確是我申請會員</a>], 如果您沒有提出申請, 請忽略本信, 謝謝";

			var from = senderEmail;
			var to = email;

			SendFromGmail(from, to, subject, body);
		}

		public virtual void SendFromGmail(string from, string to, string subject, string body)
		{
			// todo 以下是開發時,測試之用, 只是建立text file, 不真的寄出信
			var path = HttpContext.Current.Server.MapPath("~/files/");
			CreateTextFile(path, from, to, subject, body);
			return;

			// 以下是實作程式, 可以視需要真的寄出信, 或者只是單純建立text file,供開發時使用
			// ref https://dotblogs.com.tw/chichiblog/2018/04/20/122816
			var smtpAccount = from;

			// TODO 請在這裡填入密碼,或從web.config裡讀取
			var smtpPassword = "";

			var smtpServer = "smtp.gmail.com";
			var SmtpPort = 587;

			var mms = new MailMessage();
			mms.From = new MailAddress(smtpAccount);
			mms.Subject = subject;
			mms.Body = body;
			mms.IsBodyHtml = true;
			mms.SubjectEncoding = Encoding.UTF8;
			mms.To.Add(new MailAddress(to));

			using (var client = new SmtpClient(smtpServer, SmtpPort))
			{
				client.EnableSsl = true;
				client.Credentials = new NetworkCredential(smtpAccount, smtpPassword);//寄信帳密 
				client.Send(mms); //寄出信件
			}
		}

		private void CreateTextFile(string path, string from, string to, string subject, string body)
		{
			var fileName = $"{to.Replace("@", "_")} {DateTime.Now.ToString("yyyyMMdd_HHmmss")}.txt";
			var fullPath = Path.Combine(path, fileName);

			var contents = $@"from:{from}
to:{to}
subject:{subject}

{body}";
			File.WriteAllText(fullPath, contents, Encoding.UTF8);
		}
	}
