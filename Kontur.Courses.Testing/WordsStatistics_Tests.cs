using System;
using System.Linq;
using Kontur.Courses.Testing.Implementations;
using NUnit.Framework;

namespace Kontur.Courses.Testing
{
	public class WordsStatistics_Tests
	{
		public Func<IWordsStatistics> createStat = () => new WordsStatistics_CorrectImplementation(); // меняется на разные реализации при запуске exe
		public IWordsStatistics stat;

		[SetUp]
		public void SetUp()
		{
			stat = createStat();
		}

		[Test]
		public void should_be_empty_if_no_words()
		{
			CollectionAssert.IsEmpty(stat.GetStatistics());
		}

		[Test]
		public void should_return_one_tuple_if_used_single_word()
		{
			stat.AddWord("hello");
			CollectionAssert.AreEqual(new[] { Tuple.Create(1, "hello") }, stat.GetStatistics());
		}

        [Test]
        public void should_return_one_tuple_if_used_same_word_twice()
        {
            stat.AddWord("xxx");
            stat.AddWord("xxx");
            CollectionAssert.AreEqual(new[] { Tuple.Create(2, "xxx") }, stat.GetStatistics());
        }

		[Test]
		public void should_return_single_tuple_for_same_words()
		{
			stat.AddWord("hello");
			stat.AddWord("world");
			stat.AddWord("world");

			CollectionAssert.AreEqual(new[] { Tuple.Create(2, "world"), Tuple.Create(1, "hello") }, stat.GetStatistics());
		}

	    [Test]
	    public void should_return_one_tuple_if_used_empty_word()
	    {
            stat.AddWord("");
	        var s = stat.GetStatistics();
            CollectionAssert.AreNotEqual(new[] {Tuple.Create(1, "")}, stat.GetStatistics());
	    }

	    [Test]
	    public void should_return_one_tuple_if_used_two_same_words_in_different_registers()
	    {
            stat.AddWord("AAAA");
            stat.AddWord("aaaa");
            CollectionAssert.AreEqual(new[] {Tuple.Create(2, "aaaa")}, stat.GetStatistics());
	    }

        [Test]
	    public void should_return_tuples_sorted_alphabetically_if_used_same_number_of_words()
	    {
	        stat.AddWord("a");
            stat.AddWord("b");
            CollectionAssert.AreEqual((new[] {Tuple.Create(1, "a"), Tuple.Create(1, "b")}), stat.GetStatistics());
	    }

	    [Test]
	    public void should_be_empty_if_used_null_word()
	    {
            stat.AddWord(null);
            Assert.AreEqual(0, stat.GetStatistics().Count());

	    }

        [Test]
        public void should_return_one_tuple_if_difference_is_in_eleventh_symbol()
        {
            stat.AddWord("aaaaaaaaaaa");
            stat.AddWord("aaaaaaaaaab");
            CollectionAssert.AreEqual(new[] { Tuple.Create(2, "aaaaaaaaaa") }, stat.GetStatistics());
        }

        [Test]
        public void should_work_with_non_english_and_russian_letters()
        {
            stat.AddWord("Ä");
            CollectionAssert.AreEqual(new[] { Tuple.Create(1, "ä") }, stat.GetStatistics());
        }

	    [Test]
	    public void two_statistics_must_not_interfere()
	    {
	        var stat2 = createStat();
            stat.AddWord("a");
            Assert.AreEqual(0, stat2.GetStatistics().Count());
	    }

	    [Test, Timeout(2000)]
	    public void can_handle_big_number_of_words()
	    {
	        for (var i = 0; i < 1000; i++)
	        {

	            for (var j = 0; j < i; j++)
	            {
                    stat.AddWord(i.ToString());
	            }
	        }

	        var s = stat.GetStatistics().ToList();

            for (var i = 0; i < 1000-2; i++)
                Assert.GreaterOrEqual(s[i].Item1, s[i+1].Item1);
	    }

        [Test]
        public void groups_check()
        {
            stat.AddWord("3");
            stat.AddWord("3");
            stat.AddWord("2");
            stat.AddWord("2");
            stat.AddWord("1");
            stat.AddWord("1");
            stat.AddWord("0");
            stat.AddWord("0");
            CollectionAssert.AreEqual(new[] { Tuple.Create(2, "0"), Tuple.Create(2, "1"), Tuple.Create(2, "2"), Tuple.Create(2, "3") }, stat.GetStatistics());
        }
	}
}