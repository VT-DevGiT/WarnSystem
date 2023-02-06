using Neuron.Core.Dev;
using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Neuron.Modules.Commands.Command;
using PluginAPI.Core;
using Synapse3.SynapseModule;
using Synapse3.SynapseModule.Command;
using Synapse3.SynapseModule.Player;
using System.Drawing.Imaging;
using System.Linq;
using System.Text.RegularExpressions;
using WarnSystemModule;

namespace WarnSystem.Commands
{

    [Automatic]
    [SynapseRaCommand(
        CommandName = "warn",
        Aliases = new[] { "wrn" },
        Description = "Manage Warn for a player",
        Platforms = new[] { CommandPlatform.RemoteAdmin, CommandPlatform.ServerConsole },
        Parameters = new[] { "see/add/remove", "Player", "(reason or warn id)" },
        Permission = "ws"
    )]
    public class Warn : SynapseCommand
    {
        private readonly WarnSystemPlugin _plugin;
        private readonly WarnService _warn;
        private readonly PlayerService _player;

        public Warn(WarnSystemPlugin plugin, WarnService warn, PlayerService player)
        {
            _plugin = plugin;
            _warn = warn;
            _player = player;
        }

        public override void Execute(SynapseContext context, ref CommandResult result)
        {
            //Test if there is enough args
            var count = context.Arguments.Count();

            if (count < 2)
            {
                result.Response = context.Player.GetTranslation(_plugin.Translation).ArgsError;
                result.StatusCode = CommandStatusCode.Error;
                return;
            }

            var playerArg = context.Arguments[1];
            var player = playerArg.ToLower() == "me" ? context.Player : _player.GetPlayer(context.Arguments[1]);
            if (player == null)
            {
                result.Response = context.Player.GetTranslation(_plugin.Translation).PlayerNotFound;
                result.StatusCode = CommandStatusCode.Error;
                return;
            }
            var cmdType = context.Arguments[0];
            var arguments = "";
            for (int i = 2; i < count; i++)
                arguments += context.Arguments[i] + " ";
            var numberOfWarn = _warn.GetNumberOfWarns(player);

            switch (cmdType)
            {
                case "add":
                    if (!context.Player.HasPermission("ws.add"))
                    {
                        result.StatusCode = CommandStatusCode.Error;
                        result.Response = context.Player.GetTranslation(_plugin.Translation).NoPermission;
                        return;
                    }

                    var message = context.Player.GetTranslation(_plugin.Translation).WarnSuccess;
                    message = message.Format(arguments, player.NickName);
                    var bcMessage = context.Player.GetTranslation(_plugin.Translation).PlayerMessage.Format(arguments);
                    player.SendBroadcast(bcMessage, 10);
                    _warn.AddWarn(player, arguments);
                    result.Response = message;
                    break;

                case "remove" when numberOfWarn == 0:
                case "see" when numberOfWarn == 0:
                    result.Response = context.Player.GetTranslation(_plugin.Translation).NoWarn;
                    result.StatusCode = CommandStatusCode.Error;
                    break;

                case "see":
                    if (!context.Player.HasPermission("ws.see"))
                    {
                        result.StatusCode = CommandStatusCode.Error;
                        result.Response = context.Player.GetTranslation(_plugin.Translation).NoPermission;
                        return;
                    }

                    result.Response = _warn.SeeWarns(player);
                    break;

                case "remove":
                    if (!context.Player.HasPermission("ws.remove"))
                    {
                        result.StatusCode = CommandStatusCode.Error;
                        result.Response = context.Player.GetTranslation(_plugin.Translation).NoPermission;
                        return;
                    }

                    if (!int.TryParse(arguments, out int id))
                    {
                        result.Response = context.Player.GetTranslation(_plugin.Translation).WarnNotFound;
                        result.StatusCode = CommandStatusCode.Error;
                    }
                    else if (_warn.RemoveWarn(player, id))
                    {
                        result.Response = context.Player.GetTranslation(_plugin.Translation).Remove;
                    }
                    else
                    {
                        result.Response = context.Player.GetTranslation(_plugin.Translation).WarnNotFound;
                        result.StatusCode = CommandStatusCode.Error;
                    }
                    break;

                default:
                    result.StatusCode = CommandStatusCode.Error;
                    result.Response = $"\n{context.Player.GetTranslation(_plugin.Translation).TypeError}";
                    break;
            }
        }
    }
}