using System;
using System.IO;
using NUnit.Framework;
using REST_HTTP_Webservice;
using Moq;


namespace REST_HTTP_Webservice_Tests
{
    public class MessageManagerTest
    {
        [SetUp]
        public void Init()
        {
            var directoryInfo = Directory.GetParent(Environment.CurrentDirectory).Parent; 
            var dir = "";
            if (directoryInfo != null)
                dir = directoryInfo.FullName + "\\messages";
            var exist = Directory.Exists(dir);
            if (exist)
            {
                var dirFolder = new DirectoryInfo(dir);
                dirFolder.Delete(true);
            }
        }

        [TearDown]
        public void Dispose()
        {
            var directoryInfo = Directory.GetParent(Environment.CurrentDirectory).Parent; 
            var dir = "";
            if (directoryInfo != null)
                dir = directoryInfo.FullName + "\\messages";
            var exist = Directory.Exists(dir);
            if (exist)
            {
                var dirFolder = new DirectoryInfo(dir);
                dirFolder.Delete(true);
            }
        }

        [Test]
        public void TestMessageManager_Constructor()
        {
            // arrange
            var mockedRequestContext = new Mock<IRequestContext>();
            var directoryInfo = Directory.GetParent(Environment.CurrentDirectory).Parent; 
            var dir = "";

            // act
            var erg = new MessageManager(mockedRequestContext.Object);
            
            if (directoryInfo != null)
                dir = directoryInfo.FullName + "\\messages";
            var exist = Directory.Exists(dir);

            // assert
            Assert.IsTrue(exist);
            Assert.IsNotNull(erg);
        }
        
        [Test]
        public void TestMessageManager_ReadMessage()
        {
            // arrange
            var mockedRequestContext = new Mock<IRequestContext>();
            var manager = new MessageManager(mockedRequestContext.Object);
            mockedRequestContext.SetupGet(mock => mock.Path).Returns("/messages/1");
            mockedRequestContext.Setup(mock => mock.GetNotFound("No File /messages/1 found!")).Returns("");

            // act
            manager.ReadMessage();

            // assert
            mockedRequestContext.Verify(mock => mock.Path);
            mockedRequestContext.Verify(mock => mock.GetNotFound("No File /messages/1 found!"));
        }

        [Test]
        public void TestMessageManager_ListMessages()
        {
            // arrange
            var mockedRequestContext = new Mock<IRequestContext>();
            var manager = new MessageManager(mockedRequestContext.Object);
            mockedRequestContext.SetupGet(mock => mock.Path).Returns("/messages");
            mockedRequestContext.Setup(mock => mock.GetNoContent("No content found.")).Returns("");

            // act
            manager.ListMessages();

            // assert
            mockedRequestContext.Verify(mock => mock.Path);
            mockedRequestContext.Verify(mock => mock.GetNoContent("No content found."));
        }

        [Test]
        public void TestMessageManager_SaveMessage()
        {
            // arrange
            var mockedRequestContext = new Mock<IRequestContext>();
            var manager = new MessageManager(mockedRequestContext.Object);
            mockedRequestContext.SetupGet(mock => mock.Path).Returns("/messages");
            mockedRequestContext.SetupGet(mock => mock.Payload).Returns("Test");
            mockedRequestContext.SetupGet(mock => mock.ContentLength).Returns(4);
            mockedRequestContext.Setup(mock => mock.GetCreated("1")).Returns("");

            var directoryInfo = Directory.GetParent(Environment.CurrentDirectory).Parent; 
            var dir = "";
            if (directoryInfo != null)
                dir = directoryInfo.FullName + "\\messages\\1";

            // act
            manager.SaveMessage();
            var exist = File.Exists(dir);
            var fileText = File.ReadAllText(dir);

            // assert
            mockedRequestContext.Verify(mock => mock.Path);
            mockedRequestContext.Verify(mock => mock.Payload);
            mockedRequestContext.Verify(mock => mock.ContentLength);
            mockedRequestContext.Verify(mock => mock.GetCreated("1"));
            Assert.IsTrue(exist);
            Assert.AreEqual("Test", fileText);
        }

        [Test]
        public void TestMessageManager_UpdateMessage()
        {
            // arrange
            var mockedRequestContext = new Mock<IRequestContext>();
            var manager = new MessageManager(mockedRequestContext.Object);
            mockedRequestContext.SetupGet(mock => mock.Path).Returns("/messages/1");
            mockedRequestContext.SetupGet(mock => mock.Payload).Returns("Test");
            mockedRequestContext.SetupGet(mock => mock.ContentLength).Returns(4);
            mockedRequestContext.Setup(mock => mock.GetCreated("Message Changed!")).Returns("");

            var directoryInfo = Directory.GetParent(Environment.CurrentDirectory).Parent; 
            var dir = "";
            if (directoryInfo != null)
                dir = directoryInfo.FullName + "\\messages\\1";

            // act
            manager.UpdateMessage();
            var exist = File.Exists(dir);
            var fileText = File.ReadAllText(dir);

            // assert
            mockedRequestContext.Verify(mock => mock.Path);
            mockedRequestContext.Verify(mock => mock.Payload);
            mockedRequestContext.Verify(mock => mock.ContentLength);
            mockedRequestContext.Verify(mock => mock.GetCreated("Message Changed!"));
            Assert.IsTrue(exist);
            Assert.AreEqual("Test", fileText);
        }

        [Test]
        public void TestMessageManager_DeleteMessage()
        {
            // arrange
            var mockedRequestContext = new Mock<IRequestContext>();
            var manager = new MessageManager(mockedRequestContext.Object);
            mockedRequestContext.SetupGet(mock => mock.Path).Returns("/messages/1");
            mockedRequestContext.Setup(mock => mock.GetNotFound("File /messages/1 not found!")).Returns("");

            var directoryInfo = Directory.GetParent(Environment.CurrentDirectory).Parent; 
            var dir = "";
            if (directoryInfo != null)
                dir = directoryInfo.FullName + "\\messages\\1";

            // act
            manager.DeleteMessage();
            var exist = File.Exists(dir);

            // assert
            mockedRequestContext.Verify(mock => mock.Path);
            mockedRequestContext.Verify(mock => mock.GetNotFound("File /messages/1 not found!"));
            Assert.IsFalse(exist);
        }
    }
}