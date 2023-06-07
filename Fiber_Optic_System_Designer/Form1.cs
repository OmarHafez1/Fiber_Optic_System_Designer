using Fiber_Optic_System_Designer.Themes;

namespace Fiber_Optic_System_Designer
{
    public partial class Form1 : Form
    {
        List<MyLeftButton> leftPanelChilds = new List<MyLeftButton>();
        public Form1()
        {
            InitializeComponent();

            SystemDesignPanel systemDesignPanel = new SystemDesignPanel();
            SettingsPanel settingsPanel = new SettingsPanel();
            AboutUsPanel aboutUsPanel = new AboutUsPanel();

            leftPanelChilds.Add(new MyLeftButton(home_button, home_button_label, deSelectAll, UpdateUserController, systemDesignPanel));
            leftPanelChilds.Add(new MyLeftButton(settings_button, settings_button_label, deSelectAll, UpdateUserController, settingsPanel));
            leftPanelChilds.Add(new MyLeftButton(aboutus_button, aboutus_button_label, deSelectAll, UpdateUserController, aboutUsPanel));

            UpdateUserController(systemDesignPanel);
            leftPanelChilds[0].select();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            base.BackColor = KColors.KBaseColor;
        }

        private void splitContainer1_Panel1_Paint_1(object sender, PaintEventArgs e)
        {
            splitContainer1.Panel1.BackColor = KColors.K2BaseColor;
            splitContainer1.SplitterMoved += splitContainer1_SplitterMoved;

        }
        private void splitContainer1_SplitterMoved(System.Object sender, System.Windows.Forms.SplitterEventArgs e)
        {
            updateImagesWidth();
            foreach (var leftButton in leftPanelChilds)
            {
                leftButton.updateWidth(splitContainer1.Panel1.Width);
            }
        }

        public void deSelectAll()
        {
            foreach (var leftButton in leftPanelChilds)
            {
                leftButton.deSelect();
            }
        }

        public void UpdateUserController(UserControl userControl)
        {
            splitContainer1.Panel2.Controls.Clear();
            userControl.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add(userControl);
            userControl.BringToFront();
        }

        private void systemDesign1_Load(object sender, EventArgs e)
        {

        }

        public void updateImagesWidth()
        {
            //  pictureBox1.Height = Parent.Width;

        }

    }

}