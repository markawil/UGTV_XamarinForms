using NUnit.Framework;
using System;
namespace UGTVForms.Tests
{
    [TestFixture()]
    public class Test
    {
        [Test()]
        public void ShouldReturnMax()
        {
            var values = new int[] { 1, 2, 6, 10, 2, 8, 22, 1 };
            Assert.AreEqual(22, MaxValue(values, 0, -99));
        }

        int MaxValue(int[] numbers, int index, int maxValue)
        {            
            if (index == numbers.Length - 1)
            {
                return maxValue;
            }

            var localMaxValue = maxValue;
           
            if (numbers[index] > maxValue)
            {
                localMaxValue = numbers[index];
            }
            
            var localIndex = index + 1; 
                       
            return MaxValue(numbers, localIndex, localMaxValue);            
        }
    }    
        
}
