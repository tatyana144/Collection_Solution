using Collections;
using NUnit.Framework;
using System;
using System.Linq;

namespace Collection.Tests
{
    public class CollectionTests
    {

        [Test]
        public void Test_Collection_EmptyConstructor()
        {
            //Arange
            var nums = new Collection<int>();

            //Act

            //Assert
            Assert.AreEqual(0, nums.Count, "Count property");
            Assert.AreEqual(16, nums.Capacity, "Capacity property");
            Assert.That(nums.ToString(), Is.EqualTo( "[]"));
        }

        [Test]
        public void Test_Collection_ConstructorSingleItem()
        {
            //Arange
            var nums = new Collection<int>(5);

            //Act

            //Assert
            Assert.That(nums.ToString(), Is.EqualTo("[5]"));
        }
         
        [Test]
        public void Test_Collection_Add()
        {
            //Arange
            var nums = new Collection<int>(5);

            //Act
            nums.Add(6);

            //Assert
            Assert.That(nums.ToString(), Is.EqualTo("[5, 6]"));
        }

        [Test]
        public void Test_ConstructorMultipleItems()
        {
            //Arange
            var nums = new Collection<int>(5, 6);

            //Act

            //Assert
            Assert.That(nums.ToString(), Is.EqualTo("[5, 6]"));
        }
        [Test]
        public void Test_AddRange()
        {
            //Arange
            var nums = new Collection<int>();
            int oldCapicity = nums.Capacity;

            //Act
            var newNums = Enumerable.Range(1000, 2000).ToArray();
            nums.AddRange(newNums);
            //Assert
            string expectedNums = "[" + string.Join(", ", newNums) + "]";
            Assert.That(nums.ToString(), Is.EqualTo(expectedNums));
        }

        [Test]
        public void Test_AddRangeWithGrow()
        {
            //Arange
            var nums = new Collection<int>();
            int oldCapicity = nums.Capacity;

            //Act
            var newNums = Enumerable.Range(1000, 2000).ToArray();
            nums.AddRange(newNums);
            //Assert
            Assert.That(nums.Capacity, Is.GreaterThanOrEqualTo(oldCapicity));
            Assert.That(nums.Capacity, Is.GreaterThanOrEqualTo(nums.Count));
        }

        [Test]
        public void Test_GetByIndex()
        {
            //Arange
            var names = new Collection<string>("Peter", "Maria");

            //Act
            var item0 = names[0];
            var item1 = names[1];

            //Assert
            Assert.That(item0, Is.EqualTo("Peter"));
            Assert.That(item1, Is.EqualTo("Maria"));
        }

        [Test]
        public void Test_Collection_GetByInvalisIndex()
        {
            //Arange
            var names = new Collection<string>("Bob", "Joe");

            //Act


            //Assert
            Assert.That(() => { var name = names[-1]; },
                Throws.InstanceOf<System.ArgumentOutOfRangeException>());
            Assert.That(() => { var name = names[2]; },
                Throws.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(() => { var name = names[500]; },
                Throws.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(names.ToString(), Is.EqualTo("[Bob, Joe]"));
        }

        [Test]
        public void Test_collection_ToStringNestedCollections()
        {
            var names = new Collection<object>("Teddy", "Gerry");
            var nums = new Collection<int>(10, 20);
            var dates = new Collection<DateTime>();

            var nested = new Collection<object>(names, nums, dates);
            string nestedToString = nested.ToString();

            Assert.That(nestedToString, Is.EqualTo("[[Teddy, Gerry], [10, 20], []]"));
        }
        
        [Test]
        [Timeout(1000)]
        public void Test_Collection_1MillionItems()
        {
            //Arange
            const int itemsCount = 1000000;
            var nums = new Collection<int>();

            //Act
            nums.AddRange(Enumerable.Range(1, itemsCount).ToArray());

            //Assert
            Assert.That(nums.Count == itemsCount);
            Assert.That(nums.Capacity >= nums.Count);

            for (int i = itemsCount - 1; i >= 0; i--)
            {
                nums.RemoveAt(i);
            }

            Assert.That(nums.ToString() == "[]");
            Assert.That(nums.Capacity >= nums.Count);

        }

        [TestCase("Peter" , 0, "Peter")]
        [TestCase("Peter, Maria, Vasil", 1, "Maria")]
        [TestCase("Yasen, Maria, George", 2, "George")]
        public void Test_Collection_GetByValidIndex(string data, int index, string expectedValue)
        {
            //Arrange
            var nums = new Collection<string>(data.Split(", "));

            //Act
            var actual = nums[index];

            //Assert
            Assert.AreEqual(expectedValue, actual);
        }

        [TestCase("", 0)]
        [TestCase("Peter", -1)]
        [TestCase("Peter", 1)]
        [TestCase("Peter, Maria, Steve", -1)]
        [TestCase("Peter, Maria, Steve", 3)]
        public void Test_Collection_GetByInvalidIndex(string data, int index)
        {
            var items = new Collection<string>(data.Split(",", 
                StringSplitOptions.RemoveEmptyEntries));

            Assert.That(() => items[index], 
                Throws.TypeOf<ArgumentOutOfRangeException>
               () );
        }


    }
}