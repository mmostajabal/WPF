using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Classes
{
    public class FolderOperation
    {
        /**********************************************************************
        * GetFolder
        * ********************************************************************/
        public string GetFolder()
        {
            string path = "";

            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    path = dialog.SelectedPath;
                }
            }

            return path;
        }

    }
}
