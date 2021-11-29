
namespace LabEx
{
    partial class FormEx
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEx));
            this.DataGridViewEx = new System.Windows.Forms.DataGridView();
            this.ButtonSave = new System.Windows.Forms.Button();
            this.ButtonOpen = new System.Windows.Forms.Button();
            this.LabelRows = new System.Windows.Forms.Label();
            this.ButtonAddRow = new System.Windows.Forms.Button();
            this.ButtonDelRow = new System.Windows.Forms.Button();
            this.LabelColumns = new System.Windows.Forms.Label();
            this.ButtonAddColumn = new System.Windows.Forms.Button();
            this.ButtonDelColumn = new System.Windows.Forms.Button();
            this.TextBoxExpression = new System.Windows.Forms.TextBox();
            this.ButtonEnterExp = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewEx)).BeginInit();
            this.SuspendLayout();
            // 
            // DataGridViewEx
            // 
            this.DataGridViewEx.AllowUserToAddRows = false;
            this.DataGridViewEx.AllowUserToDeleteRows = false;
            this.DataGridViewEx.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.DataGridViewEx.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridViewEx.Location = new System.Drawing.Point(-3, 41);
            this.DataGridViewEx.Name = "DataGridViewEx";
            this.DataGridViewEx.ReadOnly = true;
            this.DataGridViewEx.RowHeadersWidth = 60;
            this.DataGridViewEx.RowTemplate.Height = 25;
            this.DataGridViewEx.Size = new System.Drawing.Size(843, 411);
            this.DataGridViewEx.TabIndex = 0;
            this.DataGridViewEx.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewEx_CellEnter);
            // 
            // ButtonSave
            // 
            this.ButtonSave.Location = new System.Drawing.Point(12, 7);
            this.ButtonSave.Name = "ButtonSave";
            this.ButtonSave.Size = new System.Drawing.Size(75, 28);
            this.ButtonSave.TabIndex = 1;
            this.ButtonSave.Text = "Save";
            this.ButtonSave.UseVisualStyleBackColor = true;
            this.ButtonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // ButtonOpen
            // 
            this.ButtonOpen.Location = new System.Drawing.Point(93, 7);
            this.ButtonOpen.Name = "ButtonOpen";
            this.ButtonOpen.Size = new System.Drawing.Size(75, 28);
            this.ButtonOpen.TabIndex = 2;
            this.ButtonOpen.Text = "Open";
            this.ButtonOpen.UseVisualStyleBackColor = true;
            this.ButtonOpen.Click += new System.EventHandler(this.ButtonOpen_Click);
            // 
            // LabelRows
            // 
            this.LabelRows.AutoSize = true;
            this.LabelRows.Location = new System.Drawing.Point(217, 14);
            this.LabelRows.Name = "LabelRows";
            this.LabelRows.Size = new System.Drawing.Size(35, 15);
            this.LabelRows.TabIndex = 3;
            this.LabelRows.Text = "Rows";
            // 
            // ButtonAddRow
            // 
            this.ButtonAddRow.Location = new System.Drawing.Point(258, 7);
            this.ButtonAddRow.Name = "ButtonAddRow";
            this.ButtonAddRow.Size = new System.Drawing.Size(28, 28);
            this.ButtonAddRow.TabIndex = 4;
            this.ButtonAddRow.Text = "+";
            this.ButtonAddRow.UseVisualStyleBackColor = true;
            this.ButtonAddRow.Click += new System.EventHandler(this.ButtonAddRow_Click);
            // 
            // ButtonDelRow
            // 
            this.ButtonDelRow.Location = new System.Drawing.Point(295, 7);
            this.ButtonDelRow.Name = "ButtonDelRow";
            this.ButtonDelRow.Size = new System.Drawing.Size(28, 28);
            this.ButtonDelRow.TabIndex = 5;
            this.ButtonDelRow.Text = "-";
            this.ButtonDelRow.UseVisualStyleBackColor = true;
            this.ButtonDelRow.Click += new System.EventHandler(this.ButtonDelRow_Click);
            // 
            // LabelColumns
            // 
            this.LabelColumns.AutoSize = true;
            this.LabelColumns.Location = new System.Drawing.Point(373, 14);
            this.LabelColumns.Name = "LabelColumns";
            this.LabelColumns.Size = new System.Drawing.Size(55, 15);
            this.LabelColumns.TabIndex = 6;
            this.LabelColumns.Text = "Columns";
            // 
            // ButtonAddColumn
            // 
            this.ButtonAddColumn.Location = new System.Drawing.Point(434, 7);
            this.ButtonAddColumn.Name = "ButtonAddColumn";
            this.ButtonAddColumn.Size = new System.Drawing.Size(28, 28);
            this.ButtonAddColumn.TabIndex = 7;
            this.ButtonAddColumn.Text = "+";
            this.ButtonAddColumn.UseVisualStyleBackColor = true;
            this.ButtonAddColumn.Click += new System.EventHandler(this.ButtonAddColumn_Click);
            // 
            // ButtonDelColumn
            // 
            this.ButtonDelColumn.Location = new System.Drawing.Point(471, 7);
            this.ButtonDelColumn.Name = "ButtonDelColumn";
            this.ButtonDelColumn.Size = new System.Drawing.Size(28, 28);
            this.ButtonDelColumn.TabIndex = 8;
            this.ButtonDelColumn.Text = "-";
            this.ButtonDelColumn.UseVisualStyleBackColor = true;
            this.ButtonDelColumn.Click += new System.EventHandler(this.ButtonDelColumn_Click);
            // 
            // TextBoxExpression
            // 
            this.TextBoxExpression.Location = new System.Drawing.Point(542, 11);
            this.TextBoxExpression.Name = "TextBoxExpression";
            this.TextBoxExpression.Size = new System.Drawing.Size(203, 23);
            this.TextBoxExpression.TabIndex = 9;
            this.TextBoxExpression.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxExpression_KeyDown);
            // 
            // ButtonEnterExp
            // 
            this.ButtonEnterExp.Location = new System.Drawing.Point(751, 7);
            this.ButtonEnterExp.Name = "ButtonEnterExp";
            this.ButtonEnterExp.Size = new System.Drawing.Size(75, 28);
            this.ButtonEnterExp.TabIndex = 10;
            this.ButtonEnterExp.Text = "Enter";
            this.ButtonEnterExp.UseVisualStyleBackColor = true;
            this.ButtonEnterExp.Click += new System.EventHandler(this.ButtonEnterExp_Click);
            // 
            // FormEx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 450);
            this.Controls.Add(this.ButtonEnterExp);
            this.Controls.Add(this.TextBoxExpression);
            this.Controls.Add(this.ButtonDelColumn);
            this.Controls.Add(this.ButtonAddColumn);
            this.Controls.Add(this.LabelColumns);
            this.Controls.Add(this.ButtonDelRow);
            this.Controls.Add(this.ButtonAddRow);
            this.Controls.Add(this.LabelRows);
            this.Controls.Add(this.ButtonOpen);
            this.Controls.Add(this.ButtonSave);
            this.Controls.Add(this.DataGridViewEx);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormEx";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LabEx";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEx_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewEx)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DataGridViewEx;
        private System.Windows.Forms.Button ButtonSave;
        private System.Windows.Forms.Button ButtonOpen;
        private System.Windows.Forms.Label LabelRows;
        private System.Windows.Forms.Button ButtonAddRow;
        private System.Windows.Forms.Button ButtonDelRow;
        private System.Windows.Forms.Label LabelColumns;
        private System.Windows.Forms.Button ButtonAddColumn;
        private System.Windows.Forms.Button ButtonDelColumn;
        private System.Windows.Forms.TextBox TextBoxExpression;
        private System.Windows.Forms.Button ButtonEnterExp;
    }
}

