using NUnit.Framework;

namespace Testing.Modules.TicTacToe
{
    public class SquareTests
    {
        [SetUp]
        private void SetUp()
        {
            
        }

        [Test]
        public void SquareConstructorVoidTest()
        {
            Square square = new Square();
            Assert.AreEqual(square.Value, Square.Empty);
        }
    }
}