using System;
using System.Collections.Generic;
using System.IO;

namespace EternalQuest
{
    public abstract class Goal
    {
        protected string _shortName;
        protected string _description;
        protected int _points;

        protected Goal(string name, string description, int points)
        {
            _shortName = name ?? "";
            _description = description ?? "";
            _points = points;
        }

        public abstract int RecordEvent();
        public abstract bool IsComplete();
        public virtual string GetDetailsString()
        {
            return $"{_shortName} ({_description}) - {_points} pts";
        }
        public abstract string GetStringRepresentation();

        protected static string Escape(string s) => s?.Replace("|", "&#124;") ?? "";
        protected static string Unescape(string s) => s?.Replace("&#124;", "|") ?? "";
    }

    public class SimpleGoal : Goal
    {
        private bool _isComplete;

        public SimpleGoal(string name, string description, int points)
            : base(name, description, points)
        {
            _isComplete = false;
        }

        public override int RecordEvent()
        {
            if (_isComplete)
            {
                Console.WriteLine("This goal is already complete.");
                return 0;
            }

            _isComplete = true;
            Console.WriteLine($"Completed \"{_shortName}\" — you gained {_points} points!");
            return _points;
        }

        public override bool IsComplete() => _isComplete;

        public override string GetDetailsString()
        {
            string status = _isComplete ? "[X]" : "[ ]";
            return $"{status} {_shortName} - {_description} ({_points} pts)";
        }

        public override string GetStringRepresentation()
        {
            return $"SimpleGoal:{Escape(_shortName)}|{Escape(_description)}|{_points}|{_isComplete}";
        }

        public static SimpleGoal FromStringParts(string[] parts)
        {
            string name = Unescape(parts[0]);
            string desc = Unescape(parts[1]);
            int pts = int.Parse(parts[2]);
            bool done = bool.Parse(parts[3]);
            var g = new SimpleGoal(name, desc, pts);
            g._isComplete = done;
            return g;
        }
    }

    public class EternalGoal : Goal
    {
        public EternalGoal(string name, string description, int points)
            : base(name, description, points)
        {
        }

        public override int RecordEvent()
        {
            Console.WriteLine($"Recorded \"{_shortName}\" — you gained {_points} points!");
            return _points;
        }

        public override bool IsComplete() => false;

        public override string GetDetailsString()
        {
            return $"[∞] {_shortName} - {_description} ({_points} pts each time)";
        }

        public override string GetStringRepresentation()
        {
            return $"EternalGoal:{Escape(_shortName)}|{Escape(_description)}|{_points}";
        }

        public static EternalGoal FromStringParts(string[] parts)
        {
            string name = Unescape(parts[0]);
            string desc = Unescape(parts[1]);
            int pts = int.Parse(parts[2]);
            return new EternalGoal(name, desc, pts);
        }
    }

    public class ChecklistGoal : Goal
    {
        private int _amountCompleted;
        private int _target;
        private int _bonus;

        public ChecklistGoal(string name, string description, int points, int target, int bonus)
            : base(name, description, points)
        {
            _target = Math.Max(1, target);
            _bonus = Math.Max(0, bonus);
            _amountCompleted = 0;
        }

        public override int RecordEvent()
        {
            if (IsComplete())
            {
                Console.WriteLine("This checklist goal has already been completed.");
                return 0;
            }

            _amountCompleted++;
            int awarded = _points;

            if (_amountCompleted >= _target)
            {
                awarded += _bonus;
                Console.WriteLine($"Completed \"{_shortName}\" ({_amountCompleted}/{_target}) — you gained {_points} pts + bonus {_bonus} pts!");
            }
            else
            {
                Console.WriteLine($"Recorded \"{_shortName}\" ({_amountCompleted}/{_target}) — you gained {_points} pts.");
            }

            return awarded;
        }

        public override bool IsComplete() => _amountCompleted >= _target;

        public override string GetDetailsString()
        {
            string status = IsComplete() ? "[X]" : "[ ]";
            return $"{status} {_shortName} - {_description} ({_points} pts each) Completed {_amountCompleted}/{_target}";
        }

        public override string GetStringRepresentation()
        {
            return $"ChecklistGoal:{Escape(_shortName)}|{Escape(_description)}|{_points}|{_amountCompleted}|{_target}|{_bonus}";
        }

        public static ChecklistGoal FromStringParts(string[] parts)
        {
            string name = Unescape(parts[0]);
            string desc = Unescape(parts[1]);
            int pts = int.Parse(parts[2]);
            int amt = int.Parse(parts[3]);
            int target = int.Parse(parts[4]);
            int bonus = int.Parse(parts[5]);
            var g = new ChecklistGoal(name, desc, pts, target, bonus);
            g._amountCompleted = amt;
            return g;
        }
    }

    public class GoalManager
    {
        private List<Goal> _goals = new List<Goal>();
        private int _score = 0;
        private const string SAVE_FILENAME = "goals.txt";

