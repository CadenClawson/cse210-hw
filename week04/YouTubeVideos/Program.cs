using System;
using System.Collections.Generic;

class Comment
{
    private string _commenterName;
    private string _text;

    public Comment(string commenterName, string text)
    {
        _commenterName = commenterName;
        _text = text;
    }

    public string GetCommenterName() => _commenterName;
    public string GetText() => _text;

    public void Display()
    {
        Console.WriteLine($"{_commenterName}: {_text}");
    }
}

class Video
{
    private string _title;
    private string _author;
    private int _length; // in seconds
    private List<Comment> _comments;

    public Video(string title, string author, int length)
    {
        _title = title;
        _author = author;
        _length = length;
        _comments = new List<Comment>();
    }

    public string GetTitle() => _title;
    public string GetAuthor() => _author;
    public int GetLength() => _length;

    public void AddComment(Comment comment)
    {
        _comments.Add(comment);
    }

    public int GetNumberOfComments()
    {
        return _comments.Count;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"\nTitle: {_title}");
        Console.WriteLine($"Author: {_author}");
        Console.WriteLine($"Length: {_length} seconds");
        Console.WriteLine($"Number of comments: {GetNumberOfComments()}");
        Console.WriteLine("Comments:");
        foreach (Comment c in _comments)
        {
            c.Display();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<Video> videos = new List<Video>();

        // Video 1
        Video video1 = new Video("Intro to C#", "Alice", 600);
        video1.AddComment(new Comment("Bob", "Great video!"));
        video1.AddComment(new Comment("Charlie", "Very clear explanation."));
        video1.AddComment(new Comment("Dana", "Helped me a lot, thanks!"));
        videos.Add(video1);

        // Video 2
        Video video2 = new Video("Data Structures Basics", "Eve", 1200);
        video2.AddComment(new Comment("Frank", "This was a bit fast."));
        video2.AddComment(new Comment("Grace", "Loved the visuals!"));
        video2.AddComment(new Comment("Hank", "Could you cover trees next?"));
        videos.Add(video2);

        // Video 3
        Video video3 = new Video("OOP in Practice", "John", 900);
        video3.AddComment(new Comment("Ivy", "Finally understood abstraction!"));
        video3.AddComment(new Comment("Ken", "Super useful for my homework."));
        video3.AddComment(new Comment("Liam", "Keep up the good work."));
        videos.Add(video3);

        // Iterate through videos and display info
        foreach (Video v in videos)
        {
            v.DisplayInfo();
        }
    }
}
