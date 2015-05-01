using System;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Kontur.Courses.Testing.Patterns.Specifications
{
	public class MarkdownProcessor
	{
		public string Render(string input)
		{
            //Скачанная реализация
            var emReplacer = new Regex(@"([^\w\\]|^)_(.*?[^\\])_(\W|$)");
            var strongReplacer = new Regex(@"([^\w\\]|^)__(.*?[^\\])__(\W|$)");
            input = strongReplacer.Replace(input,
                    match => match.Groups[1].Value +
                            "<strong>" + match.Groups[2].Value + "</strong>" +
                            match.Groups[3].Value);
            input = emReplacer.Replace(input,
                    match => match.Groups[1].Value +
                            "<em>" + match.Groups[2].Value + "</em>" +
                            match.Groups[3].Value);
            input = input.Replace(@"\_", "_");
            return input;
		}
	}

	[TestFixture]
	public class MarkdownProcessor_should
	{
		private readonly MarkdownProcessor md = new MarkdownProcessor();

	    [Test]
	    public void should_return_empty_string_on_empty_input()
	    {
            var input = "";
            var result = md.Render(input);
            Assert.AreEqual("", result);
	    }

	    [Test]
	    public void return_unmarked_input_if_no_marks_found()
	    {
            var input = "word";
            var result = md.Render(input);
            Assert.AreEqual("word", result);
	    }

        [TestCase("_word_", Result = "<em>word</em>")]
        [TestCase("word1 _word_ word3", Result = "word1 <em>word</em> word3")]
        [TestCase("_word_ word1", Result = "<em>word</em> word1")]
        [TestCase("word1 _word_", Result = "word1 <em>word</em>")]
        [TestCase("_word1 word2_", Result = "<em>word1 word2</em>")]
        public string surround_with_em_text_within_single_underscores(string input)
        {
            return md.Render(input);
        }

        [TestCase("__word__", Result = "<strong>word</strong>")]
        [TestCase("word1 __word__ word3", Result = "word1 <strong>word</strong> word3")]
        [TestCase("__word__ word1", Result = "<strong>word</strong> word1")]
        [TestCase("word1 __word__", Result = "word1 <strong>word</strong>")]
        [TestCase("__word1 word2__", Result = "<strong>word1 word2</strong>")]
	    public string surround_with_strong_text_within_double_underscores(string input)
        {
            return md.Render(input);
        }

        [TestCase("_word1 __word2__ word3_", Result = "<em>word1 <strong>word2</strong> word3</em>")]
        [TestCase("___word2__ word3_", Result = "<em><strong>word2</strong> word3</em>")]
        [TestCase("_word2 __word3___", Result = "<em>word2 <strong>word3</strong></em>")]
        [TestCase("___word___", Result = "<em><strong>word3</strong></em>")]
	    public string find_strong_occurence_within_em(string input)
        {
            return md.Render(input);
        }

	    [Test]
	    public void not_markdown_underlines_within_word()
	    {
	    }

	    [Test]
	    public void not_markdown_unpaired_underlines()
	    {
	    }

	    [Test]
	    public void not_markdown_escape_sequences()
	    {
	    }
	}
}
