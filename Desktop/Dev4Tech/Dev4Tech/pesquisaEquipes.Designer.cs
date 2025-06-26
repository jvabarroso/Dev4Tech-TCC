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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPesquisaEquipe = new System.Windows.Forms.TextBox();
            this.pictureBox11 = new System.Windows.Forms.PictureBox();
            this.btnEquipe = new System.Windows.Forms.PictureBox();
            this.btnCalendar = new System.Windows.Forms.PictureBox();
            this.btnRanking = new System.Windows.Forms.PictureBox();
            this.btnLogout = new System.Windows.Forms.PictureBox();
            this.btnConfig = new System.Windows.Forms.PictureBox();
            this.btnHome = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.filtroEquipes = new System.Windows.Forms.ComboBox();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.panelEquipes = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnEquipe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCalendar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRanking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLogout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnHome)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(153, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(241, 54);
            this.label1.TabIndex = 17;
            this.label1.Text = "WORKFLOW";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(341, 187);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 19);
            this.label2.TabIndex = 29;
            this.label2.Text = "EQUIPES";
            // 
            // txtPesquisaEquipe
            // 
            this.txtPesquisaEquipe.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPesquisaEquipe.Location = new System.Drawing.Point(345, 207);
            this.txtPesquisaEquipe.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPesquisaEquipe.Name = "txtPesquisaEquipe";
            this.txtPesquisaEquipe.Size = new System.Drawing.Size(492, 26);
            this.txtPesquisaEquipe.TabIndex = 30;
            this.txtPesquisaEquipe.Text = "Pesquisar equipe";
            this.txtPesquisaEquipe.Click += new System.EventHandler(this.txtPesquisaEquipe_Click);
            this.txtPesquisaEquipe.TextChanged += new System.EventHandler(this.txtPesquisaEquipe_TextChanged);
            this.txtPesquisaEquipe.Leave += new System.EventHandler(this.txtPesquisarEquipe_Leave);
            // 
            // pictureBox11
            // 
            this.pictureBox11.BackColor = System.Drawing.Color.Blue;
            this.pictureBox11.BackgroundImage = global::Dev4Tech.Properties.Resources.icon_perfil;
            this.pictureBox11.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox11.Location = new System.Drawing.Point(17, 753);
            this.pictureBox11.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox11.Name = "pictureBox11";
            this.pictureBox11.Size = new System.Drawing.Size(32, 28);
            this.pictureBox11.TabIndex = 28;
            this.pictureBox11.TabStop = false;
            // 
            // btnEquipe
            // 
            this.btnEquipe.BackColor = System.Drawing.Color.Blue;
            this.btnEquipe.BackgroundImage = global::Dev4Tech.Properties.Resources.icon_equip;
            this.btnEquipe.Location = new System.Drawing.Point(17, 65);
            this.btnEquipe.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEquipe.Name = "btnEquipe";
            this.btnEquipe.Size = new System.Drawing.Size(32, 28);
            this.btnEquipe.TabIndex = 27;
            this.btnEquipe.TabStop = false;
            this.btnEquipe.Click += new System.EventHandler(this.btnEquipe_Click);
            // 
            // btnCalendar
            // 
            this.btnCalendar.BackColor = System.Drawing.Color.Blue;
            this.btnCalendar.BackgroundImage = global::Dev4Tech.Properties.Resources.icon_calendar;
            this.btnCalendar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCalendar.Location = new System.Drawing.Point(17, 113);
            this.btnCalendar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCalendar.Name = "btnCalendar";
            this.btnCalendar.Size = new System.Drawing.Size(32, 28);
            this.btnCalendar.TabIndex = 26;
            this.btnCalendar.TabStop = false;
            this.btnCalendar.Click += new System.EventHandler(this.btnCalendar_Click);
            // 
            // btnRanking
            // 
            this.btnRanking.BackColor = System.Drawing.Color.Blue;
            this.btnRanking.BackgroundImage = global::Dev4Tech.Properties.Resources.icon_ranking;
            this.btnRanking.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRanking.Location = new System.Drawing.Point(17, 164);
            this.btnRanking.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRanking.Name = "btnRanking";
            this.btnRanking.Size = new System.Drawing.Size(32, 28);
            this.btnRanking.TabIndex = 25;
            this.btnRanking.TabStop = false;
            this.btnRanking.Click += new System.EventHandler(this.btnRanking_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.Blue;
            this.btnLogout.BackgroundImage = global::Dev4Tech.Properties.Resources.Nav_Icon_Item;
            this.btnLogout.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLogout.Location = new System.Drawing.Point(17, 700);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(32, 28);
            this.btnLogout.TabIndex = 24;
            this.btnLogout.TabStop = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnConfig
            // 
            this.btnConfig.BackColor = System.Drawing.Color.Blue;
            this.btnConfig.BackgroundImage = global::Dev4Tech.Properties.Resources.icon_config;
            this.btnConfig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnConfig.Location = new System.Drawing.Point(17, 665);
            this.btnConfig.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(32, 28);
            this.btnConfig.TabIndex = 23;
            this.btnConfig.TabStop = false;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // btnHome
            // 
            this.btnHome.BackColor = System.Drawing.Color.Blue;
            this.btnHome.BackgroundImage = global::Dev4Tech.Properties.Resources.icon_Home;
            this.btnHome.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnHome.Location = new System.Drawing.Point(17, 17);
            this.btnHome.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(32, 28);
            this.btnHome.TabIndex = 22;
            this.btnHome.TabStop = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Blue;
            this.pictureBox1.Location = new System.Drawing.Point(1, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(72, 847);
            this.pictureBox1.TabIndex = 18;
            this.pictureBox1.TabStop = false;
            // 
            // filtroEquipes
            // 
            this.filtroEquipes.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filtroEquipes.FormattingEnabled = true;
            this.filtroEquipes.Location = new System.Drawing.Point(844, 207);
            this.filtroEquipes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.filtroEquipes.Name = "filtroEquipes";
            this.filtroEquipes.Size = new System.Drawing.Size(191, 27);
            this.filtroEquipes.TabIndex = 63;
            this.filtroEquipes.SelectedIndexChanged += new System.EventHandler(this.filtroEquipes_SelectedIndexChanged);
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFiltrar.Location = new System.Drawing.Point(1040, 207);
            this.btnFiltrar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(75, 26);
            this.btnFiltrar.TabIndex = 64;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = true;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            // 
            // panelEquipes
            // 
            this.panelEquipes.Location = new System.Drawing.Point(345, 261);
            this.panelEquipes.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelEquipes.Name = "panelEquipes";
            this.panelEquipes.Size = new System.Drawing.Size(769, 468);
            this.panelEquipes.TabIndex = 68;
            this.panelEquipes.Paint += new System.Windows.Forms.PaintEventHandler(this.panelEquipes_Paint);
            // 
            // PesquisaEquipes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1685, 838);
            this.Controls.Add(this.panelEquipes);
            this.Controls.Add(this.btnFiltrar);
            this.Controls.Add(this.filtroEquipes);
            this.Controls.Add(this.txtPesquisaEquipe);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox11);
            this.Controls.Add(this.btnEquipe);
            this.Controls.Add(this.btnCalendar);
            this.Controls.Add(this.btnRanking);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PesquisaEquipes";
            this.Text = "Equipes";
            this.Load += new System.EventHandler(this.PesquisaEquipes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).EndInit();
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

        private System.Windows.Forms.PictureBox pictureBox11;
        private System.Windows.Forms.PictureBox btnEquipe;
        private System.Windows.Forms.PictureBox btnCalendar;
        private System.Windows.Forms.PictureBox btnRanking;
        private System.Windows.Forms.PictureBox btnLogout;
        private System.Windows.Forms.PictureBox btnConfig;
        private System.Windows.Forms.PictureBox btnHome;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPesquisaEquipe;
        private System.Windows.Forms.ComboBox filtroEquipes;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.FlowLayoutPanel panelEquipes;
    }
}