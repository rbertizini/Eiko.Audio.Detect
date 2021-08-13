using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.Codecs.WAV;
using CSCore.SoundIn;
using CSCore.Streams;
using CSCore.Win32;
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
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace Eiko.Audio.Detect
{
    public partial class FrmAudio : Form
    {
        //Variáveis globais
        int qtdSec = 0;
        int secMem = 0;
        int secAudio = 0;
        int secAudioWait = 0;

        int cfgTmpMem = 0;
        int cfgTmpRef = 0;
        float cfgLimPeak = 0;
        int cfgTpSens = 0;
        string cfgCnlNome = string.Empty;

        private const CaptureMode CaptureMode = Eiko.Audio.Detect.CaptureMode.LoopbackCapture;
        
        private MMDevice _selectedDevice;

        public MMDevice SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value;
                if (value != null)
                    tgsAtivar.Enabled = true;
            }
        }

        //Inicialização
        public FrmAudio()
        {
            InitializeComponent();
            SetPropertiesForStylesTabSwitches();
            this.Size = new Size(203, 263);
            RefreshDevices();
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
            { 
                //this.Size = new Size(203, 448);
                this.Size = new Size(203, 474);
            }
            else
            { 
                this.Size = new Size(203, 263);
            }
        }

        //Inicialização
        private void FrmAudio_Activated(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(35, 32, 35);
            statusBlue();
        }

        //Salvara configuração
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void cboxDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cboxDevice.SelectedIndex >= 0)
            //{
            //    ComboboxItem Item = new ComboboxItem();
            //    Item = (ComboboxItem)cboxDevice.SelectedItem;
            //    SelectedDevice = (MMDevice)Item.Value;
            //}
            //else
            //{
            //    SelectedDevice = null;
            //}
        }


        #region "Métodos"

        //Configuração dos Toggles
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
                //return enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
                return enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Communications);
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

        //Processo de timer de detecção
        private void timeProc_Tick(object sender, EventArgs e)
        {
            float peakVal = 0;

            if (IsAudioPlaying(_selectedDevice, out peakVal))
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
                        statusRed();
                    }
                    else
                    {
                        statusYellow();
                    }
                }

                //Menor sensibilidade - pende para o amarelo
                if (cfgTpSens == 1)
                {
                    if (peakVal > cfgLimPeak)
                    {
                        if (qtdSec >= 3)
                        {
                            statusRed();
                            qtdSec = 5;
                        }
                        else
                        {
                            if (picAlert.Image == Properties.Resources.alert_red)
                            {
                                qtdSec = 1;
                            }

                            statusYellow();
                            qtdSec++;
                        }
                    }
                    else
                    {
                        statusYellow();
                        if (qtdSec > 0)
                            qtdSec--;
                    }
                }
            }
            else
            {
                statusGreen();
                qtdSec = 0;

                lbPeak.Text = "0";
                lbcount.Text = qtdSec.ToString();
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

        //Status Azul
        private void statusBlue()
        {
            this.Icon = Properties.Resources.icon_alert_blue;
            picAlert.Image = Properties.Resources.alert_blue;
        }

        //Status Verde
        private void statusGreen()
        {
            this.Icon = Properties.Resources.icon_alert_green;
            picAlert.Image = Properties.Resources.alert_green;
        }

        //Status amarelo
        private void statusYellow()
        {
            this.Icon = Properties.Resources.icon_alert_yellow;
            picAlert.Image = Properties.Resources.alert_yellow;

            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal, Handle);
            TaskbarManager.Instance.SetProgressValue(0, 100, Handle);
        }

        //Status vermelho
        private void statusRed()
        {
            this.Icon = Properties.Resources.icon_alert_red;
            picAlert.Image = Properties.Resources.alert_red;

            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal, Handle);
            TaskbarManager.Instance.SetProgressValue(100, 100, Handle);
        }

        //Processo de timer de refresh
        private void timeRefresh_Tick(object sender, EventArgs e)
        {
            secAudioWait++;

            if (secAudioWait == 5)
            {
                AtivarDeteccao();

                timeRefresh.Enabled = false;
            }
        }

        //Obtenção de configuração
        private void ReadConfig()
        {
            //Leitura
            cfgTmpMem = Properties.Settings.Default.TmpMemoria;
            cfgTmpRef = Properties.Settings.Default.TmpRefresh;
            cfgLimPeak = Properties.Settings.Default.LimPeak;
            cfgTpSens = Properties.Settings.Default.TpSens;
            cfgCnlNome = Properties.Settings.Default.CnlNome;

            //Exibição
            txtMemoria.Text = cfgTmpMem.ToString();
            txtRefresh.Text = cfgTmpRef.ToString();
            txtPeak.Text = cfgLimPeak.ToString();
            if (cfgTpSens == 1)
                tgsSens.Checked = false;
            else
                tgsSens.Checked = true;

            cboxDevice.SelectedIndex = -1;
            for (int i = 0; i < cboxDevice.Items.Count; i++)
            {
                if (cboxDevice.Items[i].ToString() == cfgCnlNome)
                {
                    cboxDevice.SelectedIndex = i;
                }
            }

            //Marcar device
            SetDevice();

            //Conversão
            cfgTmpMem = cfgTmpMem * 60;
            cfgTmpRef = cfgTmpRef * 60;
        }

        //Atualização de configuração
        private void SaveConfig()
        {
            //Atualização
            Properties.Settings.Default.TmpMemoria = Int32.Parse(txtMemoria.Text);
            Properties.Settings.Default.TmpRefresh = Int32.Parse(txtRefresh.Text);
            Properties.Settings.Default.LimPeak = float.Parse(txtPeak.Text);
            if (tgsSens.Checked == false)
                Properties.Settings.Default.TpSens = 1;
            else
                Properties.Settings.Default.TpSens = 2;
            Properties.Settings.Default.CnlNome = cboxDevice.SelectedItem.ToString();
            Properties.Settings.Default.Save();

            //Restart
            DesativarDeteccao();
            ReadConfig();

            Thread.Sleep(1000);
            AtivarDeteccao();
        }

        //Marcar device
        private void SetDevice()
        {
            if (cboxDevice.SelectedIndex >= 0)
            {
                ComboboxItem Item = new ComboboxItem();
                Item = (ComboboxItem)cboxDevice.SelectedItem;
                SelectedDevice = (MMDevice)Item.Value;
            }
            else
            {
                SelectedDevice = null;
                return;
            }
        }

        //Desativar detecção
        private void DesativarDeteccao()
        {
            qtdSec = 0;
            secMem = 0;
            secAudio = 0;

            statusBlue();
            timeProc.Enabled = false;
        }

        //Ativar deteção
        private void AtivarDeteccao()
        {
            qtdSec = 0;
            secMem = 0;
            secAudio = 0;

            statusGreen();
            timeProc.Enabled = true;
        }

        //Obtem canais de audio disponíveis
        private void RefreshDevices()
        {
            cboxDevice.Items.Clear();

            using (var deviceEnumerator = new MMDeviceEnumerator())
            using (var deviceCollection = deviceEnumerator.EnumAudioEndpoints(
                CaptureMode == CaptureMode.Capture ? DataFlow.Capture : DataFlow.Render, DeviceState.Active))
            {
                foreach (var device in deviceCollection)
                {
                    var deviceFormat = WaveFormatFromBlob(device.PropertyStore[
                        new PropertyKey(new Guid(0xf19f064d, 0x82c, 0x4e27, 0xbc, 0x73, 0x68, 0x82, 0xa1, 0xbb, 0x8e, 0x4c), 0)].BlobValue);

                    ComboboxItem Item = new ComboboxItem();
                    Item.Text = device.FriendlyName;
                    //Item.Value = deviceFormat.Channels.ToString(CultureInfo.InvariantCulture);
                    Item.Value = device;

                    cboxDevice.Items.Add(Item);
                }
            }

            //Ajustando tamanho do combobox
            cboxDevice.DropDownWidth = DropDownWidth(cboxDevice);
        }

        //Ajusta o tamanho de exibição dos itens do combobox
        int DropDownWidth(ComboBox myCombo)
        {
            int maxWidth = 0, temp = 0;
            foreach (var obj in myCombo.Items)
            {
                temp = TextRenderer.MeasureText(obj.ToString(), myCombo.Font).Width;
                if (temp > maxWidth)
                {
                    maxWidth = temp;
                }
            }
            return maxWidth;
        }
        
        //Obtenção de canais de audio
        private static WaveFormat WaveFormatFromBlob(Blob blob)
        {
            if (blob.Length == 40)
                return (WaveFormat)Marshal.PtrToStructure(blob.Data, typeof(WaveFormatExtensible));
            return (WaveFormat)Marshal.PtrToStructure(blob.Data, typeof(WaveFormat));
        }

        #endregion

    }

    //Classe CaptureMode
    public enum CaptureMode
    {
        Capture,
        LoopbackCapture
    }

    //Classe ComboBoxItem
    public class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
