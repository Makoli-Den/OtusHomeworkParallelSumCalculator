using System.Drawing;

namespace ParallelSumCalculator
{
    public class SumCalculator
    {
        private int[] _array;

        public SumCalculator(int size)
        {
            _array = new int[size];
            Random rand = new Random();

            for (int i = 0; i < size; i++)
            {
                _array[i] = rand.Next(1, 100);
            }
        }

        public long CalculateSumSequential()
        {
            long sum = 0;

            foreach (int i in _array)
            {
                sum += i;
            }

            return sum;
        }

        public long CalculateSumParallel()
        {
            long sum = 0;

            Parallel.ForEach(_array, (number) =>
            {
                Interlocked.Add(ref sum, number);
            });

            return sum;
        }

        public long CalculateSumUsingThreads()
        {
            int numberOfThreads = Environment.ProcessorCount;
            int chunkSize = _array.Length / numberOfThreads;
            long[] partialSums = new long[numberOfThreads];
            Thread[] threads = new Thread[numberOfThreads];

            for (int i = 0; i < numberOfThreads; i++)
            {
                int start = i * chunkSize;
                int end = (i == numberOfThreads - 1) ? _array.Length : start + chunkSize;
                int threadIndex = i;

                threads[i] = new Thread(() =>
                {
                    long localSum = 0;
                    for (int j = start; j < end; j++)
                    {
                        localSum += _array[j];
                    }
                    partialSums[threadIndex] = localSum;
                });
                threads[i].Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            return partialSums.Sum();
        }


        public long CalculateSumPLINQ()
        {
            return _array.AsParallel().Sum(x => (long)x);
        }

        public long CalculateSumLINQ()
        {
            return _array.Sum(x => (long)x);
        }
    }
}
