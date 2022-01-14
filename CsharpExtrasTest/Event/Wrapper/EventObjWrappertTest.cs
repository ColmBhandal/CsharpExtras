using CsharpExtras.Api;
using CsharpExtras.Event.Wrapper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtrasTest.Event.Wrapper
{
    [TestFixture, Category("Unit")]
    public class EventObjWrappertTest
    {
        private ICsharpExtrasApi Api { get; } = new CsharpExtrasApi();

        [Test]
        public void GIVEN_EventWrapper_WHEN_Get_THEN_ResultReturned()
        {
            //Arrange
            Mock<IMockObj> mockObj = new Mock<IMockObj>();
            Mock<IMockEvent> mockEvent = new Mock<IMockEvent>();
            Mock<IMockGetter<int>> mockGetter = new Mock<IMockGetter<int>>();
            Mock<IMockEventHandler> mockHandler = new Mock<IMockEventHandler>();
            mockGetter.Setup(e => e.ExecuteGet(mockObj.Object)).Returns((mockEvent.Object, 7));

            IEventObjWrapper<IMockObj, IMockEvent> wrapper =
                Api.NewEventObjWrapper<IMockObj, IMockEvent>(mockObj.Object, mockHandler.Object.Handle);

            //Act
            int result = wrapper.Get(mockGetter.Object.ExecuteGet);

            //Assert
            Assert.AreEqual(7, result);
        }

        [Test]
        public void GIVEN_EventWrapper_WHEN_Get_THEN_EventTriggeredOnceWithExpectedArgument()
        {
            //Arrange
            Mock<IMockObj> mockObj = new Mock<IMockObj>();
            Mock<IMockEvent> mockEvent = new Mock<IMockEvent>();
            Mock<IMockGetter<int>> mockGetter = new Mock<IMockGetter<int>>();
            Mock<IMockEventHandler> mockHandler = new Mock<IMockEventHandler>();
            mockGetter.Setup(e => e.ExecuteGet(mockObj.Object)).Returns((mockEvent.Object, 7));

            IEventObjWrapper<IMockObj, IMockEvent> wrapper =
                Api.NewEventObjWrapper<IMockObj, IMockEvent>(mockObj.Object, mockHandler.Object.Handle);

            //Act
            wrapper.Get(mockGetter.Object.ExecuteGet);

            //Assert
            mockHandler.Verify(handler => handler.Handle(mockEvent.Object), Times.Once,
                "Expected handle method to be called exactly once with the given event");
        }

        [Test]
        public void GIVEN_ExceptionInHandler_WHEN_Get_THEN_ExceptionThrownAndExecuteGetCalled()
        {
            //Arrange
            Mock<IMockObj> mockObj = new Mock<IMockObj>();
            Mock<IMockEvent> mockEvent = new Mock<IMockEvent>();
            Mock<IMockGetter<int>> mockGetter = new Mock<IMockGetter<int>>();
            Mock<IMockEventHandler> mockHandler = new Mock<IMockEventHandler>();
            mockGetter.Setup(e => e.ExecuteGet(mockObj.Object)).Returns((mockEvent.Object, 7));

            IEventObjWrapper<IMockObj, IMockEvent> wrapper =
                Api.NewEventObjWrapper<IMockObj, IMockEvent>(mockObj.Object, mockHandler.Object.Handle);
            mockHandler.Setup(h => h.Handle(mockEvent.Object)).Throws<InvalidOperationException>();

            //Act
            Assert.Throws<InvalidOperationException>(() => wrapper.Get(mockGetter.Object.ExecuteGet));

            //Assert
            mockGetter.Verify(e => e.ExecuteGet(mockObj.Object), Times.Once, "Expected Get method to run even though handler has exception");
        }

        [Test]
        public void GIVEN_ExceptionInGetter_WHEN_Get_THEN_ExceptionThrownAndEventNotTriggered()
        {
            //Arrange
            Mock<IMockObj> mockObj = new Mock<IMockObj>();
            Mock<IMockEvent> mockEvent = new Mock<IMockEvent>();
            Mock<IMockGetter<int>> mockGetter = new Mock<IMockGetter<int>>();
            Mock<IMockEventHandler> mockHandler = new Mock<IMockEventHandler>();
            mockGetter.Setup(e => e.ExecuteGet(mockObj.Object)).Throws<InvalidOperationException>();

            IEventObjWrapper<IMockObj, IMockEvent> wrapper =
                Api.NewEventObjWrapper<IMockObj, IMockEvent>(mockObj.Object, mockHandler.Object.Handle);

            //Act
            Assert.Throws<InvalidOperationException>(() => wrapper.Get(mockGetter.Object.ExecuteGet));

            //Assert
            mockHandler.Verify(handler => handler.Handle(It.IsAny<IMockEvent>()), Times.Never,
                "Handler should not be triggered for any event if the action fails");
        }

        [Test]
        public void GIVEN_EventWrapper_WHEN_Run_THEN_ActionExecuted()
        {
            //Arrange
            Mock<IMockObj> mockObj = new Mock<IMockObj>();
            Mock<IMockEvent> mockEvent = new Mock<IMockEvent>();
            Mock<IMockExecutor> mockExecutor = new Mock<IMockExecutor>();
            Mock<IMockEventHandler> mockHandler = new Mock<IMockEventHandler>();
            mockExecutor.Setup(e => e.Execute(mockObj.Object)).Returns(mockEvent.Object);

            IEventObjWrapper<IMockObj, IMockEvent> wrapper =
                Api.NewEventObjWrapper<IMockObj, IMockEvent>(mockObj.Object, mockHandler.Object.Handle);

            //Act
            wrapper.Run(mockExecutor.Object.Execute);

            //Assert
            mockExecutor.Verify(ex => ex.Execute(mockObj.Object), Times.Once,
                "Expected execute method to be called exactly once with the given object");
        }

        [Test]
        public void GIVEN_EventWrapper_WHEN_Run_THEN_EventTriggeredOnceWithExpectedArgument()
        {
            //Arrange
            Mock<IMockObj> mockObj = new Mock<IMockObj>();
            Mock<IMockEvent> mockEvent = new Mock<IMockEvent>();
            Mock<IMockExecutor> mockExecutor = new Mock<IMockExecutor>();
            Mock<IMockEventHandler> mockHandler = new Mock<IMockEventHandler>();
            mockExecutor.Setup(e => e.Execute(mockObj.Object)).Returns(mockEvent.Object);
            
            IEventObjWrapper<IMockObj, IMockEvent> wrapper =
                Api.NewEventObjWrapper<IMockObj, IMockEvent>(mockObj.Object, mockHandler.Object.Handle);

            //Act
            wrapper.Run(mockExecutor.Object.Execute);

            //Assert
            mockHandler.Verify(handler => handler.Handle(mockEvent.Object), Times.Once,
                "Expected handle method to be called exactly once with the given event");
        }

        [Test]
        public void GIVEN_ExceptionInHandler_WHEN_Run_THEN_ExceptionThrownAndActionExecuted()
        {
            //Arrange
            Mock<IMockObj> mockObj = new Mock<IMockObj>();
            Mock<IMockEvent> mockEvent = new Mock<IMockEvent>();
            Mock<IMockExecutor> mockExecutor = new Mock<IMockExecutor>();
            Mock<IMockEventHandler> mockHandler = new Mock<IMockEventHandler>();
            mockExecutor.Setup(e => e.Execute(mockObj.Object)).Returns(mockEvent.Object);
            mockHandler.Setup(h => h.Handle(mockEvent.Object)).Throws<InvalidOperationException>();

            IEventObjWrapper<IMockObj, IMockEvent> wrapper =
                Api.NewEventObjWrapper<IMockObj, IMockEvent>(mockObj.Object, mockHandler.Object.Handle);

            //Act
            Assert.Throws<InvalidOperationException>(() => wrapper.Run(mockExecutor.Object.Execute));

            //Assert
            mockExecutor.Verify(ex => ex.Execute(mockObj.Object), Times.Once,
                "Expected execute method to be called event if the event handler has an exception in it");
        }

        [Test]
        public void GIVEN_ExceptionInAction_WHEN_Run_THEN_ExceptionThrownAndEventNotTriggered()
        {
            //Arrange
            Mock<IMockObj> mockObj = new Mock<IMockObj>();
            Mock<IMockEvent> mockEvent = new Mock<IMockEvent>();
            Mock<IMockExecutor> mockExecutor = new Mock<IMockExecutor>();
            Mock<IMockEventHandler> mockHandler = new Mock<IMockEventHandler>();
            mockExecutor.Setup(e => e.Execute(mockObj.Object)).Throws<InvalidOperationException>();

            IEventObjWrapper<IMockObj, IMockEvent> wrapper =
                Api.NewEventObjWrapper<IMockObj, IMockEvent>(mockObj.Object, mockHandler.Object.Handle);

            //Act
            Assert.Throws<InvalidOperationException>(() => wrapper.Run(mockExecutor.Object.Execute));

            //Assert
            mockHandler.Verify(handler => handler.Handle(It.IsAny<IMockEvent>()), Times.Never,
                "Handler should not be triggered for any event if the action fails");
        }
    }

    public interface IMockObj { }

    public interface IMockEvent {}

    public interface IMockEventHandler
    {
        void Handle(IMockEvent e);
    }
    
    public interface IMockExecutor
    {
        IMockEvent Execute(IMockObj obj);
    }

    public interface IMockGetter<TResult>
    {
        (IMockEvent e, TResult result) ExecuteGet(IMockObj obj);
    }
}
