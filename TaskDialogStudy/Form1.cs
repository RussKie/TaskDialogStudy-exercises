using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskDialogStudy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. Create a simple task dialog

            var result = TaskDialog.ShowDialog(this,
                text: "Are you sure you want to do this?",
                mainInstruction: "Stopping the operation might break things...",
                caption: "Confirmation",
                buttons: TaskDialogButtons.Yes | TaskDialogButtons.No,
                icon: TaskDialogIcon.Warning
                );

            Debug.WriteLine($"{MethodInfo.GetCurrentMethod().Name}: result: {result}");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 2. Create a simple task dialog with custom buttons

            var button1 = new TaskDialogCustomButton("&Save", "Save the document");
            var button2 = new TaskDialogCustomButton("Do&n't save");
            var button3 = new TaskDialogCustomButton("Cancel")
            {
                DefaultButton = true
            };

            // [!] The following doesn't work...
            //
            //var result = TaskDialog.ShowDialog(this,
            //    text: "Are you sure you want to do this?",
            //    mainInstruction: "Stopping the operation might break things...",
            //    caption: "Confirmation",
            //    buttons: button1,
            //    icon: TaskDialogIcon.Warning
            //    );

            var taskDialog = new TaskDialog();

            // [!] This feels unnecessary verbose
            taskDialog.Page.Text = "Are you sure you want to do this?";
            taskDialog.Page.MainInstruction = "Stopping the operation might break things...";
            taskDialog.Page.Caption = "Confirmation";
            taskDialog.Page.CustomButtons.Add(button1);
            taskDialog.Page.CustomButtons.Add(button2);
            taskDialog.Page.CustomButtons.Add(button3);

            var result = taskDialog.ShowDialog(this);
            Debug.WriteLine($"{MethodInfo.GetCurrentMethod().Name}: result: {result}");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 3. Create a simple task dialog 
            //    a. a custom content,
            //    b. a verification text, and
            //    c. a footer with a help link (e.g. go to https://dot.net/)

            var taskDialog = new TaskDialog();

            // [!] This feels unnecessary verbose
            taskDialog.Page.Text = "Are you sure you want to do this?";
            taskDialog.Page.MainInstruction = "Stopping the operation might break things...";
            taskDialog.Page.Caption = "Confirmation";

            // [!] These are inconsistent
            // I would expect .AllowCancel and .AllowMinimize
            taskDialog.Page.CanBeMinimized = true; // [!] How does this work????
            taskDialog.Page.AllowCancel = true;

            // [!] This is unexpected. I was expecting something like:
            //      taskDialog.Page.StandardButtons = TaskDialogButtons.Yes | TaskDialogButtons.No
            // - or -
            //      taskDialog.Page.StandardButtons.Add(TaskDialogButtons.Yes, default: true);
            //      taskDialog.Page.StandardButtons.Add(TaskDialogButtons.No);
            //
            // [!] I couldn't understand the purpose of TaskDialogStandardButton, it looks like a container for
            // TaskDialogResult, but it provides no real benefit to a caller.
            taskDialog.Page.StandardButtons.Add(TaskDialogResult.Yes);
            TaskDialogStandardButton button1 = taskDialog.Page.StandardButtons.Add(TaskDialogResult.No);
            button1.DefaultButton = true;

            taskDialog.Page.EnableHyperlinks = true;
            taskDialog.Page.HyperlinkClicked += (s, e) =>
            {
                Debug.WriteLine($"{MethodInfo.GetCurrentMethod().Name}: hyperlink clicked: {e.Hyperlink}");
            };

            // [?] It took me sometime to figure how to make hyperlinks
            var footer = new TaskDialogFooter("<a href=\"https://getdot.net/\">Download .NET!</a>");
            taskDialog.Page.Footer = footer;
            footer.Icon = TaskDialogIcon.Error;

            // [!] The official docs calls this "Verification Text" so I was looking for
            // a property/method with a matching name.
            // I tried CheckBox out of desperation, and wasn't sure it would fit the bill...
            taskDialog.Page.CheckBox = new TaskDialogCheckBox("Prompt me again")
            {
                Checked = true
            };

            var result = taskDialog.ShowDialog(this);
            Debug.WriteLine($"{MethodInfo.GetCurrentMethod().Name}: result: {result}");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // 4. Create a task dialog with command link buttons

            var taskDialog = new TaskDialog();

            taskDialog.Page.Text = "What level of difficulty do you want to play?";
            taskDialog.Page.Caption = "Minesweeper";
            // [?] Sadly this doesn't work
            //taskDialog.Page.Footer = "Note"; 
            taskDialog.Page.Footer = new TaskDialogFooter("Note: you can change the difficulty level later by clicking Options on the Game menu");

            taskDialog.Page.CustomButtonStyle = TaskDialogCustomButtonStyle.CommandLinks;
            var button1 = taskDialog.Page.CustomButtons.Add("&Beginner", "10 mines, 9 x 9 title grid");
            var button2 = taskDialog.Page.CustomButtons.Add("&Intermediate", "10 mines, 1 x 16 title grid");
            var button3 = taskDialog.Page.CustomButtons.Add("I&nsane", "259 mines, 16 x 30 title grid");

            var result = taskDialog.ShowDialog(this);
            Debug.WriteLine($"{MethodInfo.GetCurrentMethod().Name}: result: {result}");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // 5. Create a simple self-closing task dialog with a progress bar

            var taskDialog = new TaskDialog();

            var seconds = 5;

            // [!] This feels unnecessary verbose
            taskDialog.Page.MainInstruction = "Connection lost. Reconnecting...";
            taskDialog.Page.Text = $"Reconnecting in {seconds} seconds";
            taskDialog.Page.Caption = "Opps";
            taskDialog.Page.ProgressBar = new TaskDialogProgressBar
            {
                Minimum = 0,
                Maximum = 100,
                State = TaskDialogProgressBarState.Normal

                // [!] How to make it Yellow?
            };

            // [!] It is difficult to show a bitmap, there should be an overload that takes typeof(Image)
            // Perhaps simplify to
            //      taskDialog.Page.Icon = Properties.Resource1.Network;
            taskDialog.Page.Icon = new TaskDialogIcon(Icon.FromHandle(Properties.Resource1.Network.GetHicon()));

            var button1 = new TaskDialogCustomButton("&Reconnect now", "Save the document");
            var button2 = new TaskDialogCustomButton("Cancel");
            taskDialog.Page.CustomButtons.Add(button1);
            taskDialog.Page.CustomButtons.Add(button2);

            var interval = taskDialog.Page.ProgressBar.Maximum / seconds;
            var timer = new Timer();
            timer.Interval = interval;
            timer.Tick += (s, e) =>
            {
                taskDialog.Page.ProgressBar.Value++;

                if (taskDialog.Page.ProgressBar.Value % interval == 0)
                {
                    seconds--;
                    taskDialog.Page.Text = $"Reconnecting in {seconds} seconds";
                }


                if (taskDialog.Page.ProgressBar.Value >= taskDialog.Page.ProgressBar.Maximum)
                {
                    timer.Stop();
                    taskDialog.Close();
                }
            };
            timer.Start();

            taskDialog.Closed += (s, e) =>
            {
                timer.Stop();
            };

            var result = taskDialog.ShowDialog(this);
            Debug.WriteLine($"{MethodInfo.GetCurrentMethod().Name}: result: {result}");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // 6. Implement a multi page task dialog:
            //    a.  "Yes" button is enabled after checkbox is checked
            //    b. Progress bar initialises as marquee for few seconds, then runs from 0 to 100%
            //    c. Show results command link button

            var taskDialog = new TaskDialog();

            // write code here...

            taskDialog.ShowDialog(this);
        }
    }
}
