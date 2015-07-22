using System;
using System.Collections.Generic;
using System.Threading;

class LoopEngine {
	static List<Action> FxTable = new List<Action>();
	public static void FxAdd(Action Fx) {
		FxTable.Add(Fx);
	}

	public static void FxRemove(Action Fx) {
		FxTable.Remove(Fx);
	}

	public static void Start() {
		new Thread(MainLoop).Start();
	}
	static void MainLoop() {
		while (true) {
			for (int x = 0; x < FxTable.Count; x++) {
				FxTable[x].Invoke();
			}


		/*int driftPoints = 0;
		int driftRecord = 0;
		var driftSw = new Stopwatch();
		var driftWr = new Stopwatch();
		var driftRec = new Stopwatch();
		driftSw.Start();
		driftWr.Start();
		driftRec.Start();*/
		//var rand = new Random(Environment.ProcessorCount);


			/*if (Mem.ReadByte(Mem.PtrToAddr(ADDRESES.VEHICLE.VEHICLE_POINTER, ADDRESES.VEHICLE.BURNOUT_OFFSET)) == 2)
			{
				bool isRecord = false;
				if (driftSw.ElapsedMilliseconds > 10)
				{
					driftSw.Restart();
					//0.0065
					int isOnGround =  Mem.ReadInt32(Mem.PtrToAddr(ADDRESES.VEHICLE.VEHICLE_POINTER, ADDRESES.VEHICLE.IS_ON_GROUND));
					if (/*Math.Abs(Mem.ReadFloat(Mem.PtrToAddr(ADDRESES.VEHICLE.VEHICLE_POINTER, ADDRESES.VEHICLE.DIR_Z_OFFSET))) > 0.0065 && *isOnGround == 65536 || isOnGround == 68096)
					{
						//COMPLETE:
						driftPoints += (int)(Mem.ReadFloat(ADDRESES.VEHICLE.SPEED_ADDR) * 14 * Math.Abs(Mem.ReadFloat(Mem.PtrToAddr(ADDRESES.VEHICLE.VEHICLE_POINTER, ADDRESES.VEHICLE.DIR_Z_OFFSET))));
						if (driftPoints > driftRecord)
						{
							driftRecord = driftPoints;
							isRecord = true;
						}
						//write(colors.green + "DRIFT: " + driftPoints + "!!!");
					}
				}
				if (driftWr.ElapsedMilliseconds > 1)
				{
					string color;
					if (driftPoints > 20000) color = colors.green;
					else if (driftPoints > 12000) color = colors.orange;
					else if (driftPoints > 8000) color = colors.yellow;
					else if (driftPoints > 6000) color = colors.pink;
					else if (driftPoints > 5000) color = colors.purpleAndMorePink;
					else if (driftPoints > 4000) color = colors.purpleAndPink;
					else if (driftPoints > 3000) color = colors.purple;
					else if (driftPoints > 2000) color = colors.cyan;
					else if (driftPoints > 1000) color = colors.blue;
					else color = colors.white;
					if (driftPoints > 700)
						Mem.WriteStringUnicode(ADDRESES.MISSION_TEXT_ADDR, color + "DRIFT: " + driftPoints + "!!!\0\0\0\0\0");
					if (isRecord) write("New drift record!");
					isRecord = false;
					driftWr.Restart();
				}
				if (driftRec.ElapsedMilliseconds > 200)
				{
					//write("Current drift record: " + driftRecord);
					driftRec.Restart();
				}
			}
			else if (Mem.ReadByte(Mem.PtrToAddr(ADDRESES.VEHICLE.VEHICLE_POINTER, ADDRESES.VEHICLE.BURNOUT_OFFSET)) == 0 || Mem.ReadFloat(ADDRESES.VEHICLE.SPEED_ADDR) < 10)
			{
				if (driftSw.ElapsedMilliseconds > 300)
				{
					driftPoints = 0;
				}
			}*/
			Thread.Sleep(1);
		}
	}
}
