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

            // write code here...

            taskDialog.ShowDialog(this);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // 4. Create a task dialog with command link buttons

            var taskDialog = new TaskDialog();

            // write code here...

            taskDialog.ShowDialog(this);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // 5. Create a simple self-closing task dialog with a progress bar

            var taskDialog = new TaskDialog();

            // write code here...

            taskDialog.ShowDialog(this);
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
