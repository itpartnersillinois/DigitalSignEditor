using System;
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
                        DisplayUrl = "test full url",
                        DisplayUrlCompressed = "test full compressed url",
                        Option = SlideType.Image,
                        IsActive = true,
                        StartDate = null,
                        EndDate = null
                    },
                    new Slide {
                        Url = "testurl2",
                        Name = "testslidename2",
                        DisplayUrl = "test full url 2",
                        DisplayUrlCompressed = "test full compressed url 2",
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
            var jsonExpected = "{\"college\":\"college_of_education\",\"slides\":[{\"data\":\"\",\"filename\":\"testurl\",\"filenameCompressed\":\"test full compressed url\",\"filenameFull\":\"test full url\",\"title\":\"testslidename\",\"type\":\"image\"},{\"data\":\"\",\"filename\":\"testurl2\",\"filenameCompressed\":\"test full compressed url 2\",\"filenameFull\":\"test full url 2\",\"title\":\"testslidename2\",\"type\":\"weather\"}],\"title\":\"test name\",\"twitter\":\"twitter\",\"url\":\"test\"}";
            Assert.AreEqual(jsonExpected, JsonConvert.SerializeObject(githubSign));
        }

        [TestMethod]
        public void Constructor_Basic_CheckDates() {
            //Arrange
            var sign = new Sign {
                Url = "test",
                Name = "test name",
                TwitterHandle = "twitter",
                Slides = new List<Slide> {
                    new Slide {
                        Url = "testurl",
                        Name = "testslidename",
                        DisplayUrl = "test full url",
                        DisplayUrlCompressed = "test full compressed url",
                        Option = SlideType.Image,
                        IsActive = true,
                        StartDate = null,
                        EndDate = null
                    },
                    new Slide {
                        Url = "testurl2",
                        Name = "testslidename2",
                        DisplayUrl = "test full url 2",
                        DisplayUrlCompressed = "test full compressed url 2",
                        Option = SlideType.Weather,
                        IsActive = true,
                        StartDate = DateTime.Now.AddDays(-1),
                        EndDate = DateTime.Now.AddDays(1)
                    }
                }
            };

            //Act
            var githubSign = new GithubSign(sign);

            //Assert
            Assert.AreEqual("test", githubSign.Url);
            Assert.AreEqual("test name", githubSign.Title);
            Assert.AreEqual(2, githubSign.Slides.Count);
            var jsonExpected = "{\"college\":\"college_of_education\",\"slides\":[{\"data\":\"\",\"filename\":\"testurl\",\"filenameCompressed\":\"test full compressed url\",\"filenameFull\":\"test full url\",\"title\":\"testslidename\",\"type\":\"image\"},{\"data\":\"\",\"filename\":\"testurl2\",\"filenameCompressed\":\"test full compressed url 2\",\"filenameFull\":\"test full url 2\",\"title\":\"testslidename2\",\"type\":\"weather\"}],\"title\":\"test name\",\"twitter\":\"twitter\",\"url\":\"test\"}";
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

        [TestMethod]
        public void Constructor_CalendarSlideNoTwitter_Validate() {
            //Arrange
            var sign = new Sign {
                Url = "test",
                Name = "test name",
                TwitterHandle = null,
                Slides = new List<Slide> {
                    new Slide {
                        Url = "",
                        Data = "2",
                        Name = "testcalendar",
                        DisplayUrl = "",
                        DisplayUrlCompressed = "",
                        Option = SlideType.Calendar,
                        IsActive = true,
                        StartDate = null,
                        EndDate = null
                    },
                    new Slide {
                        Url = "testurl2",
                        Name = "testslidename2",
                        DisplayUrl = "test full url 2",
                        DisplayUrlCompressed = "test full compressed url 2",
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
            var jsonExpected = "{\"college\":\"college_of_education\",\"slides\":[{\"data\":\"2\",\"filename\":\"\",\"filenameCompressed\":\"\",\"filenameFull\":\"\",\"title\":\"testcalendar\",\"type\":\"calendar\"},{\"data\":\"\",\"filename\":\"testurl2\",\"filenameCompressed\":\"test full compressed url 2\",\"filenameFull\":\"test full url 2\",\"title\":\"testslidename2\",\"type\":\"weather\"}],\"title\":\"test name\",\"twitter\":\"\",\"url\":\"test\"}";
            Assert.AreEqual(jsonExpected, JsonConvert.SerializeObject(githubSign));
        }
    }
}