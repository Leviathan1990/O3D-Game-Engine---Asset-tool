using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
// This version for: For asset extractor tool..

namespace ProgBarMechanism
{
    public static class Worker
    {
        public static void InitializeProgressBar(ProgressBar progressBar, int max)
        {
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Value = 0;
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.Step = 1;
        }

        public static void UpdateProgressBar(System.Windows.Forms.ProgressBar progressBar, int step)
        {
            if (progressBar.Value + step <= progressBar.Maximum)
            {
                progressBar.Value += step;
            }

            else
            {
                progressBar.Value = progressBar.Maximum;
            }

        }

    }
}