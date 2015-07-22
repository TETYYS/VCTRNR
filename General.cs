using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using GTA_Vice_City_kodai;

using MouseKeyboardLibrary;

class General {

	public static Main MainForm;
	public static readonly uint PATCH_BASE = 0x400000;
	public const float PI = (float)Math.PI;
	public const float HALF_PI = PI / 2;

	internal static bool ByteCmp(byte[] In, byte[] Cmp) {
		if (In.Length != Cmp.Length)
			return false;
		for (int x = 0; x < In.Length; x++) {
			if (In[x] != Cmp[x]) {
				return false;
			}
		}
		return true;
	}

	public static void PromptWrite(string str) {
		str = str + '\0';
		Mem.WriteStringUnicode(ADDRESSES.TEXT1_ADDR, str);
		Thread.Sleep(10);
		Mem.WriteStringUnicode(ADDRESSES.TEXT2_ADDR, str);
	}

	[DllImport("user32.dll", CharSet = CharSet.Unicode)]
	private static extern short VkKeyScan(char ch);
	public static Keys toKey(char ch) {
		short vkey = VkKeyScan(ch);
		var retval = (Keys)(vkey & 0xff);
		int modifiers = vkey >> 8;
		if ((modifiers & 1) != 0)
			retval |= Keys.Shift;
		if ((modifiers & 2) != 0)
			retval |= Keys.Control;
		if ((modifiers & 4) != 0)
			retval |= Keys.Alt;
		return retval;
	}
	public static void KeyPress(string keys) {
		KeyEngine.KeyTimeout.Reset();
		KeyEngine.DiscardKeys = true;
		{
			char[] chrs = keys.ToCharArray();
			for (int x = 0; x < chrs.Length; x++) {
				KeyboardSimulator.KeyPress(toKey(chrs[x]));
			}
		}
		KeyEngine.KeyTimeout.Start();
		KeyEngine.DiscardKeys = false;
	}
}
class Vector {
	private float[] vec;

	public float X {
		get { return vec[0]; }
		set { vec[0] = value; }
	}
	public float Y {
		get { return vec[1]; }
		set { vec[1] = value; }
	}

	public float Z {
		get { return vec[2]; }
		set { vec[2] = value; }
	}

	public Vector(float X, float Y, float Z) {
		vec = new float[3];
		this.X = X;
		this.Y = Y;
		this.Z = Z;
	}

	public Vector() {
		vec = new float[3];
		X = 0.0f;
		Y = 0.0f;
		Z = 0.0f;
	}

	public Vector(IntPtr Memory) {
		vec = new float[3];
		X = Mem.ReadFloat(Memory);
		Y = Mem.ReadFloat((IntPtr)(Memory.ToInt32() + sizeof(float)));
		Z = Mem.ReadFloat((IntPtr)(Memory.ToInt32() + (sizeof(float) * 2)));
	}

	public static Vector operator +(Vector a, Vector b) {
		Vector ret = new Vector();
		ret.X = a.X + b.X;
		ret.Y = a.Y + b.Y;
		ret.Z = a.Z + b.Z;
		return ret;
	}

	public static Vector operator -(Vector a, Vector b) {
		Vector ret = new Vector();
		ret.X = a.X - b.X;
		ret.Y = a.Y - b.Y;
		ret.Z = a.Z - b.Z;
		return ret;
	}

	public static Vector operator *(Vector a, Vector b) {
		Vector ret = new Vector();
		ret.X = a.X * b.X;
		ret.Y = a.Y * b.Y;
		ret.Z = a.Z * b.Z;
		return ret;
	}

	public static Vector operator *(Vector a, float b) {
		Vector ret = new Vector();
		ret.X = a.X * b;
		ret.Y = a.Y * b;
		ret.Z = a.Z * b;
		return ret;
	}

	public static Vector operator /(Vector a, Vector b) {
		Vector ret = new Vector();
		ret.X = a.X / b.X;
		ret.Y = a.Y / b.Y;
		ret.Z = a.Z / b.Z;
		return ret;
	}

	public float Length() {
		return (float)Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
	}

	public void Normalize() {
		float len = Length();
		X /= len;
		Y /= len;
		Z /= len;
	}

	public static float VecToAngle2D(float X, float Y) {
		return (float)Math.Atan2(Y, X);
	}
	public void MemWrite(IntPtr Memory) {
		Mem.WriteFloat(Memory, X);
		Mem.WriteFloat((IntPtr)(Memory.ToInt32() + sizeof(float)), Y);
		Mem.WriteFloat((IntPtr)(Memory.ToInt32() + (sizeof(float) * 2)), Z);
	}
}