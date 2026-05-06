namespace prjWinCsImmoRemax
{
    partial class frmRecherche
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAllMaisons = new System.Windows.Forms.Button();
            this.grdAgents = new System.Windows.Forms.DataGridView();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtVille = new System.Windows.Forms.TextBox();
            this.btnFiltrerMaisons = new System.Windows.Forms.Button();
            this.txtPrix = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.grdMaisons = new System.Windows.Forms.DataGridView();
            this.btnFermer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAgents)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMaisons)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Blue;
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.grdAgents);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(23, 58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1439, 439);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Agents de Remax";
            // 
            // btnAllMaisons
            // 
            this.btnAllMaisons.ForeColor = System.Drawing.Color.Red;
            this.btnAllMaisons.Location = new System.Drawing.Point(1143, 62);
            this.btnAllMaisons.Name = "btnAllMaisons";
            this.btnAllMaisons.Size = new System.Drawing.Size(202, 38);
            this.btnAllMaisons.TabIndex = 3;
            this.btnAllMaisons.Text = "Toutes Maisons";
            this.btnAllMaisons.UseVisualStyleBackColor = true;
            this.btnAllMaisons.Click += new System.EventHandler(this.btnAllMaisons_Click);
            // 
            // grdAgents
            // 
            this.grdAgents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdAgents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdAgents.DefaultCellStyle = dataGridViewCellStyle3;
            this.grdAgents.Location = new System.Drawing.Point(20, 62);
            this.grdAgents.MultiSelect = false;
            this.grdAgents.Name = "grdAgents";
            this.grdAgents.ReadOnly = true;
            this.grdAgents.RowHeadersWidth = 62;
            this.grdAgents.RowTemplate.Height = 28;
            this.grdAgents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdAgents.Size = new System.Drawing.Size(1001, 338);
            this.grdAgents.TabIndex = 0;
            this.grdAgents.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdAgents_CellContentClick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(499, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(422, 46);
            this.label6.TabIndex = 6;
            this.label6.Text = "REMAX IMMOBILIER";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Blue;
            this.groupBox2.Controls.Add(this.btnFermer);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.btnAllMaisons);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtVille);
            this.groupBox2.Controls.Add(this.btnFiltrerMaisons);
            this.groupBox2.Controls.Add(this.txtPrix);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.grdMaisons);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(31, 514);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1431, 589);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Maisons de Remax";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(385, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 25);
            this.label4.TabIndex = 6;
            this.label4.Text = "Ville :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(691, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 25);
            this.label3.TabIndex = 5;
            this.label3.Text = "$";
            // 
            // txtVille
            // 
            this.txtVille.Location = new System.Drawing.Point(491, 88);
            this.txtVille.Name = "txtVille";
            this.txtVille.Size = new System.Drawing.Size(181, 30);
            this.txtVille.TabIndex = 4;
            // 
            // btnFiltrerMaisons
            // 
            this.btnFiltrerMaisons.ForeColor = System.Drawing.Color.Red;
            this.btnFiltrerMaisons.Location = new System.Drawing.Point(1200, 15);
            this.btnFiltrerMaisons.Name = "btnFiltrerMaisons";
            this.btnFiltrerMaisons.Size = new System.Drawing.Size(145, 38);
            this.btnFiltrerMaisons.TabIndex = 3;
            this.btnFiltrerMaisons.Text = "Filtrer Maisons";
            this.btnFiltrerMaisons.UseVisualStyleBackColor = true;
            this.btnFiltrerMaisons.Click += new System.EventHandler(this.btnFiltrerMaisons_Click);
            // 
            // txtPrix
            // 
            this.txtPrix.Location = new System.Drawing.Point(491, 39);
            this.txtPrix.Name = "txtPrix";
            this.txtPrix.Size = new System.Drawing.Size(181, 30);
            this.txtPrix.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(411, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Filtrer les maisons par prix maximum de : ";
            // 
            // grdMaisons
            // 
            this.grdMaisons.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdMaisons.DefaultCellStyle = dataGridViewCellStyle4;
            this.grdMaisons.Location = new System.Drawing.Point(12, 162);
            this.grdMaisons.MultiSelect = false;
            this.grdMaisons.Name = "grdMaisons";
            this.grdMaisons.ReadOnly = true;
            this.grdMaisons.RowHeadersWidth = 62;
            this.grdMaisons.RowTemplate.Height = 28;
            this.grdMaisons.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdMaisons.Size = new System.Drawing.Size(1392, 396);
            this.grdMaisons.TabIndex = 0;
            // 
            // btnFermer
            // 
            this.btnFermer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFermer.ForeColor = System.Drawing.Color.Red;
            this.btnFermer.Location = new System.Drawing.Point(1200, 112);
            this.btnFermer.Name = "btnFermer";
            this.btnFermer.Size = new System.Drawing.Size(145, 38);
            this.btnFermer.TabIndex = 8;
            this.btnFermer.Text = "Fermer";
            this.btnFermer.UseVisualStyleBackColor = true;
            this.btnFermer.Click += new System.EventHandler(this.btnFermer_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(433, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Cliquez sur un agent pour voir ses maisons.";
            // 
            // frmRecherche
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(1487, 1115);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmRecherche";
            this.Text = "frmRecherche";
            this.Load += new System.EventHandler(this.frmRecherche_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAgents)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMaisons)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView grdAgents;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnAllMaisons;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnFiltrerMaisons;
        private System.Windows.Forms.TextBox txtPrix;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView grdMaisons;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtVille;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnFermer;
        private System.Windows.Forms.Label label1;
    }
}