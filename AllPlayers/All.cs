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
			TSPlayer[] Players = TShock.Players;
			if (!self) Players = Players.Where(player => (player != null) && (player.Index != args.Player.Index)).ToArray();
			if (Command.Contains("@all") && Players.Length > 0)
			{
				foreach (TSPlayer player in TShock.Players)
				{
					try
					{
						var cmd = Command.Replace("@all", player.Name);
						Commands.HandleCommand(args.Player, cmd);
					}
					catch { }
				}
			}
			else if (Players.Length == 0) args.Player.SendMessage("No players matched.", Color.PaleGoldenrod);
			else args.Player.SendMessage("You must use @all inside your command.", Color.PaleGoldenrod);
		}

	}
}
