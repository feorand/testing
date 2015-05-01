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
		public void no_stats_if_no_words()
		{
			CollectionAssert.IsEmpty(stat.GetStatistics());
		}

		[Test]
		public void same_word_twice()
		{
			stat.AddWord("xxx");
			stat.AddWord("xxx");
			CollectionAssert.AreEqual(new[] { Tuple.Create(2, "xxx") }, stat.GetStatistics());
		}

		[Test]
		public void single_word()
		{
			stat.AddWord("hello");
			CollectionAssert.AreEqual(new[] { Tuple.Create(1, "hello") }, stat.GetStatistics());
		}

		[Test]
		public void two_same_words_one_other()
		{
			stat.AddWord("hello");
			stat.AddWord("world");
			stat.AddWord("world");

			CollectionAssert.AreEqual(new[] { Tuple.Create(2, "world"), Tuple.Create(1, "hello") }, stat.GetStatistics());
		}

	    [Test]
	    public void empty_word()
	    {
            stat.AddWord("");
	        var s = stat.GetStatistics();
            CollectionAssert.AreNotEqual(new[] {Tuple.Create(1, "")}, stat.GetStatistics());
	    }

	    [Test]
	    public void two_same_words_length_more_10_chars()
	    {
	        stat.AddWord("aaaaaaaaaaab");
            stat.AddWord("aaaaaaaaaaaa");
            CollectionAssert.AreEqual(new[] { Tuple.Create(2, "aaaaaaaaaa")}, stat.GetStatistics());
	    }

	    [Test]
	    public void two_words_diff_regs()
	    {
            stat.AddWord("AAAA");
            stat.AddWord("aaaa");
            CollectionAssert.AreEqual(new[] {Tuple.Create(2, "aaaa")}, stat.GetStatistics());
	    }

        [Test]
	    public void sorting_words()
	    {
	        stat.AddWord("a");
            stat.AddWord("b");
            CollectionAssert.AreEqual((new[] {Tuple.Create(1, "a"), Tuple.Create(1, "b")}), stat.GetStatistics());
	    }

	    [Test]
	    public void null_test()
	    {
            stat.AddWord(null);
            Assert.AreEqual(0, stat.GetStatistics().Count());

	    }

        [Test]
        public void eleventh_symbol()
        {
            stat.AddWord("aaaaaaaaaaa");
            stat.AddWord("aaaaaaaaaab");
            CollectionAssert.AreEqual(new[] { Tuple.Create(2, "aaaaaaaaaa") }, stat.GetStatistics());
        }

        [Test]
        public void non_english()
        {
            stat.AddWord("Ä ä)");
            CollectionAssert.AreEqual(new[] { Tuple.Create(1, "hello") }, stat.GetStatistics());
        }
	}
}