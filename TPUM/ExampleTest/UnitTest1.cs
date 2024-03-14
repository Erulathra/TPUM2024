namespace ExampleTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.IsTrue(2 + 2 == 4);
        }

        [TestMethod]
        public void TestMethod2()
        {
            Assert.IsTrue(1 == 1);
        }
    }
}