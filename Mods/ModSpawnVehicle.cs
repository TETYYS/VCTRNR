using System;
using static General;

namespace Mods {
	class SpawnVehicle {
		public static int CarSelectIndex;

		/*
		 public static int[] sortCars(string[] strs, Type enum_) {
			var ints = new int[strs.Length];
			var ret = new int[strs.Length];
			for (int x = 0; x < ints.Length; x++)
				ints[x] = (int)Enum.GetValues(enum_).GetValue(x);
			for (int x = 0; x < strs.Length; x++)
				for (int i = 0, c = 0; i < ints.Length; i++, c++)
					if (Enum.GetName(enum_, ints[i]) == strs[x]) {
						ret[c] = ints[i];
						c++;
					}
			return ret;
		}
		 */

		public static void SelectNext() {
			if (ADDRESSES.mode != MODE.STANDALONE)
				return;
			CarSelectIndex++;
			if (CarSelectIndex > Vehicle.Vehicles.Length - 1)
				CarSelectIndex = 0;
			else if (CarSelectIndex < 0)
				CarSelectIndex = Vehicle.Vehicles.Length - 1;

			var vehicle = Vehicle.Vehicles[CarSelectIndex];
			if (vehicle.name == "Anche" || vehicle.name == "Cj600") {
				PromptWrite(vehicle.name + " WARNING: STRANGE CAR");
			} else {
				PromptWrite(vehicle.name);
			}
			MainForm.SetStatus("Selected car ID " + vehicle.id + " (" + vehicle.name + ")");
		}

		public static void SelectPrevious() {
			if (ADDRESSES.mode != MODE.STANDALONE)
				return;
			CarSelectIndex--;
			if (CarSelectIndex > Vehicle.Vehicles.Length - 1)
				CarSelectIndex = 0;
			else if (CarSelectIndex < 0)
				CarSelectIndex = Vehicle.Vehicles.Length - 1;
			var vehicle = Vehicle.Vehicles[CarSelectIndex];
			if (vehicle.name == "Anche" || vehicle.name == "Cj600") {
				PromptWrite(vehicle.name + " WARNING: STRANGE CAR");
			} else {
				PromptWrite(vehicle.name);
			}
			MainForm.SetStatus("Selected car ID " + vehicle.id + " (" + vehicle.name + ")");
		}

		public static void Spawn() {
			if (ADDRESSES.mode != MODE.STANDALONE)
				return;
			//int[] values = arrayToIntArray(Enum.GetValues(typeof(Cars)));
			//var values = Enum.GetValues(typeof(Cars));
			//var names = values.OrderBy(x => Enum.GetNames(typeof(Cars))).ToArray();
			//string[] names = Enum.GetNames(typeof(Cars));
			//int[] values_ = Enum.GetValues(typeof(Cars));
			//Array.Sort(names);
			//values = sortCars(names, typeof(Cars));
			//var values = Enum.GetValues(typeof(Cars));
			//var names = Enum.GetNames(typeof(Cars));
			//var sorted = new List<Cars>();
			//names = names.OrderBy(e => e.ToString()).ToArray();
			var vehicle = Vehicle.Vehicles[CarSelectIndex];
			if (vehicle.name != "Coastg" || vehicle.name != "Deaddodo") {
				Mem.WriteUInt32((IntPtr)0x4ACA36, vehicle.id);
				KeyPress(ADDRESSES.CAR_SPAWN_STRING);
			} else
				PromptWrite("This " + Colors.green + "\"car\"" + Colors.white + " causes game to crash.");
		}

		public static void SpawnTurbo() {
			if (ADDRESSES.mode != MODE.STANDALONE)
				return;
			KeyPress("gettherefast");
		}
	}

	class Vehicle {
		public struct TVehicle {
			public string name;
			public byte id;

			public TVehicle(string Name, byte Id) {
				name = Name;
				id = Id;
			}
		}

