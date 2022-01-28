using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
	public class TestCommand : ModuleBase<SocketCommandContext>
	{
		[Command("nils")]
		public async Task TestAsync()
		{
			await Context.Channel.TriggerTypingAsync();

			var myReaction = new Emoji("👊");
			await Context.Message.AddReactionAsync(myReaction);

			var message = $":punch: Insgesamt gehören Nils und seine Mitstreiter zwar zu den schwächeren Denkern unserer Gemeinschaft, erstaunten uns jedoch sporadisch mit {Global.Counter} Geistesblitzen. :punch:";
			await ReplyAsync(message);
			Global.Counter++;
		}
	}
}
