using ConfigService;
using LoggerService;

namespace MailService
{
    internal class FakeMailService : IMailService
    {
        private readonly ILayeredConfigReader configReader;
        private readonly ILoggerService logger;

        public FakeMailService(ILayeredConfigReader configReader, ILoggerService logger)
        {
            this.configReader = configReader;
            this.logger = logger;
        }

        public void Send(string message)
        {
            this.logger.LogInfo("start sending email.");

            Console.WriteLine($"mail service sending email: \"{message}\" \r\n to: \"{this.configReader.GetValue("to")}\"");

            this.logger.LogInfo("email send");
        }
    }
}
