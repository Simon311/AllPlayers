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
			get { return new Version("1.0.0.0"); }
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
			get { return "Adds /doall."; }
		}

		public All(Main game)
			: base(game)
		{
			Order = 0;
		}

		public override void Initialize()
		{
			Commands.ChatCommands.Add(new Command("doall", DoAll, "doall"));
		}

		public void DoAll(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
			{
				args.Player.SendMessage("Usage: /doall [true] /command @all", Color.PaleGoldenrod);
				args.Player.SendMessage("If true is present before the command - the command will affect you as well.", Color.PaleGoldenrod);
				args.Player.SendMessage("You must use @all inside the command to reference all players.", Color.PaleGoldenrod);
				return;
			}
			bool self = args.Parameters.Count != 0 && args.Parameters[0] == "true";
			if (self) args.Parameters.RemoveAt(0);
			string Command = String.Join(" ", args.Parameters);
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
