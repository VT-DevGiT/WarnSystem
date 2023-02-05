using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Neuron.Modules.Commands.Command;
using Synapse3.SynapseModule.Command;
using Synapse3.SynapseModule.Player;
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
            if (count >= 3 || (count >= 1 && context.Arguments[1] == "see"))
            {
                //if warn see serverconsole
                if (context.Platform == CommandPlatform.ServerConsole && context.Arguments[1] == "see" && count == 1)
                {
                    result.Response = context.Player.GetTranslation(_plugin.Translation).PlayerNotFound;
                    result.StatusCode = CommandStatusCode.Error;
                    return;
                }
                //define parameter of the command
                var cmdType = context.Arguments[1];
                var arguments = "";
                var player = count == 1 ? context.Player : _player.GetPlayer(context.Arguments[2]);

                //gets all the final parameter
                for (int i = 3; i < count; i++)
                    arguments += context.Arguments[i] + " ";
                if (player is null)
                {
                    result.Response = context.Player.GetTranslation(_plugin.Translation).PlayerNotFound;
                    result.StatusCode = CommandStatusCode.Error;
                }
                else
                {
                    int numberOfWarn = _warn.GetNumberOfWarns(player);
                    switch (cmdType)
                    {
                        case "add":
                            if (context.Player.HasPermission("ws.add"))
                            {
                                var message = context.Player.GetTranslation(_plugin.Translation).WarnSuccess;
                                message = Regex.Replace(message, "%reason%", arguments, RegexOptions.IgnoreCase);
                                message = Regex.Replace(message, "%player%", player.NickName, RegexOptions.IgnoreCase);
                                string bcMessage = Regex.Replace(context.Player.GetTranslation(_plugin.Translation).PlayerMessage, "%reason%", arguments, RegexOptions.IgnoreCase);
                                player.SendBroadcast(bcMessage, 10);
                                _warn.AddWarn(player, arguments);
                                result.Response = message;
                            }
                            else
                            {
                                result.StatusCode = CommandStatusCode.Error;
                                result.Response = context.Player.GetTranslation(_plugin.Translation).NoPermission;
                            }
                            break;
                        case "remove" when numberOfWarn == 0:
                        case "see" when numberOfWarn == 0:
                            result.Response = context.Player.GetTranslation(_plugin.Translation).NoWarn;
                            result.StatusCode = CommandStatusCode.Error;
                            break;
                        case "see":
                            if (context.Player.HasPermission("ws.see"))
                            {
                                result.Response = _warn.SeeWarns(player);
                            }
                            else
                            {
                                result.StatusCode = CommandStatusCode.Error;
                                result.Response = context.Player.GetTranslation(_plugin.Translation).NoPermission;
                            }
                            break;
                        case "remove":
                            if (context.Player.HasPermission("ws.remove"))
                            {
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
                            }
                            else
                            {
                                result.StatusCode = CommandStatusCode.Error;
                                result.Response = context.Player.GetTranslation(_plugin.Translation).NoPermission;
                            }
                            break;
                        default:
                            result.StatusCode = CommandStatusCode.Error;
                            result.Response = $"\n{context.Player.GetTranslation(_plugin.Translation).TypeError}";
                            break;
                    }
                }
            }
            else
            {
                result.Response = context.Player.GetTranslation(_plugin.Translation).ArgsError;
                result.StatusCode = CommandStatusCode.Error;
            }
        }
    }
}