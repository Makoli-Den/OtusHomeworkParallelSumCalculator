using OtusHomeworkTools;
using System.Diagnostics;

namespace ParallelSumCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BaseConsoleActions.PrepareConsole("Вычисление суммы элементов массива");

            ShowMainMenu();
        }

        private static void ShowMainMenu()
        {
            ConsoleMenu menu = new ConsoleMenu("Меню вычисления суммы")
            {
                MenuItems =
                {
                    new ConsoleMenuItem("Вычислить сумму для 100 000 элементов", 0, (s, e) => { MeasureSumCalculation(100_000); }),
                    new ConsoleMenuItem("Вычислить сумму для 1 000 000 элементов", 0, (s, e) => { MeasureSumCalculation(1_000_000); }),
                    new ConsoleMenuItem("Вычислить сумму для 10 000 000 элементов", 0, (s, e) => { MeasureSumCalculation(10_000_000); }),
                    new ConsoleMenuItem("Вычислить сумму для своего количества элементов", 0, (s, e) => 
                        {
                            int count = BaseConsoleActions.AskForValidIntegerInput("\nВведите количетсво элементов: ");
                            MeasureSumCalculation(count); 
                        }),
                }
            };
            menu.OnItemAdded += (sender, e) =>
            {
                menu.DisplayMenu();
            };
            menu.DisplayMenu();
        }

        private static void MeasureSumCalculation(int size)
        {
            var calculator = new SumCalculator(size);

            Stopwatch stopwatch = Stopwatch.StartNew();
            var sequentialSum = calculator.CalculateSumSequential();
            stopwatch.Stop();
            var sequentialTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            var parallelSum = calculator.CalculateSumParallel();
            stopwatch.Stop();
            var parallelTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            var parallelThreadSum = calculator.CalculateSumUsingThreads();
            stopwatch.Stop();
            var parallelThreadTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            var linqSum = calculator.CalculateSumLINQ();
            stopwatch.Stop();
            var linqTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            var plinqSum = calculator.CalculateSumPLINQ();
            stopwatch.Stop();
            var plinqTime = stopwatch.ElapsedMilliseconds;

            Console.WriteLine();
            Console.WriteLine($"Размер массива: {size}");
            Console.WriteLine($"Сумма (последовательно): {sequentialSum}, время: {sequentialTime} мс");
            Console.WriteLine($"Сумма (параллельно): {parallelSum}, время: {parallelTime} мс");
            Console.WriteLine($"Сумма (параллельно Thread): {parallelThreadSum}, время: {parallelThreadTime} мс");
            Console.WriteLine($"Сумма (LINQ): {linqSum}, время: {linqTime} мс");
            Console.WriteLine($"Сумма (PLINQ): {plinqSum}, время: {plinqTime} мс");

            BaseConsoleActions.PressAnyToContinue(ShowMainMenu);
        }
    }
}
