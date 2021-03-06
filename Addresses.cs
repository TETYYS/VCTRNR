﻿using System;

[Flags]
public enum CarImmunities {
	BP = 1,
	FP = 2,
	EP = 4,
	CP = 8
}

public enum PedImmunities1 {
	EP = 2
}

[Flags]
public enum PedImmunities2 {
	BP = 2,
	FP = 4,
	BPFP = 6
}

public enum MODE {
	STANDALONE = 0,
	VCMP = 1,
	MODE_MAX = 2
}

public enum MODULE {
	VC = 0,
	VCMP = 1
}
class ADDRESSES {
	public static MODE mode = MODE.STANDALONE;

	public static readonly IntPtr TEXT1_ADDR = (IntPtr)0x7D3E40;
	public static readonly IntPtr TEXT2_ADDR = (IntPtr)0x939028;
	public static readonly IntPtr MISSION_TEXT_ADDR = (IntPtr)0x78CF00;
	public const string CAR_SPAWN_STRING = "betterthanwalking";
	public static readonly IntPtr PLAYER_CONTROLS_STRUCTURES = (IntPtr)0x7DBCB0;

	public static bool IsInVehicle => Mem.PtrToAddr(PLAYER.PlayerPointer, 0) != Mem.PtrToAddr(VEHICLE.VehiclePointer, 0);
	public static bool prevIsInVehicle = false;

	public static void CSetMode(string Arg) {
		if (!String.IsNullOrEmpty(Arg)) {
			if (Arg.ToUpper() == "VCMP" || Arg == "1") {
				mode = MODE.VCMP;
			} else {
				mode = MODE.STANDALONE;
			}
			General.MainForm.SetStatus("Set mode: " + (mode == MODE.STANDALONE ? "Standalone" : "VCMP"));
		} else
			General.MainForm.SetStatus("Current mode: " + (mode == MODE.VCMP ? "VCMP" : "Standalone"));
	}

	public class MOUSE {
		public static readonly IntPtr MOUSE_X = (IntPtr)0x936910; //float
		public static readonly IntPtr MOUSE_Y = (IntPtr)0x936914; //float
	}

	public class DISPLAY {

		public struct CAMERA_STATE {
			public float x, y, z;
		}

		public static CAMERA_STATE CameraGetState() {
			var ret = new CAMERA_STATE();
			ret.x = Mem.ReadFloat(CAMERA_X);
			ret.y = Mem.ReadFloat(CAMERA_Y);
			ret.z = Mem.ReadFloat(CAMERA_Z);
			return ret;
		}

		public static void CameraSetState(CAMERA_STATE State) {
			Mem.WriteFloat(CAMERA_X, State.x);
			Mem.WriteFloat(CAMERA_Y, State.y);
			Mem.WriteFloat(CAMERA_Z, State.z);
		}

		public static readonly IntPtr CAMERA_X = (IntPtr)0x7E46B8; //float
		public static readonly IntPtr CAMERA_Y = (IntPtr)0x7E46BC; //float
		public static readonly IntPtr CAMERA_Z = (IntPtr)0x7E46C0; //float

		public static readonly IntPtr CAMERA_Z_ROT = (IntPtr)0x7E48CC;
		public static readonly IntPtr CAMERA_X_ROT = (IntPtr)0x7E48BC;
	}

	public class PLAYER {
		//public static readonly IntPtr PLAYER_POINTER = (IntPtr)0x94AD28; //78F7E4
		//0A0B1230
		public static IntPtr PlayerPointerStandalone = (IntPtr)0x94AD28;

		/*
		 * 007e4b8c
		 * f48a294
		 * f540210
		 * f57b284
		 * f57b2e4
		 * f5891d0
		 * f590f80
		 * f5920b8
		 * f5920c4
		 * f5920d0
		 * f5966ac
		 * f5966b4
		 */
		public static IntPtr PlayerPointerVCMP = (IntPtr)(General.VC_BASE + 0x3E4B8C);

		public static IntPtr PlayerPointer => mode == MODE.STANDALONE ? PlayerPointerStandalone : PlayerPointerVCMP;

