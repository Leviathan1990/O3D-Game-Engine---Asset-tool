/*  Outforce Worker Code
 * 
 *  The Outforce O3D Engine Asset Tool.
 *  Designed by: Krisztian Kispeti
 *  Location: Kaposvár, HU.
 *  Contact: admin@theoutforce.hu
 *  Website: www.theoutforce.hu
 */



namespace AssetTool
{
    public partial class NProject : Form
    {
        BoxBuilder bBuilder = new BoxBuilder();
        public NProject()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            bBuilder.ShowDialog();
            this.Close();

        }
    }
}