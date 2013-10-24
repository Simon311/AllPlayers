using System;
using System.Collections.Generic;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using System.Linq;

namespace All
{
	[ApiVersion(1, 14)]
	public class All : TerrariaPlugin
	{
		public override Version Version
		{
			get { return new Version("1.0.0.1"); }
		}

		public override string Name
		{
			get { return "AllPlayers"; }
		}

		public override string Author
		{
			get { return "simon311"; }
		}

		public override string Description
		{
			get { return "Adds /# and /$."; }
		}

		public All(Main game)
			: base(game)
		{
			Order = 0;
		}

		public override void Initialize()
		{
			Commands.ChatCommands.Add(new Command("doall", DoAll, "#"));
			Commands.ChatCommands.Add(new Command("doall", DoAll, "$"));
		}

		public void DoAll(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
			{
				args.Player.SendMessage("Usage: /[#,$] /command @all", Color.PaleGoldenrod);
				args.Player.SendMessage("# - do command for all except you, $ - for all including you.", Color.PaleGoldenrod);
				args.Player.SendMessage("You must use @all inside the command to reference all players.", Color.PaleGoldenrod);
				return;
			}
			bool self = args.Message.StartsWith("$ ");
			string Command = args.Message.Replace("# ", "");
			if (self) Command = args.Message.Replace("$ ", "");
			List<TSPlayer> Players = TShock.Players.Where(player => (player != null)).ToList();
			if (!self) Players = Players.Where(player => (player.Index != args.Player.Index)).ToList();
			int Count = 0;
			if (Command.Contains("@all") && Players.Count > 0)
			{
				foreach (TSPlayer player in Players)
				{
					Count++;
					var cmd = Command.Replace("@all", player.Name);
					Commands.HandleCommand(args.Player, cmd);
				}
				args.Player.SendMessage("Affected " + Count + " players!", Color.PaleGoldenrod);
			}
			else if (Players.Count == 0) args.Player.SendMessage("No players matched.", Color.PaleGoldenrod);
			else args.Player.SendMessage("You must use @all inside your command.", Color.PaleGoldenrod);
		}

	}
}
