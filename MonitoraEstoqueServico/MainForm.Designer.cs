namespace MonitoraEstoqueServico
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            button3 = new Button();
            button2 = new Button();
            button1 = new Button();
            labelMonitoramento = new Label();
            tabPage2 = new TabPage();
            nrIntervalo = new NumericUpDown();
            lnkLogs = new LinkLabel();
            btnSalvar = new Button();
            txtNotificaEmail = new TextBox();
            strFormatoIntervalo = new ComboBox();
            nrEstoqueBaixo = new NumericUpDown();
            nrEstoqueAlto = new NumericUpDown();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nrIntervalo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nrEstoqueBaixo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nrEstoqueAlto).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(364, 243);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(button3);
            tabPage1.Controls.Add(button2);
            tabPage1.Controls.Add(button1);
            tabPage1.Controls.Add(labelMonitoramento);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(356, 215);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Monitoramento";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Location = new Point(85, 164);
            button3.Name = "button3";
            button3.Size = new Size(183, 36);
            button3.TabIndex = 3;
            button3.Text = "Reiniciar";
            button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(85, 112);
            button2.Name = "button2";
            button2.Size = new Size(183, 36);
            button2.TabIndex = 2;
            button2.Text = "Parar";
            button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(85, 57);
            button1.Name = "button1";
            button1.Size = new Size(183, 36);
            button1.TabIndex = 1;
            button1.Text = "Inicializar";
            button1.UseVisualStyleBackColor = true;
            // 
            // labelMonitoramento
            // 
            labelMonitoramento.AutoSize = true;
            labelMonitoramento.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelMonitoramento.Location = new Point(104, 12);
            labelMonitoramento.Name = "labelMonitoramento";
            labelMonitoramento.Size = new Size(154, 25);
            labelMonitoramento.TabIndex = 0;
            labelMonitoramento.Text = "Monitoramento";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(nrIntervalo);
            tabPage2.Controls.Add(lnkLogs);
            tabPage2.Controls.Add(btnSalvar);
            tabPage2.Controls.Add(txtNotificaEmail);
            tabPage2.Controls.Add(strFormatoIntervalo);
            tabPage2.Controls.Add(nrEstoqueBaixo);
            tabPage2.Controls.Add(nrEstoqueAlto);
            tabPage2.Controls.Add(label5);
            tabPage2.Controls.Add(label6);
            tabPage2.Controls.Add(label7);
            tabPage2.Controls.Add(label8);
            tabPage2.Controls.Add(label9);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(356, 215);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Configurações";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // nrIntervalo
            // 
            nrIntervalo.Location = new Point(199, 53);
            nrIntervalo.Name = "nrIntervalo";
            nrIntervalo.Size = new Size(42, 23);
            nrIntervalo.TabIndex = 25;
            // 
            // lnkLogs
            // 
            lnkLogs.AutoSize = true;
            lnkLogs.Location = new Point(219, 180);
            lnkLogs.Name = "lnkLogs";
            lnkLogs.Size = new Size(97, 15);
            lnkLogs.TabIndex = 24;
            lnkLogs.TabStop = true;
            lnkLogs.Text = "Controle de Logs";
            lnkLogs.LinkClicked += lnkLogs_LinkClicked;
            // 
            // btnSalvar
            // 
            btnSalvar.Location = new Point(74, 176);
            btnSalvar.Name = "btnSalvar";
            btnSalvar.Size = new Size(75, 23);
            btnSalvar.TabIndex = 23;
            btnSalvar.Text = "Salvar";
            btnSalvar.UseVisualStyleBackColor = true;
            btnSalvar.Click += btnSalvar_Click;
            // 
            // txtNotificaEmail
            // 
            txtNotificaEmail.Location = new Point(201, 82);
            txtNotificaEmail.Name = "txtNotificaEmail";
            txtNotificaEmail.Size = new Size(125, 23);
            txtNotificaEmail.TabIndex = 22;
            // 
            // strFormatoIntervalo
            // 
            strFormatoIntervalo.FormattingEnabled = true;
            strFormatoIntervalo.Items.AddRange(new object[] { "Segundos", "Minutos", "Horas" });
            strFormatoIntervalo.Location = new Point(247, 53);
            strFormatoIntervalo.Name = "strFormatoIntervalo";
            strFormatoIntervalo.Size = new Size(79, 23);
            strFormatoIntervalo.TabIndex = 21;
            // 
            // nrEstoqueBaixo
            // 
            nrEstoqueBaixo.Location = new Point(201, 140);
            nrEstoqueBaixo.Name = "nrEstoqueBaixo";
            nrEstoqueBaixo.Size = new Size(40, 23);
            nrEstoqueBaixo.TabIndex = 20;
            // 
            // nrEstoqueAlto
            // 
            nrEstoqueAlto.Location = new Point(201, 111);
            nrEstoqueAlto.Name = "nrEstoqueAlto";
            nrEstoqueAlto.Size = new Size(40, 23);
            nrEstoqueAlto.TabIndex = 19;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(67, 85);
            label5.Name = "label5";
            label5.Size = new Size(132, 15);
            label5.TabIndex = 17;
            label5.Text = "E-mail para notificação:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(41, 142);
            label6.Name = "label6";
            label6.Size = new Size(158, 15);
            label6.TabIndex = 16;
            label6.Text = "Quantiade de Estoque Baixo:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(41, 114);
            label7.Name = "label7";
            label7.Size = new Size(158, 15);
            label7.TabIndex = 15;
            label7.Text = "Quantidade de Estoque Alto:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(74, 57);
            label8.Name = "label8";
            label8.Size = new Size(125, 15);
            label8.TabIndex = 14;
            label8.Text = "Intervalo de Execução:";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.Location = new Point(40, 12);
            label9.Name = "label9";
            label9.Size = new Size(201, 25);
            label9.TabIndex = 13;
            label9.Text = "Configurações Gerais";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(388, 267);
            Controls.Add(tabControl1);
            Name = "MainForm";
            Text = "Form1";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nrIntervalo).EndInit();
            ((System.ComponentModel.ISupportInitialize)nrEstoqueBaixo).EndInit();
            ((System.ComponentModel.ISupportInitialize)nrEstoqueAlto).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Label label3;
        private Label label2;
        private Label label1;
        private Label labelMonitoramento;
        private LinkLabel lnkLogs;
        private Button btnSalvar;
        private TextBox txtNotificaEmail;
        private ComboBox strFormatoIntervalo;
        private NumericUpDown nrEstoqueBaixo;
        private NumericUpDown nrEstoqueAlto;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Button button1;
        private Button button3;
        private Button button2;
        private Label label4;
        private NumericUpDown numericUpDown4;
        private NumericUpDown numericUpDown3;
        private NumericUpDown nrIntervalo;
    }
}
