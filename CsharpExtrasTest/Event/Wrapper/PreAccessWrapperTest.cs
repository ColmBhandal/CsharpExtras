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
    internal class PreAccessWrapperTest
    {
        private ICsharpExtrasApi Api { get; } = new CsharpExtrasApi();

        [Test]
        public void GIVEN_Wrapper_WHEN_Get_THEN_ExpectedValueReturned()
        {
            //Arrange
            Mock<IEventHandler> mockHandler = new Mock<IEventHandler>();
            Mock<ITestObj> mockObj = new Mock<ITestObj>();
            Mock<ITestObj> mockOtherObj = new Mock<ITestObj>();
            Action<ITestObj> preAccessAction = o => mockHandler.Object.Handle(o);
            Mock<ITestGetter<ITestObj>> mockGetter = new Mock<ITestGetter<ITestObj>>();

            mockHandler.Setup(x => x.Handle(It.IsAny<ITestObj>()));
            mockGetter.Setup(g => g.Get(mockObj.Object)).Returns(mockOtherObj.Object);

            IPreAccessWrapper<ITestObj> wrapper
                = Api.NewPreAccessWrapper(mockObj.Object, preAccessAction);

            //Act
            ITestObj obj = wrapper.Get(mockGetter.Object.Get);

            //Assert
            Assert.AreSame(mockOtherObj.Object, obj);
        }

        [Test]
        public void GIVEN_Wrapper_WHEN_Get_THEN_AllOperationsRunInExpectedOrder()
        {
            //Arrange
            Mock<IEventHandler> mockHandler = new Mock<IEventHandler>();
            Mock<ITestObj> mockObj = new Mock<ITestObj>();
            Mock<ITestObj> mockOtherObj = new Mock<ITestObj>();
            Action<ITestObj> preAccessAction = o => mockHandler.Object.Handle(o);
            Mock<ITestGetter<ITestObj>> mockGetter = new Mock<ITestGetter<ITestObj>>();


            //Used to verify order of operations. See: https://stackoverflow.com/a/10609506/5134722
            int callOrder = 0;
            mockHandler.Setup(x => x.Handle(It.IsAny<ITestObj>()))
                .Callback(() => Assert.AreEqual(0, callOrder++)).Verifiable();
            mockGetter.Setup(g => g.Get(mockObj.Object)).Returns(mockOtherObj.Object)
                .Callback(() => Assert.AreEqual(1, callOrder++)).Verifiable();

            IPreAccessWrapper<ITestObj> wrapper
                = Api.NewPreAccessWrapper(mockObj.Object, preAccessAction);

            //Act
            wrapper.Get(mockGetter.Object.Get);

            //Assert
            mockObj.Verify();
            mockHandler.Verify();
        }

        [Test]
        public void GIVEN_Wrapper_WHEN_Run_THEN_AllOperationsRunInExpectedOrder()
        {
            //Arrange
            Mock<IEventHandler> mockHandler = new Mock<IEventHandler>();
            Mock<ITestObj> mockObj = new Mock<ITestObj>();
            Action<ITestObj> preAccessAction = o => mockHandler.Object.Handle(o);


            //Used to verify order of operations. See: https://stackoverflow.com/a/10609506/5134722
            int callOrder = 0;
            mockHandler.Setup(x => x.Handle(It.IsAny<ITestObj>()))
                .Callback(() => Assert.AreEqual(0, callOrder++)).Verifiable();
            mockObj.Setup(o => o.Foo())
                .Callback(() => Assert.AreEqual(1, callOrder++)).Verifiable();

            IPreAccessWrapper<ITestObj> wrapper 
                = Api.NewPreAccessWrapper(mockObj.Object, preAccessAction);

            //Act
            wrapper.Run(o => o.Foo());

            //Assert
            mockObj.Verify();
            mockHandler.Verify();
        }

        [Test]
        public void GIVEN_PreAccessException_WHEN_Run_THEN_ExceptionThrownAndActionNotExecuted()
        {
            //Arrange
            Mock<IEventHandler> mockHandler = new Mock<IEventHandler>();
            Mock<ITestObj> mockObj = new Mock<ITestObj>();
            Action<ITestObj> preAccessAction = o => mockHandler.Object.Handle(o);

            mockHandler.Setup(x => x.Handle(It.IsAny<ITestObj>()))
                .Throws<ArgumentException>();
            mockObj.Setup(o => o.Foo()).Verifiable();

            IPreAccessWrapper<ITestObj> wrapper
                = Api.NewPreAccessWrapper(mockObj.Object, preAccessAction);

            //Act
            Assert.Throws<ArgumentException>(() => wrapper.Run(o => o.Foo()));

            //Assert
            mockObj.Verify(o => o.Foo(), Times.Never());
        }

        [Test]
        public void GIVEN_PreAccessException_WHEN_Get_THEN_ExceptionThrownAndGetNotExecuted()
        {
            //Arrange
            Mock<IEventHandler> mockHandler = new Mock<IEventHandler>();
            Mock<ITestObj> mockObj = new Mock<ITestObj>();
            Mock<ITestObj> mockOtherObj = new Mock<ITestObj>();
            Action<ITestObj> preAccessAction = o => mockHandler.Object.Handle(o);
            Mock<ITestGetter<ITestObj>> mockGetter = new Mock<ITestGetter<ITestObj>>();

            mockHandler.Setup(x => x.Handle(It.IsAny<ITestObj>()))
                .Throws<ArgumentException>();
            mockGetter.Setup(g => g.Get(mockObj.Object)).Returns(mockOtherObj.Object)
                .Verifiable();

            IPreAccessWrapper<ITestObj> wrapper
                = Api.NewPreAccessWrapper(mockObj.Object, preAccessAction);

            //Act
            Assert.Throws<ArgumentException>(() => wrapper.Get(mockGetter.Object.Get));

            //Assert
            mockGetter.Verify(g => g.Get(It.IsAny<ITestObj>()), Times.Never());
        }
    }

    interface ITestGetter<TResult>
    {
        TResult Get(ITestObj obj);
    }

    interface ITestObj
    {
        void Foo();
    }
    
    interface IEventHandler
    {
        void Handle(ITestObj obj);
    }
}
