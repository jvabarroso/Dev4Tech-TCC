namespace Dev4Tech
{
    partial class PesquisaEquipes
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.picPerfil = new System.Windows.Forms.PictureBox();
            this.btnEquipe = new System.Windows.Forms.PictureBox();
            this.btnCalendar = new System.Windows.Forms.PictureBox();
            this.btnRanking = new System.Windows.Forms.PictureBox();
            this.btnLogout = new System.Windows.Forms.PictureBox();
            this.btnConfig = new System.Windows.Forms.PictureBox();
            this.btnHome = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.panelEquipes = new System.Windows.Forms.FlowLayoutPanel();
            this.txtPesquisaEquipe = new Guna.UI2.WinForms.Guna2TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.filtroEquipes = new Guna.UI2.WinForms.Guna2ComboBox();
            this.btnFiltrar = new Guna.UI2.WinForms.Guna2Button();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picPerfil)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnEquipe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCalendar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRanking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLogout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnHome)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // picPerfil
            // 
            this.picPerfil.BackColor = System.Drawing.Color.Blue;
            this.picPerfil.BackgroundImage = global::Dev4Tech.Properties.Resources.icon_perfil;
            this.picPerfil.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picPerfil.Location = new System.Drawing.Point(12, 732);
            this.picPerfil.Name = "picPerfil";
            this.picPerfil.Size = new System.Drawing.Size(24, 23);
            this.picPerfil.TabIndex = 28;
            this.picPerfil.TabStop = false;
            // 
            // btnEquipe
            // 
            this.btnEquipe.BackColor = System.Drawing.Color.Blue;
            this.btnEquipe.BackgroundImage = global::Dev4Tech.Properties.Resources.icon_equip;
            this.btnEquipe.Location = new System.Drawing.Point(12, 53);
            this.btnEquipe.Name = "btnEquipe";
            this.btnEquipe.Size = new System.Drawing.Size(24, 23);
            this.btnEquipe.TabIndex = 27;
            this.btnEquipe.TabStop = false;
            this.btnEquipe.Click += new System.EventHandler(this.btnEquipe_Click);
            // 
            // btnCalendar
            // 
            this.btnCalendar.BackColor = System.Drawing.Color.Blue;
            this.btnCalendar.BackgroundImage = global::Dev4Tech.Properties.Resources.icon_calendar;
            this.btnCalendar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCalendar.Location = new System.Drawing.Point(12, 92);
            this.btnCalendar.Name = "btnCalendar";
            this.btnCalendar.Size = new System.Drawing.Size(24, 23);
            this.btnCalendar.TabIndex = 26;
            this.btnCalendar.TabStop = false;
            this.btnCalendar.Click += new System.EventHandler(this.btnCalendar_Click);
            // 
            // btnRanking
            // 
            this.btnRanking.BackColor = System.Drawing.Color.Blue;
            this.btnRanking.BackgroundImage = global::Dev4Tech.Properties.Resources.icon_ranking;
            this.btnRanking.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRanking.Location = new System.Drawing.Point(12, 133);
            this.btnRanking.Name = "btnRanking";
            this.btnRanking.Size = new System.Drawing.Size(24, 23);
            this.btnRanking.TabIndex = 25;
            this.btnRanking.TabStop = false;
            this.btnRanking.Click += new System.EventHandler(this.btnRanking_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.Blue;
            this.btnLogout.BackgroundImage = global::Dev4Tech.Properties.Resources.Nav_Icon_Item;
            this.btnLogout.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLogout.Location = new System.Drawing.Point(12, 689);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(24, 23);
            this.btnLogout.TabIndex = 24;
            this.btnLogout.TabStop = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnConfig
            // 
            this.btnConfig.BackColor = System.Drawing.Color.Blue;
            this.btnConfig.BackgroundImage = global::Dev4Tech.Properties.Resources.icon_config;
            this.btnConfig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnConfig.Location = new System.Drawing.Point(12, 660);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(24, 23);
            this.btnConfig.TabIndex = 23;
            this.btnConfig.TabStop = false;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // btnHome
            // 
            this.btnHome.BackColor = System.Drawing.Color.Blue;
            this.btnHome.BackgroundImage = global::Dev4Tech.Properties.Resources.icon_Home;
            this.btnHome.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnHome.Location = new System.Drawing.Point(12, 14);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(24, 23);
            this.btnHome.TabIndex = 22;
            this.btnHome.TabStop = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Blue;
            this.pictureBox1.Location = new System.Drawing.Point(-3, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(54, 781);
            this.pictureBox1.TabIndex = 18;
            this.pictureBox1.TabStop = false;
            // 
            // panelEquipes
            // 
            this.panelEquipes.Location = new System.Drawing.Point(358, 271);
            this.panelEquipes.Name = "panelEquipes";
            this.panelEquipes.Size = new System.Drawing.Size(724, 380);
            this.panelEquipes.TabIndex = 68;
            this.panelEquipes.Paint += new System.Windows.Forms.PaintEventHandler(this.panelEquipes_Paint);
            // 
            // txtPesquisaEquipe
            // 
            this.txtPesquisaEquipe.BorderRadius = 8;
            this.txtPesquisaEquipe.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPesquisaEquipe.DefaultText = "";
            this.txtPesquisaEquipe.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtPesquisaEquipe.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtPesquisaEquipe.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPesquisaEquipe.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPesquisaEquipe.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPesquisaEquipe.Font = new System.Drawing.Font("Poppins", 9.75F);
            this.txtPesquisaEquipe.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPesquisaEquipe.Location = new System.Drawing.Point(358, 228);
            this.txtPesquisaEquipe.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPesquisaEquipe.Name = "txtPesquisaEquipe";
            this.txtPesquisaEquipe.PlaceholderText = "🔎 Pesquisar Equipes";
            this.txtPesquisaEquipe.SelectedText = "";
            this.txtPesquisaEquipe.Size = new System.Drawing.Size(438, 36);
            this.txtPesquisaEquipe.TabIndex = 164;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Poppins", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(350, 176);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 48);
            this.label5.TabIndex = 165;
            this.label5.Text = "Equipes";
            // 
            // filtroEquipes
            // 
            this.filtroEquipes.BackColor = System.Drawing.Color.Transparent;
            this.filtroEquipes.BorderRadius = 8;
            this.filtroEquipes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.filtroEquipes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filtroEquipes.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.filtroEquipes.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.filtroEquipes.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.filtroEquipes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.filtroEquipes.ItemHeight = 30;
            this.filtroEquipes.Location = new System.Drawing.Point(802, 229);
            this.filtroEquipes.Name = "filtroEquipes";
            this.filtroEquipes.Size = new System.Drawing.Size(188, 36);
            this.filtroEquipes.TabIndex = 166;
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.BorderRadius = 18;
            this.btnFiltrar.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnFiltrar.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnFiltrar.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnFiltrar.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnFiltrar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(88)))), ((int)(((byte)(242)))));
            this.btnFiltrar.Font = new System.Drawing.Font("Poppins", 9.75F);
            this.btnFiltrar.ForeColor = System.Drawing.Color.White;
            this.btnFiltrar.Location = new System.Drawing.Point(996, 229);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(86, 36);
            this.btnFiltrar.TabIndex = 167;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Poppins", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(103, 41);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 48);
            this.label2.TabIndex = 168;
            this.label2.Text = "WORKFLOW";
            // 
            // PesquisaEquipes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1342, 776);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnFiltrar);
            this.Controls.Add(this.filtroEquipes);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtPesquisaEquipe);
            this.Controls.Add(this.panelEquipes);
            this.Controls.Add(this.picPerfil);
            this.Controls.Add(this.btnEquipe);
            this.Controls.Add(this.btnCalendar);
            this.Controls.Add(this.btnRanking);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.pictureBox1);
            this.Name = "PesquisaEquipes";
            this.Text = "Equipes";
            this.Load += new System.EventHandler(this.PesquisaEquipes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picPerfil)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnEquipe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCalendar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRanking)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLogout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnHome)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picPerfil;
        private System.Windows.Forms.PictureBox btnEquipe;
        private System.Windows.Forms.PictureBox btnCalendar;
        private System.Windows.Forms.PictureBox btnRanking;
        private System.Windows.Forms.PictureBox btnLogout;
        private System.Windows.Forms.PictureBox btnConfig;
        private System.Windows.Forms.PictureBox btnHome;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.FlowLayoutPanel panelEquipes;
        private Guna.UI2.WinForms.Guna2TextBox txtPesquisaEquipe;
        private System.Windows.Forms.Label label5;
        private Guna.UI2.WinForms.Guna2ComboBox filtroEquipes;
        private Guna.UI2.WinForms.Guna2Button btnFiltrar;
        private System.Windows.Forms.Label label2;
    }
}