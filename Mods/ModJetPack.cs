using System;
using static General; // dog bless

namespace Mods {
	class JetPack {
		static Patch GravityDisable = new Patch(	(IntPtr)(PATCH_BASE + 0xBA953),
													new byte[] { 0x74 },
													new byte[] { 0xEB },
													1, "Gravity disable" );

		static Mod Gravity = new Mod(new [] {	new ADDRESS_SPEC((IntPtr)0x68F5F0, MODE.STANDALONE),
												new ADDRESS_SPEC((IntPtr)0x68F5F0, MODE.VCMP)
		                                    }, typeof(float), "Gravity");

		static Mod IsOnGround = new Mod(new [] {	new ADDRESS_SPEC_PTR(ADDRESSES.PLAYER.PlayerPointerStandalone, 0xE4, MODE.STANDALONE),
													new ADDRESS_SPEC_PTR(ADDRESSES.PLAYER.PlayerPointerVCMP, 0xE4, MODE.VCMP)
		                                       }, typeof(uint), "Is on ground?"); // 65536 / 0

		static bool JetPackUp;
		static bool JetPackDown;
		static bool JetPackCtl;
		static float JetPackSpeed = 0.1f;
		static bool JetPackEnabled;

		public static void Switch() {
			if (!JetPackEnabled) {
				PromptWrite("Jetpack enabled");
				JetPackEnabled = true;
				LoopEngine.FxAdd(JetPackTick);
			} else {
				PromptWrite("Jetpack disabled");
				JetPackEnabled = false;
				JetPackUp = false;
				JetPackDown = false;
				JetPackCtl = false;
				LoopEngine.FxRemove(JetPackTick);
			}
		}

		public static bool Enabled() {
			return JetPackEnabled; //GravityDisable.PatchStatus() == PATCH_STATUS.Enabled;
		}

		public static void UpEnable() { JetPackUp = true; }
		public static void UpDisable() { JetPackUp = false; }
		public static void DownEnable() { JetPackDown = true; }
		public static void DownDisable() { JetPackDown = false; }
		public static void CtlEnable() { JetPackCtl = true; }
		public static void CtlDisable() { JetPackCtl = false; }

		public static float GetSpeed() { return JetPackSpeed; }

		public static void SetSpeed(float In) {
			JetPackSpeed = In;
			PromptWrite("Jetpack speed: " + Math.Round(JetPackSpeed, 5));
		}

		static void JetPackTick() {

			if (JetPackCtl) {
				float rotz = Mem.ReadFloat(ADDRESSES.DISPLAY.CAMERA_Z_ROT);
				var xVal = Math.Abs((float)Math.Cos(rotz)) * (rotz > 0 && rotz < HALF_PI || (rotz > (PI * 2 - HALF_PI) && rotz < PI * 2) ? -1 : 1);
				var yVal = Math.Abs((float)Math.Sin(rotz)) * (rotz > PI && rotz < PI * 2 ? 1 : -1);

				var coords = new Vec3(Mem.PtrToAddr(ADDRESSES.PLAYER.PlayerPointer, ADDRESSES.PLAYER.COORDS_X_OFFSET));
				coords.X += xVal * JetPackSpeed;
				coords.Y += yVal * JetPackSpeed;
				if ((bool)Config.jetPackCtlEnableZ.Value) {
					float rotx = Mem.ReadFloat(ADDRESSES.DISPLAY.CAMERA_X_ROT);
					coords.Z += rotx / HALF_PI * JetPackSpeed;
				}
				coords.MemWrite(Mem.PtrToAddr(ADDRESSES.PLAYER.PlayerPointer, ADDRESSES.PLAYER.COORDS_X_OFFSET));
			}

			new Vec3(0, 0, 0.0066016f).MemWrite(Mem.PtrToAddr(ADDRESSES.PLAYER.PlayerPointer, ADDRESSES.PLAYER.X_MOVE_SPEED));

			if (JetPackUp)
				Mem.WriteFloat(ADDRESSES.IsInVehicle	? Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.SPEED_Z_OFFSET)
													: Mem.PtrToAddr(ADDRESSES.PLAYER.PlayerPointer, ADDRESSES.PLAYER.Z_MOVE_SPEED), JetPackSpeed * PI * 2);
			else if (JetPackDown)
				Mem.WriteFloat(ADDRESSES.IsInVehicle	? Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.SPEED_Z_OFFSET)
													: Mem.PtrToAddr(ADDRESSES.PLAYER.PlayerPointer, ADDRESSES.PLAYER.Z_MOVE_SPEED), -JetPackSpeed * PI * 2);
		}
	}
}
