using System;
using System.Threading;
using System.Threading.Tasks;

namespace Algorithm_Visualizer
{
    public static class SortingAlgorithms
    {
        public static int? activeIndex = null;

        public static async Task BubbleSort<T>(T[] arr, Action<T[]> drawCallback, CancellationToken token) where T : IComparable<T>
        {
            int n = arr.Length;
            bool swapped;

            for (int i = 0; i < n - 1; i++)
            {
                swapped = false;

                for (int j = 0; j < n - i - 1; j++)
                {
                    token.ThrowIfCancellationRequested();

                    activeIndex = j;  // 🔥 Highlight current bar

                    if (arr[j].CompareTo(arr[j + 1]) > 0)
                    {
                        (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                        swapped = true;
                    }

                    drawCallback?.Invoke((T[])arr.Clone());
                    await Task.Delay(Constants.RedrawDelay);
                }

                if (!swapped) break;
            }

            activeIndex = null; // Reset after sorting
        }


        public static async Task InsertSort(int[] arr, Action<int[]> drawCallback, CancellationToken token)
        {
            int n = arr.Length;
            for (int i = 1; i < n; i++)
            {
                int key = arr[i];
                int j = i - 1;

                while (j >= 0 && arr[j] > key)
                {
                    token.ThrowIfCancellationRequested();

                    activeIndex = j;  // 🔥 Highlight current bar
                    arr[j + 1] = arr[j];
                    j--;

                    drawCallback?.Invoke((int[])arr.Clone());
                    await Task.Delay(Constants.RedrawDelay);
                }

                arr[j + 1] = key;
                activeIndex = j + 1; // Highlight insertion point
                drawCallback?.Invoke((int[])arr.Clone());
                await Task.Delay(Constants.RedrawDelay);
            }

            activeIndex = null;
        }


        public static async Task MergeSort(int[] arr, Action<int[]> drawCallback, CancellationToken token)
        {
            await MergeSortRecursive(arr, 0, arr.Length - 1, drawCallback, token);
        }

        private static async Task MergeSortRecursive(int[] arr, int left, int right, Action<int[]> drawCallback, CancellationToken token)
        {
            if (left >= right) return;

            int mid = left + (right - left) / 2;
            await MergeSortRecursive(arr, left, mid, drawCallback, token);
            await MergeSortRecursive(arr, mid + 1, right, drawCallback, token);
            await Merge(arr, left, mid, right, drawCallback, token);
        }

        private static async Task Merge(int[] arr, int left, int mid, int right, Action<int[]> drawCallback, CancellationToken token)
        {
            int leftSize = mid - left + 1;
            int rightSize = right - mid;

            int[] leftArr = new int[leftSize];
            int[] rightArr = new int[rightSize];

            for (int idx = 0; idx < leftSize; idx++)
                leftArr[idx] = arr[left + idx];

            for (int jdx = 0; jdx < rightSize; jdx++)
                rightArr[jdx] = arr[mid + 1 + jdx];

            int i = 0, j = 0, k = left;

            while (i < leftSize && j < rightSize)
            {
                token.ThrowIfCancellationRequested();
                activeIndex = k;  // 🔥 Highlight merging element

                if (leftArr[i] <= rightArr[j])
                {
                    arr[k] = leftArr[i];
                    i++;
                }
                else
                {
                    arr[k] = rightArr[j];
                    j++;
                }
                k++;

                drawCallback?.Invoke((int[])arr.Clone());
                await Task.Delay(Constants.RedrawDelay);
            }

            while (i < leftSize)
            {
                arr[k] = leftArr[i];
                activeIndex = k;
                i++; k++;

                drawCallback?.Invoke((int[])arr.Clone());
                await Task.Delay(Constants.RedrawDelay);
            }

            while (j < rightSize)
            {
                arr[k] = rightArr[j];
                activeIndex = k;
                j++; k++;

                drawCallback?.Invoke((int[])arr.Clone());
                await Task.Delay(Constants.RedrawDelay);
            }

            activeIndex = null;
        }


        public static async Task QuickSort(int[] arr, Action<int[]> drawCallback, CancellationToken token)
        {
            await QuickSortRecursive(arr, 0, arr.Length - 1, drawCallback, token);
        }

        private static async Task QuickSortRecursive(int[] arr, int low, int high, Action<int[]> drawCallback, CancellationToken token)
        {
            if (low >= high) return;

            int pivotIndex = await Partition(arr, low, high, drawCallback, token);
            await QuickSortRecursive(arr, low, pivotIndex - 1, drawCallback, token);
            await QuickSortRecursive(arr, pivotIndex + 1, high, drawCallback, token);
        }

        private static async Task<int> Partition(int[] arr, int low, int high, Action<int[]> drawCallback, CancellationToken token)
        {
            int pivot = arr[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                token.ThrowIfCancellationRequested();

                activeIndex = j;  // 🔥 Highlight pivot comparison

                if (arr[j] < pivot)
                {
                    i++;
                    (arr[i], arr[j]) = (arr[j], arr[i]);

                    drawCallback?.Invoke((int[])arr.Clone());
                    await Task.Delay(Constants.RedrawDelay);
                }
            }

            activeIndex = i + 1;  // 🔥 Highlight pivot swap
            (arr[i + 1], arr[high]) = (arr[high], arr[i + 1]);

            drawCallback?.Invoke((int[])arr.Clone());
            await Task.Delay(Constants.RedrawDelay);

            return i + 1;
        }

    }
}
