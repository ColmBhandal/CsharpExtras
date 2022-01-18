using CsharpExtras.Api;
using CsharpExtras.Event.Notify;
using CsharpExtras.ValidatedType.Numeric.Integer;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtrasTest.Event.Notify
{

    [TestFixture, Category("Unit")]
    public class UpdateNotifierTest
    {
        private ICsharpExtrasApi Api { get; } = new CsharpExtrasApi();

        [Test]
        public void GIVEN_NotifierWithExceptionInBothHandlers_WHEN_Update_THEN_ExceptionThrownAndValueNotUpdated()
        {
            //Arrange
            IUpdateNotifier<NonnegativeInteger, int> notifier
                = Api.NewUpdateNotifier<NonnegativeInteger, int>((NonnegativeInteger)0, UpdateNonnegativeInteger);
            notifier.BeforeUpdate += (i) => throw new NotImplementedException();
            notifier.AfterUpdate += (i) => throw new NotImplementedException();

            //Act / Assert
            Assert.Throws<NotImplementedException>(() => notifier.Update(7));

            //Assert            
            int value = notifier.Value;
            Assert.AreEqual(0, value);
        }

        [Test]
        public void GIVEN_NotifierWithExceptionInAfterHandlerOnly_WHEN_Update_THEN_ExceptionThrownAndBeforeHandlerTriggered()
        {
            //Arrange
            IUpdateNotifier<NonnegativeInteger, int> notifier
                = Api.NewUpdateNotifier<NonnegativeInteger, int>((NonnegativeInteger)2, UpdateNonnegativeInteger);
            Mock<IMockIntUpdateHandler> beforeHandlerMock = GetHandleCalledVerifier("Before Update");
            notifier.BeforeUpdate += beforeHandlerMock.Object.Handle;
            notifier.AfterUpdate += (i) => throw new NotImplementedException();

            //Act / Assert
            Assert.Throws<NotImplementedException>(() => notifier.Update(5));

            //Assert            
            beforeHandlerMock.Verify();
        }

        [Test]
        public void GIVEN_NotifierWithExceptionInAfterHandlerOnly_WHEN_Update_THEN_ExceptionThrownAndValueIsUpdated()
        {
            //Arrange
            IUpdateNotifier<NonnegativeInteger, int> notifier
                = Api.NewUpdateNotifier<NonnegativeInteger, int>((NonnegativeInteger)2, UpdateNonnegativeInteger);
            Mock<IMockIntUpdateHandler> beforeHandlerMock = new Mock<IMockIntUpdateHandler>();
            notifier.BeforeUpdate += beforeHandlerMock.Object.Handle;
            notifier.AfterUpdate += (i) => throw new NotImplementedException();

            //Act / Assert
            Assert.Throws<NotImplementedException>(() => notifier.Update(5));

            //Assert            
            int value = notifier.Value;
            Assert.AreEqual(7, value);
        }

        [Test]
        public void GIVEN_NotifierWithExceptionInBeforeHandlerOnly_WHEN_Update_THEN_ExceptionThrownAndValueNotUpdated()
        {
            //Arrange
            IUpdateNotifier<NonnegativeInteger, int> notifier
                = Api.NewUpdateNotifier<NonnegativeInteger, int>((NonnegativeInteger)0, UpdateNonnegativeInteger);
            notifier.BeforeUpdate += (i) => throw new NotImplementedException();
            Mock<IMockIntUpdateHandler> afterHandlerMock = new Mock<IMockIntUpdateHandler>();
            notifier.AfterUpdate += afterHandlerMock.Object.Handle;

            //Act / Assert
            Assert.Throws<NotImplementedException>(() => notifier.Update(7));

            //Assert            
            int value = notifier.Value;
            Assert.AreEqual(0, value);
        }

        [Test]
        public void GIVEN_NotifierWithExceptionInBeforeHandlerOnly_WHEN_Update_THEN_ExceptionThrownAndAfterHandlerNotCalled()
        {
            //Arrange
            IUpdateNotifier<NonnegativeInteger, int> notifier
                = Api.NewUpdateNotifier<NonnegativeInteger, int>((NonnegativeInteger)0, UpdateNonnegativeInteger);
            notifier.BeforeUpdate += (i) => throw new NotImplementedException();
            Mock <IMockIntUpdateHandler> afterHandlerMock = new Mock<IMockIntUpdateHandler>();
            notifier.AfterUpdate += afterHandlerMock.Object.Handle;

            //Act / Assert
            Assert.Throws<NotImplementedException>(() => notifier.Update(7));

            //Assert            
            afterHandlerMock.Verify(h => h.Handle(It.IsAny<int>()), Times.Never,
                "After event should never be triggered if before event throws exception");
        }

        [Test]
        public void GIVEN_NotifierWithNoHandlers_WHEN_Update_THEN_ValueUpdated()
        {
            //Arrange
            IUpdateNotifier<NonnegativeInteger, int> notifier
                = Api.NewUpdateNotifier<NonnegativeInteger, int>((NonnegativeInteger)8, UpdateNonnegativeInteger);

            //Act
            notifier.Update(-3);

            //Assert
            int value = notifier.Value;
            Assert.AreEqual(5, value);
        }

        [Test]
        public void GIVEN_NotifierWithBeforeAndAfterHandlers_WHEN_Update_THEN_BothHandlersTriggered()
        {
            //Arrange
            IUpdateNotifier<NonnegativeInteger, int> notifier
                = Api.NewUpdateNotifier<NonnegativeInteger, int>((NonnegativeInteger)0, UpdateNonnegativeInteger);
            Mock<IMockIntUpdateHandler> beforeHandleVerifier = GetHandleCalledVerifier("Before Update");
            notifier.BeforeUpdate += beforeHandleVerifier.Object.Handle;
            Mock<IMockIntUpdateHandler> afterHandleVerifier = GetHandleCalledVerifier("After Update");
            notifier.AfterUpdate += afterHandleVerifier.Object.Handle;

            //Act
            notifier.Update(7);

            //Assert
            beforeHandleVerifier.Verify();
            afterHandleVerifier.Verify();
        }

        [Test]
        public void GIVEN_NotifierWithAfterHandlerOnly_WHEN_Update_THEN_AfeterHandlerTriggered()
        {
            //Arrange
            IUpdateNotifier<NonnegativeInteger, int> notifier
                = Api.NewUpdateNotifier<NonnegativeInteger, int>((NonnegativeInteger)0, UpdateNonnegativeInteger);
            Mock<IMockIntUpdateHandler> afterHandleVerifier = GetHandleCalledVerifier("After Update");
            notifier.AfterUpdate += afterHandleVerifier.Object.Handle;

            //Act
            notifier.Update(7);

            //Assert
            afterHandleVerifier.Verify();
        }

        [Test]
        public void GIVEN_NotifierWithBeforeHandlerOnly_WHEN_Update_THEN_BeforeHandlerTriggered()
        {
            //Arrange
            IUpdateNotifier<NonnegativeInteger, int> notifier
                = Api.NewUpdateNotifier<NonnegativeInteger, int>((NonnegativeInteger)0, UpdateNonnegativeInteger);
            Mock<IMockIntUpdateHandler> beforeHandleVerifier = GetHandleCalledVerifier("Before Update");
            notifier.BeforeUpdate += beforeHandleVerifier.Object.Handle;

            //Act
            notifier.Update(7);

            //Assert
            beforeHandleVerifier.Verify();
        }

        private static Mock<IMockIntUpdateHandler> GetHandleCalledVerifier(string expectedTriggeringEvent)
        {
            Mock<IMockIntUpdateHandler> mockHandler = new Mock<IMockIntUpdateHandler>();
            mockHandler.Setup(h => h.Handle(It.IsAny<int>()))
                .Verifiable($"Expected the handle method to be triggered by {expectedTriggeringEvent}");
            return mockHandler;
        }

        private NonnegativeInteger UpdateNonnegativeInteger(NonnegativeInteger i, int delta)
        {
            return (NonnegativeInteger)(i + delta);
        }
    }

    public interface IMockIntUpdateHandler
    {
        void Handle(int i);
    }
}
