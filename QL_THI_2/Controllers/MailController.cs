using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QL_THI_2.Models;
using Microsoft.AspNetCore.Authorization;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace QL_THI_2.Controllers
{
    public class MailController : Controller
    {
        QL_THIContext db = new QL_THIContext();
        private readonly EmailConfiguration _email;

        public MailController(EmailConfiguration email)
        {
            _email = email;
        }

        public IActionResult GuiMailThongBao(string id)
        {
            HOC_PHAN_THI H = db.HOC_PHAN_THIs.Where(a => a.ID_HP == id).FirstOrDefault();
            if(H.DAGUIMAIL_HP == false)
            {
                foreach (var i in db.NHOM_THIs.Where(a => a.ID_HP == id))
                {
                    modelNhom m = NhomController.LayThongTinNhom(i);
                    GuiEmail(m);
                }
            }

            db.Entry(H).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            H.DAGUIMAIL_HP = true;
            db.Entry(H).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();

            return Json(true);
        }

        public IActionResult GuiEmail(modelNhom n)
        {
            // n.hocPhan.maHocPhan.id + "-" + n.hocPhan.maHocPhan.tenHocPhan + ", " + n.stt
            var content = "";
            content += "<h2 style=\"color: #696cff;\">Bạn có nhóm học phần thi phụ trách đang chờ tải lên bài thi và điểm thi</h2>";
            content += "<p>Học phần: " 
                + "<strong>" + n.hocPhan.maHocPhan.ma + "-" + n.hocPhan.maHocPhan.tenHocPhan + ", Nhóm" + n.stt + "</strong>"
                + "</p>";
            string mail = db.TAI_KHOANs.Where(a => a.ID_TK == n.taiKhoan.id).Select(a => a.EMAIL_TK).FirstOrDefault();
            modelMail m = new modelMail
            {
                To = mail,
                Subject = "Thông báo nộp bài thi và điểm thi cho học phần",
                Body = content,
            };

            SendEmail(m);
            return Json(m);
        }

        public void SendEmail(modelMail message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(modelMail message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(_email.From));
            emailMessage.To.Add(MailboxAddress.Parse(message.To));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Body };
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_email.SmtpServer, _email.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_email.UserName, _email.Password);
                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
