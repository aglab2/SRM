using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Geoguessr
{
    public partial class Geoguessr : Form
    {
        Dictionary<Button, int> Buttons;
        List<Label> Labels;
        int CurrentQuestion = 0;
        int ButtonsLeft = 25;
        FontFamily FontFamily;
        Font Font;

        private void ScaleFont(Label lab, int maxSize)
        {
            SizeF extent = TextRenderer.MeasureText(lab.Text, Font);

            float hRatio = lab.Height / extent.Height;
            float wRatio = lab.Width / extent.Width;
            float ratio = (hRatio < wRatio) ? hRatio : wRatio;

            float newSize = Font.Size * ratio - 0.25f;
            if (newSize > maxSize)
                newSize = maxSize;

            lab.Font = new Font(FontFamily, newSize, lab.Font.Style);
        }

        string TitleForButtonNumber(int number)
        {
            int val = (number - 1) / 5;
            return Labels[val].Text;
        }

        int ScoreForButtonNumber(int number)
        {
            int val = (number - 1) % 5;
            return 1 + val;
        }

        int GetMultiplier()
        {
            if (ButtonsLeft > 15)
                return 1;

            if (ButtonsLeft > 5)
                return 2;

            return 4;
        }

        string ExeDir()
        {
            var exePath = System.Reflection.Assembly.GetEntryAssembly().Location;
            return Directory.GetParent(exePath).FullName;
        }

        string QuizDir()
        {
            return Path.Combine(ExeDir(), "quiz");
        }

        void SetJeopardy(bool enabled)
        {
            if (enabled)
            {
                ButtonsLeft = 0;
                foreach (var button in Buttons.Keys)
                {
                    if (button.Enabled)
                        ButtonsLeft++;
                }

                bool done = 0 == ButtonsLeft;
                int multiplier = GetMultiplier();

                foreach (var button in Buttons.Keys)
                {
                    int question = Buttons[button];
                    button.Visible = !done;
                    button.Text = (ScoreForButtonNumber(question) * 100 * multiplier).ToString();
                }
                foreach (var label in Labels)
                {
                    label.Visible = !done;
                }

                if (done)
                {
                    bool rightWinner = false;
                    try
                    {
                        var scoreL = int.Parse(textBoxScoreL.Text);
                        var scoreR = int.Parse(textBoxScoreR.Text);
                        rightWinner = scoreR > scoreL;
                    }
                    catch (Exception) { }

                    labelPictureT.Text = "Winner!";
                    ScaleFont(labelPictureT, 36);
                    labelPictureB.Text = rightWinner ? textBoxNameR.Text : textBoxNameL.Text;
                    ScaleFont(labelPictureB, 36);
                    pictureBox1.Image = rightWinner ? pictureBoxR.Image : pictureBoxL.Image;
                    pictureBox1.Visible = true;
                }
            }
            else
            {
                foreach (var button in Buttons.Keys)
                {
                    button.Visible = false;
                }
                foreach (var label in Labels)
                {
                    label.Visible = false;
                }
            }
        }

        bool GetTurn()
        {
            return labelNameR.ForeColor == Color.Yellow;
        }

        void SetTurn(bool right)
        {
            if (right)
            {
                labelNameR.ForeColor = Color.Yellow;
                labelNameL.ForeColor = Color.White;
            }
            else
            {
                labelNameR.ForeColor = Color.White;
                labelNameL.ForeColor = Color.Yellow;
            }
        }

        void DisableGuessingControls()
        {
            pictureBox1.Visible = false;
            labelPictureB.Text = "";
            labelPictureT.Text = "";
            labelTop2.Text = "";
            buttonAnswer.Enabled = false;
            buttonPoints1.Enabled = false;
            buttonPoints2.Enabled = false;
            buttonClear.Enabled = false;
        }

        public Geoguessr()
        {
            InitializeComponent();

            Buttons = new Dictionary<Button, int>
            {
                { button1, 1 },
                { button2, 6 },
                { button3, 11 },
                { button4, 16 },
                { button5, 21 },
                { button6, 2 },
                { button7, 7 },
                { button8, 12 },
                { button9, 17 },
                { button10, 22 },
                { button11, 3 },
                { button12, 8 },
                { button13, 13 },
                { button14, 18 },
                { button15, 23 },
                { button16, 4 },
                { button17, 9 },
                { button18, 14 },
                { button19, 19 },
                { button20, 24 },
                { button21, 5 },
                { button22, 10 },
                { button23, 15 },
                { button24, 20 },
                { button25, 25 },
            };
            Labels = new List<Label>
            {
                label1,
                label2,
                label3,
                label4,
                label5,
            };

            PrivateFontCollection collection = new PrivateFontCollection();
            collection.AddFontFile(Path.Combine(ExeDir(), "font.ttf"));
            FontFamily = new FontFamily("Kurri Island Med", collection);
            Font = new Font(FontFamily, 18);

            foreach (var label in Labels)
            {
                label.Parent = pictureBoxBackground;
                label.BackColor = Color.Transparent;
                label.Text = "xd" + Environment.NewLine + "peka";
                label.Font = Font;
            }

            labelNameL.Parent = pictureBoxBackground;
            labelNameL.BackColor = Color.Transparent;
            labelNameL.Text = "";
            labelNameL.Font = Font;
            ScaleFont(labelNameL, 28);

            labelScoreL.Parent = pictureBoxBackground;
            labelScoreL.BackColor = Color.Transparent;
            labelScoreL.Text = "Score: 0";
            labelScoreL.Font = Font;
            ScaleFont(labelScoreL, 28);

            labelNameR.Parent = pictureBoxBackground;
            labelNameR.BackColor = Color.Transparent;
            labelNameR.Text = "";
            labelNameR.Font = Font;
            ScaleFont(labelNameR, 28);

            labelScoreR.Parent = pictureBoxBackground;
            labelScoreR.BackColor = Color.Transparent;
            labelScoreR.Text = "Score: 0";
            labelScoreR.Font = Font;
            ScaleFont(labelScoreR, 28);

            labelPictureT.Parent = pictureBoxBackground;
            labelPictureT.BackColor = Color.Transparent;
            labelPictureT.Text = "";
            labelPictureT.Font = Font;
            ScaleFont(labelPictureT, 36);

            labelTop2.Parent = labelPictureT;
            labelTop2.BackColor = Color.Transparent;
            labelTop2.Text = "";
            labelTop2.Font = Font;
            labelTop2.TextAlign = ContentAlignment.TopLeft;
            ScaleFont(labelTop2, 20);

            labelPictureB.Parent = pictureBoxBackground;
            labelPictureB.BackColor = Color.Transparent;
            labelPictureB.Text = "";
            labelPictureB.Font = Font;
            ScaleFont(labelPictureB, 36);

            pictureBox1.Parent = pictureBoxBackground;
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Visible = false;
            pictureBoxL.Parent = pictureBoxBackground;
            pictureBoxL.BackColor = Color.Transparent;
            pictureBoxR.Parent = pictureBoxBackground;
            pictureBoxR.BackColor = Color.Transparent;

            int idx = 0;
            foreach (string title in Directory.EnumerateDirectories(QuizDir()))
            {
                Labels[idx].Text = Path.GetFileName(title);
                ScaleFont(Labels[idx], 16);
                idx++;
            }

            //Timer = new Timer() { Interval = 100 };
            //Timer.Tick += this.TimerTick;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CurrentQuestion = Buttons[sender as Button];
            (sender as Button).Enabled = false;
            SetJeopardy(false);

            var title = TitleForButtonNumber(CurrentQuestion);

            try
            {
                var imgPath = Path.Combine(QuizDir(), title, ScoreForButtonNumber(CurrentQuestion).ToString() + ".png");
                pictureBox1.Visible = true;
                pictureBox1.Image = Bitmap.FromFile(imgPath);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;     
            }
            catch (Exception) { }

            string text = "Name creator for extra points!";
            string upText = "Which hack is this image from?";
            try
            {
                var answersFile = Path.Combine(QuizDir(), title, ScoreForButtonNumber(CurrentQuestion).ToString() + ".txt");
                var answers = File.ReadAllLines(answersFile);
                var name = answers[2];
                text = $"Name {name} for extra points!";
                if (answers.Count() > 3)
                    upText = $"Which {answers[3]} is this image from?";
            }
            catch (Exception)
            { }

            labelPictureT.Text = upText;
            ScaleFont(labelPictureT, 36);
            labelTop2.Text = title;
            ScaleFont(labelTop2, 18);
            labelPictureB.Text = text;
            ScaleFont(labelPictureB, 36);

            buttonAnswer.Enabled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            labelNameL.Text = textBoxNameL.Text;
            ScaleFont(labelNameL, 28);
            try
            {
                var img = Bitmap.FromFile(Path.Combine(ExeDir(), textBoxNameL.Text + ".png"));
                pictureBoxL.Image = img;
                pictureBoxL.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch(Exception) { }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            labelNameR.Text = textBoxNameR.Text;
            ScaleFont(labelNameR, 28);
            try
            {
                var img = Bitmap.FromFile(Path.Combine(ExeDir(), textBoxNameR.Text + ".png"));
                pictureBoxR.Image = img;
                pictureBoxR.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception) { }
        }

        private void buttonAnswer_Click(object sender, EventArgs e)
        {
            try
            {
                var title = TitleForButtonNumber(CurrentQuestion);
                var answersFile = Path.Combine(QuizDir(), title, ScoreForButtonNumber(CurrentQuestion).ToString() + ".txt");
                var answers = File.ReadAllLines(answersFile);

                labelPictureT.Text = answers[0];
                ScaleFont(labelPictureT, 36);
                labelTop2.Text = "";

                labelPictureB.Text = answers[1];
                ScaleFont(labelPictureB, 36);
            }
            catch(Exception) { }

            buttonAnswer.Enabled = false;
            buttonPoints1.Enabled = true;
            buttonPoints2.Enabled = true;
            buttonClear.Enabled = true;
        }

        private void AdjustScore(bool right, int cnt)
        {
            try
            {
                var tb = right ? textBoxScoreR : textBoxScoreL;
                var score = int.Parse(tb.Text);
                score += cnt;
                tb.Text = score.ToString();
            }
            catch (Exception) { }
        }

        private void buttonPoints1_Click(object sender, EventArgs e)
        {
            AdjustScore(GetTurn(), ScoreForButtonNumber(CurrentQuestion) * 100 * GetMultiplier());
            SetTurn(!GetTurn());
            DisableGuessingControls();
            SetJeopardy(true);
        }

        private void buttonPoints2_Click(object sender, EventArgs e)
        {
            AdjustScore(GetTurn(), ScoreForButtonNumber(CurrentQuestion) * 200 * GetMultiplier());
            SetTurn(!GetTurn());
            DisableGuessingControls();
            SetJeopardy(true);
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            SetTurn(!GetTurn());
            DisableGuessingControls();
            SetJeopardy(true);
        }

        private void textBoxScoreL_TextChanged(object sender, EventArgs e)
        {
            labelScoreL.Text = "Score: " + textBoxScoreL.Text;
            ScaleFont(labelScoreL, 28);
        }

        private void textBoxScoreR_TextChanged(object sender, EventArgs e)
        {
            labelScoreR.Text = "Score: " + textBoxScoreR.Text;
            ScaleFont(labelScoreR, 28);
        }

        private void buttonTurnL_Click(object sender, EventArgs e)
        {
            SetTurn(false);
        }

        private void buttonTurnR_Click(object sender, EventArgs e)
        {
            SetTurn(true);
        }
    }
}
