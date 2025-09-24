using System;
using System.Collections.Generic;
using System.Linq;

namespace ScriptureMemorizer
{
    public class Word
    {
        private string _text;
        private bool _isHidden;

        public Word(string text)
        {
            _text = text;
            _isHidden = false;
        }

        public void Hide()
        {
            _isHidden = true;
        }

        public void Show()
        {
            _isHidden = false;
        }

        public bool IsHidden()
        {
            return _isHidden;
        }

        public string GetDisplayText()
        {
            return _isHidden ? new string('_', _text.Length) : _text;
        }
    }

    public class Reference
    {
        private string _book;
        private int _chapter;
        private int _verse;
        private int _endVerse;

        public Reference(string book, int chapter, int verse)
        {
            _book = book;
            _chapter = chapter;
            _verse = verse;
            _endVerse = verse;
        }

        public Reference(string book, int chapter, int startVerse, int endVerse)
        {
            _book = book;
            _chapter = chapter;
            _verse = startVerse;
            _endVerse = endVerse;
        }

        public string GetDisplayText()
        {
            if (_verse == _endVerse)
                return $"{_book} {_chapter}:{_verse}";
            else
                return $"{_book} {_chapter}:{_verse}-{_endVerse}";
        }
    }

    public class Scripture
    {
        private Reference _reference;
        private List<Word> _words;
        private Random _random = new Random();

        public Scripture(Reference reference, string text)
        {
            _reference = reference;
            _words = text.Split(' ').Select(word => new Word(word)).ToList();
        }

        public void HideRandomWords(int numberToHide)
        {
            for (int i = 0; i < numberToHide; i++)
            {
                var visibleWords = _words.Where(w => !w.IsHidden()).ToList();
                if (visibleWords.Count == 0) break;

                int randomIndex = _random.Next(visibleWords.Count);
                visibleWords[randomIndex].Hide();
            }
        }

        public string GetDisplayText()
        {
            string referenceText = _reference.GetDisplayText();
            string verseText = string.Join(" ", _words.Select(w => w.GetDisplayText()));
            return $"{referenceText}\n{verseText}";
        }

        public bool IsCompletelyHidden()
        {
            return _words.All(w => w.IsHidden());
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Reference reference = new Reference("Psalm", 23, 1, 4);
            string text = "The Lord is my shepherd, I lack nothing. " +
                          "He makes me lie down in green pastures, " +
                          "he leads me beside quiet waters, " +
                          "he refreshes my soul. " +
                          "He guides me along the right paths for his name’s sake. " +
                          "Even though I walk through the darkest valley, " +
                          "I will fear no evil, for you are with me; " +
                          "your rod and your staff, they comfort me.";

            Scripture scripture = new Scripture(reference, text);

            while (true)
            {
                Console.Clear();
                Console.WriteLine(scripture.GetDisplayText());
                Console.WriteLine("\nPress Enter to hide words or type 'quit' to exit.");
                string input = Console.ReadLine()?.Trim().ToLower();

                if (input == "quit") break;

                scripture.HideRandomWords(3);

                if (scripture.IsCompletelyHidden())
                {
                    Console.Clear();
                    Console.WriteLine(scripture.GetDisplayText());
                    Console.WriteLine("\nAll words are hidden. Program ended.");
                    break;
                }
            }
        }
    }
}
