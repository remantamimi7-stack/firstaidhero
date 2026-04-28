using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace firstaidhero_project
{
    public partial class Form1 : Form
    {
        // ===== المتغيرات الأساسية =====
        private MedicalQuestion[] questions;
        private RadioButton[][] allRadioButtons;
        private int score = 0;

        // ===== متغيرات اللعبة =====
        private int lives = 3;
        private Timer gameTimer;
        private int timeLeft = 300;
        private Label livesLabel;
        private Label timerLabel;

        public Form1()
        {
            InitializeComponent();
            SetupQuestions();
            BuildUI();
            StartTimer();
        }

        // ============================================================
        // دالة 1: إنشاء الأسئلة (Array of Objects)
        // ============================================================
        private void SetupQuestions()
        {
            questions = new MedicalQuestion[10];
            allRadioButtons = new RadioButton[10][];

            questions[0] = new MedicalQuestion(
                "مريض يشكو من ألم شديد في الصدر يمتد للذراع الأيسر، ما أول خطوة؟",
                new string[] { "أ) إعطاء مسكن للألم فوراً", "ب) الاتصال بالإسعاف وإجلاس المريض", "ج) إعطاؤه ماء بارد" },
                1, DifficultyLevel.Hard, "طوارئ قلبية");

            questions[1] = new MedicalQuestion(
                "طفل ابتلع جسماً غريباً ويسعل بشدة، ماذا تفعل؟",
                new string[] { "أ) تربته على ظهره بقوة (مناورة هايملك)", "ب) تعطيه ماء ليبلعه", "ج) تنتظر حتى يخرج وحده" },
                0, DifficultyLevel.Medium, "طوارئ أطفال");

            questions[2] = new MedicalQuestion(
                "مريض سكري فقد وعيه، ما السبب الأكثر احتمالاً؟",
                new string[] { "أ) ارتفاع ضغط الدم", "ب) انخفاض السكر في الدم", "ج) الإجهاد الشديد" },
                1, DifficultyLevel.Medium, "أمراض السكري");

            questions[3] = new MedicalQuestion(
                "شخص أُصيب بحرق من الدرجة الثانية في يده، ما العلاج الأولي؟",
                new string[] { "أ) وضع زبدة على الحرق", "ب) تبريد المنطقة بماء بارد لـ 10-20 دقيقة", "ج) تغطيته بقماش جاف مباشرة" },
                1, DifficultyLevel.Easy, "إسعافات أولية");

            questions[4] = new MedicalQuestion(
                "مريضة حامل تشكو من صداع شديد وتورم في القدمين، ما الاحتمال الأخطر؟",
                new string[] { "أ) تعب عادي من الحمل", "ب) تسمم الحمل (Preeclampsia)", "ج) نقص فيتامينات" },
                1, DifficultyLevel.Hard, "صحة الأم");

            questions[5] = new MedicalQuestion(
                "مريض يأخذ أدوية ضغط نسي جرعة الصباح، ماذا يفعل؟",
                new string[] { "أ) يأخذ جرعتين معاً في المساء", "ب) يتجاهل الجرعة الفائتة ويكمل جدوله", "ج) يوقف الدواء لهذا اليوم" },
                1, DifficultyLevel.Easy, "أدوية");

            questions[6] = new MedicalQuestion(
                "شخص مصاب بالربو يعاني من نوبة مفاجئة، ما أول شيء يستخدم؟",
                new string[] { "أ) بخاخ الكورتيزون الوقائي", "ب) بخاخ الموسّع السريع (Ventolin)", "ج) شرب الماء الساخن" },
                1, DifficultyLevel.Medium, "الجهاز التنفسي");

            questions[7] = new MedicalQuestion(
                "مريض يأتي بنزيف لا يتوقف من جرح في الساق، ماذا تفعل أولاً؟",
                new string[] { "أ) تضع ثلجاً على الجرح", "ب) تضغط على الجرح بقماش نظيف", "ج) ترفع قدمه فقط دون ضغط" },
                1, DifficultyLevel.Easy, "إسعافات أولية");

            questions[8] = new MedicalQuestion(
                "مريض يعاني من حساسية شديدة بعد لسعة نحلة، ما العلاج الطارئ؟",
                new string[] { "أ) مضاد حساسية عادي فقط", "ب) حقنة الإبينفرين (EpiPen) فوراً", "ج) وضعه في مكان بارد" },
                1, DifficultyLevel.Hard, "حساسية وطوارئ");

            questions[9] = new MedicalQuestion(
                "مريض مسن سقط وأصبح عاجزاً عن تحريك ساقه، ما الاشتباه الأول؟",
                new string[] { "أ) تشنج عضلي بسيط", "ب) كسر في عنق الفخذ - يحتاج تصوير فوري", "ج) ضعف عام من التعب" },
                1, DifficultyLevel.Medium, "العظام والكسور");
        }

        // ============================================================
        // دالة 2: بناء الواجهة
        // ============================================================
        private void BuildUI()
        {
            this.Text = "First Aid Hero";
            this.Size = new Size(700, 820);
            this.BackColor = Color.FromArgb(240, 248, 255);
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;

            // شريط العلوي (حياة + مؤقت)
            Panel topBar = new Panel();
            topBar.Size = new Size(700, 50);
            topBar.Location = new Point(0, 0);
            topBar.BackColor = Color.FromArgb(0, 102, 153);
            this.Controls.Add(topBar);

            livesLabel = new Label();
            livesLabel.Text = "❤️ ❤️ ❤️";
            livesLabel.Font = new Font("Arial", 16);
            livesLabel.ForeColor = Color.White;
            livesLabel.Location = new Point(10, 10);
            livesLabel.Size = new Size(200, 35);
            livesLabel.TextAlign = ContentAlignment.MiddleRight;
            topBar.Controls.Add(livesLabel);

            timerLabel = new Label();
            timerLabel.Text = "⏱️ 5:00";
            timerLabel.Font = new Font("Arial", 16, FontStyle.Bold);
            timerLabel.ForeColor = Color.White;
            timerLabel.Location = new Point(490, 10);
            timerLabel.Size = new Size(200, 35);
            timerLabel.TextAlign = ContentAlignment.MiddleLeft;
            topBar.Controls.Add(timerLabel);

            // Panel الأسئلة
            Panel mainPanel = new Panel();
            mainPanel.AutoScroll = true;
            mainPanel.Size = new Size(690, 680);
            mainPanel.Location = new Point(0, 55);
            mainPanel.Padding = new Padding(20);
            this.Controls.Add(mainPanel);

            int yPos = 20;

            Label title = new Label();
            title.Text = "🏥 اختبار المواقف الطبية";
            title.Font = new Font("Arial", 18, FontStyle.Bold);
            title.ForeColor = Color.FromArgb(0, 102, 153);
            title.Size = new Size(640, 50);
            title.Location = new Point(10, yPos);
            title.TextAlign = ContentAlignment.MiddleCenter;
            mainPanel.Controls.Add(title);
            yPos += 70;

            for (int i = 0; i < questions.Length; i++)
            {
                MedicalQuestion q = questions[i];
                Color headerColor = GetDifficultyColor(q.Difficulty);

                Panel qPanel = new Panel();
                qPanel.Size = new Size(640, 210);
                qPanel.Location = new Point(10, yPos);
                qPanel.BackColor = Color.White;
                qPanel.BorderStyle = BorderStyle.FixedSingle;
                mainPanel.Controls.Add(qPanel);

                Panel headerPanel = new Panel();
                headerPanel.Size = new Size(640, 28);
                headerPanel.Location = new Point(0, 0);
                headerPanel.BackColor = headerColor;
                qPanel.Controls.Add(headerPanel);

                Label infoLabel = new Label();
                infoLabel.Text = q.GetInfo(); // Polymorphism
                infoLabel.Font = new Font("Arial", 8, FontStyle.Bold);
                infoLabel.ForeColor = Color.White;
                infoLabel.Size = new Size(620, 28);
                infoLabel.Location = new Point(5, 0);
                infoLabel.TextAlign = ContentAlignment.MiddleRight;
                headerPanel.Controls.Add(infoLabel);

                Label qLabel = new Label();
                qLabel.Text = "س" + (i + 1) + ": " + q.QuestionText;
                qLabel.Font = new Font("Arial", 10, FontStyle.Bold);
                qLabel.ForeColor = Color.FromArgb(30, 30, 30);
                qLabel.Size = new Size(620, 55);
                qLabel.Location = new Point(10, 33);
                qLabel.TextAlign = ContentAlignment.MiddleRight;
                qPanel.Controls.Add(qLabel);

                allRadioButtons[i] = new RadioButton[3];
                for (int j = 0; j < q.Choices.Length; j++)
                {
                    RadioButton rb = new RadioButton();
                    rb.Text = q.Choices[j];
                    rb.Font = new Font("Arial", 9);
                    rb.Size = new Size(610, 28);
                    rb.Location = new Point(10, 93 + (j * 35));
                    rb.RightToLeft = RightToLeft.Yes;
                    qPanel.Controls.Add(rb);
                    allRadioButtons[i][j] = rb;
                }

                yPos += 225;
            }

            Button submitBtn = new Button();
            submitBtn.Text = "✅ تحقق من الإجابات";
            submitBtn.Font = new Font("Arial", 12, FontStyle.Bold);
            submitBtn.Size = new Size(220, 48);
            submitBtn.Location = new Point(235, 745);
            submitBtn.BackColor = Color.FromArgb(0, 153, 76);
            submitBtn.ForeColor = Color.White;
            submitBtn.FlatStyle = FlatStyle.Flat;
            submitBtn.Click += SubmitButton_Click;
            this.Controls.Add(submitBtn);
        }

        // ============================================================
        // دالة 3: المؤقت
        // ============================================================
        private void StartTimer()
        {
            gameTimer = new Timer();
            gameTimer.Interval = 1000;
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            int minutes = timeLeft / 60;
            int seconds = timeLeft % 60;
            timerLabel.Text = "⏱️ " + minutes + ":" + seconds.ToString("D2");

            if (timeLeft <= 60)
                timerLabel.ForeColor = Color.Red;

            if (timeLeft <= 0)
            {
                gameTimer.Stop();
                MessageBox.Show("⏰ انتهى الوقت!", "Game Over",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.Close();
            }
        }

        // ============================================================
        // دالة 4: تحديث القلوب
        // ============================================================
        private void UpdateLives()
        {
            // نمنع النزول تحت الصفر
            if (lives < 0) lives = 0;

            if (lives == 3) livesLabel.Text = "❤️ ❤️ ❤️";
            else if (lives == 2) livesLabel.Text = "❤️ ❤️ 🖤";
            else if (lives == 1) livesLabel.Text = "❤️ 🖤 🖤";
            else livesLabel.Text = "🖤 🖤 🖤";
        }

        // ============================================================
        // دالة 5: عند الضغط على تحقق
        // ============================================================
        private void SubmitButton_Click(object sender, EventArgs e)
        {
            gameTimer.Stop();
            score = 0;
            int wrongAnswers = 0;
            bool allAnswered = true;

            for (int i = 0; i < questions.Length; i++)
            {
                int selected = -1;

                for (int j = 0; j < allRadioButtons[i].Length; j++)
                {
                    if (allRadioButtons[i][j].Checked)
                    {
                        selected = j;
                        break;
                    }
                }

                if (selected == -1)
                {
                    allAnswered = false;
                }
                else if (questions[i].IsCorrect(selected))
                {
                    score++;
                }
                else
                {
                    wrongAnswers++;
                }
            }

            if (!allAnswered)
            {
                gameTimer.Start();
                MessageBox.Show("⚠️ يرجى الإجابة على جميع الأسئلة!", "تنبيه",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // نخصم روح لكل إجابة غلط
            lives = lives - wrongAnswers;
            UpdateLives();

            double percent = (double)score / questions.Length * 100;
            string grade = GetGrade(percent);
            string encouragement = GetEncouragement(percent);

            SaveResultToFile(score, percent, grade);

            MessageBox.Show(
                encouragement + "\n\n" +
                "🏆 نتيجتك: " + score + " من " + questions.Length + "\n" +
                "📊 النسبة: " + percent.ToString("F0") + "%\n" +
                "🎯 التقدير: " + grade + "\n" +
                "❤️ أرواح متبقية: " + (lives < 0 ? 0 : lives) + "\n\n" +
                "💾 تم حفظ النتيجة في results.txt",
                "نتيجة اللعبة",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ============================================================
        // دالة 6: رسائل التشجيع
        // ============================================================
        private string GetEncouragement(double percent)
        {
            if (percent >= 90) return "🌟 أنت بطل الإسعافات الأولية!";
            if (percent >= 75) return "💪 أداء رائع! أنت على الطريق الصح!";
            if (percent >= 60) return "👍 جيد! بعض المراجعة ستجعلك أفضل!";
            return "📚 لا تستسلم! راجع المادة وحاول مرة أخرى!";
        }

        // ============================================================
        // دالة 7: لون الصعوبة (Enum)
        // ============================================================
        private Color GetDifficultyColor(DifficultyLevel level)
        {
            switch (level)
            {
                case DifficultyLevel.Easy: return Color.FromArgb(76, 153, 0);
                case DifficultyLevel.Medium: return Color.FromArgb(204, 102, 0);
                case DifficultyLevel.Hard: return Color.FromArgb(180, 0, 0);
                default: return Color.Gray;
            }
        }

        // ============================================================
        // دالة 8: حفظ النتيجة (File Handling + Exception Handling)
        // ============================================================
        private void SaveResultToFile(int score, double percent, string grade)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("results.txt", append: true))
                {
                    writer.WriteLine("================================");
                    writer.WriteLine("التاريخ: " + DateTime.Now);
                    writer.WriteLine("النتيجة: " + score + " من " + questions.Length);
                    writer.WriteLine("النسبة: " + percent.ToString("F0") + "%");
                    writer.WriteLine("التقدير: " + grade);
                    writer.WriteLine("الأرواح المتبقية: " + (lives < 0 ? 0 : lives));
                    writer.WriteLine("================================");
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("خطأ في حفظ الملف: " + ex.Message, "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // دالة 9: التقدير
        // ============================================================
        private string GetGrade(double percent)
        {
            if (percent >= 90) return "ممتاز 🌟";
            if (percent >= 75) return "جيد جداً ✅";
            if (percent >= 60) return "جيد 👍";
            return "تحتاج مراجعة 📚";
        }
    }

    // ============================================================
    // ENUM: مستوى الصعوبة
    // ============================================================
    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    }

    // ============================================================
    // BASE CLASS: Question
    // ============================================================
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

    // ============================================================
    // CHILD CLASS: MedicalQuestion (Inheritance + Polymorphism)
    // ============================================================
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
