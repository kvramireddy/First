using System;
using System.Threading;
using NUnit.Framework;

namespace BlueKeyDigital.Tests
{
    [TestFixture]
    public class CircuitBreakerTests
    {
        CircuitBreaker cb;
        private delegate int TestDelegate(int a, int b);

        public int ValidOperation(int a, int b)
        {
            return a + b;
        }

        public void FailedOperation()
        {
            throw new TimeoutException("Network not available");
        }

        [SetUp]
        public void SetUp()
        {
            cb = new CircuitBreaker();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void CanCreateCircuitBreaker()
        {
            Assert.AreEqual(5, cb.Threshold, "Threshold should be initialised to 5 failures");
            Assert.AreEqual(60000, cb.Timeout, "Timeout should be initialised to 1 minute");
            Assert.AreEqual(100, cb.ServiceLevel, "Level of service should be 100%");
            Assert.IsNotNull(cb.IgnoredExceptionTypes, "IgnoreExceptionTypes should be initialised");
            Assert.AreEqual(0, cb.IgnoredExceptionTypes.Count, "IgnoreExceptionTypes list should not contain any values");
        }

        [Test]
        public void CanSetCircuitBreakerProperties()
        {
            cb.Threshold = 10;
            cb.Timeout = 120000;

            Assert.AreEqual(10, cb.Threshold, "Should update threshold value");
            Assert.AreEqual(120000, cb.Timeout, "Should update timeout value");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotSetInvalidThreshold()
        {
            cb.Threshold = 0;
        }

        [Test]
        public void CanExecuteOperation()
        {
            object result = cb.Execute(new TestDelegate(ValidOperation), 1, 2);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, (int)result, "Should return result of operation");
        }

        [Test]
        public void CanGetFailureCount()
        {
            try
            {
                cb.Execute(new ThreadStart(FailedOperation));
            }
            catch (OperationFailedException) { }
            Assert.AreEqual(80, cb.ServiceLevel, "Service level should have changed");

            try
            {
                cb.Execute(new ThreadStart(FailedOperation));
            }
            catch (OperationFailedException) { }
            Assert.AreEqual(60, cb.ServiceLevel, "Service level should have changed again");

            
            cb.Execute(new TestDelegate(ValidOperation), 1, 2);
            
            Assert.AreEqual(80, cb.ServiceLevel, "Operation should have succeeded and the service level improved");
        }

        [Test]
        public void CanGetOriginalException()
        {
            Exception innerException = null;
            try
            {
                cb.Execute(new ThreadStart(FailedOperation));
            }
            catch (OperationFailedException ex)
            {
                innerException = ex.InnerException;
            }
            Assert.IsInstanceOfType(typeof(TimeoutException), innerException, "Should contain original exception");
        }

        [Test]
        [ExpectedException(typeof(OpenCircuitException))]
        public void CanTripBreaker()
        {
            for (int i = 0; i < cb.Threshold + 5; i++)
            {
                try
                {
                    cb.Execute(new ThreadStart(FailedOperation));
                }
                catch (OperationFailedException) { }
                catch (OpenCircuitException)
                {
                    Assert.AreEqual(0, cb.ServiceLevel, "Service level should be zero");
                    throw;
                }
            }
        }

        [Test]
        public void CanResetBreaker()
        {
            try
            {
                cb.Execute(new ThreadStart(FailedOperation));
            }
            catch (OperationFailedException) { }
            catch (OpenCircuitException)
            {
                cb.Reset();
                Assert.AreEqual(CircuitBreakerState.Closed, cb.State, "Circuit should closed on reset");
                Assert.AreEqual(0, cb.ServiceLevel, "Service level should remain zero");
            }
        }

        [Test]
        [ExpectedException(typeof(OpenCircuitException))]
        public void CanForceTripBreaker()
        {
            Assert.AreEqual(CircuitBreakerState.Closed, cb.State, "Circuit should be initially closed");

            cb.Trip();

            Assert.AreEqual(CircuitBreakerState.Open, cb.State, "Circuit should be open on trip");

            // Calling execute when circuit is tripped should throw an OpenCircuitException
            cb.Execute(new TestDelegate(ValidOperation), 1, 2);
        }

        [Test]
        public void CanForceResetBreaker()
        {
            Assert.AreEqual(CircuitBreakerState.Closed, cb.State, "Circuit should be initially closed");

            cb.Trip();

            Assert.AreEqual(CircuitBreakerState.Open, cb.State, "Circuit should be open on trip");

            cb.Reset();

            Assert.AreEqual(CircuitBreakerState.Closed, cb.State, "Circuit should closed on reset");
            Assert.AreEqual(100, cb.ServiceLevel, "Service level should still be 100 percent");

            object result = cb.Execute(new TestDelegate(ValidOperation), 1, 2);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, (int)result, "Should return result of operation");
        }

