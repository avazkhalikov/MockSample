using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace UnitTestMock
{
    public interface ILoveThisLibrary
    {
        bool DownloadExists(string v);
    }

    public interface IInfo
    {
        string name { get; }
        int healthy { get; }
        string legs { get; set; }
    }

    public class Animal
    {
        private readonly IInfo _info;
        public string legs() => _info.legs;
        public string name() => _info.name;
        public int health() => _info.healthy;
        public Animal(IInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }
            _info = info;
        }
    }

    public class Information : IInfo
    {
        public string name { get; set; }
        public int healthy { get => healthyRand(); }
        public string legs { get; set; }

        public int healthyRand()
        {
            var rand = new Random();

            return rand.Next(10);
        }

    }

    [TestClass]
    public class UnitTest1
    {

        //code from https://github.com/moq/moq4 
        [TestMethod]
        public void TestMethodFromGitHub()
        {
            var mock = new Mock<ILoveThisLibrary>();

            // WOW! No record/replay weirdness?! :)
            mock.Setup(library => library.DownloadExists("2.0.0.0"))
                .Returns(true);

            // Use the Object property on the mock to get a reference to the object
            // implementing ILoveThisLibrary, and then exercise it by calling
            // methods on it
            ILoveThisLibrary lovable = mock.Object;
            bool download = lovable.DownloadExists("2.0.0.0");

            // Verify that the given method was indeed called with the expected value at most once
            mock.Verify(library => library.DownloadExists("2.0.0.0"), Times.AtMostOnce());
        }

        [TestMethod]
        public void TestMethodAnimalOneInterface()
        {
            // var mockInfo = new Mock<Information>();
            
            //first we create IInterface based on architecture! Now all team members do their own job!
            //ALL You care is INPUT and OUTPUT, based on "IInternface"! NO LOGIC or Repository Dependency!
            //TeamWork!
            
            var mockInfo = new Mock<IInfo>();
            mockInfo.SetupAllProperties();
            mockInfo.SetupGet(p => p.name).Returns("jojo");

            mockInfo.SetupSequence(p => p.legs)
                .Returns("4")
                .Returns("1");

            
            var anim = new Animal(mockInfo.Object);

            // Assert.AreEqual(5, anim.health());
            Assert.AreEqual("4", anim.legs());
            Assert.AreEqual("jojo", anim.name()); 


            //good one!
            mockInfo.VerifyAll();
        }

        [TestMethod]
        public void TestMethodAnimalOneImplemented()
        {
             var mockInfo = new Mock<Information>();
            mockInfo.Object.legs = "4";
            mockInfo.Object.name = "bobo";
            mockInfo.Object.healthyRand();     
            
            var anim = new Animal(mockInfo.Object);   //accessing the Animal Object!

            //Assert.AreEqual("4", anim.Legs());
            //Assert.AreEqual("2", anim.Legs());
            Assert.AreEqual("13", anim.legs());

            Assert.AreEqual(5, anim.health());
            // Assert.AreEqual("jojo", anim.name());

        }

    }
}
