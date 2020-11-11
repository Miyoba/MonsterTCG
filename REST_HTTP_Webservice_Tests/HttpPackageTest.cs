using NUnit.Framework;
using REST_HTTP_Webservice;

namespace REST_HTTP_Webservice_Tests
{
    public class HttpPackageTest
    {
        [Test]
        public void TestHttpPackage_Constructor_GET()
        {
            // arrange
            string testText = "GET / HTTP/1.1" +
                              "Host: localhost:8000"+
                              "User-Agent: curl/7.55.1"+
                              "Accept: */*"+
                              "";
            // act
            HttpPackage request = new HttpPackage(testText);

            // assert
            Assert.IsNotNull(request);
            Assert.AreEqual(request.Request, "GET");
            Assert.AreEqual(request.Path, "/");
            Assert.AreEqual(request.Version, "HTTP/1.1");
            Assert.AreEqual(request.Host, "localhost:8000");
            Assert.AreEqual(request.UserAgent, "curl/7.55.1");
            Assert.AreEqual(request.Accept, "*/*");
        }

        [Test]
        public void TestHttpPackage_Constructor_POST()
        {
            // arrange
            string testText = "POST /message HTTP/1.1" +
                              "Host: localhost:8000"+
                              "User-Agent: curl/7.55.1"+
                              "Accept: */*"+
                              "Content-Length: 5"+
                              "Content-Type: text/plain"+
                              "";
            // act
            HttpPackage request = new HttpPackage(testText);
            request.Payload = "Abcde";

            // assert
            Assert.IsNotNull(request);
            Assert.AreEqual(request.Request, "POST");
            Assert.AreEqual(request.Path, "/message");
            Assert.AreEqual(request.Version, "HTTP/1.1");
            Assert.AreEqual(request.Host, "localhost:8000");
            Assert.AreEqual(request.UserAgent, "curl/7.55.1");
            Assert.AreEqual(request.Accept, "*/*");
            Assert.AreEqual(request.ContentLength, 5);
            Assert.AreEqual(request.ContentType, "text/plain");
            Assert.AreEqual(request.Payload, "Abcde");
        }

        [Test]
        public void TestHttpPackage_GetOk()
        {
            //TODO
            // arrange
            
            // act
            
            // assert
            
            Assert.Pass();
        }

        [Test]
        public void TestHttpPackage_GetCreated()
        {
            //TODO
            // arrange
            
            // act
            
            // assert
            
            Assert.Pass();
        }

        [Test]
        public void TestHttpPackage_GetNoContent()
        {
            //TODO
            // arrange
            
            // act
            
            // assert
            
            Assert.Pass();
        }

        [Test]
        public void TestHttpPackage_GetBadRequest()
        {
            //TODO
            // arrange
            
            // act
            
            // assert
            
            Assert.Pass();
        }

        [Test]
        public void TestHttpPackage_GetNotFound()
        {
            //TODO
            // arrange
            
            // act
            
            // assert
            
            Assert.Pass();
        }
    }
}