        [Test]
        public void CanCloseBreakerAfterTimeout()
        {
            cb.Timeout = 500; // Shorten timeout to 500 milliseconds

            cb.Trip();

            Assert.AreEqual(CircuitBreakerState.Open, cb.State, "Circuit should be open on trip");

            Thread.Sleep(1000);

            Assert.AreEqual(CircuitBreakerState.HalfOpen, cb.State, "Circuit should be half-open after timeout");

            try
            {
                // Attempt failed operation
                cb.Execute(new ThreadStart(FailedOperation));
            }
            catch (OperationFailedException) { }

            Assert.AreEqual(CircuitBreakerState.Open, cb.State, "Circuit should be re-opened after failed operation");

            Thread.Sleep(1000);

            Assert.AreEqual(CircuitBreakerState.HalfOpen, cb.State, "Circuit should be half-open again after timeout");

            // Attempt successful operation
            cb.Execute(new TestDelegate(ValidOperation), 1, 2);

            Assert.AreEqual(CircuitBreakerState.Closed, cb.State, "Circuit should closed after successful operation");
        }

        [Test]
        public void CanIgnoreExceptionTypes()
        {
            cb.IgnoredExceptionTypes.Add(typeof(TimeoutException));

            try
            {
                // Attempt operation
                cb.Execute(new ThreadStart(FailedOperation));
            }
            catch (TimeoutException) { }

            Assert.AreEqual(100, cb.ServiceLevel, "Service level should still be 100%");
        }

        [Test]
        public void CanRaiseStateChangedEvent()
        {
            bool stateChangedEventFired = false;
            cb.StateChanged += (sender, e) =>
            {
                if (cb.State == CircuitBreakerState.Closed)
                {
                    stateChangedEventFired = true;
                }
            };

            cb.Trip();

            Assert.AreEqual(CircuitBreakerState.Open, cb.State, "Circuit should be open on trip");

            cb.Reset();

            Assert.AreEqual(CircuitBreakerState.Closed, cb.State, "Circuit should closed on reset");
            Assert.IsTrue(stateChangedEventFired, "StateChanged event should be fired on reset");

            stateChangedEventFired = false;

            // Reset an already closed circuit
            cb.Reset();

            Assert.IsFalse(stateChangedEventFired, "StateChanged event should be only be fired when state changes");
        }

        [Test]
        public void CanRaiseServiceLevelChangedEvent()
        {
            bool serviceLevelChangedEventFired = false;
            cb.ServiceLevelChanged += (sender, e) => { serviceLevelChangedEventFired = true; };

            try
            {
                cb.Execute(new ThreadStart(FailedOperation));
            }
            catch (OperationFailedException) { }

            Assert.IsTrue(serviceLevelChangedEventFired, "ServiceLevelChanged event should be fired on failure");
        }

        [Test]
        public void CanThrowInvokerException()
        {
            Exception verifyException = null;
            try
            {
                // Cause the DynamicInvoke method to throw an exception
                cb.Execute(null);
            }
            catch (Exception ex)
            {
                verifyException = ex;
            }

            Assert.IsNotNull(verifyException, "Exception expected");
            Assert.IsInstanceOfType(typeof(NullReferenceException), verifyException, "Should throw a NullReferenceException");
            Assert.IsNull(verifyException.InnerException, "Exception should not contain an inner exception");
            Assert.AreEqual(100, cb.ServiceLevel, "An invoker exception will not affect the service level");
        }
    }
}
