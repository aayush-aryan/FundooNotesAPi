using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonLayer.Model
{
    public class MSMQModel
    {
        MessageQueue messageQueue = new MessageQueue();

        public void sendData2Queue(string token)
        {
            messageQueue.Path = @".\private$\Token";
            if (!MessageQueue.Exists(messageQueue.Path))
            {
                MessageQueue.Create(messageQueue.Path);
            }

            messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            messageQueue.ReceiveCompleted += MessageQueue_ReceiveCompleted;
            messageQueue.Send(token);
            messageQueue.BeginReceive();
            messageQueue.Close();
        }

        private void MessageQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                var msg = messageQueue.EndReceive(e.AsyncResult);
                string token = msg.Body.ToString();
                string subject = "FundooNotesResetLink";
                string body = token;
                var smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("log2aayusharyan@gmail.com", "xrqkwjrvzrhbbqxy"),
                    EnableSsl = true

                };
                smtp.Send("log2aayusharyan@gmail.com", "log2aayusharyan@gmail.com", subject, body);


                messageQueue.BeginReceive();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
