using System;

namespace Mods {
	class Game {
		public static Mod FPS = new Mod(new [] {	new ADDRESS_SPEC((IntPtr)0x9B48EC, MODE.STANDALONE),
													new ADDRESS_SPEC((IntPtr)0x9B48EC, MODE.VCMP),
											   }, typeof(int), "Max game FPS");

		public static Mod HUD = new Mod(new [] {	new ADDRESS_SPEC((IntPtr)0x86963A, MODE.STANDALONE),
													new ADDRESS_SPEC((IntPtr)0x86963A, MODE.VCMP),
		                                       }, typeof(byte), "HUD visible");
	}
}
