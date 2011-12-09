namespace CalorieTracker
{
    partial class Form1
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
            this.mealTreeView = new System.Windows.Forms.TreeView();
            this.foodListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.foodNameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.caloriesPer100gTextBox = new System.Windows.Forms.TextBox();
            this.gramsTextBox = new System.Windows.Forms.TextBox();
            this.addFoodButton = new System.Windows.Forms.Button();
            this.eatNowButton = new System.Windows.Forms.Button();
            this.settingsButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.plotButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // mealTreeView
            // 
            this.mealTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mealTreeView.Location = new System.Drawing.Point(0, 0);
            this.mealTreeView.Name = "mealTreeView";
            this.mealTreeView.Size = new System.Drawing.Size(206, 410);
            this.mealTreeView.TabIndex = 0;
            this.mealTreeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mealTreeView_KeyDown);
            // 
            // foodListView
            // 
            this.foodListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.foodListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.foodListView.Location = new System.Drawing.Point(0, 0);
            this.foodListView.Name = "foodListView";
            this.foodListView.Size = new System.Drawing.Size(212, 328);
            this.foodListView.TabIndex = 1;
            this.foodListView.UseCompatibleStateImageBehavior = false;
            this.foodListView.View = System.Windows.Forms.View.Details;
            this.foodListView.ItemActivate += new System.EventHandler(this.foodListView_ItemActivate);
            this.foodListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.foodListView_KeyDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 140;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Kcal";
            // 
            // foodNameTextBox
            // 
            this.foodNameTextBox.Location = new System.Drawing.Point(92, 13);
            this.foodNameTextBox.Name = "foodNameTextBox";
            this.foodNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.foodNameTextBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "CaloriesPer100g";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(49, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Grams";
            // 
            // caloriesPer100gTextBox
            // 
            this.caloriesPer100gTextBox.Location = new System.Drawing.Point(92, 37);
            this.caloriesPer100gTextBox.Name = "caloriesPer100gTextBox";
            this.caloriesPer100gTextBox.Size = new System.Drawing.Size(100, 20);
            this.caloriesPer100gTextBox.TabIndex = 6;
            // 
            // gramsTextBox
            // 
            this.gramsTextBox.Location = new System.Drawing.Point(92, 61);
            this.gramsTextBox.Name = "gramsTextBox";
            this.gramsTextBox.Size = new System.Drawing.Size(100, 20);
            this.gramsTextBox.TabIndex = 7;
            // 
            // addFoodButton
            // 
            this.addFoodButton.Location = new System.Drawing.Point(29, 87);
            this.addFoodButton.Name = "addFoodButton";
            this.addFoodButton.Size = new System.Drawing.Size(75, 23);
            this.addFoodButton.TabIndex = 8;
            this.addFoodButton.Text = "Add";
            this.addFoodButton.UseVisualStyleBackColor = true;
            this.addFoodButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // eatNowButton
            // 
            this.eatNowButton.Location = new System.Drawing.Point(110, 87);
            this.eatNowButton.Name = "eatNowButton";
            this.eatNowButton.Size = new System.Drawing.Size(75, 23);
            this.eatNowButton.TabIndex = 9;
            this.eatNowButton.Text = "Eat now";
            this.eatNowButton.UseVisualStyleBackColor = true;
            this.eatNowButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // settingsButton
            // 
            this.settingsButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.settingsButton.Location = new System.Drawing.Point(0, 0);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(105, 31);
            this.settingsButton.TabIndex = 10;
            this.settingsButton.Text = "Settings";
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.button3_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(422, 445);
            this.splitContainer1.SplitterDistance = 206;
            this.splitContainer1.TabIndex = 13;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.mealTreeView);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.plotButton);
            this.splitContainer2.Panel2.Controls.Add(this.settingsButton);
            this.splitContainer2.Size = new System.Drawing.Size(206, 445);
            this.splitContainer2.SplitterDistance = 410;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.foodListView);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.label1);
            this.splitContainer3.Panel2.Controls.Add(this.foodNameTextBox);
            this.splitContainer3.Panel2.Controls.Add(this.label2);
            this.splitContainer3.Panel2.Controls.Add(this.caloriesPer100gTextBox);
            this.splitContainer3.Panel2.Controls.Add(this.addFoodButton);
            this.splitContainer3.Panel2.Controls.Add(this.eatNowButton);
            this.splitContainer3.Panel2.Controls.Add(this.gramsTextBox);
            this.splitContainer3.Panel2.Controls.Add(this.label3);
            this.splitContainer3.Size = new System.Drawing.Size(212, 445);
            this.splitContainer3.SplitterDistance = 328;
            this.splitContainer3.TabIndex = 0;
            // 
            // plotButton
            // 
            this.plotButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plotButton.Location = new System.Drawing.Point(105, 0);
            this.plotButton.Name = "plotButton";
            this.plotButton.Size = new System.Drawing.Size(101, 31);
            this.plotButton.TabIndex = 11;
            this.plotButton.Text = "Plot";
            this.plotButton.UseVisualStyleBackColor = true;
            this.plotButton.Click += new System.EventHandler(this.plotButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 445);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "CalorieTracker";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView mealTreeView;
        private System.Windows.Forms.ListView foodListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.TextBox foodNameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox caloriesPer100gTextBox;
        private System.Windows.Forms.TextBox gramsTextBox;
        private System.Windows.Forms.Button addFoodButton;
        private System.Windows.Forms.Button eatNowButton;
        private System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Button plotButton;
    }
}

