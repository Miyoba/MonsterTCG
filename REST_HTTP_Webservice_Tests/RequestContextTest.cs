using System.IO;
using NUnit.Framework;
using REST_HTTP_Webservice;

namespace REST_HTTP_Webservice_Tests
{
    public class RequestContextTest
    {
        [Test]
        public void TestRequestContext_Constructor_GET()
        {
            // arrange
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream) { AutoFlush = true };
            var reader = new StreamReader(memoryStream);

            var requestReceived = "GET /messages HTTP/1.1\r\n" +
                          "Host: localhost:8000\r\n"+
                          "User-Agent: curl/7.55.1\r\n"+
                          "Accept: */*\r\n"+
                          "\r\n";
            writer.Write(requestReceived);
            memoryStream.Position = 0;

            // act
            RequestContext request = new RequestContext(reader);

            // assert
            Assert.IsNotNull(request);
            Assert.AreEqual(request.Request, "GET");
            Assert.AreEqual(request.Path, "/messages");
            Assert.AreEqual(request.Version, "HTTP/1.1");
            Assert.AreEqual(request.Host, "localhost:8000");
            Assert.AreEqual(request.UserAgent, "curl/7.55.1");
            Assert.AreEqual(request.Accept, "*/*");
        }

        [Test]
        public void TestRequestContext_Constructor_POST()
        {
            // arrange
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream) { AutoFlush = true };
            var reader = new StreamReader(memoryStream);

            var requestReceived = "POST /messages HTTP/1.1\r\n" +
                                  "Host: localhost:8000\r\n"+
                                  "User-Agent: curl/7.55.1\r\n"+
                                  "Accept: */*\r\n"+
                                  "Content-Length: 5\r\n"+
                                  "Content-Type: text/plain\r\n"+
                                  "\r\n"+
                                  "Abcde\r\n";
            writer.Write(requestReceived);
            memoryStream.Position = 0;

            // act
            RequestContext request = new RequestContext(reader);

