using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.BusinessLogic
{
	public static class WordGenerator
	{

		private static Random rnd = new Random();

		public static string CreateRandomWordNumberCombination()
		{
			string[] words = { "Bold", "Think", "Friend", "Pony", "Fall", "Easy", "apple", "mango", "papaya", "banana", "guava", "pineapple" };
			int randomNumber = rnd.Next(2000, 3000);
			string randomString = $"{words[rnd.Next(0, words.Length)]}{randomNumber}";

			return randomString;

		}
	}
}
