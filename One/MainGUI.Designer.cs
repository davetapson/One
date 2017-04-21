namespace One
{
    partial class MainGUI
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtMessages = new System.Windows.Forms.TextBox();
            this.btnOrder = new System.Windows.Forms.Button();
            this.btnOrderLimit = new System.Windows.Forms.Button();
            this.btnOrderMKTShort = new System.Windows.Forms.Button();
            this.btnOrderLMTShort = new System.Windows.Forms.Button();
            this.btnTestDB = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtMessages
            // 
            this.txtMessages.Location = new System.Drawing.Point(12, 92);
            this.txtMessages.Multiline = true;
            this.txtMessages.Name = "txtMessages";
            this.txtMessages.Size = new System.Drawing.Size(733, 157);
            this.txtMessages.TabIndex = 1;
            // 
            // btnOrder
            // 
            this.btnOrder.Location = new System.Drawing.Point(93, 12);
            this.btnOrder.Name = "btnOrder";
            this.btnOrder.Size = new System.Drawing.Size(75, 23);
            this.btnOrder.TabIndex = 2;
            this.btnOrder.Text = "Order MKT";
            this.btnOrder.UseVisualStyleBackColor = true;
            this.btnOrder.Click += new System.EventHandler(this.btnOrder_Click);
            // 
            // btnOrderLimit
            // 
            this.btnOrderLimit.Location = new System.Drawing.Point(93, 41);
            this.btnOrderLimit.Name = "btnOrderLimit";
            this.btnOrderLimit.Size = new System.Drawing.Size(75, 23);
            this.btnOrderLimit.TabIndex = 3;
            this.btnOrderLimit.Text = "Order LMT";
            this.btnOrderLimit.UseVisualStyleBackColor = true;
            this.btnOrderLimit.Click += new System.EventHandler(this.btnOrderLimit_Click);
            // 
            // btnOrderMKTShort
            // 
            this.btnOrderMKTShort.Location = new System.Drawing.Point(174, 12);
            this.btnOrderMKTShort.Name = "btnOrderMKTShort";
            this.btnOrderMKTShort.Size = new System.Drawing.Size(97, 23);
            this.btnOrderMKTShort.TabIndex = 4;
            this.btnOrderMKTShort.Text = "Order MKT Short";
            this.btnOrderMKTShort.UseVisualStyleBackColor = true;
            this.btnOrderMKTShort.Click += new System.EventHandler(this.btnOrderMKTShort_Click);
            // 
            // btnOrderLMTShort
            // 
            this.btnOrderLMTShort.Location = new System.Drawing.Point(175, 40);
            this.btnOrderLMTShort.Name = "btnOrderLMTShort";
            this.btnOrderLMTShort.Size = new System.Drawing.Size(96, 23);
            this.btnOrderLMTShort.TabIndex = 5;
            this.btnOrderLMTShort.Text = "Order LMT Short";
            this.btnOrderLMTShort.UseVisualStyleBackColor = true;
            this.btnOrderLMTShort.Click += new System.EventHandler(this.btnOrderLMTShort_Click);
            // 
            // btnTestDB
            // 
            this.btnTestDB.Location = new System.Drawing.Point(277, 12);
            this.btnTestDB.Name = "btnTestDB";
            this.btnTestDB.Size = new System.Drawing.Size(75, 23);
            this.btnTestDB.TabIndex = 6;
            this.btnTestDB.Text = "Test DB";
            this.btnTestDB.UseVisualStyleBackColor = true;
            this.btnTestDB.Click += new System.EventHandler(this.btnTestDB_Click);
            // 
            // MainGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 261);
            this.Controls.Add(this.btnTestDB);
            this.Controls.Add(this.btnOrderLMTShort);
            this.Controls.Add(this.btnOrderMKTShort);
            this.Controls.Add(this.btnOrderLimit);
            this.Controls.Add(this.btnOrder);
            this.Controls.Add(this.txtMessages);
            this.Controls.Add(this.btnConnect);
            this.Name = "MainGUI";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainGUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtMessages;
        private System.Windows.Forms.Button btnOrder;
        private System.Windows.Forms.Button btnOrderLimit;
        private System.Windows.Forms.Button btnOrderMKTShort;
        private System.Windows.Forms.Button btnOrderLMTShort;
        private System.Windows.Forms.Button btnTestDB;
    }
}

