namespace ElementEditorAddIn
{
    partial class ElementEditorForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.elementHost = new System.Windows.Forms.Integration.ElementHost();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.eleListBox = new System.Windows.Forms.ListBox();
            this.categoryLabel = new System.Windows.Forms.Label();
            this.selectCategoryButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.seperator1 = new System.Windows.Forms.Label();
            this.paraEditButton = new System.Windows.Forms.Button();
            this.paraEditTextBox = new System.Windows.Forms.TextBox();
            this.paraEditLabel = new System.Windows.Forms.Label();
            this.paraTypeLabel = new System.Windows.Forms.Label();
            this.paraTypeTextBox = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.filterValueTextBox = new System.Windows.Forms.TextBox();
            this.filterOptionComboBox = new System.Windows.Forms.ComboBox();
            this.paraComboBox = new System.Windows.Forms.ComboBox();
            this.commomParaLabel = new System.Windows.Forms.Label();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.elementHost);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(595, 553);
            this.splitContainer1.SplitterDistance = 306;
            this.splitContainer1.TabIndex = 0;
            // 
            // elementHost
            // 
            this.elementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost.Location = new System.Drawing.Point(0, 0);
            this.elementHost.Name = "elementHost";
            this.elementHost.Size = new System.Drawing.Size(591, 302);
            this.elementHost.TabIndex = 0;
            this.elementHost.Child = null;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.eleListBox);
            this.splitContainer2.Panel1.Controls.Add(this.categoryLabel);
            this.splitContainer2.Panel1.Controls.Add(this.selectCategoryButton);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.label1);
            this.splitContainer2.Panel2.Controls.Add(this.seperator1);
            this.splitContainer2.Panel2.Controls.Add(this.paraEditButton);
            this.splitContainer2.Panel2.Controls.Add(this.paraEditTextBox);
            this.splitContainer2.Panel2.Controls.Add(this.paraEditLabel);
            this.splitContainer2.Panel2.Controls.Add(this.paraTypeLabel);
            this.splitContainer2.Panel2.Controls.Add(this.paraTypeTextBox);
            this.splitContainer2.Panel2.Controls.Add(this.searchButton);
            this.splitContainer2.Panel2.Controls.Add(this.filterValueTextBox);
            this.splitContainer2.Panel2.Controls.Add(this.filterOptionComboBox);
            this.splitContainer2.Panel2.Controls.Add(this.paraComboBox);
            this.splitContainer2.Panel2.Controls.Add(this.commomParaLabel);
            this.splitContainer2.Size = new System.Drawing.Size(595, 243);
            this.splitContainer2.SplitterDistance = 278;
            this.splitContainer2.TabIndex = 0;
            // 
            // eleListBox
            // 
            this.eleListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.eleListBox.FormattingEnabled = true;
            this.eleListBox.Location = new System.Drawing.Point(22, 46);
            this.eleListBox.MaximumSize = new System.Drawing.Size(216, 0);
            this.eleListBox.MinimumSize = new System.Drawing.Size(216, 147);
            this.eleListBox.Name = "eleListBox";
            this.eleListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.eleListBox.Size = new System.Drawing.Size(216, 147);
            this.eleListBox.TabIndex = 0;
            // 
            // categoryLabel
            // 
            this.categoryLabel.AutoSize = true;
            this.categoryLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.categoryLabel.Location = new System.Drawing.Point(96, 11);
            this.categoryLabel.Name = "categoryLabel";
            this.categoryLabel.Size = new System.Drawing.Size(55, 15);
            this.categoryLabel.TabIndex = 1;
            this.categoryLabel.Text = "Category";
            // 
            // selectCategoryButton
            // 
            this.selectCategoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.selectCategoryButton.Location = new System.Drawing.Point(22, 204);
            this.selectCategoryButton.MaximumSize = new System.Drawing.Size(102, 26);
            this.selectCategoryButton.MinimumSize = new System.Drawing.Size(102, 26);
            this.selectCategoryButton.Name = "selectCategoryButton";
            this.selectCategoryButton.Size = new System.Drawing.Size(102, 26);
            this.selectCategoryButton.TabIndex = 3;
            this.selectCategoryButton.Text = "Select Category";
            this.selectCategoryButton.UseVisualStyleBackColor = true;
            this.selectCategoryButton.Click += new System.EventHandler(this.selectCategoryButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(3, 165);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(303, 2);
            this.label1.TabIndex = 14;
            // 
            // seperator1
            // 
            this.seperator1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.seperator1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.seperator1.Location = new System.Drawing.Point(3, 95);
            this.seperator1.Name = "seperator1";
            this.seperator1.Size = new System.Drawing.Size(303, 2);
            this.seperator1.TabIndex = 13;
            // 
            // paraEditButton
            // 
            this.paraEditButton.Location = new System.Drawing.Point(26, 201);
            this.paraEditButton.Name = "paraEditButton";
            this.paraEditButton.Size = new System.Drawing.Size(108, 23);
            this.paraEditButton.TabIndex = 12;
            this.paraEditButton.Text = "Edit Parameter";
            this.paraEditButton.UseVisualStyleBackColor = true;
            this.paraEditButton.Click += new System.EventHandler(this.paraEditButton_Click);
            // 
            // paraEditTextBox
            // 
            this.paraEditTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.paraEditTextBox.Location = new System.Drawing.Point(140, 175);
            this.paraEditTextBox.Name = "paraEditTextBox";
            this.paraEditTextBox.Size = new System.Drawing.Size(150, 20);
            this.paraEditTextBox.TabIndex = 11;
            // 
            // paraEditLabel
            // 
            this.paraEditLabel.Location = new System.Drawing.Point(26, 178);
            this.paraEditLabel.Name = "paraEditLabel";
            this.paraEditLabel.Size = new System.Drawing.Size(76, 20);
            this.paraEditLabel.TabIndex = 10;
            this.paraEditLabel.Text = "New Value :";
            // 
            // paraTypeLabel
            // 
            this.paraTypeLabel.Location = new System.Drawing.Point(26, 71);
            this.paraTypeLabel.Name = "paraTypeLabel";
            this.paraTypeLabel.Size = new System.Drawing.Size(108, 20);
            this.paraTypeLabel.TabIndex = 0;
            this.paraTypeLabel.Text = "Parameter Type :";
            // 
            // paraTypeTextBox
            // 
            this.paraTypeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.paraTypeTextBox.Location = new System.Drawing.Point(140, 68);
            this.paraTypeTextBox.Name = "paraTypeTextBox";
            this.paraTypeTextBox.ReadOnly = true;
            this.paraTypeTextBox.Size = new System.Drawing.Size(150, 20);
            this.paraTypeTextBox.TabIndex = 9;
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(26, 132);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(108, 23);
            this.searchButton.TabIndex = 8;
            this.searchButton.Text = "Search Elements";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // filterValueTextBox
            // 
            this.filterValueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterValueTextBox.Location = new System.Drawing.Point(140, 106);
            this.filterValueTextBox.Name = "filterValueTextBox";
            this.filterValueTextBox.Size = new System.Drawing.Size(150, 20);
            this.filterValueTextBox.TabIndex = 7;
            // 
            // filterOptionComboBox
            // 
            this.filterOptionComboBox.FormattingEnabled = true;
            this.filterOptionComboBox.Location = new System.Drawing.Point(26, 105);
            this.filterOptionComboBox.Name = "filterOptionComboBox";
            this.filterOptionComboBox.Size = new System.Drawing.Size(108, 21);
            this.filterOptionComboBox.TabIndex = 6;
            this.filterOptionComboBox.SelectedIndexChanged += new System.EventHandler(this.filterOptionComboBox_SelectedIndexChanged);
            // 
            // paraComboBox
            // 
            this.paraComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.paraComboBox.FormattingEnabled = true;
            this.paraComboBox.Location = new System.Drawing.Point(26, 41);
            this.paraComboBox.Name = "paraComboBox";
            this.paraComboBox.Size = new System.Drawing.Size(264, 21);
            this.paraComboBox.TabIndex = 5;
            this.paraComboBox.SelectedIndexChanged += new System.EventHandler(this.paraComboBox_SelectedIndexChanged);
            // 
            // commomParaLabel
            // 
            this.commomParaLabel.AutoSize = true;
            this.commomParaLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.commomParaLabel.Location = new System.Drawing.Point(91, 11);
            this.commomParaLabel.Name = "commomParaLabel";
            this.commomParaLabel.Size = new System.Drawing.Size(119, 15);
            this.commomParaLabel.TabIndex = 3;
            this.commomParaLabel.Text = "Common Parameter";
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // ElementEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 553);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ElementEditorForm";
            this.Text = "r";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListBox eleListBox;
        private System.Windows.Forms.Label categoryLabel;
        private System.Windows.Forms.Label commomParaLabel;
        private System.Windows.Forms.ComboBox paraComboBox;
        private System.Windows.Forms.TextBox filterValueTextBox;
        private System.Windows.Forms.ComboBox filterOptionComboBox;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.Button selectCategoryButton;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Label paraTypeLabel;
        private System.Windows.Forms.TextBox paraTypeTextBox;
        private System.Windows.Forms.TextBox paraEditTextBox;
        private System.Windows.Forms.Label paraEditLabel;
        private System.Windows.Forms.Button paraEditButton;

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label seperator1;
        private System.Windows.Forms.Integration.ElementHost elementHost;
    }
}