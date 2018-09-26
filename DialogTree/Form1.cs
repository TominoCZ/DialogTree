using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogTree
{
    public partial class Form1 : Form
    {
        private DialogTree _dialogTree;

        public Form1()
        {
            InitializeComponent();

            _dialogTree = new DialogTree();

            var m1 = new Message("Hi. How's it going?");
            var m2 = new Message("That's great!");
            var m3 = new Message("Well that sucks..");

            var a1 = new Answer("Ok", m2);
            var a2 = new Answer("Not Ok", m3);

            m1.Answers = new[] { a1, a2 };

            _dialogTree.SetTree(m1);

            UpdateDialog();
        }

        private void UpdateDialog()
        {
            var active = _dialogTree.GetCurrent();

            if (active == null)
                return;

            if (active.Answers.Length == 0)
                _dialogTree.Reset();
            
            lblMessage.Text = active.Text;

            lbAnswers.Items.Clear();

            foreach (var answer in active.Answers)
            {
                lbAnswers.Items.Add(answer.Text);
            }
        }

        private void btnAnswer_Click(object sender, EventArgs e)
        {
            var selected = lbAnswers.SelectedIndex;

            _dialogTree.Answer(selected);

            UpdateDialog();
        }
    }

    class DialogTree
    {
        private Message _first;

        public void SetTree(Message q)
        {
            _first = q;
        }

        public Message GetCurrent()
        {
            Message current = _first;

            while (current != null && current.Chosen != -1)
            {
                current = current.Next;
            }

            return current;
        }

        public void Answer(int index)
        {
            var current = GetCurrent();

            if (current == null)
                return;

            current.Chosen = index;
        }

        public void Reset()
        {
            Message current = _first;

            while (current != null && current.Chosen != -1)
            {
                current.Chosen = -1;
                current = current.Next;
            }
        }
    }

    class Message
    {
        public string Text;

        public Answer[] Answers;

        public Message Next => Chosen == -1 ? null : Answers[Chosen].Next;

        public int Chosen = -1;

        public Message(string text, params Answer[] answers)
        {
            Text = text;
            Answers = answers;
        }
    }

    class Answer
    {
        public string Text;

        public Message Next;

        public Answer(string text, Message next)
        {
            Text = text;
            Next = next;
        }
    }
}
