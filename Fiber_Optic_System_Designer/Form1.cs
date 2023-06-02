using System.ComponentModel;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Fiber_Optic_System_Designer
{
    public partial class Form1 : Form
    {
        List<LeftButton> leftPanelChilds = new List<LeftButton>();
        public Form1()
        {
            InitializeComponent();
            leftPanelChilds.Add(new LeftButton(home_button, home_button_label, deSelectAll));
            leftPanelChilds.Add(new LeftButton(settings_button, settings_button_label, deSelectAll));
            leftPanelChilds.Add(new LeftButton(aboutus_button, aboutus_button_label, deSelectAll));
            leftPanelChilds[0].select();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            base.BackColor = KColors.KBaseColor;
        }

        private void splitContainer1_Panel1_Paint_1(object sender, PaintEventArgs e)
        {
            splitContainer1.Panel1.BackColor = KColors.KLeftPanelColor;
            splitContainer1.SplitterMoved += splitContainer1_SplitterMoved;

        }
        private void splitContainer1_SplitterMoved(System.Object sender, System.Windows.Forms.SplitterEventArgs e)
        {
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

        private void systemDesign1_Load(object sender, EventArgs e)
        {

        }
    }

    class LeftButton
    {
        public Panel button;
        public Label label;
        public int width;
        bool isSelected = false;
        Action deSelectAll;
        public LeftButton(Panel button, Label label, Action deSelectAll)
        {
            this.button = button;
            this.label = label;
            this.deSelectAll = deSelectAll;
            setLeftButtonThemes();
        }
        public void deSelect()
        {
            isSelected = false;
            updateColor();
        }

        public void select()
        {
            isSelected = true;
            updateColor();
        }

        public void updateWidth(int width)
        {
            this.width = width;
            button.Width = width;
        }

        public void updateColor()
        {
            label.BackColor = isSelected ? KColors.KLeftButtonColor_Selected : KColors.KLeftButtonColor_NotSelected;
        }

        public void setLeftButtonThemes()
        {
            label.Padding = new Padding(0, 5, 0, 5);
            label.ForeColor = Color.White;
            label.AutoSize = false;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Dock = DockStyle.Fill;
            updateColor();
            updateWidth(button.Parent.Width);
            button.Height = 65;
            PropertyChangedEventHandler pp;
            label.MouseEnter += delegate (object? sender, EventArgs e)
            {
                label.BackColor = KColors.LightenBy(label.BackColor, 10);
            };
            label.MouseLeave += delegate (object? sender, EventArgs e)
            {
                updateColor();
            };
            label.MouseClick += delegate (object? sender, MouseEventArgs e)
            {
                deSelectAll();
                select();
            };

        }
    }
}