namespace D_Projekt
{
    partial class Form1
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
            btn_spawnTank = new Button();
            btn_spawnTower = new Button();
            lbl_moneyTowers = new Label();
            lbl_moneyEnemys = new Label();
            SuspendLayout();
            // 
            // btn_spawnTank
            // 
            btn_spawnTank.Location = new Point(0, 0);
            btn_spawnTank.Name = "btn_spawnTank";
            btn_spawnTank.Size = new Size(75, 23);
            btn_spawnTank.TabIndex = 0;
            // 
            // btn_spawnTower
            // 
            btn_spawnTower.Location = new Point(12, 712);
            btn_spawnTower.Name = "btn_spawnTower";
            btn_spawnTower.Size = new Size(105, 29);
            btn_spawnTower.TabIndex = 1;
            btn_spawnTower.TabStop = false;
            btn_spawnTower.Text = "SpawnTower";
            btn_spawnTower.UseVisualStyleBackColor = true;
            btn_spawnTower.MouseClick += btn_spawnTower_MouseClick;
            // 
            // lbl_moneyTowers
            // 
            lbl_moneyTowers.AutoSize = true;
            lbl_moneyTowers.Location = new Point(864, 9);
            lbl_moneyTowers.Name = "lbl_moneyTowers";
            lbl_moneyTowers.Size = new Size(50, 20);
            lbl_moneyTowers.TabIndex = 2;
            lbl_moneyTowers.Text = "label1";
            // 
            // lbl_moneyEnemys
            // 
            lbl_moneyEnemys.AutoSize = true;
            lbl_moneyEnemys.Location = new Point(920, 9);
            lbl_moneyEnemys.Name = "lbl_moneyEnemys";
            lbl_moneyEnemys.Size = new Size(50, 20);
            lbl_moneyEnemys.TabIndex = 3;
            lbl_moneyEnemys.Text = "label2";
            // 
            // Form1
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(982, 753);
            Controls.Add(lbl_moneyEnemys);
            Controls.Add(lbl_moneyTowers);
            Controls.Add(btn_spawnTower);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Form1";
            Text = "TowerDefenseMultiplayer";
            Paint += Form1_Paint;
            KeyDown += Form1_KeyDown;
            MouseUp += Form1_MouseUp;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn_spawnTank;
        private Button btn_spawnTower;
        private Label lbl_moneyTowers;
        private Label lbl_moneyEnemys;
    }
}
