using System.IO;
using System.Net.Sockets;
using MonsterTCG;
using Moq;
using NUnit.Framework;

namespace MonsterTCG_Tests
{
    class ServerTest
    {
        [Test]
        public void TestServer_ClientHandler_Constructor()
        {
            // arrange
            var mockedTcpHandler = new Mock<ITcpHandler>();
            mockedTcpHandler.Setup(x => x.AcceptTcpClient()).Returns(new Mock<TcpClient>().Object);

            // act
            ClientHandler clientHandler = new ClientHandler(mockedTcpHandler.Object);

            // assert
            mockedTcpHandler.Verify(x => x.AcceptTcpClient());
        }

        [Test]
        public void TestServer_ClientHandler_ExecuteRequest()
        {
            // arrange
            var mockedTcpHandler = new Mock<ITcpHandler>();
            var mockedTcpClient = new Mock<TcpClient>();
            Stream stream = new MemoryStream();
            var writer = new StreamWriter(stream) { AutoFlush = true };

            var requestReceived = "GET / HTTP/1.1\r\n" +
                                  "Host: localhost:10001\r\n"+
                                  "User-Agent: curl/7.55.1\r\n"+
                                  "Accept: */*\r\n"+
                                  "\r\n";
            writer.Write(requestReceived);
            stream.Position = 0;

            mockedTcpHandler.Setup(x => x.AcceptTcpClient()).Returns(mockedTcpClient.Object);
            mockedTcpHandler.Setup(x => x.GetStream(mockedTcpClient.Object)).Returns(stream);

            // act
            ClientHandler clientHandler = new ClientHandler(mockedTcpHandler.Object);
            clientHandler.ExecuteRequest();

            // assert
            mockedTcpHandler.Verify(x => x.AcceptTcpClient());
            mockedTcpHandler.Verify(x => x.GetStream(mockedTcpClient.Object));
        }

        [Test]
        public void TestServer_ClientHandler_CloseClient()
        {
            // arrange
            var mockedTcpHandler = new Mock<ITcpHandler>();
            var mockedTcpClient = new Mock<TcpClient>();

            mockedTcpHandler.Setup(x => x.AcceptTcpClient()).Returns(mockedTcpClient.Object);
            mockedTcpHandler.Setup(x => x.CloseClient(mockedTcpClient.Object));

            // act
            ClientHandler clientHandler = new ClientHandler(mockedTcpHandler.Object);
            clientHandler.CloseClient();

            // assert
            mockedTcpHandler.Verify(x => x.AcceptTcpClient());
            mockedTcpHandler.Verify(x => x.CloseClient(mockedTcpClient.Object));
        }

        [Test]
        public void TestServer_ContextManager_Constructor()
        {
            // arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream) { AutoFlush = true };
            var reader = new StreamReader(stream);

            var requestReceived = "GET / HTTP/1.1\r\n" +
                                  "Host: localhost:10001\r\n"+
                                  "User-Agent: curl/7.55.1\r\n"+
                                  "Accept: */*\r\n"+
                                  "\r\n";
            writer.Write(requestReceived);
            stream.Position = 0;


            // act
            ContextManager contextManager = new ContextManager(reader);

            // assert
            Assert.NotNull(contextManager);
        }

        [Test]
        public void TestServer_ContextManager_DataExists()
        {
            // arrange
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream) { AutoFlush = true };
            var reader = new StreamReader(stream);

            var requestReceived = "GET / HTTP/1.1\r\n" +
                                  "Host: localhost:10001\r\n"+
                                  "User-Agent: curl/7.55.1\r\n"+
                                  "Accept: */*\r\n"+
                                  "\r\n";
            writer.Write(requestReceived);
            stream.Position = 0;


            // act
            ContextManager contextManager = new ContextManager(reader);

            // assert
            Assert.NotNull(contextManager);
            Assert.IsTrue(contextManager.Information.ContainsKey("Path"));
            Assert.IsTrue(contextManager.Information.ContainsKey("Version"));
            Assert.IsTrue(contextManager.Information.ContainsKey("Request"));
        }

        [Test]
        public void TestServer_ContextManager_NoData()
        {
            // arrange
            var stream = new MemoryStream();
            var reader = new StreamReader(stream);

            // act
            ContextManager contextManager = new ContextManager(reader);

            // assert
            Assert.NotNull(contextManager);
            Assert.IsFalse(contextManager.Information.ContainsKey("Path"));
            Assert.IsFalse(contextManager.Information.ContainsKey("Version"));
            Assert.IsFalse(contextManager.Information.ContainsKey("Request"));
        }

        [Test]
        public void TestServer_ResponseManager_Constructor()
        {
            // arrange
            var mockedResponse = new Mock<IResponse>();

            // act
            var responseManager = new ResponseManager(mockedResponse.Object, "Test");

            // assert
            Assert.NotNull(responseManager);
        }

        [Test]
        public void TestServer_ResponseManager_ProcessTextResponseOk()
        {
            // arrange
            var mockedResponse = new Mock<IResponse>();
            mockedResponse.Setup(x => x.Status).Returns(StatusCodesEnum.Ok);
            mockedResponse.Setup(x => x.Content).Returns("");
            mockedResponse.Setup(x => x.ContentType).Returns("text/plain");

            var response = "";
            response += "HTTP/1.1 200 OK\r\n"+
                   "Server: Test\r\n"+
                   "Content-Type: text/plain\r\n"+
                   "Content-Length: 0\r\n"+
                   "\r\n"+
                   "";

            // act
            var responseManager = new ResponseManager(mockedResponse.Object, "Test");
            var erg = responseManager.ProcessResponse();

            // assert
            Assert.NotNull(responseManager);
            Assert.AreEqual(erg, response);
        }

        [Test]
        public void TestServer_ResponseManager_ProcessJsonResponseOk()
        {
            // arrange
            var mockedResponse = new Mock<IResponse>();
            mockedResponse.Setup(x => x.Status).Returns(StatusCodesEnum.Ok);
            mockedResponse.Setup(x => x.Content).Returns("");
            mockedResponse.Setup(x => x.ContentType).Returns("application/json");

            var response = "";
            response += "HTTP/1.1 200 OK\r\n"+
                        "Server: Test\r\n"+
                        "Content-Type: application/json\r\n"+
                        "Content-Length: 0\r\n"+
                        "\r\n"+
                        "";

            // act
            var responseManager = new ResponseManager(mockedResponse.Object, "Test");
            var erg = responseManager.ProcessResponse();

            // assert
            Assert.NotNull(responseManager);
            Assert.AreEqual(erg, response);
        }
    }
}
