namespace CollatzConjectureProver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        CollatzCalculator collatzCalculator = new CollatzCalculator();

        private async void button1_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                collatzCalculator = new CollatzCalculator();
                //collatzCalculator.SearchByQ_BYOneShot(3, 1, 6, 7);
                collatzCalculator.calcallnumbers(13);
                //collatzCalculator.SearchByQ_Parallel(3, 1, 7, 8);
                //collatzCalculator.SearchByQ_Parallel(2, 1, 7, 7);
            });
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            richTextBox1.Text = "Proved Percentage (Decrease or Loop): " + collatzCalculator.PercentageProved * 100 + " %" +
                "\r\nAbsolute Percentage: " + collatzCalculator.RelativePercentage * 100 + " %" +
                "\r\nBase Percentage: " + collatzCalculator.Percentage * 100 + " %" +
                "\r\nTime : " + collatzCalculator.sp.ElapsedMilliseconds + " ms" +
                "\r\nHighest Pow : " + collatzCalculator.highest +
                "\r\nDepth Reached : " + collatzCalculator.DepthReached;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            float percentage = 0;
            float log2_15 = 0.5849625007f;
            for(int n=1;n<=100; n++)
            {
                float nz=log2_15 * n;
                for (int z=1;z<=100;z++)
                {
                    if (z > nz)
                    {
                        percentage += MathF.Pow(0.5f, n) * MathF.Pow(0.5f, z - 1);
                        break;
                    }
                }
            }
            MessageBox.Show(percentage + "");
        }
    }
}