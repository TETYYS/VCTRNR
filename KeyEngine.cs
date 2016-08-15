using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using Mods;

using MouseKeyboardLibrary;

class KeyEngine {
	/// <summary>
	/// Enumeration for virtual keys.
	/// </summary>
	public enum VirtualKeys : ushort {
		LeftButton = 0x01,
		RightButton = 0x02,
		Cancel = 0x03,
		MiddleButton = 0x04,
		ExtraButton1 = 0x05,
		ExtraButton2 = 0x06,
		Back = 0x08,
		Tab = 0x09,
		Clear = 0x0C,
		Return = 0x0D,
		Shift = 0x10,
		Control = 0x11,
		Menu = 0x12,
		Pause = 0x13,
		CapsLock = 0x14,
		Kana = 0x15,
		Hangeul = 0x15,
		Hangul = 0x15,
		Junja = 0x17,
		Final = 0x18,
		Hanja = 0x19,
		Kanji = 0x19,
		Escape = 0x1B,
		Convert = 0x1C,
		NonConvert = 0x1D,
		Accept = 0x1E,
		ModeChange = 0x1F,
		Space = 0x20,
		Prior = 0x21,
		Next = 0x22,
		End = 0x23,
		Home = 0x24,
		Left = 0x25,
		Up = 0x26,
		Right = 0x27,
		Down = 0x28,
		Select = 0x29,
		Print = 0x2A,
		Execute = 0x2B,
		Snapshot = 0x2C,
		Insert = 0x2D,
		Delete = 0x2E,
		Help = 0x2F,
		N0 = 0x30,
		N1 = 0x31,
		N2 = 0x32,
		N3 = 0x33,
		N4 = 0x34,
		N5 = 0x35,
		N6 = 0x36,
		N7 = 0x37,
		N8 = 0x38,
		N9 = 0x39,
		A = 0x41,
		B = 0x42,
		C = 0x43,
		D = 0x44,
		E = 0x45,
		F = 0x46,
		G = 0x47,
		H = 0x48,
		I = 0x49,
		J = 0x4A,
		K = 0x4B,
		L = 0x4C,
		M = 0x4D,
		N = 0x4E,
		O = 0x4F,
		P = 0x50,
		Q = 0x51,
		R = 0x52,
		S = 0x53,
		T = 0x54,
		U = 0x55,
		V = 0x56,
		W = 0x57,
		X = 0x58,
		Y = 0x59,
		Z = 0x5A,
		LeftWindows = 0x5B,
		RightWindows = 0x5C,
		Application = 0x5D,
		Sleep = 0x5F,
		Numpad0 = 0x60,
		Numpad1 = 0x61,
		Numpad2 = 0x62,
		Numpad3 = 0x63,
		Numpad4 = 0x64,
		Numpad5 = 0x65,
		Numpad6 = 0x66,
		Numpad7 = 0x67,
		Numpad8 = 0x68,
		Numpad9 = 0x69,
		Multiply = 0x6A,
		Add = 0x6B,
		Separator = 0x6C,
		Subtract = 0x6D,
		Decimal = 0x6E,
		Divide = 0x6F,
		F1 = 0x70,
		F2 = 0x71,
		F3 = 0x72,
		F4 = 0x73,
		F5 = 0x74,
		F6 = 0x75,
		F7 = 0x76,
		F8 = 0x77,
		F9 = 0x78,
		F10 = 0x79,
		F11 = 0x7A,
		F12 = 0x7B,
		F13 = 0x7C,
		F14 = 0x7D,
		F15 = 0x7E,
		F16 = 0x7F,
		F17 = 0x80,
		F18 = 0x81,
		F19 = 0x82,
		F20 = 0x83,
		F21 = 0x84,
		F22 = 0x85,
		F23 = 0x86,
		F24 = 0x87,
		NumLock = 0x90,
		ScrollLock = 0x91,
		NEC_Equal = 0x92,
		Fujitsu_Jisho = 0x92,
		Fujitsu_Masshou = 0x93,
		Fujitsu_Touroku = 0x94,
		Fujitsu_Loya = 0x95,
		Fujitsu_Roya = 0x96,
		LeftShift = 0xA0,
		RightShift = 0xA1,
		LeftControl = 0xA2,
		RightControl = 0xA3,
		LeftMenu = 0xA4,
		RightMenu = 0xA5,
		BrowserBack = 0xA6,
		BrowserForward = 0xA7,
		BrowserRefresh = 0xA8,
		BrowserStop = 0xA9,
		BrowserSearch = 0xAA,
		BrowserFavorites = 0xAB,
		BrowserHome = 0xAC,
		VolumeMute = 0xAD,
		VolumeDown = 0xAE,
		VolumeUp = 0xAF,
		MediaNextTrack = 0xB0,
		MediaPrevTrack = 0xB1,
		MediaStop = 0xB2,
		MediaPlayPause = 0xB3,
		LaunchMail = 0xB4,
		LaunchMediaSelect = 0xB5,
		LaunchApplication1 = 0xB6,
		LaunchApplication2 = 0xB7,
		OEM1 = 0xBA,
		OEMPlus = 0xBB,
		OEMComma = 0xBC,
		OEMMinus = 0xBD,
		OEMPeriod = 0xBE,
		OEM2 = 0xBF,
		OEM3 = 0xC0,
		OEM4 = 0xDB,
		OEM5 = 0xDC,
		OEM6 = 0xDD,
		OEM7 = 0xDE,
		OEM8 = 0xDF,
		OEMAX = 0xE1,
		OEM102 = 0xE2,
		ICOHelp = 0xE3,
		ICO00 = 0xE4,
		ProcessKey = 0xE5,
		ICOClear = 0xE6,
		Packet = 0xE7,
		OEMReset = 0xE9,
		OEMJump = 0xEA,
		OEMPA1 = 0xEB,
		OEMPA2 = 0xEC,
		OEMPA3 = 0xED,
		OEMWSCtrl = 0xEE,
		OEMCUSel = 0xEF,
		OEMATTN = 0xF0,
		OEMFinish = 0xF1,
		OEMCopy = 0xF2,
		OEMAuto = 0xF3,
		OEMENLW = 0xF4,
		OEMBackTab = 0xF5,
		ATTN = 0xF6,
		CRSel = 0xF7,
		EXSel = 0xF8,
		EREOF = 0xF9,
		Play = 0xFA,
		Zoom = 0xFB,
		Noname = 0xFC,
		PA1 = 0xFD,
		OEMClear = 0xFE
	}

