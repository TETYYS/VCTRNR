using System;
using System.Threading;

using Coords = Vector;

namespace Mods {
	class VehiclePhys {
		public struct VEHICLE_PHYS_STATE {
			public Vector roll;
			public Vector dir;
			public Coords coords;
			public Vector speed;
			public Vector turn;
		}

		public static VEHICLE_PHYS_STATE VehicleGetPhysState(IntPtr VehiclePointer) {
			var ret =		new VEHICLE_PHYS_STATE();
			ret.coords =	new Coords(Mem.PtrToAddr(VehiclePointer, ADDRESSES.VEHICLE.COORDS_X_OFFSET));
			ret.dir =		new Vector(Mem.PtrToAddr(VehiclePointer, ADDRESSES.VEHICLE.DIR_X_OFFSET));
			ret.roll =		new Vector(Mem.PtrToAddr(VehiclePointer, ADDRESSES.VEHICLE.ROLL_X_OFFSET));
			ret.speed =		new Vector(Mem.PtrToAddr(VehiclePointer, ADDRESSES.VEHICLE.SPEED_X_OFFSET));
			ret.turn =		new Vector(Mem.PtrToAddr(VehiclePointer, ADDRESSES.VEHICLE.TURN_X_OFFSET));
			return ret;
		}

		public static void VehicleSetPhysState(IntPtr VehiclePointer, VEHICLE_PHYS_STATE State) {
			State.coords.	MemWrite(Mem.PtrToAddr(VehiclePointer, ADDRESSES.VEHICLE.COORDS_X_OFFSET));
			State.dir.		MemWrite(Mem.PtrToAddr(VehiclePointer, ADDRESSES.VEHICLE.DIR_X_OFFSET));
			State.roll.		MemWrite(Mem.PtrToAddr(VehiclePointer, ADDRESSES.VEHICLE.ROLL_X_OFFSET));
			State.speed.	MemWrite(Mem.PtrToAddr(VehiclePointer, ADDRESSES.VEHICLE.SPEED_X_OFFSET));
			State.turn.		MemWrite(Mem.PtrToAddr(VehiclePointer, ADDRESSES.VEHICLE.TURN_X_OFFSET));
		}

