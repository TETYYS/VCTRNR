using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using MouseKeyboardLibrary;

using Mods;

namespace GTA_Vice_City_kodai {
	public partial class Main : Form {
		public static bool readCfg = false;

		public Main() {
			InitializeComponent();
		}

		private void Form_Load(object sender, EventArgs e) {
			General.MainForm = this;
			Text = "GTA: Vice City cheat codes (v" + Assembly.GetExecutingAssembly().GetName().Version + ")";
			Mem.init("gta-vc");
			if (!Mem.CheckProcess()) {
				t_status.Text = "GTA VC not found! Type \"Refresh\" when ready.";
			}

			LoopEngine.FxAdd(() => {
				if (ADDRESSES.prevIsInVehicle != ADDRESSES.IsInVehicle) {
					if (ADDRESSES.IsInVehicle)
						Config.LoadInVehicleConfig();
					else
						Config.LoadOnFootConfig();
					SetStatus(ADDRESSES.IsInVehicle ? "On vehicle" : "On foot");
				}
				ADDRESSES.prevIsInVehicle = ADDRESSES.IsInVehicle;
			});

			LoopEngine.Start();
			Config.LoadGlobalConfig();
			KeyEngine.Start();
		}

		public byte[] savedVehicle;
		public void SetStatus(string In) {
			Invoke(new MethodInvoker(() => {
				t_status.Text = In;
			}));
		}

		private void b_exec_Click(object sender, EventArgs e) {
			CommandEngine.Parse(t_command.Text);
		}

		private void Main_FormClosing(object sender, FormClosingEventArgs e) {
			Environment.Exit(0);
		}
	}
}
