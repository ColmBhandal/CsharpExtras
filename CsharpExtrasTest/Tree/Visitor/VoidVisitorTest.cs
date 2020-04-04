using CsharpExtras.Tree.Integer;
using CsharpExtras.Tree.Visitor.Counter;
using CsharpExtras.Tree.Visitor.Integer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tree.Visitor
{
    [TestFixture]
    public class VoidVisitorTest
    {
        [Test]
        [Category("Unit")]
        public void GivenMockTreeWhenVisitedByProductVisitorThenProductOfPayloadsReturned()
        {
            IIntegerAccumulationVisitor visitor = new IntegerProductVisitorImpl();
            RunIntegerAccumulatorAndAssertCorrectResult(visitor, 1, 1330065408);
        }

        [Test]
        [Category("Unit")]
        public void GivenMockTreeWhenVisitedBySummationVisitorThenSumOfPayloadsReturned()
        {
            IIntegerAccumulationVisitor visitor = new IntegerSummationVisitorImpl();
            RunIntegerAccumulatorAndAssertCorrectResult(visitor, 0, 12);
        }

        private void RunIntegerAccumulatorAndAssertCorrectResult(IIntegerAccumulationVisitor visitor, 
            int expectedBefore, int expectedAfter)
        {
            IChildSetIntegerTree mockTree = GenerateMockTree();
            Assert.AreEqual(expectedBefore, visitor.Result, "GIVEN: Result before visitation is incorrect");
            visitor.Visit(mockTree);
            Assert.AreEqual(expectedAfter, visitor.Result, "Visitor's result is not as expected after visitation");
        }

        [Test]
        [Category("Unit")]
        public void GivenMockTreeWhenVisitedByNodeCounterThenCorrectNumberOfNodesIsReturned()
        {
            NodeCounterVisitorImpl<int> visitor = new NodeCounterVisitorImpl<int>();
            RunCountAndAssertionsForCounterVisitorTest(visitor, 9);
        }

        [Test]
        [Category("Unit")]
        public void GivenMockTreeWhenVisitedByLeafNodeCounterThenCountOfLeafNodesOnlyReturned()
        {
            LeaftCounterVisitorImpl<int> visitor = new LeaftCounterVisitorImpl<int>();
            RunCountAndAssertionsForCounterVisitorTest(visitor, 6);
        }

        private void RunCountAndAssertionsForCounterVisitorTest(NodeCounterVisitorBase<int> visitor, int expectedCount)
        {
            IChildSetIntegerTree mockTree = GenerateMockTree();
            Assert.Zero(visitor.Count, "GIVEN: Expected node count to be zero before starting the operation");
            mockTree.Accept(visitor);
            Assert.AreEqual(expectedCount, visitor.Count, "Expected the visitor count to be equal to the # of nodes in the tree");
        }

        IChildSetIntegerTree GenerateMockTree()
        {
            IChildSetIntegerTree leftSubtree = new IntegerTreeImpl(4)
                    .WithLeaf(-2)
                    .WithLeaf(-7)
                    .WithLeaf(-188);
            IChildSetIntegerTree rightSubtree = new IntegerTreeImpl(-4)
                    .WithLeaf(2)
                    .WithLeaf(7)
                    .WithLeaf(188);
            //Note: the left and right were chosen purposely to cancel each other out. So the total sum should be equal to the root's payload.
            IChildSetIntegerTree root = new IntegerTreeImpl(12)
                .WithChild(leftSubtree)
                .WithChild(rightSubtree);
            return root;
        }
    }

}
