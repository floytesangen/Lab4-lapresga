using System;
using NUnit.Framework;
using Expedia;
using Rhino.Mocks;

namespace ExpediaTest
{
	[TestFixture()]
	public class CarTest
	{	
		private Car targetCar;
		private MockRepository mocks;
		
		[SetUp()]
		public void SetUp()
		{
			targetCar = new Car(5);
			mocks = new MockRepository();
		}
		
		[Test()]
		public void TestThatCarInitializes()
		{
			Assert.IsNotNull(targetCar);
		}	
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForFiveDays()
		{
			Assert.AreEqual(50, targetCar.getBasePrice()	);
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForTenDays()
		{
            var target = new Car(10);
			Assert.AreEqual(80, target.getBasePrice());	
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForSevenDays()
		{
			var target = new Car(7);
			Assert.AreEqual(10*7*.8, target.getBasePrice());
		}
		
		[Test()]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestThatCarThrowsOnBadLength()
		{
			new Car(-5);
		}

        [Test()]
        public void TestThatCarDoesGetCarLocationFromTheDatabase()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();

            String carLocation = "there";
            String anotherCarLocation = "everywhere";

            using (mocks.Record())
            {
                mockDatabase.getCarLocation(17);
                LastCall.Return(carLocation);
                mockDatabase.getCarLocation(5);
                LastCall.Return(anotherCarLocation);
            }
            var target = new Car(10);
            target.Database = mockDatabase;

            String result;

            result = target.getCarLocation(5);
            Assert.AreEqual(result, anotherCarLocation);

            result = target.getCarLocation(17);
            Assert.AreEqual(result, carLocation);
        }

        [Test()]
        public void TestThatCarDoesGetMileageFromDatabase()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();
            int mileage = 100;
            mockDatabase.Miles = mileage;
            var target = new Car(10);
            target.Database = mockDatabase;
            int miles = target.Mileage;
            Assert.AreEqual(miles, mileage);
        }

        [Test()]
        public void TestThatObjectMother()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();
            int mileage = 100;
            mockDatabase.Miles = mileage;
            var target = ObjectMother.BMW();
            target.Database = mockDatabase;
            int miles = target.Mileage;
            String name = target.Name;
            Assert.AreEqual(miles, mileage);
            Assert.AreEqual("BMW Z4 Roadster", name);
        }
	}
}
