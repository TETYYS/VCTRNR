using System;
using static General;

public enum PATCH_STATUS {
	Enabled,
	Disabled
}

public class Patch : IPatch {
	string PatchName;

	public MODULE Module {get;}
	public int[] Offsets {get;}

	public byte[][] EnabledBytes {get;}

	public byte[][] DisabledBytes {get;}

	public byte[] Size {get;}

	public int AddressBlocks {get;}

	public void PatchEnable() {
		if (Module == MODULE.VCMP && VCMP_BASE == 0) {
			MainForm.SetStatus(PatchName + " failed. No VCMP module base.");
			return;
		}
		for (int x = 0; x < AddressBlocks; x++)
			Mem.WriteByteArray((IntPtr)((Module == MODULE.VC ? VC_BASE : VCMP_BASE) + Offsets[x]), EnabledBytes[x]);

		MainForm.SetStatus(PatchName + " is now enabled");
	}
	public void PatchDisable() {
		for (int x = 0; x < AddressBlocks; x++) {
			Mem.WriteByteArray((IntPtr)((Module == MODULE.VC ? VC_BASE : VCMP_BASE) + Offsets[x]), DisabledBytes[x]);
		}
		MainForm.SetStatus(PatchName + " is now disabled");
	}
	public void PatchSwitch() {
		if (PatchStatus() == PATCH_STATUS.Enabled)
			PatchDisable();
		else
			PatchEnable();
	}
	public PATCH_STATUS PatchStatus() {
		bool flag = true;
		for (int x = 0; x < AddressBlocks; x++) {
			if (!ByteCmp(Mem.ReadByteArray((IntPtr)(Module == MODULE.VC ? VC_BASE : VCMP_BASE + Offsets[x]), Size[x]), EnabledBytes[x])) {
				flag = false;
				break;
			}
		}
		return flag ? PATCH_STATUS.Enabled : PATCH_STATUS.Disabled;
	}
	public void CPatchSwitch(string Arg) {
		if (!string.IsNullOrEmpty(Arg)) {
			if (Arg == "1" || Arg == "TRUE" || Arg == "ENABLED" || Arg == "ENABLE" || Arg == "E") {
				PatchEnable();
				MainForm.SetStatus(PatchName + " is now enabled");
			} else if (Arg == "0" || Arg == "FALSE" || Arg == "DISABLED" || Arg == "DISABLE" || Arg == "D") {
				PatchDisable();
				MainForm.SetStatus(PatchName + " is now disabled");
			} else
				MainForm.SetStatus("Syntax error");
		} else
			MainForm.SetStatus(PatchName + " is " + (PatchStatus() == PATCH_STATUS.Enabled ? "Enabled" : "Disabled"));
	}
	public Patch(MODULE Module, int Offset, byte[] EnabledBytes, byte[] DisabledBytes, byte Size, string PatchName) {
		this.Module =			Module;
		this.Offsets =			new[] { Offset };
		AddressBlocks =			1;
		this.EnabledBytes =		new[] { EnabledBytes };
		this.DisabledBytes =	new[] { DisabledBytes };
		this.Size =				new[] { Size };
		this.PatchName =		PatchName;
	}

	public Patch(MODULE Module, int[] Offsets, byte[][] EnabledBytes, byte[][] DisabledBytes, byte[] Size, string PatchName) {
		this.Module =           Module;
		this.Offsets =          Offsets;
		AddressBlocks =         Offsets.Length;
		this.EnabledBytes =		EnabledBytes;
		this.DisabledBytes =	DisabledBytes;
		this.Size =				Size;
		this.PatchName =		PatchName;
	}
}

public interface IPatch {
	byte[] Size {get;}
	MODULE Module {get;}
	int[] Offsets {get;}
	byte[][] EnabledBytes {get;}
	byte[][] DisabledBytes {get;}
	int AddressBlocks {get;}
	void PatchEnable();
	void PatchDisable();
	void PatchSwitch();
	PATCH_STATUS PatchStatus();
}

public struct ADDRESS_SPEC {
	public MODE Mode;
	public IntPtr Address;

	public ADDRESS_SPEC(IntPtr Address, MODE Mode) {
		this.Mode = Mode;
		this.Address = Address;
	}
}

public struct ADDRESS_SPEC_PTR {
	public MODE Mode;
	public IntPtr Address;
	public int Offset;

