using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Interactions;

namespace DiscordBot.Commands
{
	public class WhoAmICommand : InteractionModuleBase<SocketInteractionContext>
	{
		[SlashCommand("whoami", "Who?")]
		public async Task WhoAmI()
		{
			var replies = new List<string>
			{
				"A Robot",
				"a piece of scrap metal",
				"highly intelligent being", 
				"i don't know"
			};

			var answer = replies[new Random().Next(replies.Count - 1)];
			
			await RespondAsync($"**{answer}**");
		}
  }
}