		public static void Flip() {
			if (!ADDRESSES.IsInVehicle)
				return;
			IntPtr tx = Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.DIR_X_OFFSET);
			IntPtr ty = Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.DIR_Y_OFFSET);
			/*int x = Convert.ToInt32(Convert.ToSingle(Math.Round(Mem.ReadFloat(tx))));
			int y = Convert.ToInt32(Convert.ToSingle(Math.Round(Mem.ReadFloat(tx))));*/
			float x = Mem.ReadFloat(tx);
			float y = Mem.ReadFloat(ty);
			if (fpp(x, -1))
				Mem.WriteFloat(tx, 1.1f);
			else if (fpp(x, 1))
				Mem.WriteFloat(tx, -1.1f);
			else if (fpp(x, 0)) {
				if (fpp(y, -1))
					Mem.WriteFloat(ty, 1.1f);
				else if (fpp(y, 1))
					Mem.WriteFloat(ty, -1.1f);
				else if (fpp(y, 0))
					Mem.WriteFloat(tx, -1.1f);
			}
			/*if (x == 0 && y == 0)
				Mem.WriteFloat(tx, -1.0f);

			if (x == 1 && y == 0)
				Mem.WriteFloat(tx, -1.0f);

			if (x == 0 && y == 1)
				Mem.WriteFloat(ty, -1.0f);

			if (x == 1 && y == 1)
				Mem.WriteFloat(ty, -1.0f);


			if (x == 0 && y == -1)
				Mem.WriteFloat(ty, 1.0f);

			if (x == -1 && y == 0)
				Mem.WriteFloat(tx, 1.0f);

			if (x == -1 && y == -1)
				Mem.WriteFloat(tx, 1.0f);


			if (x == 1 && y == -1)
				Mem.WriteFloat(tx, -1.0f);
			if (x == -1 && y == 1)
				Mem.WriteFloat(ty, -1.0f);
			if (x == -1 && y == -1)
				Mem.WriteFloat(tx, 1.0f);*/
		}
		// LEFT:
		// -1 -1
		// 1 -1
		// -1 1

		public static void InstantStop() {
			Vector speed = new Vector(Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.SPEED_X_OFFSET));
			speed *= 0;
			speed.MemWrite(Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.SPEED_X_OFFSET));
		}

		static bool fpp(float comp, int to) {
			if (to == 0)
				return (comp > -0.5f && comp < 0.5f);
			if (to == 1)
				return (comp > 0.5f);
			if (to == -1)
				return (comp < -0.5f);
			return false;
		}

		static bool _AutoMowerEnabled;
		static bool AutoMowerEnabled {
			get {
				return _AutoMowerEnabled;
			}
			set {
				if (value)
					LoopEngine.FxAdd(AutoMowerTick);
				else
					LoopEngine.FxRemove(AutoMowerTick);
				_AutoMowerEnabled = value;
			}
		}

		static Coords AutoMowerCoords;
		public static void AutoMowerSwitch() {
			if (!ADDRESSES.IsInVehicle)
				return;
			if (!AutoMowerEnabled) {
				AutoMowerCoords = new Vector(Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.COORDS_X_OFFSET));
				AutoMowerEnabled = true;
			} else {
				AutoMowerEnabled = false;
				IntPtr turnZ = Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.TURN_Z_OFFSET);
				Mem.WriteFloat(turnZ, 0.0f);
			}
		}

		static void AutoMowerTick() {
			if (!ADDRESSES.IsInVehicle) {
				AutoMowerEnabled = false;
				return;
			}
			IntPtr turn_z = Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.TURN_Z_OFFSET);
			Mem.WriteFloat(turn_z, 8.0f);

			if (((string)(Config.autoMowerLockAxis.Value)).ToLower().Contains("x")) {
				IntPtr coords_x = Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.COORDS_X_OFFSET);
				Mem.WriteFloat(coords_x, AutoMowerCoords.X);
			}
			if (((string)(Config.autoMowerLockAxis.Value)).ToLower().Contains("y")) {
				IntPtr coords_y = Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.COORDS_Y_OFFSET);
				Mem.WriteFloat(coords_y, AutoMowerCoords.Y);
			}
			if (((string)(Config.autoMowerLockAxis.Value)).ToLower().Contains("z")) {
				IntPtr coords_z = Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.COORDS_Z_OFFSET);
				Mem.WriteFloat(coords_z, AutoMowerCoords.Z);
			}
		}

		public static void SpeedBoost() {
			if (!ADDRESSES.IsInVehicle)
				return;
			Vector speed = new Vector(Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.SPEED_X_OFFSET));
			if (Math.Abs(speed.X) < 0.01f && Math.Abs(speed.Y) < 0.01f) {
				float dirX = Mem.ReadFloat(Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.DIR_X_OFFSET));
				float dirY = Mem.ReadFloat(Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.DIR_Y_OFFSET));
				//float angle = Vector.VecToAngle2D(dirX, dirY);
				Mem.WriteFloat(Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.SPEED_X_OFFSET), dirX * (float)Config.vehicleSpeedMultiplierSet.Value);
				Mem.WriteFloat(Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.SPEED_Y_OFFSET), dirY * (float)Config.vehicleSpeedMultiplierSet.Value);
			}
			speed *= (float)Config.vehicleSpeedMultiplierSet.Value;
			speed.MemWrite(Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.SPEED_X_OFFSET));
		}

		public static void Destroy() {
			if (!ADDRESSES.IsInVehicle)
				return;
			var vState = VehicleGetPhysState(ADDRESSES.VEHICLE.VehiclePointer);
			var cState = ADDRESSES.DISPLAY.CameraGetState();

			vState.speed *= 1000;
			vState.speed.MemWrite(Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.SPEED_X_OFFSET));

			Thread.Sleep(100);

			VehicleSetPhysState(ADDRESSES.VEHICLE.VehiclePointer, vState);
			Thread.Sleep(10);
			VehicleSetPhysState(ADDRESSES.VEHICLE.VehiclePointer, vState);
			ADDRESSES.DISPLAY.CameraSetState(cState);
		}

		public static void DestroyAll() {
			if (ADDRESSES.mode != MODE.STANDALONE)
				return;
			General.KeyPress("bigbang");
		}

		public static Patch FallingOffBike = new Patch((IntPtr)(General.PATCH_BASE + 0x21393D), new byte[] { 0xE9, 0xBC, 0x0E, 0x00, 0x00, 0x90 }, new byte[] { 0x0F, 0x84, 0xBB, 0x0E, 0x00, 0x90 }, 6, "No falling off the bike");
		//Mem.WriteByte(Mem.PtrToAddr(ADDRESES.PLAYER.PlayerPointer, 0x244), 0x01);

		public static void ForwardTeleport() {
			if (!ADDRESSES.IsInVehicle)
				return;
			General.MainForm.SetStatus("Forward teleport (" + DateTime.Now.Ticks + ")");

			Vector roll = new Vector(Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.ROLL_X_OFFSET));
			Vector speed = new Vector(Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.SPEED_X_OFFSET));
			Coords coords = new Coords(Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.COORDS_X_OFFSET));

			General.MainForm.SetStatus("X: " + Math.Round(roll.X, 2) + ", Y: " + Math.Round(roll.Y, 2) + ", Z: " + Math.Round(roll.Z, 2) +
								"// X: " + Math.Round(speed.X, 2) + ", Y: " + Math.Round(speed.Y, 2) + ", Z: " + Math.Round(speed.Z, 2));

			coords.X += -roll.Y * 10;
			coords.Y += roll.X * 10;

			coords.MemWrite(Mem.PtrToAddr(ADDRESSES.VEHICLE.VehiclePointer, ADDRESSES.VEHICLE.COORDS_X_OFFSET));
		}

		public static Mod Weight = new Mod(new [] {
			                                          new ADDRESS_SPEC_PTR(ADDRESSES.VEHICLE.VehiclePointerStandalone, 0xB8, MODE.STANDALONE),
													  new ADDRESS_SPEC_PTR(ADDRESSES.VEHICLE.VehiclePointerVCMP, 0xB8, MODE.VCMP)
		                                          }, typeof(float), "Vehicle weight", true);

		public static void CSetWeight(string Arg) {
			if (!ADDRESSES.IsInVehicle) {
				General.MainForm.SetStatus("Not in vehicle | setVehicleWeight");
				return;
			}
			if (!string.IsNullOrEmpty(Arg)) {
				if (Arg.ToUpper() == "INF") {
					Weight.Set(Single.MaxValue);
					General.MainForm.SetStatus("Vehicle weight set to ∞");
				} else {
					Weight.Set(Single.Parse(Arg));
					General.MainForm.SetStatus("Vehicle weight set to " + Single.Parse(Arg));
				}
			} else {
				if (Math.Abs((float)Weight.Get() - Single.MaxValue) < Single.Epsilon)
					General.MainForm.SetStatus("Vehicle weight is ∞");
				else
					General.MainForm.SetStatus("Vehicle weight is " + Weight.Get());
			}
		}

		public static Patch DisableExplosions = new Patch((IntPtr)(General.PATCH_BASE + 0x188A77), new byte[] { 0x90, 0x90 }, new byte[] { 0x75, 0x09 }, 2, "Disable vehicle explosions");

		public static Patch DriveOnWater = new Patch((IntPtr)(General.PATCH_BASE + 0x193908), new byte[] { 0x90, 0x90 }, new byte[] { 0x74, 0x07 }, 2, "Drive on water");
	}
}
