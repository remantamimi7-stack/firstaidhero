using System;

namespace firstaidhero_project
{
    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    }

    public class Question
    {
        public string QuestionText { get; set; }
        public string[] Choices { get; set; }
        public int CorrectIndex { get; set; }
        public DifficultyLevel Difficulty { get; set; }

        public Question(string questionText, string[] choices, int correctIndex, DifficultyLevel difficulty)
        {
            QuestionText = questionText;
            Choices = choices;
            CorrectIndex = correctIndex;
            Difficulty = difficulty;
        }

        public bool IsCorrect(int selectedIndex)
        {
            return selectedIndex == CorrectIndex;
        }

        public virtual string GetInfo()
        {
            return "سؤال عام | الصعوبة: " + Difficulty;
        }
    }

    public class MedicalQuestion : Question
    {
        public string MedicalCategory { get; set; }

        public MedicalQuestion(string questionText, string[] choices,
                               int correctIndex, DifficultyLevel difficulty,
                               string category)
            : base(questionText, choices, correctIndex, difficulty)
        {
            MedicalCategory = category;
        }

        public override string GetInfo()
        {
            return "سؤال طبي | الفئة: " + MedicalCategory + " | الصعوبة: " + Difficulty;
        }
    }
}