        public void Start()
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("=== Eternal Quest ===");
                Console.WriteLine($"Score: {_score}");
                Console.WriteLine("1. Create Goal");
                Console.WriteLine("2. List Goals");
                Console.WriteLine("3. Record Event");
                Console.WriteLine("4. Display Score");
                Console.WriteLine("5. Save Goals");
                Console.WriteLine("6. Load Goals");
                Console.WriteLine("7. Exit");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1": CreateGoal(); break;
                    case "2": ListGoals(); break;
                    case "3": RecordEvent(); break;
                    case "4": DisplayScore(); break;
                    case "5": SaveGoals(); break;
                    case "6": LoadGoals(); break;
                    case "7": running = false; break;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }

            Console.WriteLine("Goodbye!");
        }

        public void DisplayScore()
        {
            Console.WriteLine($"Current score: {_score} points.");
        }

        public void ListGoals()
        {
            if (_goals.Count == 0)
            {
                Console.WriteLine("No goals yet.");
                return;
            }

            Console.WriteLine("Goals:");
            for (int i = 0; i < _goals.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_goals[i].GetDetailsString()}");
            }
        }

        public void CreateGoal()
        {
            Console.WriteLine("Choose goal type:");
            Console.WriteLine("1. Simple Goal (one-time)");
            Console.WriteLine("2. Eternal Goal (repeatable)");
            Console.WriteLine("3. Checklist Goal (complete N times)");
            Console.Write("Choice: ");
            string t = Console.ReadLine()?.Trim();

            Console.Write("Short name: ");
            string name = Console.ReadLine() ?? "";
            Console.Write("Description: ");
            string desc = Console.ReadLine() ?? "";

            int points = PromptInt("Points for each completion: ");

            switch (t)
            {
                case "1":
                    _goals.Add(new SimpleGoal(name, desc, points));
                    Console.WriteLine("Simple goal created.");
                    break;
                case "2":
                    _goals.Add(new EternalGoal(name, desc, points));
                    Console.WriteLine("Eternal goal created.");
                    break;
                case "3":
                    int target = PromptInt("How many times to complete? ");
                    int bonus = PromptInt("Bonus points when target reached: ");
                    _goals.Add(new ChecklistGoal(name, desc, points, target, bonus));
                    Console.WriteLine("Checklist goal created.");
                    break;
                default:
                    Console.WriteLine("Invalid goal type.");
                    break;
            }
        }

        public void RecordEvent()
        {
            if (_goals.Count == 0)
            {
                Console.WriteLine("No goals to record.");
                return;
            }

            ListGoals();
            int idx = PromptInt("Enter goal number to record: ") - 1;
            if (idx < 0 || idx >= _goals.Count)
            {
                Console.WriteLine("Invalid goal number.");
                return;
            }

            Goal g = _goals[idx];
            int gained = g.RecordEvent();
            _score += gained;
        }

        public void SaveGoals()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(SAVE_FILENAME))
                {
                    writer.WriteLine(_score);
                    foreach (var g in _goals)
                    {
                        writer.WriteLine(g.GetStringRepresentation());
                    }
                }
                Console.WriteLine($"Saved {_goals.Count} goals to {SAVE_FILENAME}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving: {ex.Message}");
            }
        }

        public void LoadGoals()
        {
            if (!File.Exists(SAVE_FILENAME))
            {
                Console.WriteLine($"No save file found ({SAVE_FILENAME}).");
                return;
            }

            try
            {
                string[] lines = File.ReadAllLines(SAVE_FILENAME);
                var loaded = new List<Goal>();
                int newScore = 0;

                if (lines.Length > 0)
                {
                    int.TryParse(lines[0], out newScore);
                }

                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (string.IsNullOrEmpty(line)) continue;

                    int colonIndex = line.IndexOf(':');
                    if (colonIndex < 0) continue;
                    string type = line.Substring(0, colonIndex);
                    string rest = line.Substring(colonIndex + 1);
                    string[] parts = rest.Split('|');

                    Goal g = null;
                    switch (type)
                    {
                        case "SimpleGoal":
                            if (parts.Length >= 4) g = SimpleGoal.FromStringParts(parts);
                            break;
                        case "EternalGoal":
                            if (parts.Length >= 3) g = EternalGoal.FromStringParts(parts);
                            break;
                        case "ChecklistGoal":
                            if (parts.Length >= 6) g = ChecklistGoal.FromStringParts(parts);
                            break;
                        default:
                            Console.WriteLine($"Unknown goal type in save: {type}");
                            break;
                    }

                    if (g != null) loaded.Add(g);
                }

                _goals = loaded;
                _score = newScore;
                Console.WriteLine($"Loaded {_goals.Count} goals. Score set to {_score}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading: {ex.Message}");
            }
        }

        private int PromptInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string s = Console.ReadLine()?.Trim() ?? "";
                if (int.TryParse(s, out int v)) return v;
                Console.WriteLine("Please enter a valid integer.");
            }
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            var manager = new GoalManager();
            manager.Start();
        }
    }
}