	public enum KEY_PRESS_TYPE {
		KEY_DOWN = 1,
		KEY_UP = 2,
		KEY_PRESS = 3
	}

	static KeyboardHook KbdHook = new KeyboardHook();

	public static Stopwatch KeyTimeout = new Stopwatch();
	public static bool DiscardKeys;

	[DllImport("user32.dll")]
	private static extern IntPtr GetForegroundWindow();

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, [Out] StringBuilder lParam);
	public static string GetWindowTextRaw(IntPtr hwnd) {
		// Allocate correct string length first
		var length = (int)SendMessage(hwnd, 0x000E, IntPtr.Zero, new StringBuilder());
		var sb = new StringBuilder(length + 1);
		SendMessage(hwnd, 0x000D, (IntPtr)sb.Capacity, sb);
		return sb.ToString();
	}

	public static void Start() {
		KbdHook.KeyDown += HotKeyDown;
		KbdHook.KeyUp += HotKeyUp;
		KbdHook.KeyPress += HotKeyPress;
		DiscardKeys = false;

		KbdHook.Start();
		KeyTimeout.Start();
	}

	static void HotKeyDown(object sender, KeyEventArgs e) {
		HotkeyProcessor(KEY_PRESS_TYPE.KEY_DOWN, e, null);
	}

	static void HotKeyUp(object sender, KeyEventArgs e) {
		HotkeyProcessor(KEY_PRESS_TYPE.KEY_UP, e, null);
	}

	static void HotKeyPress(object sender, KeyPressEventArgs e) {
		HotkeyProcessor(KEY_PRESS_TYPE.KEY_PRESS, null, e);
	}

	static bool KeyCtrl;
	static bool KeyShift;
	static bool KeyAlt;

	static void HotkeyProcessor(KEY_PRESS_TYPE KeyPressType, KeyEventArgs e, KeyPressEventArgs ep) {
		if (KeyPressType == KEY_PRESS_TYPE.KEY_DOWN && (VirtualKeys)e.KeyCode == VirtualKeys.F12) {
			Console.Beep();
			Process[] proc = Process.GetProcessesByName("gta-vc");
			if (proc.Length == 0 || proc.Length > 1) {
				General.MainForm.SetStatus("gta-vc not found: " + proc.Length);
			} else {
				Console.Beep();
				proc[0].Kill();
				General.MainForm.SetStatus("Success");
			}
		}
		if (KeyTimeout.ElapsedMilliseconds < 500) {
			if (DiscardKeys)
				return;
		} else {
			KeyTimeout.Reset();
			KeyTimeout.Start();
			if (GetWindowTextRaw(GetForegroundWindow()) != "GTA: Vice City" && GetWindowTextRaw(GetForegroundWindow()) != "Vice City Multiplayer (GTA:VC)") {
				DiscardKeys = true;
				return;
			}
			DiscardKeys = false;
		}

		if (e == null)
			return;

		VirtualKeys keyCode = (VirtualKeys)e.KeyCode;

		if (KeyPressType == KEY_PRESS_TYPE.KEY_DOWN) {
			switch (keyCode) {
				case VirtualKeys.B:
					Guns.EnableFastShoot();
					break;
				case VirtualKeys.RightControl:
				case VirtualKeys.LeftControl:
					KeyCtrl = true;
					break;
				case VirtualKeys.RightShift:
				case VirtualKeys.LeftShift:
					KeyShift = true;
					break;
				case VirtualKeys.RightMenu:
				case VirtualKeys.LeftMenu:
					KeyShift = true;
					break;
				#region Car spawn
				case VirtualKeys.End:
				case VirtualKeys.Home:
				case VirtualKeys.Insert:
					if (ADDRESSES.mode == MODE.STANDALONE) {
						switch (keyCode) {
							case VirtualKeys.End:	SpawnVehicle.SelectNext();		break;
							case VirtualKeys.Home:	SpawnVehicle.SelectPrevious();	break;
							case VirtualKeys.Insert:SpawnVehicle.Spawn();			break;
						}
					}
					break;
				#endregion Car spawn
				#region Jetpack
				case VirtualKeys.P:	JetPack.Switch();	break;
				case VirtualKeys.O:
				case VirtualKeys.L:
				case VirtualKeys.K:
				case VirtualKeys.OEM4:
				case VirtualKeys.OEM6:
					if (JetPack.Enabled()) {
						switch (keyCode) {
							case VirtualKeys.O:		JetPack.UpEnable();								break;
							case VirtualKeys.L:		JetPack.DownEnable();							break;
							case VirtualKeys.K:		JetPack.CtlEnable();							break;
							case VirtualKeys.OEM4:	JetPack.SetSpeed(JetPack.GetSpeed() / 1.5f);	break;
							case VirtualKeys.OEM6:	JetPack.SetSpeed(JetPack.GetSpeed() * 1.5f);	break;
						}
					}
					break;
				#endregion Jetpack
				case VirtualKeys.X:	VehiclePhys.Flip();			break;
				case VirtualKeys.Z:	VehiclePhys.InstantStop();	break;
				case VirtualKeys.Numpad1:
					if (KeyCtrl) {
						if (!KeyAlt)
							Health.GodMode.PatchSwitch();
						else
							Health.VehicleGodMode.PatchSwitch();
					} else
						Health.Heal();
					break;
				case VirtualKeys.Numpad2:
				case VirtualKeys.Numpad3:
				case VirtualKeys.Numpad4:
				case VirtualKeys.Numpad5:
					if (ADDRESSES.mode == MODE.STANDALONE) {
						switch (keyCode) {
							case VirtualKeys.Numpad2:	Cops.PatchEnable();			break;
							case VirtualKeys.Numpad3:	SpawnVehicle.SpawnTurbo();	break;
							case VirtualKeys.Numpad4:	Guns.ProfessionalTools();	break;
							case VirtualKeys.Numpad5:	Cops.PatchDisable();		break;
						}
					}
					break;
				case VirtualKeys.Numpad6:
					if (KeyCtrl)
						VehiclePhys.AutoMowerSwitch();
					break;
				case VirtualKeys.Numpad7:
					if (KeyCtrl)
						VehiclePhys.Bash();
					else
						VehiclePhys.SpeedBoost();
					break;
				case VirtualKeys.Numpad8:
					if (KeyCtrl)
						VehiclePhys.ForwardTeleport();
					break;
				case VirtualKeys.Numpad9:
					VehiclePhys.DestroyAll();
					break;
				case VirtualKeys.Numpad0:
					PlayerVars.MoneyAdd(100000);
					break;
				case VirtualKeys.N:
					if (KeyCtrl && KeyShift)
						VehiclePhys.FallingOffBike.PatchSwitch();
					break;
				case VirtualKeys.F6:
					General.PromptWrite((ADDRESSES.IsInVehicle ? "Vehicle" : "Player"));
					break;
			}
		}
		if (KeyPressType == KEY_PRESS_TYPE.KEY_UP) {
			switch (keyCode) {
				case VirtualKeys.RightControl:
				case VirtualKeys.LeftControl:
					KeyCtrl = false;
					break;
				case VirtualKeys.RightShift:
				case VirtualKeys.LeftShift:
					KeyShift = false;
					break;
				#region Jetpack
				case VirtualKeys.O:
				case VirtualKeys.L:
				case VirtualKeys.K:
					if (JetPack.Enabled()) {
						switch (keyCode) {
							case VirtualKeys.O:
								JetPack.UpDisable();
								break;
							case VirtualKeys.L:
								JetPack.DownDisable();
								break;
							case VirtualKeys.K:
								JetPack.CtlDisable();
								break;
						}
					}
					break;
				#endregion Jetpack
				case VirtualKeys.B:
					Guns.DisableFastShoot();
					break;
			}
		}
	}
}