	public ADDRESS_SPEC_PTR(IntPtr Address, int Offset, MODE Mode) {
		this.Mode = Mode;
		this.Address = Address;
		this.Offset = Offset;
	}
}

public class Mod : IMod {
	object _Address;
	public bool VehicleOnly {get;}

	public Type DataType {get;}

	public string Name {get;}

	public IntPtr Address {
		get {
			var specs = _Address as ADDRESS_SPEC[];
			var specsPtr = _Address as ADDRESS_SPEC_PTR[];
			if (specs != null) {
				for (int x = 0; x < specs.Length; x++) {
					if (specs[x].Mode == ADDRESSES.mode)
						return specs[x].Address;
				}
			}
			if (specsPtr != null) {
				for (int x = 0; x < specsPtr.Length; x++) {
					if (specsPtr[x].Mode == ADDRESSES.mode)
						return Mem.PtrToAddr(specsPtr[x].Address, specsPtr[x].Offset);
				}
			}
			try {
				var spec1 = (ADDRESS_SPEC)(((object[])_Address)[0]);
				var spec2 = (ADDRESS_SPEC_PTR)(((object[])_Address)[1]);

				if (spec1.Mode == ADDRESSES.mode)
					return spec1.Address;
				if (spec2.Mode == ADDRESSES.mode)
					return Mem.PtrToAddr(spec2.Address, spec2.Offset);
			} catch {
				return IntPtr.Zero;
			}
			return IntPtr.Zero;
		}
	}
	public void Set(object Value) {
		if (DataType == typeof(float))
			Mem.WriteFloat(Address, Convert.ToSingle(Value));
		else if (DataType == typeof(uint))
			Mem.WriteUInt32(Address, Convert.ToUInt32(Value));
		else if (DataType == typeof(int))
			Mem.WriteInt32(Address, Convert.ToInt32(Value));
		else if (DataType == typeof(bool))
			Mem.WriteBoolean(Address, Convert.ToBoolean(Value));
		else if (DataType == typeof(byte))
			Mem.WriteByte(Address, Convert.ToByte(Value));
		else
			throw new NotImplementedException("Not implemented data type");
	}
	public object Get() {
		if (DataType == typeof(float))
			return Mem.ReadFloat(Address);
		if (DataType == typeof(uint))
			return Mem.ReadUInt32(Address);
		if (DataType == typeof(int))
			return Mem.ReadInt32(Address);
		if (DataType == typeof(bool))
			return Mem.ReadBoolean(Address);
		if (DataType == typeof(byte))
			return Mem.ReadByte(Address);
		throw new NotImplementedException("Not implemented data type");
    }

	public Mod(ADDRESS_SPEC[] Address, Type DataType, string Name, bool VehicleOnly = false) {
		_Address = Address;
		this.DataType = DataType;
		this.Name = Name;
		this.VehicleOnly = VehicleOnly;
	}

	public Mod(ADDRESS_SPEC_PTR[] Address, Type DataType, string Name, bool VehicleOnly = false) {
		_Address = Address;
		this.DataType = DataType;
		this.Name = Name;
		this.VehicleOnly = VehicleOnly;
	}

	public Mod(ADDRESS_SPEC Address, ADDRESS_SPEC_PTR Address2, Type DataType, string Name, bool VehicleOnly = false) {
		_Address = new object[2];
		((object[])_Address)[0] = Address;
		((object[])_Address)[1] = Address2;
		this.DataType = DataType;
		this.Name = Name;
		this.VehicleOnly = VehicleOnly;
	}

	public void CModSet(string Arg) {
		if (!string.IsNullOrEmpty(Arg)) {
			if (Arg.ToUpper() == "INF") {
				Set(Single.MaxValue);
				General.MainForm.SetStatus(Name + " set to ∞");
			} else {
				Set(Single.Parse(Arg));
				General.MainForm.SetStatus(Name + " set to " + Get());
			}
		} else {
			if (Math.Abs((float)Get() - Single.MaxValue) < Single.Epsilon)
				General.MainForm.SetStatus(Name + " is ∞");
			else
				General.MainForm.SetStatus(Name + " is " + Get());
		}
	}
}

public interface IMod {
	IntPtr Address {get;}
	Type DataType {get;}
	string Name {get;}
	bool VehicleOnly {get;}
	void Set(object Value);
	object Get();
}