		public static TVehicle[] Vehicles = {
		new TVehicle("Addy", 187),
		new TVehicle("Admiral", 175),
		new TVehicle("Aggio", 192),
		new TVehicle("Airtrain", 180),
		new TVehicle("Angel", 166),
		new TVehicle("Ambulance", 146),

		new TVehicle("Baggage", 215),
		new TVehicle("Banshee", 159),
		new TVehicle("Barracks", 163),
		new TVehicle("Benson", 229),
		new TVehicle("Bfinject", 154),
		new TVehicle("Blistac", 226),
		new TVehicle("Bloodra", 234),
		new TVehicle("Bloodrb", 235),
		new TVehicle("Bobcat", 152),
		new TVehicle("Boxville", 228),
		new TVehicle("Burrito", 212),
		new TVehicle("Bus", 161),

		new TVehicle("Cabbie", 168),
		new TVehicle("Cheetah", 145),
		new TVehicle("Chopper", 165),
		new TVehicle("Coach", 167),
		new TVehicle("Coastg", 202),
		new TVehicle("Comet", 210),
		new TVehicle("Cuban", 164),

		new TVehicle("Deaddodo", 181),
		new TVehicle("Deluxo", 211),
		new TVehicle("Dinghy", 203),

		new TVehicle("Enforcer", 157),
		new TVehicle("Esperant", 149),

		new TVehicle("Fbicar", 147),
		new TVehicle("Fbiranch", 220),
		new TVehicle("Flatbed", 185),
		new TVehicle("Firetruk", 137),
		new TVehicle("Freeway", 193),

		new TVehicle("Gangbur", 179),
		new TVehicle("Glendale", 196),
		new TVehicle("Greenwood", 222),

		new TVehicle("Hermes", 204),
		new TVehicle("Hotrina", 232),
		new TVehicle("Hotrinb", 233),
		new TVehicle("Hotring", 224),
		new TVehicle("Hunter", 155),

		new TVehicle("Idaho", 131),
		new TVehicle("Infernus", 141),

		new TVehicle("Jetmax", 223),

		new TVehicle("Kaufman", 216),

		new TVehicle("Landstal", 130),
		new TVehicle("Linerun", 133),
		new TVehicle("Lovefist", 201),

		new TVehicle("Manana", 140),
		new TVehicle("Marquis", 214),
		new TVehicle("Maverick", 217),
		new TVehicle("Mesa", 230),
		new TVehicle("Moonbeam", 148),
		new TVehicle("Mrwhoop", 153),
		new TVehicle("Mule", 144),

		new TVehicle("Oceanic", 197),

		new TVehicle("Packer", 173),
		new TVehicle("Patriot", 200),
		new TVehicle("Pcj600", 191),
		new TVehicle("Peren", 134),
		new TVehicle("Pheonix", 207),
		new TVehicle("Pizzaboy", 178),
		new TVehicle("Police", 156),
		new TVehicle("Polmav", 227),
		new TVehicle("Pony", 143),
		new TVehicle("Predator", 160),

		new TVehicle("Rancher", 219),
		new TVehicle("Rcbandit", 171),
		new TVehicle("Rcbaron", 194),
		new TVehicle("Rcgoblin", 231),
		new TVehicle("Rcraider", 195),
		new TVehicle("Reefer", 183),
		new TVehicle("Regina", 209),
		new TVehicle("Rhino", 162),
		new TVehicle("Rio", 136),
		new TVehicle("Romero", 172),
		new TVehicle("Rumpo", 170),

		new TVehicle("Sabre", 205),
		new TVehicle("Sabretur", 206),
		new TVehicle("Sanchez", 198),
		new TVehicle("Sandking", 225),
		new TVehicle("Seaspar", 177),
		new TVehicle("Securica", 158),
		new TVehicle("Sentinel", 135),
		new TVehicle("Sentxs", 174),
		new TVehicle("Skimmer", 190),
		new TVehicle("Spand", 213),
		new TVehicle("Sparro", 199),
		new TVehicle("Speeder", 182),
		new TVehicle("Squalo", 176),
		new TVehicle("Stallion", 169),
		new TVehicle("Stinger", 132),
		new TVehicle("Stretch", 139),

		new TVehicle("Taxi", 150),
		new TVehicle("Topfun", 189),
		new TVehicle("Trash", 138),
		new TVehicle("Tropic", 184),

		new TVehicle("Vcnmav", 218),
		new TVehicle("Vicechee", 236),
		new TVehicle("Virgo", 221),
		new TVehicle("Voodoo", 142),

		new TVehicle("Walton", 208),
		new TVehicle("Washing", 151),

		new TVehicle("Yankee", 186),

		new TVehicle("Zebra", 188)
	};
		public enum Cars {

			Addy = 187,
			Admiral = 175,
			Aggio = 192,
			Airtrain = 180,
			Angel = 166,
			Ambulance = 146,

			Baggage = 215,
			Banshee = 159,
			Barracks = 163,
			Benson = 229,
			Bfinject = 154,
			Blistac = 226,
			Bloodra = 234,
			Bloodrb = 235,
			Bobcat = 152,
			Boxville = 228,
			Burrito = 212,
			Bus = 161,

			Cabbie = 168,
			Cheetah = 145,
			Chopper = 165,
			Coach = 167,
			Coastg = 202,
			Comet = 210,
			Cuban = 164,

			Deaddodo = 181,
			Deluxo = 211,
			Dinghy = 203,

			Enforcer = 157,
			Esperant = 149,

			Fbicar = 147,
			Fbiranch = 220,
			Flatbed = 185,
			Firetruk = 137,
			Freeway = 193,

			Gangbur = 179,
			Glendale = 196,
			Greenwood = 222,

			Hermes = 204,
			Hotrina = 232,
			Hotrinb = 233,
			Hotring = 224,
			Hunter = 155,

			Idaho = 131,
			Infernus = 141,

			Jetmax = 223,

			Kaufman = 216,

			Landstal = 130,
			Linerun = 133,
			Lovefist = 201,

			Manana = 140,
			Marquis = 214,
			Maverick = 217,
			Mesa = 230,
			Moonbeam = 148,
			Mrwhoop = 153,
			Mule = 144,

			Oceanic = 197,

			Packer = 173,
			Patriot = 200,
			Pcj600 = 191,
			Peren = 134,
			Pheonix = 207,
			Pizzaboy = 178,
			Police = 156,
			Polmav = 227,
			Pony = 143,
			Predator = 160,

			Rancher = 219,
			Rcbandit = 171,
			Rcbaron = 194,
			Rcgoblin = 231,
			Rcraider = 195,
			Reefer = 183,
			Regina = 209,
			Rhino = 162,
			Rio = 136,
			Romero = 172,
			Rumpo = 170,

			Sabre = 205,
			Sabretur = 206,
			Sanchez = 198,
			Sandking = 225,
			Seaspar = 177,
			Securica = 158,
			Sentinel = 135,
			Sentxs = 174,
			Skimmer = 190,
			Spand = 213,
			Sparro = 199,
			Speeder = 182,
			Squalo = 176,
			Stallion = 169,
			Stinger = 132,
			Stretch = 139,

			Taxi = 150,
			Topfun = 189,
			Trash = 138,
			Tropic = 184,

			Vcnmav = 218,
			Vicechee = 236,
			Virgo = 221,
			Voodoo = 142,

			Walton = 208,
			Washing = 151,

			Yankee = 186,

			Zebra = 188,
		}
	}
}
