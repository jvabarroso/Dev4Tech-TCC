namespace Dev4Tech
{
    partial class Cadastro_empresa_admin
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtCadAdmCargo = new System.Windows.Forms.TextBox();
            this.txtCadAdmCPF = new System.Windows.Forms.TextBox();
            this.txtCadAdmTelefone = new System.Windows.Forms.TextBox();
            this.txtCadAdmEmail = new System.Windows.Forms.TextBox();
            this.txtCadAdmSenha = new System.Windows.Forms.TextBox();
            this.txtCadAdmConfirmSenha = new System.Windows.Forms.TextBox();
            this.txtCadAdmNome = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblLoginAdm = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCadAdmDataNasc = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCadastrar
            // 
            this.btnCadastrar.Location = new System.Drawing.Point(285, 720);
            this.btnCadastrar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCadastrar.Name = "btnCadastrar";
            this.btnCadastrar.Size = new System.Drawing.Size(100, 28);
            this.btnCadastrar.TabIndex = 54;
            this.btnCadastrar.Text = "Cadastrar";
            this.btnCadastrar.UseVisualStyleBackColor = true;
            this.btnCadastrar.Click += new System.EventHandler(this.btnCadastrar_Click);
            // 
            // btnVoltar
            // 
            this.btnVoltar.Location = new System.Drawing.Point(285, 756);
            this.btnVoltar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(100, 28);
            this.btnVoltar.TabIndex = 53;
            this.btnVoltar.Text = "Voltar";
            this.btnVoltar.UseVisualStyleBackColor = true;
            this.btnVoltar.Click += new System.EventHandler(this.btnVoltar_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Dev4Tech.Properties.Resources.Group_33__1_;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(773, 22);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(764, 678);
            this.pictureBox1.TabIndex = 52;
            this.pictureBox1.TabStop = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(197, 149);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(263, 17);
            this.label12.TabIndex = 51;
            this.label12.Text = "Se você já possui uma conta, você pode";
            // 
            // txtCadAdmCargo
            // 
            this.txtCadAdmCargo.Location = new System.Drawing.Point(201, 282);
            this.txtCadAdmCargo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCadAdmCargo.Name = "txtCadAdmCargo";
            this.txtCadAdmCargo.Size = new System.Drawing.Size(195, 22);
            this.txtCadAdmCargo.TabIndex = 50;
            this.txtCadAdmCargo.Text = "Digite seu cargo na empresa";
            this.txtCadAdmCargo.Click += new System.EventHandler(this.txtCadAdmCargo_Click);
            // 
            // txtCadAdmCPF
            // 
            this.txtCadAdmCPF.Location = new System.Drawing.Point(201, 350);
            this.txtCadAdmCPF.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCadAdmCPF.Name = "txtCadAdmCPF";
            this.txtCadAdmCPF.Size = new System.Drawing.Size(121, 22);
            this.txtCadAdmCPF.TabIndex = 49;
            this.txtCadAdmCPF.Text = "Digite seu CPF";
            this.txtCadAdmCPF.Click += new System.EventHandler(this.txtCadAdmCPF_Click);
            // 
            // txtCadAdmTelefone
            // 
            this.txtCadAdmTelefone.Location = new System.Drawing.Point(201, 481);
            this.txtCadAdmTelefone.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCadAdmTelefone.Name = "txtCadAdmTelefone";
            this.txtCadAdmTelefone.Size = new System.Drawing.Size(112, 22);
            this.txtCadAdmTelefone.TabIndex = 48;
            this.txtCadAdmTelefone.Text = "99-9999-99999";
            this.txtCadAdmTelefone.Click += new System.EventHandler(this.txtCadAdmTelefone_Click);
            // 
            // txtCadAdmEmail
            // 
            this.txtCadAdmEmail.Location = new System.Drawing.Point(201, 545);
            this.txtCadAdmEmail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCadAdmEmail.Name = "txtCadAdmEmail";
            this.txtCadAdmEmail.Size = new System.Drawing.Size(195, 22);
            this.txtCadAdmEmail.TabIndex = 47;
            this.txtCadAdmEmail.Text = "Digite seu email institucional";
            this.txtCadAdmEmail.Click += new System.EventHandler(this.txtCadAdmEmail_Click);
            // 
            // txtCadAdmSenha
            // 
            this.txtCadAdmSenha.Location = new System.Drawing.Point(201, 612);
            this.txtCadAdmSenha.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCadAdmSenha.Name = "txtCadAdmSenha";
            this.txtCadAdmSenha.Size = new System.Drawing.Size(121, 22);
            this.txtCadAdmSenha.TabIndex = 46;
            this.txtCadAdmSenha.Text = "Digite sua senha";
            this.txtCadAdmSenha.Click += new System.EventHandler(this.txtCadAdmSenha_Click);
            // 
            // txtCadAdmConfirmSenha
            // 
            this.txtCadAdmConfirmSenha.Location = new System.Drawing.Point(201, 676);
            this.txtCadAdmConfirmSenha.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCadAdmConfirmSenha.Name = "txtCadAdmConfirmSenha";
            this.txtCadAdmConfirmSenha.Size = new System.Drawing.Size(145, 22);
            this.txtCadAdmConfirmSenha.TabIndex = 45;
            this.txtCadAdmConfirmSenha.Text = "Confirme sua senha";
            this.txtCadAdmConfirmSenha.Click += new System.EventHandler(this.txtCadAdmConfirmSenha_Click);
            // 
            // txtCadAdmNome
            // 
            this.txtCadAdmNome.Location = new System.Drawing.Point(201, 217);
            this.txtCadAdmNome.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCadAdmNome.Name = "txtCadAdmNome";
            this.txtCadAdmNome.Size = new System.Drawing.Size(145, 22);
            this.txtCadAdmNome.TabIndex = 44;
            this.txtCadAdmNome.Text = "Digite seu nome";
            this.txtCadAdmNome.Click += new System.EventHandler(this.txtCadAdmNome_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(197, 656);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(150, 17);
            this.label11.TabIndex = 43;
            this.label11.Text = "Confirmação de senha";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(197, 592);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 17);
            this.label10.TabIndex = 42;
            this.label10.Text = "Senha";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(197, 526);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 17);
            this.label9.TabIndex = 41;
            this.label9.Text = "Email";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(197, 462);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 17);
            this.label8.TabIndex = 40;
            this.label8.Text = "Telefone";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(197, 402);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(134, 17);
            this.label7.TabIndex = 39;
            this.label7.Text = "Data de nascimento";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(197, 330);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 17);
            this.label6.TabIndex = 38;
            this.label6.Text = "CPF";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(197, 262);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 17);
            this.label5.TabIndex = 37;
            this.label5.Text = "Cargo";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(197, 197);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label4.Size = new System.Drawing.Size(45, 17);
            this.label4.TabIndex = 36;
            this.label4.Text = "Nome";
            // 
            // lblLoginAdm
            // 
            this.lblLoginAdm.AutoSize = true;
            this.lblLoginAdm.Location = new System.Drawing.Point(463, 149);
            this.lblLoginAdm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLoginAdm.Name = "lblLoginAdm";
            this.lblLoginAdm.Size = new System.Drawing.Size(78, 17);
            this.lblLoginAdm.TabIndex = 35;
            this.lblLoginAdm.TabStop = true;
            this.lblLoginAdm.Text = "Entrar aqui";
            this.lblLoginAdm.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblLoginAdm_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(197, 118);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(302, 17);
            this.label3.TabIndex = 34;
            this.label3.Text = "Faça o cadastro do representante da empresa";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(197, 85);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 17);
            this.label2.TabIndex = 33;
            this.label2.Text = "Cadastro do administrador";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 17);
            this.label1.TabIndex = 32;
            this.label1.Text = "WORKFLOW";
            // 
            // txtCadAdmDataNasc
            // 
            this.txtCadAdmDataNasc.Location = new System.Drawing.Point(201, 422);
            this.txtCadAdmDataNasc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCadAdmDataNasc.Name = "txtCadAdmDataNasc";
            this.txtCadAdmDataNasc.Size = new System.Drawing.Size(211, 22);
            this.txtCadAdmDataNasc.TabIndex = 55;
            this.txtCadAdmDataNasc.Text = "Digite sua data de nascimento";
            this.txtCadAdmDataNasc.Click += new System.EventHandler(this.txtCadAdmDataNasc_Click);
            // 
            // Cadastro_empresa_admin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1685, 838);
            this.Controls.Add(this.txtCadAdmDataNasc);
            this.Controls.Add(this.btnCadastrar);
            this.Controls.Add(this.btnVoltar);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtCadAdmCargo);
            this.Controls.Add(this.txtCadAdmCPF);
            this.Controls.Add(this.txtCadAdmTelefone);
            this.Controls.Add(this.txtCadAdmEmail);
            this.Controls.Add(this.txtCadAdmSenha);
            this.Controls.Add(this.txtCadAdmConfirmSenha);
            this.Controls.Add(this.txtCadAdmNome);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblLoginAdm);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Cadastro_empresa_admin";
            this.Text = "Cadastro_empresa_admin";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCadastrar;
        private System.Windows.Forms.Button btnVoltar;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtCadAdmCargo;
        private System.Windows.Forms.TextBox txtCadAdmCPF;
        private System.Windows.Forms.TextBox txtCadAdmTelefone;
        private System.Windows.Forms.TextBox txtCadAdmEmail;
        private System.Windows.Forms.TextBox txtCadAdmSenha;
        private System.Windows.Forms.TextBox txtCadAdmConfirmSenha;
        private System.Windows.Forms.TextBox txtCadAdmNome;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel lblLoginAdm;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCadAdmDataNasc;
    }
}