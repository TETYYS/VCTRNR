using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

public interface IConfigVar {
	void ConfigVariableSet(string Arg, string VariableName);
	void ConfigVariableEnableSet(ref bool Variable, string Arg, string VariableName);
}

public class ConfigVar {
	object _Value;

	public object Value {
		get {
			return _Value;
		}
		set {
			if (fxTick != null)
				if ((bool)value)
					LoopEngine.FxAdd(fxTick);
				else
					LoopEngine.FxRemove(fxTick);
			_Value = value;
		}
	}
	Action fxTick;
	string Name;

	public void ConfigVariableSet(string Arg) {
		if (Value is bool) {
			if (!string.IsNullOrEmpty(Arg)) {
				if (Arg == "1" || Arg == "TRUE" || Arg == "ENABLED" || Arg == "ENABLE" || Arg == "E") {
					Value = true;
				} else if (Arg == "0" || Arg == "FALSE" || Arg == "DISABLED" || Arg == "DISABLE" || Arg == "D") {
					Value = false;
				} else {
					General.MainForm.SetStatus("Syntax error");
					return;
				}
				General.MainForm.SetStatus(Name + " set to " + ((bool)Value ? "Enabled" : "Disabled"));
			} else
				General.MainForm.SetStatus(Name + " is " + ((bool)Value ? "Enabled" : "Disabled"));
		} else if (Value is float) {
			if (!string.IsNullOrEmpty(Arg)) {
				if (Arg.ToUpper() == "INF") {
					Value = Single.MaxValue;
					General.MainForm.SetStatus(Name + " set to ∞");
				} else {
					Value = Single.Parse(Arg);
					General.MainForm.SetStatus(Name + " set to " + Value);
				}
			} else {
				if (Math.Abs((float)Value - Single.MaxValue) < Single.Epsilon)
					General.MainForm.SetStatus(Name + " is ∞");
				else
					General.MainForm.SetStatus(Name + " is " + Value);
			}
		} else if (Value is string) {
			if (!String.IsNullOrEmpty(Arg)) {
				Value = Arg;
				General.MainForm.SetStatus(Name + " set to " + Value);
			} else
				General.MainForm.SetStatus(Name + " is " + Value);
		} else {
			throw new NotImplementedException();
		}
	}

	public ConfigVar(float In, string Name) {
		Value = In;
		this.Name = Name;
	}

	public ConfigVar(bool In, string Name, Action fxTick = null) {
		Value = In;
		this.Name = Name;
		this.fxTick = fxTick;
	}

	public ConfigVar(string In, string Name) {
		Value = In;
		this.Name = Name;
	}
}

class Config {
	public static Stopwatch titleTimeout = new Stopwatch();

	public static ConfigVar hpSet = new ConfigVar(100.0f, "HpSet");
	public static ConfigVar apSet = new ConfigVar(100, "ApSet");
	public static ConfigVar vehicleHpSet = new ConfigVar(1000, "VehicleHpSet");
	public static ConfigVar vehicleSpeedMultiplierSet = new ConfigVar(2.0f, "VehicleSpeedMultiplierSet");
	public static ConfigVar autoMowerLockAxis = new ConfigVar("", "LockAutoMowerAxis");
	public static ConfigVar jetPackSpeed = new ConfigVar(1.0f, "JetPack speed");
	public static ConfigVar jetPackCtlEnableZ = new ConfigVar(false, "JetPack Control enable Z axis");

	public static void LoadGlobalConfig() {
		string[] cfg = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\cfg.ini");
		General.MainForm.SetStatus("Global config:");
		for (int x = 0; x < cfg.Length; x++) {
			Console.Write("\t");
			CommandEngine.Parse(cfg[x]);
		}
		General.MainForm.SetStatus("End");
	}

	public static void LoadInVehicleConfig() {
		string[] cfg = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\oncar.ini");
		General.MainForm.SetStatus("On Vehicle config:");
		for (int x = 0; x < cfg.Length; x++) {
			Console.Write("\t");
			CommandEngine.Parse(cfg[x]);
		}
		General.MainForm.SetStatus("End");
	}

	public static void LoadOnFootConfig() {
		string[] cfg = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\onfoot.ini");
		General.MainForm.SetStatus("On Foot config:");
		for (int x = 0; x < cfg.Length; x++) {
			Console.Write("\t");
			CommandEngine.Parse(cfg[x]);
		}
		General.MainForm.SetStatus("End");
	}
}