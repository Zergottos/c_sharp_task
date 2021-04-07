using System;
using System.Net;
using System.Net.Mail;

namespace Console_Application_C_
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter a .csv file name: ");
            
            string file = "";
            while(file == "") {
                file = Console.ReadLine();

                try {
                    FileProcessor.process(file);
                } catch (Exception e) {
                    Console.WriteLine("Please, try again...");
                    file = "";
                    continue;
                }
            }

            Console.WriteLine("Enter credentials...");

            string sender = "";
            while(sender == "") {
                Console.WriteLine("Enter e-mail: ");
                sender = Console.ReadLine();
            }
            
            Console.WriteLine("Enter your password and press Enter: ");
            string password = writePassWithStars();

            Console.WriteLine("Enter reciever e-mail: ");
            string reciever = "";
            while(reciever == "") {
                reciever = Console.ReadLine();
            }

            Console.WriteLine($"File: {file}, credentials user:{sender}, reciever: {reciever}");

            try {
                var smtpClient = new SmtpClient("smtp.abv.bg")
                {
                    Port = 465,
                    Credentials = new NetworkCredential(sender, password),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(reciever),
                    Subject = "subject",
                    Body = "<h1>Hello</h1>",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(reciever);

                // Couldn't sent the e-mail, so didn't finished the attachment :(
                // var attachment = new Attachment("ReportByCountry.csv");
                // mailMessage.Attachments.Add(attachment);

                smtpClient.Send(mailMessage);
            } catch (Exception e) {
                Console.WriteLine("Failed to send an e-mail! Closing the program...");
            } finally {
                Console.WriteLine("Press ESC key to exit...");
            }
            
            while(Console.ReadKey().Key != ConsoleKey.Escape) {}
        }

        public static string writePassWithStars() {
            string password = "";
            ConsoleKeyInfo key = Console.ReadKey(true);
            while (key.Key != ConsoleKey.Enter) {
                if(key.Key == ConsoleKey.Backspace) {
                    if(password.Length > 0) {
                        password = password.Remove(password.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                } else {

                    password = password + key.KeyChar;
                    
                    Console.Write("*");
                }

                key = Console.ReadKey(true);
            };

            Console.WriteLine();

            return password;
        }
    }
}