		public const int COORDS_X_OFFSET = 0x34; //float
		public const int COORDS_Y_OFFSET = 0x38; //float
		public const int COORDS_Z_OFFSET = 0x3C; //float

		public const int AP_OFFSET = 0x358; //float
		public const int ROTATION_OFFSET = 0x378; //float
		public const int X_MOVE_SPEED = 0x70; //float
		public const int Y_MOVE_SPEED = 0x74; //float
		public const int Z_MOVE_SPEED = 0x78; //float
		public const int IS_ON_GROUND = 0xE4; //DWORD  65536/0
		public const int ADRENALINE_DURATION_OFFSET = 0x634; //DWORD
		public const int ADRENALINE_OFFSET = 0x63B; //byte 1/0

		public const int STATE_OFFSET = 0x244; //DWORD
		public enum STATES : uint {
			NORMAL_ON_FOOT = 0x01,
			WALKING = 0x07,
			FIRST_PERSON_AIMING = 0x0C,
			RUNNING_PUNCHING = 0x10,
			ATTACK_WTH_ANY_WEAPONS = 0x10,
			STANDING_PUNCHING = 0x11,
			STANDING_GETTING_PUNCHED = 0x11,
			LIFTING_PHONE = 0x13,
			AUTOAIM = 0x16,
			RUNNING_TO_ENTER_VEHICLE = 0x18,
			WEAK_DODGE = 0x1F,
			ANSWERING_MOBILE = 0x24,
			JUMPING = 0x29,
			LAYING_ONTO_GROUND = 0x2A,
			GETTING_BACK_UP = 0x2B,
			DODGE_CAR = 0x2D,
			SITTING_IN_VEHICLE = 0x32,
			DEATH_BEFORE_WASTED_SCREEN = 0x36,
			WASTED_SCREEN = 0x37,
			CAR_JACK = 0x38,
			GETTING_CAR_JACKED = 0x39,
			ENTERING_VEHICLE = 0x3A,
			EXITING_VEHICLE = 0x3C,
			BUSTED = 0x3E
		}
	}

	public class VEHICLE {

		// 0068F590 - speed multiplier
		public static IntPtr VehiclePointerStandalone = (IntPtr)0x7E49C0;

		/*
		 * 007e49c0
		 * 007e4e94
		 * 0f590ffc
		 */
		public static IntPtr VehiclePointerVCMP = (IntPtr)(General.VC_BASE + 0x3E49C0);

		public static IntPtr VehiclePointer => mode == MODE.STANDALONE ? VehiclePointerStandalone : VehiclePointerVCMP;

		public const int ROLL_X_OFFSET = 0x04; //float
		public const int ROLL_Y_OFFSET = 0x08; //float
		public const int ROLL_Z_OFFSET = 0x0C; //float
		public const int DIR_X_OFFSET = 0x14; //float
		public const int DIR_Y_OFFSET = 0x18; //float
		public const int DIR_Z_OFFSET = 0x1C; //float
		public const int COORDS_X_OFFSET = 0x34; //float
		public const int COORDS_Y_OFFSET = 0x38; //float
		public const int COORDS_Z_OFFSET = 0x3C; //float
		public const int SPEED_X_OFFSET = 0x70; //float
		public const int SPEED_Y_OFFSET = 0x74; //float
		public const int SPEED_Z_OFFSET = 0x78; //float
		public const int TURN_X_OFFSET = 0x7C; //float
		public const int TURN_Y_OFFSET = 0x80; //float
		public const int TURN_Z_OFFSET = 0x84; //float
		public const int IS_ON_GROUND = 0xE4; //DWORD 65536/68096/0

		public const int BURNOUT_OFFSET = 0x5CC; //byte
		public static readonly IntPtr SPEED_ADDR = (IntPtr)0x821F7C;
	}
}
public static class Colors {
	public static string gray = "~a~";
	public static string blue = "~b~";
	public static string cyan = "~c~";
	public static string pink = "~g~";
	public static string white = "~i~";
	public static string orange = "~o~";
	public static string purple = "~p~";
	public static string purpleAndPink = "~q~";
	public static string purpleAndMorePink = "~r~";
	public static string silver = "~s~";
	public static string green = "~t~";
	public static string fadingBlue = "~x~";
	public static string yellow = "~y~";
}