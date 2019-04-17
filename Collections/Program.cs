using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Collections
{
	class Program
	{
		static void Main(string[] args)
		{

			List<Person> people = new List<Person>()
			{
				new Person("Homer", "Simpson", 47),
				new Person("Marge", "Simpson", 45),
				new Person("Lisa", "Simpson", 9),
				new Person("Bart", "Simpson", 8)
			};

			ObservableCollection<Person> obsPeople = new ObservableCollection<Person>() { people[0], people[1] };
			obsPeople.CollectionChanged += people_CollectionChanged; // Subscrive on event

			obsPeople.Insert(obsPeople.Count - 1, people[2]);
			obsPeople.Remove(obsPeople[2]);
		}

		private static void people_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			Console.Write($"Action: {e.Action}");
			if(e.Action==NotifyCollectionChangedAction.Add)
				Console.WriteLine($": {e.NewItems[e.NewStartingIndex-1]}");
			else if(e.Action==NotifyCollectionChangedAction.Remove)
			{
				foreach(var p in e.OldItems)
					Console.WriteLine(p);
			}
				
		}
	}

	internal class SortPeopleByAge : IComparer<Person>
	{
		public int Compare(Person x, Person y)
		{
			if (x?.Age > y?.Age)
				return 1;
			else if (x?.Age < y?.Age)
				return -1;
			else
				return 0;
		}
	}

	internal class Person
	{
		public Int32 Age { get; set; }
		public String FirstName { get; set; }
		public String LastName { get; set; }

		public Person()
		{

		}

		public Person(String firstName, String lastName, Int32 age)
		{
			FirstName = firstName;
			LastName = lastName;
			Age = age;
		}

		public override String ToString()
		{
			return $"Name: {FirstName} {LastName}, Age: {Age}";
		}
	}
}
