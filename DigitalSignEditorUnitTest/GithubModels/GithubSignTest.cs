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
            var jsonExpected = "{\"College\":\"college_of_education\",\"Slides\":[{\"Filename\":\"testurl\",\"Type\":\"image\"},{\"Filename\":\"testurl2\",\"Type\":\"weather\"}],\"Title\":\"test name\",\"Twitter\":\"\",\"Url\":\"test\"}";
            Assert.AreEqual(jsonExpected, JsonConvert.SerializeObject(githubSign));
        }
    }
}