            // assert
            Assert.IsNotNull(request);
            Assert.AreEqual(request.Request, "POST");
            Assert.AreEqual(request.Path, "/messages");
            Assert.AreEqual(request.Version, "HTTP/1.1");
            Assert.AreEqual(request.Host, "localhost:8000");
            Assert.AreEqual(request.UserAgent, "curl/7.55.1");
            Assert.AreEqual(request.Accept, "*/*");
            Assert.AreEqual(request.ContentLength, 5);
            Assert.AreEqual(request.ContentType, "text/plain");
            Assert.AreEqual(request.Payload, "Abcde");

        }

        [Test]
        public void TestRequestContext_CheckRequest_CorrectRequest()
        {
            // arrange
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream) { AutoFlush = true };
            var reader = new StreamReader(memoryStream);

            var requestReceived = "GET /messages HTTP/1.1\r\n" +
                                  "Host: localhost:8000\r\n"+
                                  "User-Agent: curl/7.55.1\r\n"+
                                  "Accept: */*\r\n"+
                                  "\r\n";
            writer.Write(requestReceived);
            memoryStream.Position = 0;

            // act
            RequestContext request = new RequestContext(reader);
            var erg = request.CheckRequest();

            // assert
            Assert.IsTrue(erg);
        }

        [Test]
        public void TestRequestContext_GetOk()
        {
            // arrange
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream) { AutoFlush = true };
            var reader = new StreamReader(memoryStream);

            var requestReceived = "GET /messages HTTP/1.1\r\n" +
                                  "Host: localhost:8000\r\n"+
                                  "User-Agent: curl/7.55.1\r\n"+
                                  "Accept: */*\r\n"+
                                  "\r\n";
            writer.Write(requestReceived);
            memoryStream.Position = 0;

            // act
            RequestContext request = new RequestContext(reader);
            var erg = request.GetOk("Ok");
            
            // assert
            var exp = "HTTP/1.1 200 OK\r\n"+
                      "Server: MyServer\r\n"+
                      "Content-Type: text/plain\r\n"+
                      "Content-Length: 2\r\n"+
                      "\r\n"+
                      "Ok";
            Assert.AreEqual(exp, erg);
        }

        [Test]
        public void TestRequestContext_GetCreated()
        {
            // arrange
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream) { AutoFlush = true };
            var reader = new StreamReader(memoryStream);

            var requestReceived = "GET /messages HTTP/1.1\r\n" +
                                  "Host: localhost:8000\r\n"+
                                  "User-Agent: curl/7.55.1\r\n"+
                                  "Accept: */*\r\n"+
                                  "\r\n";
            writer.Write(requestReceived);
            memoryStream.Position = 0;

            // act
            RequestContext request = new RequestContext(reader);
            var erg = request.GetCreated("Created");
            
            // assert
            var exp = "HTTP/1.1 201 Created\r\n"+
                      "Server: MyServer\r\n"+
                      "Content-Type: text/plain\r\n"+
                      "Content-Length: 7\r\n"+
                      "\r\n"+
                      "Created";
            Assert.AreEqual(exp, erg);
        }

        [Test]
        public void TestRequestContext_GetNoContent()
        {
            // arrange
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream) { AutoFlush = true };
            var reader = new StreamReader(memoryStream);

            var requestReceived = "GET /messages HTTP/1.1\r\n" +
                                  "Host: localhost:8000\r\n"+
                                  "User-Agent: curl/7.55.1\r\n"+
                                  "Accept: */*\r\n"+
                                  "\r\n";
            writer.Write(requestReceived);
            memoryStream.Position = 0;

            // act
            RequestContext request = new RequestContext(reader);
            var erg = request.GetNoContent("No content");
            
            // assert
            var exp = "HTTP/1.1 204 No Content\r\n"+
                      "Server: MyServer\r\n"+
                      "Content-Type: text/plain\r\n"+
                      "Content-Length: 10\r\n"+
                      "\r\n"+
                      "No content";
            Assert.AreEqual(exp, erg);
        }

        [Test]
        public void TestRequestContext_GetBadRequest()
        {
            // arrange
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream) { AutoFlush = true };
            var reader = new StreamReader(memoryStream);

            var requestReceived = "GET /messages HTTP/1.1\r\n" +
                                  "Host: localhost:8000\r\n"+
                                  "User-Agent: curl/7.55.1\r\n"+
                                  "Accept: */*\r\n"+
                                  "\r\n";
            writer.Write(requestReceived);
            memoryStream.Position = 0;

            // act
            RequestContext request = new RequestContext(reader);
            var erg = request.GetBadRequest("Bad request");
            
            // assert
            var exp = "HTTP/1.1 400 Bad Request\r\n"+
                      "Server: MyServer\r\n"+
                      "Content-Type: text/plain\r\n"+
                      "Content-Length: 11\r\n"+
                      "\r\n"+
                      "Bad request";
            Assert.AreEqual(exp, erg);
        }

        [Test]
        public void TestRequestContext_GetNotFound()
        {
            // arrange
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream) { AutoFlush = true };
            var reader = new StreamReader(memoryStream);

            var requestReceived = "GET /messages HTTP/1.1\r\n" +
                                  "Host: localhost:8000\r\n"+
                                  "User-Agent: curl/7.55.1\r\n"+
                                  "Accept: */*\r\n"+
                                  "\r\n";
            writer.Write(requestReceived);
            memoryStream.Position = 0;

            // act
            RequestContext request = new RequestContext(reader);
            var erg = request.GetNotFound("404");
            
            // assert
            var exp = "HTTP/1.1 404 Not Found\r\n"+
                      "Server: MyServer\r\n"+
                      "Content-Type: text/plain\r\n"+
                      "Content-Length: 3\r\n"+
                      "\r\n"+
                      "404";
            Assert.AreEqual(exp, erg);
        }
    }
}