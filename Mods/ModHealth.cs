using System;
using System.Diagnostics;
using System.Threading;

using static General;

namespace Mods {
	public class Health {
		public static Mod HP = new Mod(new[]	{	new ADDRESS_SPEC_PTR(ADDRESSES.PLAYER.PlayerPointerStandalone, 0x354, MODE.STANDALONE),
													new ADDRESS_SPEC_PTR(ADDRESSES.PLAYER.PlayerPointerVCMP, 0x524, MODE.VCMP)
												},
												typeof(float), "Health");

		public static Mod VehicleHP = new Mod(new[]	{	new ADDRESS_SPEC_PTR(ADDRESSES.VEHICLE.VehiclePointerStandalone, 0x204, MODE.STANDALONE),
														new ADDRESS_SPEC_PTR(ADDRESSES.VEHICLE.VehiclePointerVCMP, 0x204, MODE.VCMP)
													},
												typeof(float), "Vehicle health");

		public static void Heal() {
			//ptr = Mem.PtrToAddr(ADDRESES.PLAYER.PlayerPointer, ADDRESES.PLAYER.AP_OFFSET);
			//Mem.WriteFloat(ptr, apSet);
			if (ADDRESSES.IsInVehicle)
				VehicleHP.Set(Config.vehicleHpSet.Value);
			else
				HP.Set(Config.hpSet.Value);
			//TODO: Fix wheels or maybe chassis?
			PromptWrite(Colors.blue + "Health & armor cheat");
		}

		public static Patch GodMode = new Patch(
			(IntPtr)(PATCH_BASE + 0x1267DC),
			new byte[] { 0xEB, 0x10 },
			new byte[] { 0x75, 0x15 },
			2,
			"God Mode");

		public static Patch VehicleGodMode = new Patch(
			new[] {
				(IntPtr)(PATCH_BASE + 0x19C2AA),
				(IntPtr)(PATCH_BASE + 0x188A77)
			},
			new[] {
				new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 },
				new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 } // ???
			},
			new[] {
				new byte[] { 0xD8, 0xAB, 0x04, 0x02, 0x00, 0x00, 0xD9, 0x9B, 0x04, 0x02, 0x00, 0x00 },
				new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 } // ???
			},
			new byte[]  {
				12,
				6
			},
			"Vehicle God Mode");

		public static ConfigVar lockVehicleHp = new ConfigVar(false, "LockVehicleHp", LockVehicleHpTick);
		public static ConfigVar lockPlayerHp = new ConfigVar(false, "LockPlayerHp", LockPlayerHpTick);

		public static void LockVehicleHpTick() {
			if (ADDRESSES.IsInVehicle)
				VehicleHP.Set((float)Config.vehicleHpSet.Value);
		}

		public static void LockPlayerHpTick() {
			if (!ADDRESSES.IsInVehicle)
				HP.Set((float)Config.hpSet.Value);
		}
	}
}
