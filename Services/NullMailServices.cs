using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.ConfigureServices
{
  public class NullMailServices : IMailServices
  {
    private readonly ILogger<NullMailServices> logger;

    public NullMailServices(ILogger<NullMailServices> logger)
    {
      this.logger = logger;
    }

    public void SendMessage(string to, string subject, string body)
    {
      logger.LogInformation($"To: {to} Subject: {subject} Body: {body}");
    }
  }
}
