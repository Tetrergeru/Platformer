using System;
using System.Linq;
using NUnit.Framework;

namespace viper_script.Tests
{
    [TestFixture]
    public class MultiTreeNodeTest
    {
        [Test]
        public void EmptyNode()
        {
            var n = new MultiTreeNode<string>("test");
            Assert.AreEqual(n.GetChildren(), new MultiTreeNode<string>[] { });
        }

        [Test]
        public void OneChild()
        {
            var n = new MultiTreeNode<string>("1");
            n.AddChild(new MultiTreeNode<string>("2"));
            Assert.AreEqual(n.GetChildren().Select(t => t.Data), new[] {"2"});
        }

        [Test]
        public void SeveralChildren()
        {
            var n = new MultiTreeNode<string>("1");
            n.AddChild(new MultiTreeNode<string>("2"));
            n.AddChild(new MultiTreeNode<string>("3"));
            n.AddChild(new MultiTreeNode<string>("4"));
            n.AddChild(new MultiTreeNode<string>("5"));
            Assert.AreEqual(n.GetChildren().Select(t => t.Data), new[] {"5", "4", "3", "2"});
        }

        [Test]
        public void PopChild()
        {
            var n = new MultiTreeNode<string>("1");
            n.AddChild(new MultiTreeNode<string>("2"));
            n.AddChild(new MultiTreeNode<string>("3"));
            Assert.AreEqual(n.GetChildren().Select(t => t.Data), new[] {"3", "2"});
            Assert.AreEqual(n.PopChild().Data, "3");
            Assert.AreEqual(n.GetChildren().Select(t => t.Data), new[] {"2"}, "Incorrect GetChildren after ChildPop");
        }

        [Test]
        public void ParentsAreOk()
        {
            var n = new MultiTreeNode<int>(0);

            for (var i = 1; i <= 4; i++)
                n.AddChild(new MultiTreeNode<int>(i));

            foreach (var ch in n.GetChildren())
                Assert.AreSame(ch.Parent, n, "Parent is incorrect");
        }

        [Test]
        public void ZeroChildrenCount()
        {
            var n = new MultiTreeNode<int>(1);
            Assert.AreEqual(n.Count, 0, "Child count was incorrect");
        }

        [Test]
        public void ZeroChildrenCountAfterPop()
        {
            var n = new MultiTreeNode<int>(1);
            n.PopChild();
            Assert.AreEqual(n.Count, 0, "Child count was incorrect");
        }

        [Test]
        public void ManyChildrenCount()
        {
            var n = new MultiTreeNode<int>(0);

            for (var i = 1; i <= 4; i++)
                n.AddChild(new MultiTreeNode<int>(i));

            Assert.AreEqual(n.Count, 4, "Child count was incorrect");
        }

        [Test]
        public void ManyChildrenCountAfterPop()
        {
            var n = new MultiTreeNode<int>(0);

            for (var i = 1; i <= 4; i++)
                n.AddChild(new MultiTreeNode<int>(i));

            n.PopChild();
            n.PopChild();
            Assert.AreEqual(n.Count, 2, "Child count was incorrect");
        }

        [Test]
        public void ManyChildrenCountAllPoped()
        {
            var n = new MultiTreeNode<int>(0);

            for (var i = 1; i <= 4; i++)
                n.AddChild(new MultiTreeNode<int>(i));

            for (var i = 0; i < 4; i++)
                n.PopChild();

            Assert.AreEqual(n.Count, 0, "Child count was incorrect");
        }

        [Test]
        public void ChildByIdx()
        {
            var n = new MultiTreeNode<int>(0);

            for (var i = 1; i <= 4; i++)
                n.AddChild(new MultiTreeNode<int>(i));

            Assert.AreEqual(n[3].Data, 1, "Child by idx was incorrect");
        }

        [Test]
        public void ChildByIncorrectIdx()
        {
            var n = new MultiTreeNode<int>(0);
            n.AddChild(new MultiTreeNode<int>(1));
            try
            {
                var x = n[3].Data;
            }
            catch (ArgumentException)
            {
                return;
            }
            Assert.Fail();
        }
    }
}