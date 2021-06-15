using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Email
{
    public class Utilits
    {
        static string FOOTER_TXT = "<p>Дополнительно отчеты можно создать в веб-интерфейсе сайта <a href=\"http://агитатор21.рус\">http://агитатор21.рус</a> под своим персональным логином и паролем полученным от организаторов.</p>" + 
            "<p style=\"color:red\"><b>Текст письма сформирован автоматически. Прошу не отвечать на адрес отправителя.</b></p>";

        public static List<KeyValuePair<String, String>> KeyValue = new List<KeyValuePair<string, string>>();

        public static Stream GetStreamFromUrl(string url)
        {
            if (!Directory.Exists("temp"))
            {
                Directory.CreateDirectory("temp");
            }

            if (KeyValue.Count(t => t.Key == url) > 0)
            {
                string path = "temp/" + KeyValue.First(t => t.Key == url).Value;
                return new MemoryStream(File.ReadAllBytes(path));
            }

            byte[] imageData = null;

            using (var wc = new System.Net.WebClient())
                imageData = wc.DownloadData(url);

            String value = Guid.NewGuid().ToString();
            File.WriteAllBytes("temp/" + value, imageData);
            KeyValue.Add(new KeyValuePair<string, string>(url, value));

            return new MemoryStream(imageData);
        }

        public static void SendMail(MailMessage mail)
        {
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            // логин и пароль
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("mysmtp1987@gmail.com", "Bussine$Perfect");
            smtp.EnableSsl = true;
            //smtp.Send(mail);
        }

        public static void SendToMails(MailAddress from, string login, string[] emails, List<PentahoUrlBuilder> reports, string title)
        {
            string userDateString = DateTime.Now.AddDays(-1).ToString("dd.MM.yyyy");
            if (emails.Length > 0)
            {
                try
                {
                    MailAddress to = new MailAddress(emails[0]);
                    MailMessage mail = new MailMessage(from, to);

                    foreach (PentahoUrlBuilder urlBuilder in reports)
                    {
                        mail.Attachments.Add(new Attachment(Utilits.GetStreamFromUrl(urlBuilder.Url), urlBuilder.Description + urlBuilder.Extension));
                    }

                    mail.Subject = "Ежедневный отчет по результатам ОДД Агитаторов (" + title + ")";
                    mail.Body = FOOTER_TXT + "<p>Во вложении " + mail.Attachments.Count + " отчет (ов) за " + userDateString + ":</p><ul>";
                    foreach (PentahoUrlBuilder urlBuilder in reports)
                    {
                        mail.Body += "<li>" + urlBuilder.Description + "</li>";
                    }
                    mail.Body += "</ul><br />";

                    // письмо представляет код html
                    mail.IsBodyHtml = true;

                    if (emails.Length > 1)
                    {
                        for (int i = 1; i < emails.Length; i++)
                        {
                            MailAddress copy = new MailAddress(emails[i]);
                            mail.CC.Add(copy);
                        }
                    }

                    SendMail(mail);
                    Console.WriteLine(string.Join(";", emails) + "<" + login + ">: sended " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERR] " + e.ToString());
                }
            }
        }
    }
}
