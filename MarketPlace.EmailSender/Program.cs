
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using MarketPlace.EmailSender.Model;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.Json;

ConnectionFactory factory = new();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "Rabbit Receiver 1 App";

IConnection cnn = await factory.CreateConnectionAsync();

IChannel channel = await cnn.CreateChannelAsync();

string exchangeName = "DemoExchange";
string routingKey = "demo-routing-key";
string queueName = "DemoQueue";

await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct);
await channel.QueueDeclareAsync(queueName, false, false, false, null);
await channel.QueueBindAsync(queueName, exchangeName, routingKey, null);

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (sender, args) =>
{
    //Task.Delay(TimeSpan.FromSeconds(3)).Wait();
    var body = args.Body.ToArray();
    string message = Encoding.UTF8.GetString(body);

    var emailData = JsonSerializer.Deserialize<EmailData>(message);

    SendEmail(emailData);
    Console.WriteLine($" [x] Sent email to {emailData.To}");

    //await channel.BasicAckAsync(args.DeliveryTag, false);

};



string consumerTag = await channel.BasicConsumeAsync(queueName, true, consumer);
Console.WriteLine("Waiting for messages. Press [Enter] to exit.");
Console.ReadLine();
//Console.ReadLine();
//await channel.BasicCancelAsync(consumerTag);
//await channel.CloseAsync();
//await cnn.CloseAsync();


void SendEmail(EmailData emailData)
{
    try
    {
        using (var smtpClient = new SmtpClient("smtp.gmail.com"))
        {
            smtpClient.Port = 25; // Change this to your SMTP port
            smtpClient.Credentials = new NetworkCredential("bmttttcarlos2@gmail.com", "uelh axlu hnsk qonv");
            smtpClient.EnableSsl = true;

            var mailMessage = new MailMessage
            {
                From = new MailAddress("bmttttcarlos2@gmail.com"),
                Subject = emailData.Subject,
                Body = emailData.Body,
                IsBodyHtml = false
            };

            mailMessage.To.Add(emailData.To);

            smtpClient.Send(mailMessage);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($" [!] Failed to send email: {ex.Message}");
    }
}