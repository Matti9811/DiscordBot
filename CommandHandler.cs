using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;

namespace DiscordBot
{
  public class CommandHandler
  {
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _interactionService;
    private readonly CommandService _commandService;
    private readonly IServiceProvider _provider;
    private const string Prefix = "!";

    public CommandHandler(DiscordSocketClient client, InteractionService interactionService, IServiceProvider provider, CommandService commandService)
    {
      _client = client;
      _commandService = commandService;
      _interactionService = interactionService;
      _provider = provider;
    }

    public async Task InitializeAsync()
    {
      await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
      await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);

      await _interactionService.RegisterCommandsGloballyAsync(true);

      _client.InteractionCreated += HandleInteraction;
      _client.MessageReceived += ClientMessageReceived;
			_commandService.CommandExecuted += OnCommandExecuted;

      _interactionService.SlashCommandExecuted += SlashCommandExecuted;
      _interactionService.ContextCommandExecuted += ContextCommandExecuted;
      _interactionService.ComponentCommandExecuted += ComponentCommandExecuted;
    }

		private Task ComponentCommandExecuted(ComponentCommandInfo arg1, IInteractionContext arg2, Discord.Interactions.IResult arg3)
    {
      if (!arg3.IsSuccess)
      {
        switch (arg3.Error)
        {
          case InteractionCommandError.UnmetPrecondition:
            // implement
            break;
          case InteractionCommandError.UnknownCommand:
            // implement
            break;
          case InteractionCommandError.BadArgs:
            // implement
            break;
          case InteractionCommandError.Exception:
            // implement
            break;
          case InteractionCommandError.Unsuccessful:
            // implement
            break;
          default:
            break;
        }
      }

      return Task.CompletedTask;
    }

    private Task ContextCommandExecuted(ContextCommandInfo arg1, IInteractionContext arg2, Discord.Interactions.IResult arg3)
    {
      if (!arg3.IsSuccess)
      {
        switch (arg3.Error)
        {
          case InteractionCommandError.UnmetPrecondition:
            // implement
            break;
          case InteractionCommandError.UnknownCommand:
            // implement
            break;
          case InteractionCommandError.BadArgs:
            // implement
            break;
          case InteractionCommandError.Exception:
            // implement
            break;
          case InteractionCommandError.Unsuccessful:
            // implement
            break;
          default:
            break;
        }
      }

      return Task.CompletedTask;
    }

    private Task SlashCommandExecuted(SlashCommandInfo arg1, IInteractionContext arg2, Discord.Interactions.IResult arg3)
    {
      if (!arg3.IsSuccess)
      {
        switch (arg3.Error)
        {
          case InteractionCommandError.UnmetPrecondition:
            // implement
            break;
          case InteractionCommandError.UnknownCommand:
            // implement
            break;
          case InteractionCommandError.BadArgs:
            // implement
            break;
          case InteractionCommandError.Exception:
            // implement
            break;
          case InteractionCommandError.Unsuccessful:
            // implement
            break;
          default:
            break;
        }
      }

      return Task.CompletedTask;
    }

    private async Task HandleInteraction(SocketInteraction arg)
    {
      try
      {
        var ctx = new SocketInteractionContext(_client, arg);
        await _interactionService.ExecuteCommandAsync(ctx, _provider);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
        if (arg.Type == InteractionType.ApplicationCommand)
        {
          await arg.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
        }
      }
    }

    private async Task ClientMessageReceived(SocketMessage socketMessage)
    {
      if (!(socketMessage is SocketUserMessage message))
      {
        return;
      }

      if (message.Source != MessageSource.User)
      {
        return;
      }

      if(message.Content == ":punch:")
			{
        var myReaction = new Emoji("👊");
        await message.AddReactionAsync(myReaction);
        return;
			}

      var argPos = 0;
      if (!message.HasStringPrefix(Prefix, ref argPos) && !message.HasMentionPrefix(_client.CurrentUser, ref argPos))
      {
        return;
      }

      var context = new SocketCommandContext(_client, message);
      await _commandService.ExecuteAsync(context, argPos, _provider);
    }

    private async Task OnCommandExecuted(Optional<CommandInfo> commandInfo, ICommandContext commandContext, Discord.Commands.IResult result)
    {
      if (result.IsSuccess)
      {
        return;
      }

      await commandContext.Channel.SendMessageAsync(result.ErrorReason);
    }
  }
}
