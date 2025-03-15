using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Algorithm_Visualizer
{
    public partial class Visualizer : Form
    {
        private int[] _unorderedArray;
        private CancellationTokenSource _cts;

        public Visualizer()
        {
            InitializeComponent();

            // Generate random array for visualization
            Constants.BarWidth = (panelDraw.Width / Constants.InitialArrLen) - Constants.BarMargin;
            UnorderedArray = Utility.RandomGeneration.GenerateRandomArray<int>(
                Constants.InitialArrLen,
                () => Utility.RandomGeneration.Random.Next(1, panelDraw.Height)
            );
            panelDraw.Invalidate();
        }

        public int[] UnorderedArray
        {
            get => _unorderedArray;
            set => _unorderedArray = value;
        }


        private void FocusLost_Changed(object sender, EventArgs e)
        {
            SortingAlgorithms.activeIndex = null;
            var txtBx = (TextBox)sender;
            if (int.TryParse(txtBx.Text, out int txtBxValue))
            {
                Constants.BarWidth = (panelDraw.Width / txtBxValue) - Constants.BarMargin;
                UnorderedArray = Utility.RandomGeneration.GenerateRandomArray<int>(
                    txtBxValue,
                    () => Utility.RandomGeneration.Random.Next(1, panelDraw.Height)
                );
            }
            else 
            {
                ResetArray();
            }
            panelDraw.Invalidate();
        }

        private void TxtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only digits and control keys (Backspace, Delete, etc.)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Block invalid characters
            }
        }

        private void TxtInput_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtInput.Text, out int value))
            {
                if (value < 1)
                    txtInput.Text = "1";  // Enforce lower limit
                else if (value > 100)
                    txtInput.Text = "100"; // Enforce upper limit
            }
            else if (txtInput.Text != "")
            {
                txtInput.Text = "1"; // Default value if cleared
            }
        }


        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetArray();
        }

        private void ResetArray()
        {
            SortingAlgorithms.activeIndex = null;
            UnorderedArray = Utility.RandomGeneration.GenerateRandomArray<int>(
                panelDraw.Width / (Constants.BarWidth + Constants.BarMargin),
                () => Utility.RandomGeneration.Random.Next(1, panelDraw.Height)
            );

            panelDraw.Invalidate();
        }

        private async void BtnStart_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                // If button says "Stop", cancel the sorting
                if (btn.Text == "Stop")
                {
                    _cts?.Cancel();
                    return;
                }

                // Otherwise, start sorting
                _cts = new CancellationTokenSource();
                btn.Text = "Stop";
                btnReset.Enabled = false;

                try
                {
                    int[] clonedArray = (int[])_unorderedArray.Clone();
                    var selectedAlgorithm = (Enums.Algorithm)sortComboBox.SelectedItem;

                    switch (selectedAlgorithm)
                    {
                        case Enums.Algorithm.BubbleSort:
                            await SortingAlgorithms.BubbleSort(clonedArray, arr => DrawArray(arr), _cts.Token);
                            break;
                        case Enums.Algorithm.InsertSort:
                            await SortingAlgorithms.InsertSort(clonedArray, arr => DrawArray(arr), _cts.Token);
                            break;
                        case Enums.Algorithm.MergeSort:
                            await SortingAlgorithms.MergeSort(clonedArray, arr => DrawArray(arr), _cts.Token);
                            break;
                        case Enums.Algorithm.QuickSort:
                            await SortingAlgorithms.QuickSort(clonedArray, arr => DrawArray(arr), _cts.Token);
                            break;
                    }
                }
                catch (OperationCanceledException)
                {
                    Debug.WriteLine("Sorting stopped.");
                }
                finally
                {
                    btnReset.Enabled = true;
                    btn.Text = "Sort";
                }
            }
        }


        private void panelDraw_Paint(object sender, PaintEventArgs e)
        {
            if (_unorderedArray != null)
            {
                DrawArray(e.Graphics, _unorderedArray); // Uses PaintEventArgs.Graphics
            }
        }


        private void DrawArray(int[] arr)
        {
            _unorderedArray = arr;
            panelDraw.Invalidate();  // 🚀 Triggers repaint, handled by Paint event
        }


        private void DrawArray(Graphics g, int[] arr)
        {
            g.Clear(Color.White);  // Clear previous frame

            int arraySize = arr.Length;
            int panelHeight = panelDraw.Height;
            int maxValue = arr.Max();

            for (int i = 0; i < arraySize; i++)
            {
                int barHeight = (int)((float)arr[i] / maxValue * panelHeight);
                int xPosition = i * (Constants.BarWidth + Constants.BarMargin);
                int yPosition = panelHeight - barHeight;

                // 🔥 Highlight the actively sorted bar in green
                Brush brush = (SortingAlgorithms.activeIndex.HasValue && SortingAlgorithms.activeIndex.Value == i) ? Brushes.Red : Brushes.Teal;

                g.FillRectangle(brush, xPosition, yPosition, Constants.BarWidth, barHeight);
            }
        }

    }
}
