using System;

namespace Mods {
	class Guns {
		public static void ProfessionalTools() {
			if (ADDRESSES.mode != MODE.STANDALONE)
				return;
			General.KeyPress("professionaltools");
		}

		public static Patch InfiniteAmmo = new Patch(
			MODULE.VCMP, 0xD39E3,
			new byte[] { 0x90, 0x90, 0x90 },
			new byte[] { 0xFF, 0x4E, 0x08 },
			3, "Infinite ammo");

		static bool FastShootEnabled;

		static void FastShootLoop() {
			Mem.WriteUInt32(Mem.PtrToAddr(ADDRESSES.PLAYER.PlayerPointer, ADDRESSES.PLAYER.STATE_OFFSET), (uint)ADDRESSES.PLAYER.STATES.FIRST_PERSON_AIMING);
		}

		public static void EnableFastShoot()
		{
			LoopEngine.FxAdd(FastShootLoop);
			FastShootEnabled = true;
		}

		public static void DisableFastShoot()
		{
			LoopEngine.FxRemove(FastShootLoop);
			FastShootEnabled = false;
		}

		public static bool IsFastShootEnabled() {
			return FastShootEnabled;
		}
	}
}
