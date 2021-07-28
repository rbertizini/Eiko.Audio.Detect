using CSCore.CoreAudioAPI;
using JCS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eiko.Audio.Detect
{
    public partial class FrmAudio : Form
    {
        int qtdSec = 0;
        int secMem = 0;
        int secAudio = 0;
        int secAudioWait = 0;

        int cfgTmpMem = 0;
        int cfgTmpRef = 0;
        float cfgLimPeak = 0;
        int cfgTpSens = 0;

        public FrmAudio()
        {
            InitializeComponent();
            SetPropertiesForStylesTabSwitches();
            this.Size = new Size(203, 263);
            ReadConfig();
        }

        //Exibir no topo
        private void tgsTopo_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = tgsTopo.Checked;
        }

        //Ativar detecção
        private void tgsAtivar_CheckedChanged(object sender, EventArgs e)
        {
            if (tgsAtivar.Checked == true)
            {
                AtivarDeteccao();
            }
            else
            {
                DesativarDeteccao();
            }
        }

        //Exibir\Ocultar configuração
        private void btnConf_Click(object sender, EventArgs e)
        {
            if (this.Size.Height == 263)
                this.Size = new Size(203, 448);
            else
                this.Size = new Size(203, 263);
        }

        //Inicialização
        private void FrmAudio_Activated(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(35, 32, 35);
            this.Icon = Properties.Resources.icon_alert_blue;
        }

        //Salvara configuração
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }


        #region "Métodos"

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

            ToggleSwitchAndroidRenderer customizedSensRenderer = new ToggleSwitchAndroidRenderer();
            customizedSensRenderer.OnButtonColor = Color.Red;
            customizedSensRenderer.OffButtonColor = Color.Yellow;

            tgsSens.Style = JCS.ToggleSwitch.ToggleSwitchStyle.Android;
            tgsSens.Size = new Size(78, 23);
            tgsSens.OnText = "Alta";
            tgsSens.OnFont = new Font(this.Font.FontFamily, 8, FontStyle.Bold);
            tgsSens.OnForeColor = Color.White;
            tgsSens.OffText = "Baixa";
            tgsSens.OffFont = new Font(this.Font.FontFamily, 8, FontStyle.Bold);
            tgsSens.OffForeColor = Color.FromArgb(141, 123, 141);
            tgsSens.SetRenderer(customizedSensRenderer);
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

        private void timeProc_Tick(object sender, EventArgs e)
        {
            float peakVal = 0;

            if (IsAudioPlaying(GetDefaultRenderDevice(), out peakVal))
            {
                lbPeak.Text = peakVal.ToString();
                lbcount.Text = qtdSec.ToString();


                if (cfgTpSens == 2)
                {
                    //Maior sensibilidade - pende para o vermelho
                    if (peakVal > cfgLimPeak)
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
                }

                //Menor sensibilidade - pende para o amarelo
                if (cfgTpSens == 1)
                {
                    if (peakVal > cfgLimPeak)
                    {
                        if (qtdSec >= 3)
                        {
                            this.Icon = Properties.Resources.icon_alert_red;
                            picAlert.Image = Properties.Resources.alert_red;
                            qtdSec = 5;
                        }
                        else
                        {
                            if (picAlert.Image == Properties.Resources.alert_red)
                            {
                                qtdSec = 1;
                            }

                            this.Icon = Properties.Resources.icon_alert_yellow;
                            picAlert.Image = Properties.Resources.alert_yellow;
                            qtdSec++;
                        }
                    }
                    else
                    {
                        this.Icon = Properties.Resources.icon_alert_yellow;
                        picAlert.Image = Properties.Resources.alert_yellow;
                        if (qtdSec > 0)
                            qtdSec--;
                    }
                }
            }
            else
            {
                this.Icon = Properties.Resources.icon_alert_green;
                picAlert.Image = Properties.Resources.alert_green;
                if (qtdSec > 0)
                    qtdSec--;
            }

            //Controle de memória
            secMem++;
            if (secMem == cfgTmpMem)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                secMem = 0;
            }

            //Controle de audio
            secAudio++;
            if (secAudio == cfgTmpRef)
            {
                DesativarDeteccao();

                secAudio = 0;
                secAudioWait = 0;

                timeRefresh.Enabled = true;
            }
        }

        private void timeRefresh_Tick(object sender, EventArgs e)
        {
            secAudioWait++;

            if (secAudioWait == 5)
            {
                AtivarDeteccao();

                timeRefresh.Enabled = false;
            }
        }

        private void ReadConfig()
        {
            cfgTmpMem = Properties.Settings.Default.TmpMemoria;
            cfgTmpRef = Properties.Settings.Default.TmpRefresh;
            cfgLimPeak = Properties.Settings.Default.LimPeak;
            cfgTpSens = Properties.Settings.Default.TpSens;

            txtMemoria.Text = cfgTmpMem.ToString();
            txtRefresh.Text = cfgTmpRef.ToString();
            txtPeak.Text = cfgLimPeak.ToString();
            if (cfgTpSens == 1)
                tgsSens.Checked = false;
            else
                tgsSens.Checked = true;
        }

        private void SaveConfig()
        {
            cfgTmpMem = Int32.Parse(txtMemoria.Text);
            cfgTmpRef = Int32.Parse(txtRefresh.Text);
            cfgLimPeak = float.Parse(txtPeak.Text);
            if (tgsSens.Checked == false)
                cfgTpSens = 1;
            else
                cfgTpSens = 2;

            Properties.Settings.Default.TmpMemoria = cfgTmpMem;
            Properties.Settings.Default.TmpRefresh = cfgTmpRef;
            Properties.Settings.Default.LimPeak = cfgLimPeak;
            Properties.Settings.Default.TpSens = cfgTpSens;
            Properties.Settings.Default.Save();

            DesativarDeteccao();
            ReadConfig();

            Thread.Sleep(1000);
            AtivarDeteccao();
        }


        private void DesativarDeteccao()
        {
            qtdSec = 0;
            secMem = 0;
            secAudio = 0;

            this.Icon = Properties.Resources.icon_alert_blue;
            picAlert.Image = Properties.Resources.alert_blue;
            timeProc.Enabled = false;
        }

        private void AtivarDeteccao()
        {
            qtdSec = 0;
            secMem = 0;
            secAudio = 0;

            this.Icon = Properties.Resources.icon_alert_green;
            picAlert.Image = Properties.Resources.alert_green;
            timeProc.Enabled = true;
        }

        #endregion

    }
}
