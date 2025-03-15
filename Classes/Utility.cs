using System;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Forms;

namespace Algorithm_Visualizer
{
    internal class Utility
    {
        public static class RandomGeneration
        {
            public static Random Random = new Random();

            public static T[] GenerateRandomArray<T>(int size, Func<T> generator) where T : IComparable<T>
            {
                T[] arr = new T[size];
                for (int i = 0; i < size; i++)
                {
                    arr[i] = generator(); // Use the generator function to create values
                }
                return arr;
            }
        }

        public static class UserInterface 
        {
            public static void ApplyMargins(Control parent, Padding margin)
            {
                foreach (Control control in parent.Controls)
                {
                    Padding calculatedMargin = margin;

                    if (control is Label)
                    {
                        calculatedMargin.Top = calculatedMargin.Top * 2;
                    }
                    else if (control is TextBox)
                    { }
                    else if (control is Button)
                    { }
                    else if (control is ComboBox)
                    { }
                    else if (control is TextBox)
                    { }

                    control.Margin = calculatedMargin; // Apply margin to current control


                    // Recursively apply to children (for nested panels)
                    if (control.HasChildren)
                    {
                        ApplyMargins(control, margin);
                    }
                }
            }

        }
    }
}
