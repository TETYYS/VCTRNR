﻿using System;
using System.Diagnostics;
using System.Threading;

using static General;

namespace Mods {
	public class Health {
		public static Mod HP = new Mod(new[]	{	new ADDRESS_SPEC_PTR(ADDRESSES.PLAYER.PlayerPointerStandalone, 0x354, MODE.STANDALONE),
													new ADDRESS_SPEC_PTR(ADDRESSES.PLAYER.PlayerPointerVCMP, 0x524, MODE.VCMP)
												},
												typeof(float), "Health");

		public static Mod VehicleHP = new Mod(new[]	{	new ADDRESS_SPEC_PTR(ADDRESSES.VEHICLE.VehiclePointerVCMP, 0x204, MODE.VCMP),
														new ADDRESS_SPEC_PTR(ADDRESSES.VEHICLE.VehiclePointerStandalone, 0x204, MODE.STANDALONE)
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
			MODULE.VC,
			new[] {
				0x1267DC,
				0x1267D5
			},
			new[] {
				new byte[] { 0xEB, 0x10 },
				new byte[] { 0x90, 0x90 }
			},
			new[] {
				new byte[] { 0x75, 0x15 },
				new byte[] { 0x75, 0x1C }
			},
			new byte[] {
				2,
				2
			},
			"God Mode");

		public static Patch VehicleGodMode = new Patch(
			MODULE.VC,
			new[] {
				0x1A9801,
				0x188A77
			},
			new[] {
				new byte[] { 0xc7, 0x41, 0x04, 0x00, 0x00, 0x00, 0x00, 0xc2, 0x04 },
				new byte[] { 0x90, 0x90 }
			},
			new[] {
				new byte[] { 0x88, 0x41, 0x04, 0xc2, 0x04, 0x00, 0x00, 0x00, 0x00 },
				new byte[] { 0x75, 0x09 }
			},
			new byte[]  {
				9,
				2
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
