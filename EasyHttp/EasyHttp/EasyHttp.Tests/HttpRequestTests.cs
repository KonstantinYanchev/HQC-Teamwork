using System;
using System.Collections.Generic;
using System.Net;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using EasyHttp.Codecs;
using EasyHttp.Codecs.JsonFXExtensions;
using EasyHttp.Configuration;
using EasyHttp.Contracts;
using EasyHttp.Http;

namespace EasyHttp.Tests
{
    [TestClass]
    public class HttpRequestTests
    {
        [TestMethod]
        public void CheckIfExtraHeaderIsAddedWhenValueIsNull_RawHeadersCountShouldBeZero()
        {
            IEncoderDecoderConfiguration configuration = new DefaultEncoderDecoderConfiguration();
            IEncoder encoder = configuration.GetEncoder();
            HttpRequest httpRequest = new HttpRequest(encoder);

            httpRequest.AddExtraHeader("header", null);

            Assert.AreEqual(0, httpRequest.RawHeaders.Count);
        }

        [TestMethod]
        public void CheckIfExtraHeaderIsAddedWithAddExtraHeaderMethod_RawHeadersCountShouldBeOne()
        {
            IEncoderDecoderConfiguration configuration = new DefaultEncoderDecoderConfiguration();
            IEncoder encoder = configuration.GetEncoder();
            HttpRequest httpRequest = new HttpRequest(encoder);

            httpRequest.AddExtraHeader("header", "value");

            Assert.AreEqual(1, httpRequest.RawHeaders.Count);
        }

        [TestMethod]
        public void CheckIfOnlyDistinctHeadersAreAddedWithAddExtraHeaderMethod_RawHeadersCountShouldBeOne()
        {
            IEncoderDecoderConfiguration configuration = new DefaultEncoderDecoderConfiguration();
            IEncoder encoder = configuration.GetEncoder();
            HttpRequest httpRequest = new HttpRequest(encoder);

            httpRequest.AddExtraHeader("header", "value");
            httpRequest.AddExtraHeader("header", "other value");

            Assert.AreEqual(1, httpRequest.RawHeaders.Count);
        }
    }
}
