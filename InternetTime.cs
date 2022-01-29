using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordBot
{
	public static class InternetTime
	{    
    public static async Task<DateTimeOffset?> GetCurrentTimeAsync()
    {
      using (var client = new HttpClient())
      {
        try
        {
          var result = await client.GetAsync("https://google.com", HttpCompletionOption.ResponseHeadersRead);
          return result.Headers.Date;
        }
        catch
        {
          return null;
        }
      }
    }
  }
}
