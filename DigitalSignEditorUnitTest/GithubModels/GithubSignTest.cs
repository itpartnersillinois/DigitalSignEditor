using System.Collections.Generic;
using DigitalSignEditor.Data.Models;
using DigitalSignEditor.GithubModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DigitalSignEditorUnitTest.GithubModels {

    [TestClass]
    public class GithubSignTest {

        [TestMethod]
        public void Constructor_Basic_Validate() {
            //Arrange
            var sign = new Sign {
                Url = "test",
                Name = "test name",
                TwitterHandle = "twitter",
                Slides = new List<Slide> {
                    new Slide {
                        Url = "testurl",
                        Name = "testslidename",
                        Option = SlideType.Image,
                        IsActive = true,
                        StartDate = null,
                        EndDate = null
                    },
                    new Slide {
                        Url = "testurl2",
                        Name = "testslidename2",
                        Option = SlideType.Weather,
                        IsActive = true,
                        StartDate = null,
                        EndDate = null
                    }
                }
            };

            //Act
            var githubSign = new GithubSign(sign);

            //Assert
            Assert.AreEqual("test", githubSign.Url);
            Assert.AreEqual("test name", githubSign.Title);
            Assert.AreEqual(2, githubSign.Slides.Count);
            var jsonExpected = "{\"college\":\"college_of_education\",\"slides\":[{\"filename\":\"testurl\",\"type\":\"image\"},{\"filename\":\"testurl2\",\"type\":\"weather\"}],\"title\":\"test name\",\"twitter\":\"twitter\",\"url\":\"test\"}";
            Assert.AreEqual(jsonExpected, JsonConvert.SerializeObject(githubSign));
        }

        [TestMethod]
        public void Constructor_NoSlidesNoTwitter_Validate() {
            //Arrange
            var sign = new Sign {
                Url = "test",
                Name = "test name",
                Slides = null,
                TwitterHandle = null
            };

            //Act
            var githubSign = new GithubSign(sign);

            //Assert
            Assert.AreEqual("test", githubSign.Url);
            Assert.AreEqual("test name", githubSign.Title);
            Assert.AreEqual(0, githubSign.Slides.Count);
            var jsonExpected = "{\"college\":\"college_of_education\",\"slides\":[],\"title\":\"test name\",\"twitter\":\"\",\"url\":\"test\"}";
            Assert.AreEqual(jsonExpected, JsonConvert.SerializeObject(githubSign));
        }
    }
}