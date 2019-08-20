namespace CSharpErgoBoard
{
    partial class Popup
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
            this.id_buttonOkay = new System.Windows.Forms.Button();
            this.id_labelText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // id_buttonOkay
            // 
            this.id_buttonOkay.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.id_buttonOkay.Location = new System.Drawing.Point(110, 38);
            this.id_buttonOkay.Name = "id_buttonOkay";
            this.id_buttonOkay.Size = new System.Drawing.Size(75, 23);
            this.id_buttonOkay.TabIndex = 0;
            this.id_buttonOkay.Text = "Okay";
            this.id_buttonOkay.UseVisualStyleBackColor = true;
            this.id_buttonOkay.Click += new System.EventHandler(this.Id_buttonOkay_Click);
            // 
            // id_labelText
            // 
            this.id_labelText.AutoSize = true;
            this.id_labelText.Location = new System.Drawing.Point(57, 9);
            this.id_labelText.Name = "id_labelText";
            this.id_labelText.Size = new System.Drawing.Size(178, 13);
            this.id_labelText.TabIndex = 1;
            this.id_labelText.Text = "If you read this something happened";
            // 
            // Popup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 69);
            this.Controls.Add(this.id_labelText);
            this.Controls.Add(this.id_buttonOkay);
            this.Name = "Popup";
            this.Text = "Popup Title";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button id_buttonOkay;
        private System.Windows.Forms.Label id_labelText;
    }
}