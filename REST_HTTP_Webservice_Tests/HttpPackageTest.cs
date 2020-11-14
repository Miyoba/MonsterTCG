using System.Collections.Generic;
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
            var testText = new List<string>();
            testText.Add("GET / HTTP/1.1");
            testText.Add("Host: localhost:8000");
            testText.Add("User-Agent: curl/7.55.1");
            testText.Add("Accept: */*");
            testText.Add("");

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
            var testText = new List<string>();
            testText.Add("POST /messages HTTP/1.1");
            testText.Add("Host: localhost:8000");
            testText.Add("User-Agent: curl/7.55.1");
            testText.Add("Accept: */*");
            testText.Add("Content-Length: 5");
            testText.Add("Content-Type: text/plain");
            testText.Add("");

            // act
            HttpPackage request = new HttpPackage(testText);
            request.Payload = "Abcde";

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
        public void TestHttpPackage_GetInfo()
        {
            //TODO
            // arrange
            var testText = new List<string>();
            testText.Add("POST /messages HTTP/1.1");
            testText.Add("Host: localhost:8000/messages/1");
            testText.Add("User-Agent: curl/7.55.1");
            testText.Add("Accept: */*");
            testText.Add("Content-Length: 5");
            testText.Add("Content-Type: text/plain");
            testText.Add("");

            // act
            HttpPackage request = new HttpPackage(testText);
            request.Payload = "Abcde";
            var dict = request.GetInfo();

            // assert
            Assert.IsTrue(dict.ContainsKey("Request"));
            Assert.AreEqual("POST",dict["Request"]);
            Assert.IsTrue(dict.ContainsKey("Path"));
            Assert.AreEqual("/messages",dict["Path"]);
            Assert.IsTrue(dict.ContainsKey("Version"));
            Assert.AreEqual("HTTP/1.1",dict["Version"]);
            Assert.IsTrue(dict.ContainsKey("UserAgent"));
            Assert.AreEqual("curl/7.55.1",dict["UserAgent"]);
            Assert.IsTrue(dict.ContainsKey("Accept"));
            Assert.AreEqual("*/*",dict["Accept"]);
            Assert.IsTrue(dict.ContainsKey("ContentLength"));
            Assert.AreEqual("5",dict["ContentLength"]);
            Assert.IsTrue(dict.ContainsKey("ContentType"));
            Assert.AreEqual("text/plain",dict["ContentType"]);
            Assert.IsTrue(dict.ContainsKey("Payload"));
            Assert.AreEqual("Abcde",dict["Payload"]);
        }

        [Test]
        public void TestHttpPackage_CheckRequest_CorrectRequest()
        {
            //TODO
            // arrange
            var testText = new List<string>();
            testText.Add("POST /messages HTTP/1.1");
            testText.Add("Host: localhost:8000/messages/1");
            testText.Add("User-Agent: curl/7.55.1");
            testText.Add("Accept: */*");
            testText.Add("Content-Length: 5");
            testText.Add("Content-Type: text/plain");
            testText.Add("");
            
            // act
            HttpPackage request = new HttpPackage(testText);
            request.Payload = "Abcde";
            var erg = request.CheckRequest();

            // assert
            Assert.IsTrue(erg);
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