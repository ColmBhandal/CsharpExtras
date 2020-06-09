using CsharpExtras.Visitor;
using CsharpExtras.Visitor.Result;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtrasTest.Visitor.Result
{
    [TestFixture]
    class ResultStorageVisitorTest
    {
    }

    #region Compile Time Test
    //Compile time tests are code to test our types via the compiler. The code doesn't need to be run.
    class MockVisitable : IMockVisitable
    {
        public void Accept(IVisitorBase<IMockVisitable> visitor)
        {
            throw new NotImplementedException();
        }
    }

    internal interface IMockVisitable : IVisitableBase<IMockVisitable>
    {
    }

    class MockStorageVisitor : ResultStorageVisitorBase<IMockVisitable, IMockType>, IMockStorageVisitor
    {
        protected override IMockType DefaultResult()
        {
            return new MockClass();
        }
    }

    internal class MockClass : IMockType { }

    internal interface IMockType
    {
    }

    internal interface IMockStorageVisitor
    {
    }

    #endregion
}
