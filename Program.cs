using System;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Newtonsoft.Json;

namespace DiscordBot
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Global.Counter = 1;
			var programm = new Program();
			programm.StartBotAsync().GetAwaiter().GetResult();
		}

		public async Task StartBotAsync()
		{
			using (StreamReader r = new StreamReader("config.json"))
			{
				string json = r.ReadToEnd();
				Config config = JsonConvert.DeserializeObject<Config>(json);

				using (var services = ConfigureServices())
				{
					var client = services.GetRequiredService<DiscordSocketClient>();
					var commandHandler = services.GetRequiredService<CommandHandler>();

					client.Ready += () => ReadyAsync(commandHandler);
					client.Log += LogAsync;

					await client.LoginAsync(TokenType.Bot, config.Token);
					await client.StartAsync();

					await client.SetGameAsync("Bot-Game");
					await client.SetStatusAsync(UserStatus.DoNotDisturb);

					while (true)
					{
						var now = DateTime.Now;
						if (now.Hour == 13 && now.Minute == 37)
						{
							var channel = await client.GetChannelAsync(190913208016437248) as IMessageChannel;
							await channel.SendMessageAsync(":punch:");
						}
						await Task.Delay(60000);
					}
				}
			}
		}

		private async Task ReadyAsync(CommandHandler commandHandler)
		{
			await commandHandler.InitializeAsync();
		}

		private ServiceProvider ConfigureServices()
		{
			var config = new CommandServiceConfig
			{
				CaseSensitiveCommands = false,
				IgnoreExtraArgs = true,
			};
			return new ServiceCollection()
				.AddSingleton<DiscordSocketClient>()
				.AddSingleton(x => new CommandService(config))
				.AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
				.AddSingleton<CommandHandler>()
				.BuildServiceProvider();
		}

		private static Task LogAsync(LogMessage message)
		{
			switch (message.Severity)
			{
				case LogSeverity.Critical:
				case LogSeverity.Error:
					Console.ForegroundColor = ConsoleColor.Red;
					break;
				case LogSeverity.Warning:
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;
				case LogSeverity.Info:
					Console.ForegroundColor = ConsoleColor.White;
					break;
				case LogSeverity.Verbose:
				case LogSeverity.Debug:
					Console.ForegroundColor = ConsoleColor.DarkGray;
					break;
			}
			Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message} {message.Exception}");
			Console.ResetColor();

			return Task.CompletedTask;
		}
	}
}
