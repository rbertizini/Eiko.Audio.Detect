
namespace Eiko.Audio.Detect
{
    partial class FrmAudio
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAudio));
            this.picAlert = new System.Windows.Forms.PictureBox();
            this.timeProc = new System.Windows.Forms.Timer(this.components);
            this.lbPeak = new System.Windows.Forms.Label();
            this.lbcount = new System.Windows.Forms.Label();
            this.tgsTopo = new JCS.ToggleSwitch();
            this.tgsAtivar = new JCS.ToggleSwitch();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnConf = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.timeRefresh = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picAlert)).BeginInit();
            this.SuspendLayout();
            // 
            // picAlert
            // 
            this.picAlert.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picAlert.ErrorImage = ((System.Drawing.Image)(resources.GetObject("picAlert.ErrorImage")));
            this.picAlert.Image = ((System.Drawing.Image)(resources.GetObject("picAlert.Image")));
            this.picAlert.InitialImage = ((System.Drawing.Image)(resources.GetObject("picAlert.InitialImage")));
            this.picAlert.Location = new System.Drawing.Point(18, 45);
            this.picAlert.Name = "picAlert";
            this.picAlert.Size = new System.Drawing.Size(150, 180);
            this.picAlert.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picAlert.TabIndex = 2;
            this.picAlert.TabStop = false;
            // 
            // timeProc
            // 
            this.timeProc.Interval = 1000;
            this.timeProc.Tick += new System.EventHandler(this.timeProc_Tick);
            // 
            // lbPeak
            // 
            this.lbPeak.AutoSize = true;
            this.lbPeak.ForeColor = System.Drawing.Color.White;
            this.lbPeak.Location = new System.Drawing.Point(15, 248);
            this.lbPeak.Name = "lbPeak";
            this.lbPeak.Size = new System.Drawing.Size(13, 13);
            this.lbPeak.TabIndex = 5;
            this.lbPeak.Text = "0";
            // 
            // lbcount
            // 
            this.lbcount.AutoSize = true;
            this.lbcount.ForeColor = System.Drawing.Color.White;
            this.lbcount.Location = new System.Drawing.Point(124, 248);
            this.lbcount.Name = "lbcount";
            this.lbcount.Size = new System.Drawing.Size(13, 13);
            this.lbcount.TabIndex = 6;
            this.lbcount.Text = "0";
            // 
            // tgsTopo
            // 
            this.tgsTopo.Location = new System.Drawing.Point(102, 24);
            this.tgsTopo.Name = "tgsTopo";
            this.tgsTopo.OffFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tgsTopo.OnFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tgsTopo.Size = new System.Drawing.Size(78, 23);
            this.tgsTopo.TabIndex = 8;
            this.tgsTopo.CheckedChanged += new JCS.ToggleSwitch.CheckedChangedDelegate(this.tgsTopo_CheckedChanged);
            // 
            // tgsAtivar
            // 
            this.tgsAtivar.Location = new System.Drawing.Point(9, 24);
            this.tgsAtivar.Name = "tgsAtivar";
            this.tgsAtivar.OffFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tgsAtivar.OnFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tgsAtivar.Size = new System.Drawing.Size(78, 23);
            this.tgsAtivar.TabIndex = 7;
            this.tgsAtivar.CheckedChanged += new JCS.ToggleSwitch.CheckedChangedDelegate(this.tgsAtivar_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(30, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Ativar";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(109, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Fixar topo";
            // 
            // btnConf
            // 
            this.btnConf.BackgroundImage = global::Eiko.Audio.Detect.Properties.Resources.gear_conf;
            this.btnConf.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnConf.FlatAppearance.BorderSize = 0;
            this.btnConf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConf.Image = global::Eiko.Audio.Detect.Properties.Resources.gear_gray;
            this.btnConf.Location = new System.Drawing.Point(164, 202);
            this.btnConf.Name = "btnConf";
            this.btnConf.Size = new System.Drawing.Size(15, 15);
            this.btnConf.TabIndex = 14;
            this.btnConf.UseVisualStyleBackColor = true;
            this.btnConf.Click += new System.EventHandler(this.btnConf_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(15, 235);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Peak";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(124, 235);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Counter";
            // 
            // timeRefresh
            // 
            this.timeRefresh.Interval = 1000;
            this.timeRefresh.Tick += new System.EventHandler(this.timeRefresh_Tick);
            // 
            // FrmAudio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(187, 224);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnConf);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tgsTopo);
            this.Controls.Add(this.tgsAtivar);
            this.Controls.Add(this.lbcount);
            this.Controls.Add(this.lbPeak);
            this.Controls.Add(this.picAlert);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmAudio";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Detecção de audio";
            this.Activated += new System.EventHandler(this.FrmAudio_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.picAlert)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox picAlert;
        private System.Windows.Forms.Timer timeProc;
        private System.Windows.Forms.Label lbPeak;
        private System.Windows.Forms.Label lbcount;
        private JCS.ToggleSwitch tgsAtivar;
        private JCS.ToggleSwitch tgsTopo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnConf;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer timeRefresh;
    }
}

