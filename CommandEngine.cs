using System;

using Mods;

class CommandEngine {

	struct COMMAND {
		public string Command;
		public Action<string> Function;

		public COMMAND(string Command, Action<string> Function) {
			this.Command = Command;
			this.Function = Function;
		}
	}

	static readonly COMMAND[] CommandTable = {
		new COMMAND("REFRESH",
			Arg => {
				Mem.init("gta-vc");
				if (!Mem.CheckProcess())
					General.MainForm.SetStatus("GTA VC not found! Type \"Refresh\" when ready.");
				else
					Config.LoadGlobalConfig();
			}),
		new COMMAND("SETMODE",
			ADDRESSES.CSetMode),
		new COMMAND("SETHP",
			Arg => Config.hpSet.ConfigVariableSet(Arg)),
		new COMMAND("SETAP",
			Arg => Config.apSet.ConfigVariableSet(Arg)),
		new COMMAND("SETVEHICLEHP",
			Arg => Config.vehicleHpSet.ConfigVariableSet(Arg)),
		new COMMAND("SETVEHICLEWEIGHT",
			Arg => VehiclePhys.Weight.CModSet(Arg)),
		new COMMAND("SETVEHICLESPEEDMULTIPLIER",
			Arg => Config.vehicleSpeedMultiplierSet.ConfigVariableSet(Arg)),
		new COMMAND("SETPLAYERWEIGHT",
			Arg => PlayerVars.Weight.CModSet(Arg)),
		new COMMAND("SETSTAMINA",
			Arg => PlayerVars.Stamina.CModSet(Arg)),
		new COMMAND("SETMAXFPS",
			Arg => Game.FPS.CModSet(Arg)),
		new COMMAND("LOCKVEHICLEHP",
			Arg => Health.lockVehicleHp.ConfigVariableSet(Arg)),
		new COMMAND("LOCKPLAYERHP",
			Arg => Health.lockPlayerHp.ConfigVariableSet(Arg)),
		new COMMAND("AUTOMOWERLOCKAXIS",
			Arg => Config.autoMowerLockAxis.ConfigVariableSet(Arg)),
		new COMMAND("GODMODE",
			Arg => Health.GodMode.CPatchSwitch(Arg)),
		new COMMAND("VEHICLEGODMODE",
			Arg => Health.VehicleGodMode.CPatchSwitch(Arg)),
		new COMMAND("DISABLEFALLINGOFFBIKE",
			Arg => VehiclePhys.FallingOffBike.CPatchSwitch(Arg)),
		new COMMAND("INFINITERUN",
			Arg => PlayerVars.InfiniteRun.CPatchSwitch(Arg)),
		new COMMAND("DISABLEVEHICLEEXPLOSIONS",
			Arg => VehiclePhys.DisableExplosions.CPatchSwitch(Arg)),
		new COMMAND("DRIVEONWATER",
			Arg => VehiclePhys.DriveOnWater.CPatchSwitch(Arg)),
		new COMMAND("HUD",
			Arg => Game.HUD.CModSet(Arg)),
		new COMMAND("JETPACKCTLENABLEZ",
			Arg => Config.jetPackCtlEnableZ.ConfigVariableSet(Arg)),
		new COMMAND("INFINITEAMMO",
			Arg => Guns.InfiniteAmmo.CPatchSwitch(Arg)),
	};

	public static void Parse(string Command) {
		if (String.IsNullOrEmpty(Command))
			return;
		Command = Command.ToUpper();

		for (int x = 0; x < CommandTable.Length; x++) {
			if (!Command.StartsWith(CommandTable[x].Command))
				continue;

			string Arg;

			if (!Command.Contains("(") || !Command.Contains(")"))
				Arg = null;
			else
				Arg = Command.Substring(CommandTable[x].Command.Length + 1, Command.Length - CommandTable[x].Command.Length - 2);

			CommandTable[x].Function(Arg);
			return;
		}
		General.MainForm.SetStatus("Command not found");
	}
}
