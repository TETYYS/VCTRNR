using System;

namespace Mods {
	public class Cops {
		public IntPtr[] Address => null;
		public int AddressBlocks => 0;
		public byte[][] DisabledBytes => null;
		public byte[][] EnabledBytes => null;
		public byte[] Size => new byte[0];

		public static void PatchEnable() {
			if (ADDRESSES.mode != MODE.STANDALONE)
				return;
			General.KeyPress("leavemealone");
		}

		public static void PatchDisable() {
			if (ADDRESSES.mode != MODE.STANDALONE)
				return;
			General.KeyPress("youwonttakemealive");
		}

		public static PATCH_STATUS PatchStatus() {
			if (ADDRESSES.mode != MODE.STANDALONE)
				return PATCH_STATUS.Disabled;
			IntPtr pWantedLevel = Mem.PtrToAddr(ADDRESSES.PLAYER.PlayerPointerStandalone, 0x5F4);
			return Mem.ReadUInt32(Mem.PtrToAddr(pWantedLevel, 0x00)) == 0 ? PATCH_STATUS.Enabled : PATCH_STATUS.Disabled;
		}

		public static void PatchSwitch() {
			if (ADDRESSES.mode != MODE.STANDALONE)
				return;
			if (PatchStatus() == PATCH_STATUS.Enabled)
				PatchDisable();
			else
				PatchEnable();
		}
	}
}