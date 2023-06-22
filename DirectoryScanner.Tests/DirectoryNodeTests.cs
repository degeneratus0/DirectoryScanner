using NUnit.Framework;

namespace DirectoryScanner.Tests
{
    public class DirectoryNodeTests
    {
        private const string TestDirectory = "TestDirectory";

        [SetUp]
        public void SetUp()
        {
            Directory.CreateDirectory(TestDirectory);
        }

        [Test]
        public void NameIsCorrectForDirectory()
        {
            DirectoryNode node = new DirectoryNode(TestDirectory);

            Assert.AreEqual(TestDirectory, node.Name);
        }

        [Test]
        public void NameIsCorrectForFile()
        {
            string fileName = "test.txt";
            string filePath = TestDirectory + "/" + fileName;
            File.Create(filePath).Close();

            DirectoryNode node = new DirectoryNode(filePath);

            Assert.AreEqual(fileName, node.Name);
        }

        [Test]
        public void SizeIsCorrectForDirectoryAndFile()
        {
            string filePath = TestDirectory + "/test.txt";
            File.Create(filePath).Close();
            File.WriteAllText(filePath, "test");

            DirectoryNode node = new DirectoryNode(TestDirectory);
            node.ScanDirectory();

            Assert.AreEqual(4, node.Size);
            Assert.AreEqual(4, node.Children.First().Size);
        }

        [Test]
        public void MimeTypeIsCorrectForDirectory()
        {
            DirectoryNode node = new DirectoryNode(TestDirectory);

            Assert.AreEqual("directory", node.MimeType);
        }

        [Test]
        public void MimeTypeIsCorrectForFile()
        {
            string filePath = TestDirectory + "/test.txt";
            File.Create(filePath).Close();

            DirectoryNode node = new DirectoryNode(filePath);

            Assert.AreEqual("text/plain", node.MimeType);
        }

        [Test]
        public void IsDirectoryIsCorrectForDirectory()
        {
            DirectoryNode node = new DirectoryNode(TestDirectory);

            Assert.AreEqual(true, node.IsDirectory);
        }

        [Test]
        public void IsDirectoryIsCorrectForFile()
        {
            string filePath = TestDirectory + "/test.txt";
            File.Create(filePath).Close();

            DirectoryNode node = new DirectoryNode(filePath);

            Assert.AreEqual(false, node.IsDirectory);
        }

        [Test]
        public void IsDirectoryTreeCorrect()
        {
            string testDirectory1 = TestDirectory + "/test1";
            string testDirectory2 = TestDirectory + "/test2";
            string testDirectory11 = TestDirectory + "/test1/test11";
            Directory.CreateDirectory(testDirectory1);
            Directory.CreateDirectory(testDirectory2);
            Directory.CreateDirectory(testDirectory11);

            string filePath1 = testDirectory1 + "/test1.txt";
            string filePath11 = testDirectory11 + "/test2.txt";
            File.Create(filePath1).Close();
            File.WriteAllText(filePath1, "test");
            File.Create(filePath11).Close();
            File.WriteAllText(filePath11, "test");

            DirectoryNode node = new DirectoryNode(TestDirectory);
            node.ScanDirectory();

            Assert.AreEqual(8, node.Size);
            Assert.AreEqual(TestDirectory, node.Name);

            DirectoryNode test2 = node.Children.Last();
            Assert.AreEqual("test2", test2.Name);

            DirectoryNode test1 = node.Children.First();
            Assert.AreEqual("test1", test1.Name);

            DirectoryNode testTxt1 = test1.Children.Last();
            Assert.AreEqual("test1.txt", testTxt1.Name);

            DirectoryNode test11 = test1.Children.First();
            Assert.AreEqual("test11", test11.Name);

            DirectoryNode testTxt2 = test11.Children.First();
            Assert.AreEqual("test2.txt", testTxt2.Name);
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete(TestDirectory, true);
        }
    }
}