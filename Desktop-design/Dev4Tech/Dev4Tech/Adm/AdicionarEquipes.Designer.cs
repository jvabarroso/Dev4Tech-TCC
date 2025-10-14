namespace Dev4Tech
{
    partial class AdicionarEquipes
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
            Guna.UI2.WinForms.Guna2GradientButton guna2GradientButton1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdicionarEquipes));
            this.picPerfil = new System.Windows.Forms.PictureBox();
            this.btnEquipes = new System.Windows.Forms.PictureBox();
            this.btnCalendar = new System.Windows.Forms.PictureBox();
            this.btnRanking = new System.Windows.Forms.PictureBox();
            this.btnLogout = new System.Windows.Forms.PictureBox();
            this.btnConfig = new System.Windows.Forms.PictureBox();
            this.btnHome = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.picBoxFtEquipe = new Guna.UI2.WinForms.Guna2PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAddMembro = new Guna.UI2.WinForms.Guna2GradientButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.panelDadosFunc = new System.Windows.Forms.Panel();
            this.btnCriarEquipe = new Guna.UI2.WinForms.Guna2Button();
            this.btnFtEquipe = new Guna.UI2.WinForms.Guna2Button();
            this.txtNomeEquipe = new Guna.UI2.WinForms.Guna2TextBox();
            this.customInstaller1 = new MySql.Data.MySqlClient.CustomInstaller();
            this.cmbCategoriaEquipe = new System.Windows.Forms.ComboBox();
            this.cbmEmailMembro = new System.Windows.Forms.ComboBox();
            guna2GradientButton1 = new Guna.UI2.WinForms.Guna2GradientButton();
            ((System.ComponentModel.ISupportInitialize)(this.picPerfil)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnEquipes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCalendar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRanking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLogout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnHome)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxFtEquipe)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2GradientButton1
            // 
            guna2GradientButton1.BorderRadius = 20;
            guna2GradientButton1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            guna2GradientButton1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            guna2GradientButton1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            guna2GradientButton1.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            guna2GradientButton1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            guna2GradientButton1.FillColor = System.Drawing.Color.White;
            guna2GradientButton1.FillColor2 = System.Drawing.Color.White;
            guna2GradientButton1.Font = new System.Drawing.Font("Poppins", 9.75F);
            guna2GradientButton1.ForeColor = System.Drawing.Color.DimGray;
            guna2GradientButton1.Location = new System.Drawing.Point(938, 889);
            guna2GradientButton1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            guna2GradientButton1.Name = "guna2GradientButton1";
            guna2GradientButton1.Size = new System.Drawing.Size(254, 69);
            guna2GradientButton1.TabIndex = 226;
            guna2GradientButton1.Text = "Cancelar";
            guna2GradientButton1.Click += new System.EventHandler(this.btnHome_Click_1);
            // 
            // picPerfil
            // 
            this.picPerfil.BackColor = System.Drawing.Color.Blue;
            this.picPerfil.BackgroundImage = global::Dev4Tech.Properties.Resources.icon_perfil;
            this.picPerfil.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picPerfil.Location = new System.Drawing.Point(20, 936);
            this.picPerfil.Margin = new System.Windows.Forms.Padding(6);
            this.picPerfil.Name = "picPerfil";
            this.picPerfil.Size = new System.Drawing.Size(36, 35);
            this.picPerfil.TabIndex = 44;
            this.picPerfil.TabStop = false;
            // 
            // btnEquipes
            // 
            this.btnEquipes.BackColor = System.Drawing.Color.Blue;
            this.btnEquipes.BackgroundImage = global::Dev4Tech.Properties.Resources.icon_equip;
            this.btnEquipes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnEquipes.Location = new System.Drawing.Point(20, 74);
            this.btnEquipes.Margin = new System.Windows.Forms.Padding(6);
            this.btnEquipes.Name = "btnEquipes";
            this.btnEquipes.Size = new System.Drawing.Size(36, 35);
            this.btnEquipes.TabIndex = 43;
            this.btnEquipes.TabStop = false;
            this.btnEquipes.Click += new System.EventHandler(this.btnEquipes_Click_1);
            // 
            // btnCalendar
            // 
            this.btnCalendar.BackColor = System.Drawing.Color.Blue;
            this.btnCalendar.BackgroundImage = global::Dev4Tech.Properties.Resources.icon_calendar;
            this.btnCalendar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCalendar.Location = new System.Drawing.Point(20, 134);
            this.btnCalendar.Margin = new System.Windows.Forms.Padding(6);
            this.btnCalendar.Name = "btnCalendar";
            this.btnCalendar.Size = new System.Drawing.Size(36, 35);
            this.btnCalendar.TabIndex = 42;
            this.btnCalendar.TabStop = false;
            this.btnCalendar.Click += new System.EventHandler(this.btnCalendar_Click);
            // 
            // btnRanking
            // 
            this.btnRanking.BackColor = System.Drawing.Color.Blue;
            this.btnRanking.BackgroundImage = global::Dev4Tech.Properties.Resources.icon_ranking;
            this.btnRanking.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRanking.Location = new System.Drawing.Point(20, 198);
            this.btnRanking.Margin = new System.Windows.Forms.Padding(6);
            this.btnRanking.Name = "btnRanking";
            this.btnRanking.Size = new System.Drawing.Size(36, 35);
            this.btnRanking.TabIndex = 41;
            this.btnRanking.TabStop = false;
            this.btnRanking.Click += new System.EventHandler(this.btnRanking_Click_1);
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.Blue;
            this.btnLogout.BackgroundImage = global::Dev4Tech.Properties.Resources.Nav_Icon_Item;
            this.btnLogout.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLogout.Location = new System.Drawing.Point(20, 870);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(6);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(36, 35);
            this.btnLogout.TabIndex = 40;
            this.btnLogout.TabStop = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click_1);
            // 
            // btnConfig
            // 
            this.btnConfig.BackColor = System.Drawing.Color.Blue;
            this.btnConfig.BackgroundImage = global::Dev4Tech.Properties.Resources.icon_config;
            this.btnConfig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnConfig.Location = new System.Drawing.Point(20, 827);
            this.btnConfig.Margin = new System.Windows.Forms.Padding(6);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(36, 35);
            this.btnConfig.TabIndex = 39;
            this.btnConfig.TabStop = false;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // btnHome
            // 
            this.btnHome.BackColor = System.Drawing.Color.Blue;
            this.btnHome.BackgroundImage = global::Dev4Tech.Properties.Resources.icon_Home;
            this.btnHome.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnHome.Location = new System.Drawing.Point(20, 14);
            this.btnHome.Margin = new System.Windows.Forms.Padding(6);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(36, 35);
            this.btnHome.TabIndex = 38;
            this.btnHome.TabStop = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Blue;
            this.pictureBox1.Location = new System.Drawing.Point(-3, -5);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(81, 1200);
            this.pictureBox1.TabIndex = 37;
            this.pictureBox1.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Poppins", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.label6.Location = new System.Drawing.Point(277, 502);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(166, 36);
            this.label6.TabIndex = 228;
            this.label6.Text = "Foto da equipe";
            // 
            // picBoxFtEquipe
            // 
            this.picBoxFtEquipe.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picBoxFtEquipe.BorderRadius = 100;
            this.picBoxFtEquipe.ImageRotate = 0F;
            this.picBoxFtEquipe.InitialImage = ((System.Drawing.Image)(resources.GetObject("picBoxFtEquipe.InitialImage")));
            this.picBoxFtEquipe.Location = new System.Drawing.Point(211, 187);
            this.picBoxFtEquipe.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.picBoxFtEquipe.Name = "picBoxFtEquipe";
            this.picBoxFtEquipe.Size = new System.Drawing.Size(300, 301);
            this.picBoxFtEquipe.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBoxFtEquipe.TabIndex = 227;
            this.picBoxFtEquipe.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Poppins", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.label4.Location = new System.Drawing.Point(727, 556);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 36);
            this.label4.TabIndex = 223;
            this.label4.Text = "Membros";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Poppins", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.label3.Location = new System.Drawing.Point(725, 433);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(204, 36);
            this.label3.TabIndex = 221;
            this.label3.Text = "Adicionar membro";
            // 
            // btnAddMembro
            // 
            this.btnAddMembro.BorderRadius = 19;
            this.btnAddMembro.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAddMembro.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAddMembro.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAddMembro.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAddMembro.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAddMembro.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(88)))), ((int)(((byte)(242)))));
            this.btnAddMembro.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(88)))), ((int)(((byte)(242)))));
            this.btnAddMembro.Font = new System.Drawing.Font("Poppins", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddMembro.ForeColor = System.Drawing.Color.White;
            this.btnAddMembro.Location = new System.Drawing.Point(1695, 472);
            this.btnAddMembro.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAddMembro.Name = "btnAddMembro";
            this.btnAddMembro.Size = new System.Drawing.Size(69, 48);
            this.btnAddMembro.TabIndex = 220;
            this.btnAddMembro.Text = "+";
            this.btnAddMembro.Click += new System.EventHandler(this.btnAddMembro_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Poppins", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.label2.Location = new System.Drawing.Point(727, 313);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(224, 36);
            this.label2.TabIndex = 217;
            this.label2.Text = "Categoria da equipe";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Poppins", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.label1.Location = new System.Drawing.Point(726, 187);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(183, 36);
            this.label1.TabIndex = 216;
            this.label1.Text = "Nome da equipe";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Poppins", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(718, 91);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(504, 78);
            this.label15.TabIndex = 214;
            this.label15.Text = "Cadastro de equipes";
            // 
            // panelDadosFunc
            // 
            this.panelDadosFunc.Location = new System.Drawing.Point(731, 596);
            this.panelDadosFunc.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelDadosFunc.Name = "panelDadosFunc";
            this.panelDadosFunc.Size = new System.Drawing.Size(930, 227);
            this.panelDadosFunc.TabIndex = 213;
            // 
            // btnCriarEquipe
            // 
            this.btnCriarEquipe.BorderRadius = 20;
            this.btnCriarEquipe.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCriarEquipe.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCriarEquipe.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCriarEquipe.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCriarEquipe.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(88)))), ((int)(((byte)(242)))));
            this.btnCriarEquipe.Font = new System.Drawing.Font("Poppins", 9.75F);
            this.btnCriarEquipe.ForeColor = System.Drawing.Color.White;
            this.btnCriarEquipe.Location = new System.Drawing.Point(1245, 889);
            this.btnCriarEquipe.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCriarEquipe.Name = "btnCriarEquipe";
            this.btnCriarEquipe.Size = new System.Drawing.Size(250, 69);
            this.btnCriarEquipe.TabIndex = 230;
            this.btnCriarEquipe.Text = "Criar Equipe ➕";
            this.btnCriarEquipe.Click += new System.EventHandler(this.btnCriarEquipe_Click);
            // 
            // btnFtEquipe
            // 
            this.btnFtEquipe.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnFtEquipe.BorderRadius = 20;
            this.btnFtEquipe.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnFtEquipe.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnFtEquipe.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnFtEquipe.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnFtEquipe.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(88)))), ((int)(((byte)(242)))));
            this.btnFtEquipe.Font = new System.Drawing.Font("Poppins", 9.75F);
            this.btnFtEquipe.ForeColor = System.Drawing.Color.White;
            this.btnFtEquipe.Image = ((System.Drawing.Image)(resources.GetObject("btnFtEquipe.Image")));
            this.btnFtEquipe.Location = new System.Drawing.Point(211, 556);
            this.btnFtEquipe.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnFtEquipe.Name = "btnFtEquipe";
            this.btnFtEquipe.Size = new System.Drawing.Size(300, 62);
            this.btnFtEquipe.TabIndex = 232;
            this.btnFtEquipe.Text = "Carregar Foto";
            this.btnFtEquipe.Click += new System.EventHandler(this.btnFtEquipe_Click);
            // 
            // txtNomeEquipe
            // 
            this.txtNomeEquipe.BorderColor = System.Drawing.Color.Black;
            this.txtNomeEquipe.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNomeEquipe.DefaultText = "";
            this.txtNomeEquipe.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtNomeEquipe.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtNomeEquipe.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNomeEquipe.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNomeEquipe.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNomeEquipe.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtNomeEquipe.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNomeEquipe.Location = new System.Drawing.Point(731, 228);
            this.txtNomeEquipe.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtNomeEquipe.Name = "txtNomeEquipe";
            this.txtNomeEquipe.PlaceholderText = "Digite o nome da equipe";
            this.txtNomeEquipe.SelectedText = "";
            this.txtNomeEquipe.Size = new System.Drawing.Size(925, 60);
            this.txtNomeEquipe.TabIndex = 233;
            // 
            // cmbCategoriaEquipe
            // 
            this.cmbCategoriaEquipe.Font = new System.Drawing.Font("Poppins", 9F);
            this.cmbCategoriaEquipe.FormattingEnabled = true;
            this.cmbCategoriaEquipe.Location = new System.Drawing.Point(733, 352);
            this.cmbCategoriaEquipe.Name = "cmbCategoriaEquipe";
            this.cmbCategoriaEquipe.Size = new System.Drawing.Size(923, 39);
            this.cmbCategoriaEquipe.TabIndex = 234;
            // 
            // cbmEmailMembro
            // 
            this.cbmEmailMembro.Font = new System.Drawing.Font("Poppins", 9F);
            this.cbmEmailMembro.FormattingEnabled = true;
            this.cbmEmailMembro.Location = new System.Drawing.Point(733, 472);
            this.cbmEmailMembro.Name = "cbmEmailMembro";
            this.cbmEmailMembro.Size = new System.Drawing.Size(923, 39);
            this.cbmEmailMembro.TabIndex = 235;
            // 
            // AdicionarEquipes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1898, 1024);
            this.Controls.Add(this.cbmEmailMembro);
            this.Controls.Add(this.cmbCategoriaEquipe);
            this.Controls.Add(this.txtNomeEquipe);
            this.Controls.Add(this.btnFtEquipe);
            this.Controls.Add(this.btnCriarEquipe);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.picBoxFtEquipe);
            this.Controls.Add(guna2GradientButton1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnAddMembro);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.panelDadosFunc);
            this.Controls.Add(this.picPerfil);
            this.Controls.Add(this.btnEquipes);
            this.Controls.Add(this.btnCalendar);
            this.Controls.Add(this.btnRanking);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "AdicionarEquipes";
            this.Text = "AdicionarEquipes";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.AdicionarEquipes_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.picPerfil)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnEquipes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCalendar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRanking)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLogout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnHome)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxFtEquipe)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picPerfil;
        private System.Windows.Forms.PictureBox btnEquipes;
        private System.Windows.Forms.PictureBox btnCalendar;
        private System.Windows.Forms.PictureBox btnRanking;
        private System.Windows.Forms.PictureBox btnLogout;
        private System.Windows.Forms.PictureBox btnConfig;
        private System.Windows.Forms.PictureBox btnHome;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label6;
        private Guna.UI2.WinForms.Guna2PictureBox picBoxFtEquipe;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2GradientButton btnAddMembro;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Panel panelDadosFunc;
        private Guna.UI2.WinForms.Guna2Button btnCriarEquipe;
        private Guna.UI2.WinForms.Guna2Button btnFtEquipe;
        private Guna.UI2.WinForms.Guna2TextBox txtNomeEquipe;
        private MySql.Data.MySqlClient.CustomInstaller customInstaller1;
        private System.Windows.Forms.ComboBox cmbCategoriaEquipe;
        private System.Windows.Forms.ComboBox cbmEmailMembro;
        //private System.Windows.Forms.DataGridView dataGridView1;
    }
}