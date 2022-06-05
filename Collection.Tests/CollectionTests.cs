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

        [Test]
        public void Test_Collections_RemoveAtStart()
        {
            //Arange
            var names = new Collection<string>("Peter", "Maria", "Steve", "Mia");

            //Act
            var removed = names.RemoveAt(0);

            //Assert
            Assert.That(removed, Is.EqualTo("Peter"));
            Assert.That(names.ToString(), Is.EqualTo("[Maria, Steve, Mia]"));
        }

        [Test]
        public void Test_Collections_RemoveAtMiddle()
        {
            //Arange
            var names = new Collection<string>("Peter", "Maria", "Steve", "Mia");

            //Act
            var removed = names.RemoveAt(2);

            //Assert
            Assert.That(removed, Is.EqualTo("Steve"));
            Assert.That(names.ToString(), Is.EqualTo("[Peter, Maria, Mia]"));
        }

        [Test]
        public void Test_Collections_RemoveAtEnd()
        {
            //Arange
            var names = new Collection<string>("Peter", "Maria", "Steve", "Mia");

            //Act
            var removed = names.RemoveAt(3);

            //Assert
            Assert.That(removed, Is.EqualTo("Mia"));
            Assert.That(names.ToString(), Is.EqualTo("[Peter, Maria, Steve]"));
        }

        [Test]
        public void Test_Collection_AddWithGrow()
        {
            //Arrange
            var nums = new Collection<int>();
            var oldCapacity = nums.Capacity;

            //Act
            for (int i = 0; i <= 10; i++)
            {
                nums.Add(i);
            }
            string expectedNums = string.Join(", ", nums);

            //Assert
            Assert.That(nums.Capacity, Is.GreaterThanOrEqualTo(nums.Count));
            Assert.That(nums.Capacity, Is.GreaterThanOrEqualTo(oldCapacity));
            Assert.That(nums.ToString(), Is.EqualTo(expectedNums));

        }

        [Test]
        public void Test_Collection_CountAndCapacity()
        {
            //Arrange
            var nums = new Collection<int>();

            //Act
            for (int i = 0; i < 100; i++)
            {
                nums.Add(i);
            }

            //Assert
            Assert.That(nums.Count==100);
            Assert.That(nums.Capacity, Is.GreaterThanOrEqualTo(nums.Count));


        }

        [Test]
        public void Test_Collection_ExchangeFirstLast()
        {
            //Arragne
            var nums = new Collection<int>(1, 2, 3, 4);

            //Act
            nums.Exchange(0, 3);

            //Assert
            Assert.That(nums.ToString, Is.EqualTo("[4, 2, 3, 1]"));
        }

        [Test]
        public void Test_Collection_ExchangeInvalidIndex()
        {
            //Arrange
            var nums = new Collection<int>(1, 2, 3, 4);

            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => nums.Exchange(0,5));
        }

        [Test]
        public void Test_Collection_ExchangeMiddle()
        {
            //Arrange
            var nums = new Collection<int>(1, 2, 3, 4);

            //Act
            nums.Exchange(1, 2);

            //Assert

            Assert.That(nums.ToString(), Is.EqualTo("[1, 3, 2, 4]"));
        }

        [Test]
        public void Test_Collection_AddAtEnd()
        {
            //Arrange
            var nums = new Collection<int>(1, 2, 3, 4, 5, 6);

            //Act
            nums.InsertAt(6, 7);

            //Assert
            Assert.That(nums.ToString, Is.EqualTo("[1, 2, 3, 4, 5, 6, 7]")); ;
        }

        [Test]
        public void Test_Collection_InsertByInvalidIndex()
        {
            //Arrange
            var names = new Collection<string>("Vasil", "Boryana");

            //Assert
            Assert.That(() => names.InsertAt(-1, "Peter"),
                Throws.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(() => names.InsertAt(4, "Matea"),
                Throws.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(names.ToString(), Is.EqualTo("[Vasil, Boryana]"));
        }
        [Test]
        public void Test_Collection_InstertWithGrow()
        {
            //Arrange
            var nums = new Collection<int>();
            int oldCpacity = nums.Capacity;

            //Act
            for (int i = 0; i <= 4; i++)
            {
                nums.InsertAt(i, i+1);
            }

            //Assert
            Assert.That(nums.Capacity, Is.GreaterThanOrEqualTo(oldCpacity));
            Assert.That(nums.ToString(), Is.EqualTo("[1, 2, 3, 4, 5]")) ;
            Assert.That(nums.Capacity, Is.GreaterThanOrEqualTo(nums.Count));
        }
        [Test]
        public void Test_Collection_RemoveAll()
        {
            //Arrange
            var nums = new Collection<int>(1, 2, 3, 4, 5);

            //Act
            for (int i = 0; i <= 4; i++)
            {
                nums.RemoveAt(0);
            }

            //Assert
            Assert.That(nums.Count ==0);
        }

        [Test]
        public void Test_Collection_RemoveAtEnd()
        {
            //Arrange
            var nums = new Collection<int>(1, 2, 3, 4, 5, 6);

            //Act
            nums.RemoveAt(5);

            //Assert
            Assert.That(nums.ToString(), Is.EqualTo("[1, 2, 3, 4, 5]"));
        }

        [Test]
        public void Test_Collection_RemoveAtMiddle()
        {
            //Arrange
            var nums = new Collection<int>(1, 2, 3, 4, 5, 6);

            //Act
            nums.RemoveAt(2);

            //Assert
            Assert.That(nums.ToString(), Is.EqualTo("[1, 2, 4, 5, 6]"));
        }

        [Test]
        public void Test_Collection_RemoveAtStart()
        {
            //Arrange
            var nums = new Collection<int>(1, 2, 3, 4, 5, 6);

            //Act
            nums.RemoveAt(0);

            //Assert
            Assert.That(nums.ToString(), Is.EqualTo("[2, 3, 4, 5, 6]"));
        }

        [Test]
        public void Test_Collection_RemoveByInvalidIndex()
        {
            //Arrange
            var nums = new Collection<int>(1, 2, 3, 4, 5);

            //Assert
            Assert.That(() => nums.RemoveAt(-1), 
                Throws.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(() => nums.RemoveAt(5),
                Throws.InstanceOf<ArgumentOutOfRangeException>());

        }

        [Test]
        public void Test_Collection_SetByIndex()
        {
            // Arrange
            var nums = new Collection<int>(1, 2, 3, 4, 5);

            // Act
            nums[3] = 10;

            // Assert
            Assert.That(nums.ToString(), Is.EqualTo("[1, 2, 3, 10, 5]"));
        }


        [Test]
        public void Test_Collection_SetByInvalidIndex()
        {
            // Arrange
            var nums = new Collection<int>(1, 2, 3, 4, 5);

            // Assert
            Assert.That(() => { nums[-1] = 10; }, 
                Throws.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(() => { nums[6] = 10; }, 
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Test_Collection_ToStringCollectionOfCollections()
        {
            // Arrange
            var firstNums = new Collection<int>(1, 2);
            var secondNums = new Collection<int>(3, 4);
            var allNums = new Collection<object>(firstNums, secondNums);

            // Assert
            Assert.That(allNums.ToString(), Is.EqualTo("[[1, 2], [3, 4]]"));
        }

        [Test]
        public void Test_Collection_ToStringEmpty()
        {
            // Arrange
            var nums = new Collection<string>(String.Empty);

            // Assert
            Assert.That(nums.Count == 1);
            Assert.That(nums.ToString(), Is.EqualTo("[]"));
        }

        [Test]
        public void Test_Collection_ToStringSingle()
        {
            // Arrange
            var names = new Collection<string>("Tatyana");

            // Assert
            Assert.That(names.ToString(), Is.EqualTo("[Tatyana]"));
        }
    }
}