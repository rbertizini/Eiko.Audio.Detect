using CSCore.CoreAudioAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eiko.Audio.Detect
{
    public partial class FrmAudio : Form
    {
        int qtdSec = 0;

        public FrmAudio()
        {
            InitializeComponent();
            SetPropertiesForStylesTabSwitches();
        }

        public void SetPropertiesForStylesTabSwitches()
        {
            tgsAtivar.Style = JCS.ToggleSwitch.ToggleSwitchStyle.Android;
            tgsAtivar.Size = new Size(78, 23);
            tgsAtivar.OnText = "LIG";
            tgsAtivar.OnFont = new Font(this.Font.FontFamily, 8, FontStyle.Bold);
            tgsAtivar.OnForeColor = Color.White;
            tgsAtivar.OffText = "DESL";
            tgsAtivar.OffFont = new Font(this.Font.FontFamily, 8, FontStyle.Bold);
            tgsAtivar.OffForeColor = Color.FromArgb(141, 123, 141);

            tgsTopo.Style = JCS.ToggleSwitch.ToggleSwitchStyle.Android;
            tgsTopo.Size = new Size(78, 23);
            tgsTopo.OnText = "LIG";
            tgsTopo.OnFont = new Font(this.Font.FontFamily, 8, FontStyle.Bold);
            tgsTopo.OnForeColor = Color.White;
            tgsTopo.OffText = "DESL";
            tgsTopo.OffFont = new Font(this.Font.FontFamily, 8, FontStyle.Bold);
            tgsTopo.OffForeColor = Color.FromArgb(141, 123, 141);
        }

        //Processo de audio
        public MMDevice GetDefaultRenderDevice()
        {
            using (var enumerator = new MMDeviceEnumerator())
            {
                return enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
            }
        }

        //Detecção de audio
        public bool IsAudioPlaying(MMDevice device, out float peakVal)
        {
            using (var meter = AudioMeterInformation.FromDevice(device))
            {
                peakVal = meter.PeakValue;
                return meter.PeakValue > 0;
            }
        }

        //Inicialização
        private void FrmAudio_Activated(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(35, 32, 35);
            this.Icon = Properties.Resources.icon_alert_blue;
        }

        private void chkAtivar_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void timeProc_Tick(object sender, EventArgs e)
        {
            float peakVal = 0;

            if (IsAudioPlaying(GetDefaultRenderDevice(), out peakVal))
            {
                lbPeak.Text = peakVal.ToString();
                lbcount.Text = qtdSec.ToString();


                //Maior sensibilidade - pende para o vermelho
                if (peakVal > 0.001)
                {
                    if (qtdSec < 5)
                        qtdSec++;
                }
                else
                {
                    if (qtdSec > 0)
                        qtdSec--;
                }

                if (qtdSec >= 3)
                {
                    this.Icon = Properties.Resources.icon_alert_red;
                    picAlert.Image = Properties.Resources.alert_red;
                }
                else
                {
                    this.Icon = Properties.Resources.icon_alert_yellow;
                    picAlert.Image = Properties.Resources.alert_yellow;
                }


                //Menor sensibilidade - pende para o amarelo
                //if (peakVal > 0.001)
                //{
                //    if (qtdSec >= 3)
                //    {
                //        this.Icon = Properties.Resources.icon_alert_red;
                //        picAlert.Image = Properties.Resources.alert_red;
                //        qtdSec = 5;
                //    }
                //    else
                //    {
                //        if (picAlert.Image == Properties.Resources.alert_red)
                //        {
                //            qtdSec = 1;
                //        }
                //
                //        this.Icon = Properties.Resources.icon_alert_yellow;
                //        picAlert.Image = Properties.Resources.alert_yellow;
                //        qtdSec++;
                //    }
                //}
                //else
                //{
                //    this.Icon = Properties.Resources.icon_alert_yellow;
                //    picAlert.Image = Properties.Resources.alert_yellow;
                //    if (qtdSec > 0)
                //        qtdSec--;
                //}
            }
            else
            {
                this.Icon = Properties.Resources.icon_alert_green;
                picAlert.Image = Properties.Resources.alert_green;
                if (qtdSec > 0)
                    qtdSec--;
            }
        }

        private void tgsAtivar_CheckedChanged(object sender, EventArgs e)
        {
            qtdSec = 0;
            if (tgsAtivar.Checked == true)
            {
                this.Icon = Properties.Resources.icon_alert_green;
                picAlert.Image = Properties.Resources.alert_green;
                timeProc.Enabled = true;
            }
            else
            {
                this.Icon = Properties.Resources.icon_alert_blue;
                picAlert.Image = Properties.Resources.alert_blue;
                timeProc.Enabled = false;
            }
        }

        private void tgsTopo_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = tgsTopo.Checked;
        }

        private void btnConf_Click(object sender, EventArgs e)
        {
            if (this.Size.Height == 263)
                this.Size = new Size(203, 309);
            else
                this.Size = new Size(203, 263);
        }
    }
}
