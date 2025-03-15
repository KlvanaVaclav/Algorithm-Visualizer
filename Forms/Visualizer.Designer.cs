using System;
using System.Windows.Forms;
using static Algorithm_Visualizer.Enums;

namespace Algorithm_Visualizer
{
    partial class Visualizer
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelDraw = new BufferedPanel(); // Use double-buffered panel
            this.btnStart = new System.Windows.Forms.Button();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.lblInput = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.sortComboBox = new System.Windows.Forms.ComboBox();
            // 
            // panelDraw
            // 
            this.panelDraw.Name = "panelDraw";
            this.panelDraw.TabIndex = 0;
            this.panelDraw.Dock = DockStyle.Fill;
            this.panelDraw.Paint += new System.Windows.Forms.PaintEventHandler(this.panelDraw_Paint);
            //
            // txtInput
            //
            lblInput.Text = "Array length";
            lblInput.AutoSize = true;
            lblInput.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            txtInput.Width = 120;
            txtInput.LostFocus += FocusLost_Changed; 
            txtInput.KeyPress += TxtInput_KeyPress;
            txtInput.TextChanged += TxtInput_TextChanged;


            // 
            // btnStart
            // 
            this.btnStart.Name = "btnStart";
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Sort";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // btnReset
            // 
            this.btnReset.Name = "btnReset";
            this.btnReset.TabIndex = 1;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // sortComboBox
            // 
            this.sortComboBox.FormattingEnabled = true;
            this.sortComboBox.Name = "sortComboBox";
            this.sortComboBox.TabIndex = 3;
            this.sortComboBox.SelectedIndexChanged += BtnReset_Click;
            foreach (Enums.Algorithm algo in Enum.GetValues(typeof(Enums.Algorithm)))
            {
                sortComboBox.Items.Add(algo);
            }
            sortComboBox.SelectedIndex = 0;

            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1584, 861);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.DoubleBuffered = true;
            this.Text = "Visualizer";
            // Main layout panel
            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 90F)); // 80% space for drawing
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F)); // 20% space for controls

            // Sub-panel for buttons and inputs
            var controlPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            var upperRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            var buttonRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };



            upperRow.Controls.Add(lblInput);
            // Add controls to the FlowLayoutPanel
            upperRow.Controls.Add(txtInput);
            upperRow.Controls.Add(sortComboBox);
            buttonRow.Controls.Add(btnStart);
            buttonRow.Controls.Add(btnReset);
            controlPanel.Controls.Add(upperRow);
            controlPanel.Controls.Add(buttonRow);

            int marginSize = 5; // Adjust dynamically based on form width
            Padding dynamicMargin = new Padding(marginSize);

            Utility.UserInterface.ApplyMargins(controlPanel, dynamicMargin);


            // Add panels to main layout
            mainLayout.Controls.Add(panelDraw, 0, 0);  // Drawing panel in the top row
            mainLayout.Controls.Add(controlPanel, 0, 1); // Buttons in the bottom row

            // Add main layout to the form
            this.Controls.Add(mainLayout);


            this.ResumeLayout(false);
        }

        #endregion

        void ApplyMargins(Control parent, Padding margin)
        {
            foreach (Control control in parent.Controls)
            {
                control.Margin = margin; // Apply margin to current control

                // Recursively apply to children (for nested panels)
                if (control.HasChildren)
                {
                    ApplyMargins(control, margin);
                }
            }
        }


        private BufferedPanel panelDraw;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Label lblInput;
        private System.Windows.Forms.ComboBox sortComboBox;
    }

    public class BufferedPanel : Panel
    {
        public BufferedPanel()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }
    }
}
