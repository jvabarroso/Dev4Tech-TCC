namespace Dev4Tech
{
    partial class cadastro_funcionário
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
            this.btnCadastrar = new System.Windows.Forms.Button();
            this.btnVoltar = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.txtCadFuncEmail = new System.Windows.Forms.TextBox();
            this.txtCadFuncSenha = new System.Windows.Forms.TextBox();
            this.txtCadFuncConfirmSenha = new System.Windows.Forms.TextBox();
            this.txtCadFuncNome = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblLogin = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cbBoxCargoFunc = new System.Windows.Forms.ComboBox();
            this.txtCadFuncTelefone = new System.Windows.Forms.MaskedTextBox();
            this.txtCadFuncDataNasc = new System.Windows.Forms.MaskedTextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtCadFuncCPF = new System.Windows.Forms.MaskedTextBox();
            this.txtEndereço = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtEndereçoNum = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnMostrarSenha = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCadastrar
            // 
            this.btnCadastrar.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCadastrar.Location = new System.Drawing.Point(241, 725);
            this.btnCadastrar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCadastrar.Name = "btnCadastrar";
            this.btnCadastrar.Size = new System.Drawing.Size(100, 28);
            this.btnCadastrar.TabIndex = 78;
            this.btnCadastrar.Text = "Cadastrar";
            this.btnCadastrar.UseVisualStyleBackColor = true;
            this.btnCadastrar.Click += new System.EventHandler(this.btnCadastrar_Click);
            // 
            // btnVoltar
            // 
            this.btnVoltar.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVoltar.Location = new System.Drawing.Point(241, 761);
            this.btnVoltar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(100, 28);
            this.btnVoltar.TabIndex = 77;
            this.btnVoltar.Text = "Voltar";
            this.btnVoltar.UseVisualStyleBackColor = true;
            this.btnVoltar.Click += new System.EventHandler(this.btnVoltar_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(144, 142);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(243, 20);
            this.label12.TabIndex = 75;
            this.label12.Text = "Se já possuir uma conta, você pode";
            // 
            // txtCadFuncEmail
            // 
            this.txtCadFuncEmail.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCadFuncEmail.Location = new System.Drawing.Point(169, 545);
            this.txtCadFuncEmail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCadFuncEmail.Name = "txtCadFuncEmail";
            this.txtCadFuncEmail.Size = new System.Drawing.Size(284, 26);
            this.txtCadFuncEmail.TabIndex = 71;
            this.txtCadFuncEmail.Text = "gui@hotmail.com";
            this.txtCadFuncEmail.Click += new System.EventHandler(this.txtCadFuncEmail_Click);
            this.txtCadFuncEmail.TextChanged += new System.EventHandler(this.txtCadFuncEmail_TextChanged);
            this.txtCadFuncEmail.Enter += new System.EventHandler(this.txtCadFuncEmail_Enter);
            this.txtCadFuncEmail.Leave += new System.EventHandler(this.txtCadFuncEmail_Leave);
            // 
            // txtCadFuncSenha
            // 
            this.txtCadFuncSenha.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCadFuncSenha.Location = new System.Drawing.Point(169, 612);
            this.txtCadFuncSenha.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCadFuncSenha.Name = "txtCadFuncSenha";
            this.txtCadFuncSenha.Size = new System.Drawing.Size(157, 26);
            this.txtCadFuncSenha.TabIndex = 70;
            this.txtCadFuncSenha.Text = "func";
            this.txtCadFuncSenha.Click += new System.EventHandler(this.txtCadFuncSenha_Click);
            this.txtCadFuncSenha.TextChanged += new System.EventHandler(this.txtCadFuncSenha_TextChanged);
            this.txtCadFuncSenha.Enter += new System.EventHandler(this.txtCadFuncSenha_Enter);
            this.txtCadFuncSenha.Leave += new System.EventHandler(this.txtCadFuncSenha_Leave);
            // 
            // txtCadFuncConfirmSenha
            // 
            this.txtCadFuncConfirmSenha.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCadFuncConfirmSenha.Location = new System.Drawing.Point(169, 676);
            this.txtCadFuncConfirmSenha.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCadFuncConfirmSenha.Name = "txtCadFuncConfirmSenha";
            this.txtCadFuncConfirmSenha.Size = new System.Drawing.Size(157, 26);
            this.txtCadFuncConfirmSenha.TabIndex = 69;
            this.txtCadFuncConfirmSenha.Text = "func";
            this.txtCadFuncConfirmSenha.Click += new System.EventHandler(this.txtCadFuncConfirmSenha_Click);
            this.txtCadFuncConfirmSenha.TextChanged += new System.EventHandler(this.txtCadFuncConfirmSenha_TextChanged);
            this.txtCadFuncConfirmSenha.Enter += new System.EventHandler(this.txtCadFuncConfirmSenha_Enter);
            this.txtCadFuncConfirmSenha.Leave += new System.EventHandler(this.txtCadFuncConfirmSenha_Leave);
            // 
            // txtCadFuncNome
            // 
            this.txtCadFuncNome.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCadFuncNome.Location = new System.Drawing.Point(169, 217);
            this.txtCadFuncNome.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCadFuncNome.Name = "txtCadFuncNome";
            this.txtCadFuncNome.Size = new System.Drawing.Size(284, 26);
            this.txtCadFuncNome.TabIndex = 68;
            this.txtCadFuncNome.Text = "Guilherme";
            this.txtCadFuncNome.Click += new System.EventHandler(this.txtCadFuncNome_Click);
            this.txtCadFuncNome.TextChanged += new System.EventHandler(this.txtCadFuncNome_TextChanged);
            this.txtCadFuncNome.Enter += new System.EventHandler(this.txtCadFuncNome_Enter);
            this.txtCadFuncNome.Leave += new System.EventHandler(this.txtCadFuncNome_Leave);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(165, 656);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(145, 19);
            this.label11.TabIndex = 67;
            this.label11.Text = "Confirmação de senha";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(165, 592);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(46, 19);
            this.label10.TabIndex = 66;
            this.label10.Text = "Senha";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(165, 526);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 19);
            this.label9.TabIndex = 65;
            this.label9.Text = "Email";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(165, 330);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 19);
            this.label6.TabIndex = 62;
            this.label6.Text = "CPF";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(165, 262);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 19);
            this.label5.TabIndex = 61;
            this.label5.Text = "Cargo";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(165, 197);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label4.Size = new System.Drawing.Size(46, 19);
            this.label4.TabIndex = 60;
            this.label4.Text = "Nome";
            // 
            // lblLogin
            // 
            this.lblLogin.AutoSize = true;
            this.lblLogin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogin.Location = new System.Drawing.Point(404, 142);
            this.lblLogin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(81, 20);
            this.lblLogin.TabIndex = 59;
            this.lblLogin.TabStop = true;
            this.lblLogin.Text = "Entrar aqui";
            this.lblLogin.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblLogin_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(144, 111);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(292, 20);
            this.label3.TabIndex = 58;
            this.label3.Text = "Faça o cadastro do funcionário ingressante";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(144, 78);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(169, 20);
            this.label2.TabIndex = 57;
            this.label2.Text = "Cadastro do funcionário";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(51, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 37);
            this.label1.TabIndex = 56;
            this.label1.Text = "WORKFLOW";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.pictureBox2.BackgroundImage = global::Dev4Tech.Properties.Resources._141;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(773, 65);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(589, 438);
            this.pictureBox2.TabIndex = 80;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Dev4Tech.Properties.Resources.Group_33__2_;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(709, 14);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(733, 677);
            this.pictureBox1.TabIndex = 76;
            this.pictureBox1.TabStop = false;
            // 
            // cbBoxCargoFunc
            // 
            this.cbBoxCargoFunc.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBoxCargoFunc.FormattingEnabled = true;
            this.cbBoxCargoFunc.Items.AddRange(new object[] {
            "RH",
            "Contabilidade",
            "Estagiário"});
            this.cbBoxCargoFunc.Location = new System.Drawing.Point(169, 282);
            this.cbBoxCargoFunc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbBoxCargoFunc.Name = "cbBoxCargoFunc";
            this.cbBoxCargoFunc.Size = new System.Drawing.Size(171, 27);
            this.cbBoxCargoFunc.TabIndex = 81;
            this.cbBoxCargoFunc.Text = "Selecione o Cargo";
            this.cbBoxCargoFunc.SelectedIndexChanged += new System.EventHandler(this.cbBoxCargo_SelectedIndexChanged);
            // 
            // txtCadFuncTelefone
            // 
            this.txtCadFuncTelefone.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCadFuncTelefone.Location = new System.Drawing.Point(168, 476);
            this.txtCadFuncTelefone.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCadFuncTelefone.Mask = "+00 (99) 00000-0000";
            this.txtCadFuncTelefone.Name = "txtCadFuncTelefone";
            this.txtCadFuncTelefone.Size = new System.Drawing.Size(159, 26);
            this.txtCadFuncTelefone.TabIndex = 85;
            this.txtCadFuncTelefone.Text = "5588808080800";
            // 
            // txtCadFuncDataNasc
            // 
            this.txtCadFuncDataNasc.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCadFuncDataNasc.Location = new System.Drawing.Point(169, 417);
            this.txtCadFuncDataNasc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCadFuncDataNasc.Mask = "00/00/0000";
            this.txtCadFuncDataNasc.Name = "txtCadFuncDataNasc";
            this.txtCadFuncDataNasc.Size = new System.Drawing.Size(86, 26);
            this.txtCadFuncDataNasc.TabIndex = 84;
            this.txtCadFuncDataNasc.Text = "21082005";
            this.txtCadFuncDataNasc.ValidatingType = typeof(System.DateTime);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(165, 457);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(59, 19);
            this.label13.TabIndex = 83;
            this.label13.Text = "Telefone";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(165, 398);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(131, 19);
            this.label14.TabIndex = 82;
            this.label14.Text = "Data de nascimento";
            // 
            // txtCadFuncCPF
            // 
            this.txtCadFuncCPF.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCadFuncCPF.Location = new System.Drawing.Point(169, 350);
            this.txtCadFuncCPF.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCadFuncCPF.Mask = "000.000.000-00";
            this.txtCadFuncCPF.Name = "txtCadFuncCPF";
            this.txtCadFuncCPF.Size = new System.Drawing.Size(157, 26);
            this.txtCadFuncCPF.TabIndex = 86;
            this.txtCadFuncCPF.Text = "78744394985";
            // 
            // txtEndereço
            // 
            this.txtEndereço.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEndereço.Location = new System.Drawing.Point(397, 283);
            this.txtEndereço.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtEndereço.Name = "txtEndereço";
            this.txtEndereço.Size = new System.Drawing.Size(244, 26);
            this.txtEndereço.TabIndex = 88;
            this.txtEndereço.Text = "Avenida Paulista";
            this.txtEndereço.TextChanged += new System.EventHandler(this.txtEndereço_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(393, 262);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 19);
            this.label7.TabIndex = 87;
            this.label7.Text = "Endereço";
            // 
            // txtEndereçoNum
            // 
            this.txtEndereçoNum.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEndereçoNum.Location = new System.Drawing.Point(397, 350);
            this.txtEndereçoNum.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtEndereçoNum.Name = "txtEndereçoNum";
            this.txtEndereçoNum.Size = new System.Drawing.Size(73, 26);
            this.txtEndereçoNum.TabIndex = 89;
            this.txtEndereçoNum.Text = "530";
            this.txtEndereçoNum.TextChanged += new System.EventHandler(this.txtEndereçoNum_TextChanged);
            this.txtEndereçoNum.Enter += new System.EventHandler(this.txtEndereçoNum_Enter);
            this.txtEndereçoNum.Leave += new System.EventHandler(this.txtEndereçoNum_Leave);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(393, 327);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(139, 19);
            this.label8.TabIndex = 90;
            this.label8.Text = "Número do endereço";
            // 
            // btnMostrarSenha
            // 
            this.btnMostrarSenha.Location = new System.Drawing.Point(333, 612);
            this.btnMostrarSenha.Name = "btnMostrarSenha";
            this.btnMostrarSenha.Size = new System.Drawing.Size(61, 26);
            this.btnMostrarSenha.TabIndex = 91;
            this.btnMostrarSenha.Text = "Ocultar";
            this.btnMostrarSenha.UseVisualStyleBackColor = true;
            this.btnMostrarSenha.Click += new System.EventHandler(this.btnMostrarSenha_Click);
            // 
            // cadastro_funcionário
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1685, 838);
            this.Controls.Add(this.btnMostrarSenha);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtEndereçoNum);
            this.Controls.Add(this.txtEndereço);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtCadFuncCPF);
            this.Controls.Add(this.txtCadFuncTelefone);
            this.Controls.Add(this.txtCadFuncDataNasc);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.cbBoxCargoFunc);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.btnCadastrar);
            this.Controls.Add(this.btnVoltar);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtCadFuncEmail);
            this.Controls.Add(this.txtCadFuncSenha);
            this.Controls.Add(this.txtCadFuncConfirmSenha);
            this.Controls.Add(this.txtCadFuncNome);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblLogin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "cadastro_funcionário";
            this.Text = "cadastro_funcionário";
            this.Load += new System.EventHandler(this.cadastro_funcionário_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCadastrar;
        private System.Windows.Forms.Button btnVoltar;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtCadFuncEmail;
        private System.Windows.Forms.TextBox txtCadFuncSenha;
        private System.Windows.Forms.TextBox txtCadFuncConfirmSenha;
        private System.Windows.Forms.TextBox txtCadFuncNome;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel lblLogin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ComboBox cbBoxCargoFunc;
        private System.Windows.Forms.MaskedTextBox txtCadFuncTelefone;
        private System.Windows.Forms.MaskedTextBox txtCadFuncDataNasc;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.MaskedTextBox txtCadFuncCPF;
        private System.Windows.Forms.TextBox txtEndereço;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtEndereçoNum;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnMostrarSenha;
    }
}