using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NumberTheory
{
    /// <summary>
    /// Implements various versions of the Sieve of Eratosthenes algorithm for prime number generation.
    /// </summary>
    public static class SieveOfEratosthenes
    {
        /// <summary>
        /// Sequential implementation of the Sieve of Eratosthenes.
        /// </summary>
        public static IEnumerable<int> GetPrimeNumbersSequentialAlgorithm(int n)
        {
            if (n <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "Upper limit must be greater than 0.");
            }

            if (n < 2)
            {
                return Enumerable.Empty<int>();
            }

            var isComposite = new bool[n + 1];
            var primes = new List<int>();

            for (int p = 2; p <= n; p++)
            {
                if (!isComposite[p])
                {
                    primes.Add(p);
                    for (int multiple = p * 2; multiple <= n; multiple += p)
                    {
                        isComposite[multiple] = true;
                    }
                }
            }

            return primes;
        }

        /// <summary>
        /// Modified sequential implementation with two-stage processing.
        /// </summary>
        public static IEnumerable<int> GetPrimeNumbersModifiedSequentialAlgorithm(int n)
        {
            if (n <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "Upper limit must be greater than 0.");
            }

            if (n < 2)
            {
                return Enumerable.Empty<int>();
            }

            int sqrtN = (int)Math.Sqrt(n);
            var basePrimes = GetPrimeNumbersSequentialAlgorithm(sqrtN).ToList();

            if (sqrtN >= n)
            {
                return basePrimes;
            }

            var isComposite = new bool[n - sqrtN + 1];

            foreach (var p in basePrimes)
            {
                int start = Math.Max(p * p, ((sqrtN + p) / p) * p);
                for (int multiple = start; multiple <= n; multiple += p)
                {
                    isComposite[multiple - sqrtN] = true;
                }
            }

            var primes = new List<int>(basePrimes);
            for (int i = sqrtN + 1; i <= n; i++)
            {
                if (!isComposite[i - sqrtN])
                {
                    primes.Add(i);
                }
            }

            return primes;
        }

        /// <summary>
        /// Parallel implementation using data decomposition.
        /// </summary>
        public static IEnumerable<int> GetPrimeNumbersConcurrentDataDecomposition(int n)
        {
            if (n <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "Upper limit must be greater than 0.");
            }

            if (n < 2)
            {
                return Enumerable.Empty<int>();
            }

            int sqrtN = (int)Math.Sqrt(n);
            var basePrimes = GetPrimeNumbersSequentialAlgorithm(sqrtN).ToList();

            if (sqrtN >= n)
            {
                return basePrimes;
            }

            var isComposite = new bool[n - sqrtN + 1];
            int processorCount = Environment.ProcessorCount;
            var rangeSize = (n - sqrtN + processorCount - 1) / processorCount;

            Parallel.For(
                0,
                processorCount,
                threadId =>
                {
                    int start = sqrtN + 1 + threadId * rangeSize;
                    int end = Math.Min(start + rangeSize - 1, n);

                    foreach (var p in basePrimes)
                    {
                        int firstMultiple = Math.Max(p * p, ((start + p - 1) / p) * p);
                        for (int multiple = firstMultiple; multiple <= end; multiple += p)
                        {
                            Volatile.Write(ref isComposite[multiple - sqrtN], true);
                        }
                    }
                });

            var primes = new List<int>(basePrimes);
            for (int i = sqrtN + 1; i <= n; i++)
            {
                if (!Volatile.Read(ref isComposite[i - sqrtN]))
                {
                    primes.Add(i);
                }
            }

            return primes;
        }

        /// <summary>
        /// Parallel implementation using prime decomposition.
        /// </summary>
        public static IEnumerable<int> GetPrimeNumbersConcurrentBasicPrimesDecomposition(int n)
        {
            if (n <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "Upper limit must be greater than 0.");
            }

            if (n < 2)
            {
                return Enumerable.Empty<int>();
            }

            int sqrtN = (int)Math.Sqrt(n);
            var basePrimes = GetPrimeNumbersSequentialAlgorithm(sqrtN).ToList();

            if (sqrtN >= n)
            {
                return basePrimes;
            }

            var isComposite = new bool[n - sqrtN + 1];
            var lockObject = new object();

            Parallel.ForEach(
                basePrimes,
                p =>
                {
                    int start = Math.Max(p * p, ((sqrtN + p) / p) * p);
                    for (int multiple = start; multiple <= n; multiple += p)
                    {
                        lock (lockObject)
                        {
                            isComposite[multiple - sqrtN] = true;
                        }
                    }
                });

            var primes = new List<int>(basePrimes);
            for (int i = sqrtN + 1; i <= n; i++)
            {
                if (!isComposite[i - sqrtN])
                {
                    primes.Add(i);
                }
            }

            return primes;
        }

        /// <summary>
        /// Thread pool-based implementation with proper synchronization.
        /// </summary>
        public static IEnumerable<int> GetPrimeNumbersConcurrentWithThreadPool(int n)
        {
            if (n <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), "Upper limit must be greater than 0.");
            }

            if (n < 2)
            {
                return Enumerable.Empty<int>();
            }

            int sqrtN = (int)Math.Sqrt(n);
            var basePrimes = GetPrimeNumbersSequentialAlgorithm(sqrtN).ToList();

            if (sqrtN >= n)
            {
                return basePrimes;
            }

            var isComposite = new bool[n - sqrtN + 1];
            var lockObject = new object();
            using var countdownEvent = new CountdownEvent(basePrimes.Count);

            foreach (var prime in basePrimes)
            {
                ThreadPool.QueueUserWorkItem(
                    state =>
                    {
                        try
                        {
                            int p = (int)state!;
                            int start = Math.Max(p * p, ((sqrtN + p) / p) * p);
                            for (int multiple = start; multiple <= n; multiple += p)
                            {
                                lock (lockObject)
                                {
                                    isComposite[multiple - sqrtN] = true;
                                }
                            }
                        }
                        finally
                        {
                            countdownEvent.Signal();
                        }
                    },
                    prime);
            }

            countdownEvent.Wait();

            var primes = new List<int>(basePrimes);
            for (int i = sqrtN + 1; i <= n; i++)
            {
                if (!isComposite[i - sqrtN])
                {
                    primes.Add(i);
                }
            }

            return primes;
        }
    }
}
