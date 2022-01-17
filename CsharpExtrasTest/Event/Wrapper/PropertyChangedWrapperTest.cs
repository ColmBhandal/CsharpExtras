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
    public class PropertyChangedWrapperTest
    {
        private ICsharpExtrasApi Api { get; } = new CsharpExtrasApi();

        [Test]
        public void GIVEN_Wrapper_WHEN_Get_THEN_ExpectedValueReturned()
        {
            //Arrange

            Mock<IMockBefore> mockBefore = new Mock<IMockBefore>();
            Mock<IMockAfter> mockAfter = new Mock<IMockAfter>();
            Mock<IMockObj> mockObj = new Mock<IMockObj>();
            Mock<IMockEvent> mockEvent = new Mock<IMockEvent>();
            Mock<IMockGenerator> mockGenerator = new Mock<IMockGenerator>();
            Mock<IMockGetter<int>> mockGetter = new Mock<IMockGetter<int>>();

            mockObj.Setup(o => o.GetBefore).Returns(mockBefore.Object);
            mockGetter.Setup(e => e.Get(mockObj.Object)).Returns(72);
            mockObj.Setup(o => o.GetAfter).Returns(mockAfter.Object);
            mockGenerator.Setup(g => g.Generate(mockBefore.Object, mockAfter.Object)).Returns(mockEvent.Object);

            IPropertyChangedWrapper<IMockObj, IMockEvent> wrapper = Api.NewPropertyChangedWrapper
                (mockObj.Object, o => o.GetBefore, o => o.GetAfter, mockGenerator.Object.Generate);

            //Act
            int result = wrapper.Get(mockGetter.Object.Get);

            //Assert
            Assert.AreEqual(72, result);
        }

        [Test]
        public void GIVEN_Wrapper_WHEN_Get_THEN_AllOperationsRunInExpectedOrder()
        {
            //Arrange

            Mock<IMockBefore> mockBefore = new Mock<IMockBefore>();
            Mock<IMockAfter> mockAfter = new Mock<IMockAfter>();
            Mock<IMockObj> mockObj = new Mock<IMockObj>();
            Mock<IMockEvent> mockEvent = new Mock<IMockEvent>();
            Mock<IMockGenerator> mockGenerator = new Mock<IMockGenerator>();
            Mock<IMockEventHandler> mockHandler = new Mock<IMockEventHandler>();
            Mock<IMockGetter<int>> mockGetter = new Mock<IMockGetter<int>>();

            //Used to verify order of operations. See: https://stackoverflow.com/a/10609506/5134722
            int callOrder = 0;
            mockObj.Setup(o => o.GetBefore).Returns(mockBefore.Object)
                .Callback(() => Assert.AreEqual(0, callOrder++)).Verifiable();
            mockGetter.Setup(e => e.Get(mockObj.Object))
                .Callback(() => Assert.AreEqual(1, callOrder++)).Returns(72).Verifiable();
            mockObj.Setup(o => o.GetAfter).Returns(mockAfter.Object)
                .Callback(() => Assert.AreEqual(2, callOrder++)).Verifiable();
            mockGenerator.Setup(g => g.Generate(mockBefore.Object, mockAfter.Object)).Returns(mockEvent.Object)
                .Callback(() => Assert.AreEqual(3, callOrder++)).Verifiable();
            mockHandler.Setup(h => h.Handle(mockEvent.Object))
                .Callback(() => Assert.AreEqual(4, callOrder++)).Verifiable();

            IPropertyChangedWrapper<IMockObj, IMockEvent> wrapper = Api.NewPropertyChangedWrapper
                (mockObj.Object, o => o.GetBefore, o => o.GetAfter, mockGenerator.Object.Generate);
            wrapper.OnPropertyChanged += mockHandler.Object.Handle;

            //Act
            wrapper.Get(mockGetter.Object.Get);

            //Assert
            mockObj.Verify();
            mockGetter.Verify();
            mockGenerator.Verify();
            mockHandler.Verify();
        }

        [Test]
        public void GIVEN_Wrapper_WHEN_Run_THEN_AllOperationsRunInExpectedOrder()
        {
            
            //Arrange

            Mock<IMockBefore> mockBefore = new Mock<IMockBefore>();
            Mock<IMockAfter> mockAfter = new Mock<IMockAfter>(); 
            Mock<IMockObj> mockObj = new Mock<IMockObj>();
            Mock<IMockEvent> mockEvent = new Mock<IMockEvent>();
            Mock<IMockGenerator> mockGenerator = new Mock<IMockGenerator>();
            Mock<IMockEventHandler> mockHandler = new Mock<IMockEventHandler>();
            Mock<IMockActionExecutor> mockExecutor = new Mock<IMockActionExecutor>();

            //Used to verify order of operations. See: https://stackoverflow.com/a/10609506/5134722
            int callOrder = 0;
            mockObj.Setup(o => o.GetBefore).Returns(mockBefore.Object)
                .Callback(() => Assert.AreEqual(0, callOrder++)).Verifiable();
            mockExecutor.Setup(e => e.Execute(mockObj.Object))
                .Callback(() => Assert.AreEqual(1, callOrder++)).Verifiable();
            mockObj.Setup(o => o.GetAfter).Returns(mockAfter.Object)
                .Callback(() => Assert.AreEqual(2, callOrder++)).Verifiable();
            mockGenerator.Setup(g => g.Generate(mockBefore.Object, mockAfter.Object)).Returns(mockEvent.Object)
                .Callback(() => Assert.AreEqual(3, callOrder++)).Verifiable();
            mockHandler.Setup(h => h.Handle(mockEvent.Object))
                .Callback(() => Assert.AreEqual(4, callOrder++)).Verifiable();

            IPropertyChangedWrapper <IMockObj, IMockEvent> wrapper = Api.NewPropertyChangedWrapper
                (mockObj.Object, o => o.GetBefore, o => o.GetAfter, mockGenerator.Object.Generate);
            wrapper.OnPropertyChanged += mockHandler.Object.Handle;

            //Act
            wrapper.Run(mockExecutor.Object.Execute);

            //Assert
            mockObj.Verify();
            mockExecutor.Verify();
            mockGenerator.Verify();
            mockHandler.Verify();
        }
    }
}
