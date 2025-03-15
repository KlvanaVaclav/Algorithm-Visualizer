using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Algorithm_Visualizer
{
    public partial class Form1 : Form
    {
        private int[] _unorderedArray;
        private CancellationTokenSource _cts;

        public Form1()
        {
            InitializeComponent();

            // Generate random array for visualization
            UnorderedArray = Utility.RandomGeneration.GenerateRandomArray<int>(
                panelDraw.Width / (Constants.BarWidth+Constants.BarMargin),
                () => Utility.RandomGeneration.Random.Next(1, panelDraw.Height)
            );

            panelDraw.Invalidate(); // Force repaint
        }

        public int[] UnorderedArray
        {
            get => _unorderedArray;
            set => _unorderedArray = value;
        }


        private void BtnReset_Click(object sender, EventArgs e)
        {
            UnorderedArray = Utility.RandomGeneration.GenerateRandomArray<int>(
                panelDraw.Width / (Constants.BarWidth+Constants.BarMargin),
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
