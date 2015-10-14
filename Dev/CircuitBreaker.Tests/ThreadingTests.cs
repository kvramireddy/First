using System;
using System.Threading;
using NUnit.Framework;
using Osherove.ThreadTester;

namespace BlueKeyDigital.Tests
{
    /// <summary>
    /// This class tests the circuit breaker using multiple threads.
    /// </summary>
    [TestFixture]
    public class ThreadingTests
    {
        CircuitBreaker cb;
        int executeCount;
        int serviceFailureCount;
        int halfOpenStateChangeCount;
        int closedStateChangeCount;
        int openStateChangeCount;
        int openCircuitCount;

        [SetUp]
        public void SetUp()
        {
            this.cb = new CircuitBreaker(5, 1000);
            this.cb.StateChanged += new EventHandler(cb_StateChanged);
            this.executeCount = 0;
            this.serviceFailureCount = 0;
            this.halfOpenStateChangeCount = 0;
            this.closedStateChangeCount = 0;
            this.openStateChangeCount = 0;
            this.openCircuitCount = 0;
        }

        [TearDown]
        public void TearDown()
        {
        }

        private void cb_StateChanged(object sender, EventArgs e)
        {
            switch (cb.State)
            {
                case CircuitBreakerState.Open:
                    this.openStateChangeCount++;
                    break;
                case CircuitBreakerState.HalfOpen:
                    this.halfOpenStateChangeCount++;
                    break;
                case CircuitBreakerState.Closed:
                    this.closedStateChangeCount++;
                    break;
            }
        }

        private void MockService()
        {
            Thread.Sleep(10);

            // Fail after 10 calls
            if (this.executeCount >= 10)
            {
                if (this.serviceFailureCount < 20)
                {
                    this.serviceFailureCount++;

                    throw new Exception("Simulated failure");
                }
            }
        }

        [Test]
        public void CanExecuteUsingMultipleThreads()
        {
            int threadCount = 50;
            int totalFailureCount = 0;

            // Create 10 sets of 50 threads to execute operation
            for (int j = 0; j < 10; j++)
            {
                ThreadTester tt = new ThreadTester();
                for (int i = 0; i < threadCount; i++)
                {
                    tt.AddThreadAction(delegate
                    {
                        try
                        {
                            cb.Execute(new ThreadStart(MockService));
                        }
                        catch (OperationFailedException) { Interlocked.Increment(ref totalFailureCount); }
                        catch (OpenCircuitException) { Interlocked.Increment(ref openCircuitCount); }

                        Interlocked.Increment(ref executeCount);
                    });
                }
                tt.StartAllThreads(22500);
                tt.RunBehavior = ThreadRunBehavior.RunUntilAllThreadsFinish;

                // Wait 50 milliseconds before next thread cycle
                Thread.Sleep(500);
            }

            Assert.AreEqual(500, executeCount, "All 10 50-thread cycles should have executed operation");
            Assert.AreEqual(20, totalFailureCount, "Service should fail 20 times");
            Assert.AreEqual(50, openCircuitCount, "Should encounter 50 open circuits as one whole thread cycle will be rejected");
            Assert.AreEqual(1, openStateChangeCount, "Should change to open state once");
            Assert.AreEqual(1, halfOpenStateChangeCount, "Should change to half-open state once");
            Assert.AreEqual(1, closedStateChangeCount, "Should change to closed state once");
            Assert.AreEqual(CircuitBreakerState.Closed, cb.State, "Circuit breaker should end up closed");
            Assert.AreEqual(100, cb.ServiceLevel, "Service level should be 100");
        }
    }
}
