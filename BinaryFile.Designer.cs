﻿namespace BinaryFileIO
{
    partial class BinaryFile
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
            this.dgv = new System.Windows.Forms.DataGridView();
            this.btInActivate = new System.Windows.Forms.Button();
            this.btUpdate = new System.Windows.Forms.Button();
            this.btReWrite = new System.Windows.Forms.Button();
            this.tbIndex = new System.Windows.Forms.TextBox();
            this.tbGo = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgv.Location = new System.Drawing.Point(0, 0);
            this.dgv.Name = "dgv";
            this.dgv.Size = new System.Drawing.Size(784, 512);
            this.dgv.TabIndex = 0;
            // 
            // btInActivate
            // 
            this.btInActivate.Location = new System.Drawing.Point(697, 527);
            this.btInActivate.Name = "btInActivate";
            this.btInActivate.Size = new System.Drawing.Size(75, 23);
            this.btInActivate.TabIndex = 1;
            this.btInActivate.Text = "Inactivate";
            this.btInActivate.UseVisualStyleBackColor = true;
            this.btInActivate.Click += new System.EventHandler(this.btInActivate_Click);
            // 
            // btUpdate
            // 
            this.btUpdate.Location = new System.Drawing.Point(616, 527);
            this.btUpdate.Name = "btUpdate";
            this.btUpdate.Size = new System.Drawing.Size(75, 23);
            this.btUpdate.TabIndex = 2;
            this.btUpdate.Text = "Update";
            this.btUpdate.UseVisualStyleBackColor = true;
            this.btUpdate.Click += new System.EventHandler(this.btUpdate_Click);
            // 
            // btReWrite
            // 
            this.btReWrite.Location = new System.Drawing.Point(535, 527);
            this.btReWrite.Name = "btReWrite";
            this.btReWrite.Size = new System.Drawing.Size(75, 23);
            this.btReWrite.TabIndex = 3;
            this.btReWrite.Text = "ReWrite";
            this.btReWrite.UseVisualStyleBackColor = true;
            this.btReWrite.Click += new System.EventHandler(this.btReWrite_Click);
            // 
            // tbIndex
            // 
            this.tbIndex.Location = new System.Drawing.Point(13, 527);
            this.tbIndex.Name = "tbIndex";
            this.tbIndex.Size = new System.Drawing.Size(100, 20);
            this.tbIndex.TabIndex = 4;
            // 
            // tbGo
            // 
            this.tbGo.Location = new System.Drawing.Point(454, 527);
            this.tbGo.Name = "tbGo";
            this.tbGo.Size = new System.Drawing.Size(75, 23);
            this.tbGo.TabIndex = 5;
            this.tbGo.Text = "Go";
            this.tbGo.UseVisualStyleBackColor = true;
            this.tbGo.Click += new System.EventHandler(this.tbGo_Click);
            // 
            // BinaryFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.tbGo);
            this.Controls.Add(this.tbIndex);
            this.Controls.Add(this.btReWrite);
            this.Controls.Add(this.btUpdate);
            this.Controls.Add(this.btInActivate);
            this.Controls.Add(this.dgv);
            this.Name = "BinaryFile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BinaryFile";
            this.Load += new System.EventHandler(this.BinaryFile_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button btInActivate;
        private System.Windows.Forms.Button btUpdate;
        private System.Windows.Forms.Button btReWrite;
        private System.Windows.Forms.TextBox tbIndex;
        private System.Windows.Forms.Button tbGo;
    }
}