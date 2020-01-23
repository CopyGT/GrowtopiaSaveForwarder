namespace ChattuyCS
{
    partial class mainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.send_packet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // send_packet
            // 
            this.send_packet.Location = new System.Drawing.Point(164, 131);
            this.send_packet.Name = "send_packet";
            this.send_packet.Size = new System.Drawing.Size(116, 42);
            this.send_packet.TabIndex = 0;
            this.send_packet.Text = "Send Packet";
            this.send_packet.UseVisualStyleBackColor = true;
            this.send_packet.Click += new System.EventHandler(this.send_packet_Click);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(124, 0);
            this.Controls.Add(this.send_packet);
            this.MaximizeBox = false;
            this.Name = "mainForm";
            this.Opacity = 0D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Chattuy";
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button send_packet;
    }
}

