using System.Drawing;

namespace ImageTest1
{
    public class FormManager
    {
        public static System.Collections.Generic.List<Form1> formList = new System.Collections.Generic.List<Form1>();

        public static Form1 GetNewForm()
        {
            Form1 form1 = new Form1();
            formList.Add(form1);
            return form1;
        }

        public static Form1 GetForm(int index)
        {
            return formList[index];
        }

        public static Form1 GetFirstForm()
        {
            return formList[0];
        }

        public static Form1 GetSecondForm()
        {
            if (formList.Count >= 2)
            {
                return formList[1];
            }
            else
            {
                return null;
            }
        }

        public static Form1 GetOtherForm(Form1 form)
        {
            if (form == formList[0])
            {
                return GetSecondForm();
            }
            else
            {
                return formList[0];
            }
        }

        public static void CloseForm(Form1 form)
        {
            formList.Remove(form);
            form.Close();
        }

        public static void ShowMessage(string message)
        {
            Form1 form1 = FormManager.GetFirstForm();

            form1.message = message;
            form1.messageForegroundBrush = Brushes.Black;
            form1.messageBackgroundBrush = Brushes.White;
            form1.Refresh();
        }

        public static void ShowMessage2(string message)
        {
            Form1 form1 = FormManager.GetFirstForm();

            if (form1.ProgramMode == ProgramModeType.Zengo)
            {
                form1.message2Count = 2;
                form1.message2 = message;
                form1.message2ForegroundBrush = Brushes.Black;
                form1.message2BackgroundBrush = Brushes.White;
                form1.Refresh();
            }
            else
            {
                form1.message = message;
                form1.messageForegroundBrush = Brushes.Black;
                form1.messageBackgroundBrush = Brushes.White;
                form1.Refresh();
            }
        }

        public static void ShowErrorMessage(string message)
        {
            Form1 form1 = FormManager.GetFirstForm();

            form1.message = message;
            form1.messageForegroundBrush = Brushes.Black;
            form1.messageBackgroundBrush = Brushes.Yellow;
            form1.Refresh();
        }

        public static void ShowErrorMessage2(string message)
        {
            Form1 form1 = FormManager.GetFirstForm();

            if (form1.ProgramMode == ProgramModeType.Zengo)
            {
                form1.message2Count = 2;
                form1.message2 = message;
                form1.message2ForegroundBrush = Brushes.Black;
                form1.message2BackgroundBrush = Brushes.Yellow;
                form1.Refresh();
            }
            else
            {
                form1.message = message;
                form1.messageForegroundBrush = Brushes.Black;
                form1.messageBackgroundBrush = Brushes.Yellow;
                form1.Refresh();
            }
        }

    }
}
