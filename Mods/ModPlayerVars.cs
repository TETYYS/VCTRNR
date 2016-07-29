using System;
using static General;

namespace Mods {
	class PlayerVars {
		public static Mod Money = new Mod(new[] { new ADDRESS_SPEC((IntPtr)0x94ADC8, MODE.STANDALONE) }, typeof(uint), "Money");

		public static void MoneyAdd(uint Amount) {
			if (ADDRESSES.mode != MODE.STANDALONE)
				return;
			Money.Set(((uint)Money.Get() + Amount));
		}

		public static Mod Weight = new Mod(new[] {	new ADDRESS_SPEC_PTR(ADDRESSES.PLAYER.PlayerPointerStandalone, 0xB8, MODE.STANDALONE),
													new ADDRESS_SPEC_PTR(ADDRESSES.PLAYER.PlayerPointerVCMP, 0xB8, MODE.VCMP),
		                                         }, typeof(float), "Player weight");

		public static Mod Stamina = new Mod(new [] {	new ADDRESS_SPEC_PTR(ADDRESSES.PLAYER.PlayerPointerStandalone, 0x600, MODE.STANDALONE),
														new ADDRESS_SPEC_PTR(ADDRESSES.PLAYER.PlayerPointerVCMP, 0x600, MODE.VCMP),
		                                           }, typeof(float), "Player stamina");

		public static Patch InfiniteRun = new Patch(
			MODULE.VC, 0x136F25,
			new byte[] { 0xEB },
			new byte[] { 0x75 }, 1, "Infinite run");
	}
}
