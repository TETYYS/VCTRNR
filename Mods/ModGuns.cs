using System;

namespace Mods {
	class Guns {
		public static void ProfessionalTools() {
			if (ADDRESSES.mode != MODE.STANDALONE)
				return;
			General.KeyPress("professionaltools");
		}

		public static Patch NoReload = new Patch(	(IntPtr)(General.PATCH_BASE + 0x1D4ABE),
													new byte[] { 0x90, 0x90, 0x90 },
													new byte[] { 0xFF, 0x4E, 0x08 },
													3, "No reload");

		public static Patch FastShoot = new Patch(	(IntPtr)(General.PATCH_BASE + 0x1D4ABE),
													new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 },
													new byte[] { 0x0F, 0x84, 0xF2, 0x02, 0x00, 0x00 },
													6, "Fast shoot");
	}
}
