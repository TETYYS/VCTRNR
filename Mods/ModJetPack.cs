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
		static float JetPackSpeed;

		public static void Switch() {
			if (GravityDisable.PatchStatus() == PATCH_STATUS.Disabled) {
				GravityDisable.PatchEnable();
				LoopEngine.FxAdd(JetPackTick);
				PromptWrite("Jetpack enabled");
			} else {
				LoopEngine.FxRemove(JetPackTick);
				GravityDisable.PatchDisable();
				PromptWrite("Jetpack disabled");
				JetPackUp = false;
				JetPackDown = false;
				JetPackCtl = false;
			}
		}

		public static bool Enabled() {
			return GravityDisable.PatchStatus() == PATCH_STATUS.Enabled;
		}

		public static void UpEnable() { JetPackUp = true; }
		public static void UpDisable() { JetPackUp = false; }
		public static void DownEnable() { JetPackUp = true; }
		public static void DownDisable() { JetPackUp = false; }
		public static void CtlEnable() { JetPackUp = true; }
		public static void CtlDisable() { JetPackUp = false; }

		public static float GetSpeed() { return JetPackSpeed; }

		public static void SetSpeed(float In) {
			JetPackSpeed = In;
			PromptWrite("Jetpack speed: " + Math.Round(JetPackSpeed, 3));
		}

		static void JetPackTick() {
			if (ADDRESSES.IsInVehicle) {
				Vector speed = new Vector(Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.SPEED_X_OFFSET));
				speed *= 0;
				speed.Z = (float)Gravity.Get();
				speed.MemWrite(Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.SPEED_X_OFFSET));
			} else {
				Vector speed = new Vector(Mem.PtrToAddr(ADDRESSES.PLAYER.PlayerPointer, ADDRESSES.PLAYER.X_MOVE_SPEED));
				speed *= 0;
				speed.MemWrite(Mem.PtrToAddr(ADDRESSES.PLAYER.PlayerPointer, ADDRESSES.PLAYER.X_MOVE_SPEED));
			}
			IsOnGround.Set(0);

			if (JetPackCtl) {
				/*IntPtr ptr = IsInVehicle ? ADDRESES.VEHICLE.VehiclePointer : ADDRESES.PLAYER.PlayerPointer;
				int xMove = IsInVehicle ? ADDRESES.VEHICLE.SPEED_X_OFFSET : ADDRESES.PLAYER.X_MOVE_SPEED;
				int yMove = IsInVehicle ? ADDRESES.VEHICLE.SPEED_Y_OFFSET : ADDRESES.PLAYER.Y_MOVE_SPEED;
				int rotOffset = IsInVehicle ? ADDRESES.VEHICLE.ROLL_Z_OFFSET : ADDRESES.PLAYER.ROTATION_OFFSET;

				IntPtr xAddr = Mem.PtrToAddr(ptr, xMove);
				IntPtr yAddr = Mem.PtrToAddr(ptr, yMove);
				IntPtr rotationAddr = Mem.PtrToAddr(ptr, rotOffset);
				float rot = Mem.ReadFloat(rotationAddr);
				float xVal = 0.0f, yVal = 0.0f;

				if (HALF_PI > rot > 0.0f && rot < HALF_PI) {
					xVal = -rot;
				} else if (rot < 0.0f && rot > -HALF_PI) {
					xVal = Math.Abs(rot);
				} else if (rot > HALF_PI && rot <= PI) {
					xVal = -(PI - rot);
				} else if (rot >= -PI && rot < -HALF_PI) {
					xVal = PI - Math.Abs(rot);
				}

				if (rot > 0.0f && rot < HALF_PI) {
					yVal = 1.5f - rot;
				} else if (rot < 0.0f && rot > -HALF_PI) {
					yVal = 1.5f - Math.Abs(rot);
				} else if (rot > HALF_PI && rot <= PI) {
					yVal = -(rot - 1.5f);
				} else if (rot >= -PI && rot < -HALF_PI) {
					yVal = -(Math.Abs(rot) - 1.5f);
				}

				Mem.WriteFloat(xAddr, xVal * jetPackSpeed);
				Mem.WriteFloat(yAddr, yVal * jetPackSpeed);*/
				IntPtr ptr = ADDRESSES.IsInVehicle ? ADDRESSES.VEHICLE.VehiclePointer : ADDRESSES.PLAYER.PlayerPointer;
				float rot = Mem.ReadFloat(Mem.PtrToAddr(ptr, ADDRESSES.IsInVehicle ? ADDRESSES.VEHICLE.ROLL_Z_OFFSET : ADDRESSES.PLAYER.ROTATION_OFFSET));
				float xVal = 0.0f, yVal = 0.0f;

				if (rot > 0.0f && rot < HALF_PI) {
					xVal = -rot;
				} else if (rot < 0.0f && rot > -HALF_PI) {
					xVal = Math.Abs(rot);
				} else if (rot > HALF_PI && rot <= PI) {
					xVal = -(PI - rot);
				} else if (rot >= -PI && rot < -HALF_PI) {
					xVal = PI - Math.Abs(rot);
				}

				if (rot > 0.0f && rot < HALF_PI) {
					yVal = 1.5f - rot;
				} else if (rot < 0.0f && rot > -HALF_PI) {
					yVal = 1.5f - Math.Abs(rot);
				} else if (rot > HALF_PI && rot <= PI) {
					yVal = -(rot - 1.5f);
				} else if (rot >= -PI && rot < -HALF_PI) {
					yVal = -(Math.Abs(rot) - 1.5f);
				}

				Mem.WriteFloat(Mem.PtrToAddr(ptr, ADDRESSES.IsInVehicle ? ADDRESSES.VEHICLE.SPEED_X_OFFSET : ADDRESSES.PLAYER.X_MOVE_SPEED), xVal * JetPackSpeed);
				Mem.WriteFloat(Mem.PtrToAddr(ptr, ADDRESSES.IsInVehicle ? ADDRESSES.VEHICLE.SPEED_Y_OFFSET : ADDRESSES.PLAYER.Y_MOVE_SPEED), yVal * JetPackSpeed);
			}
			if (JetPackUp)
				Mem.WriteFloat(ADDRESSES.IsInVehicle	? Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.SPEED_Z_OFFSET)
													: Mem.PtrToAddr(ADDRESSES.PLAYER.PlayerPointer, ADDRESSES.PLAYER.Z_MOVE_SPEED), JetPackSpeed);
			else if (JetPackDown)
				Mem.WriteFloat(ADDRESSES.IsInVehicle	? Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.SPEED_Z_OFFSET)
													: Mem.PtrToAddr(ADDRESSES.PLAYER.PlayerPointer, ADDRESSES.PLAYER.Z_MOVE_SPEED), -JetPackSpeed);
		}
	}
}
