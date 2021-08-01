using System.Collections.Generic;

namespace UserInterface.Helpers
{
    public class Team
    {
        public string TeamName { get; set; }
        public bool IsNamingMode { get; set; }
        public int TeamScore { get; set; }
        public int MistakesCount { get; set; }
        public int CorrectAnswers { get; set; }

        public Team()
        {
            IsNamingMode = false;
            MistakesCount = 0;
            TeamScore = 0;
            CorrectAnswers = 0;
            TeamName = "без названия";
        }
    }
    public class LogicHelper
    {
        public bool IsGameBegun { get; set; }
        public bool SelectedTeam { get; set; }
        public Team Left { get; set; }
        public Team Right { get; set; }
        public int RoundCount { get; set; }
        public List<string> StringsList { get; set; }
        public List<bool> AnswersMatrix { get; set; }

        public LogicHelper()
        {
            Left = new Team();
            Right = new Team();
            IsGameBegun = false;
            RoundCount = 0;
            StringsList = new List<string>(7)
            {
                "названия команд ?",
                new string('x', 40),
                new string('x', 40),
                new string('x', 40),
                new string('x', 40),
                new string('x', 40),
                new string('x', 40)
            };
        }

        public void PrepareGame()
        {
            Left.MistakesCount = 0;
            Right.MistakesCount = 0;
            MainWindow.RoundsLabel.Content = " ? ";
            MainWindow.QuestionLabel.Content = StringsList[0];
        }
    }
}