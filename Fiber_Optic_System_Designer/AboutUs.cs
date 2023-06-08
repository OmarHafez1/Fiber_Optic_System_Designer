using System.Diagnostics;

namespace Fiber_Optic_System_Designer
{
    public partial class AboutUsPanel : UserControl
    {
        public AboutUsPanel()
        {
            InitializeComponent();
            WebBrowser webBrowser = new WebBrowser();
            webBrowser.DocumentText = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n  <style>\r\n    body {\r\n      font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;\r\n    }\r\n  </style>\r\n</head>\r\n<body>\r\n  <h2>About Me</h2>\r\n\r\n  <p>\r\n    I am Omar A. Hafez, a dedicated software engineer with a strong passion for discrete mathematics and algorithms. I thrive on solving complex problems and have garnered significant expertise in this area. As an enthusiast of code competitions, I have achieved the rank of expert on Codeforces and ranked 12th in Egypt on Project Euler by successfully solving over 130 problems.\r\n  </p>\r\n\r\n  <h2>Professional Background</h2>\r\n\r\n  <p>\r\n    With a focus on continuous growth, I am currently engaged in developing Flutter applications. I am driven by a desire to expand my knowledge and skillset, and in the near future, I plan to delve into the captivating field of AI. This interest stems from my fascination with the potential of AI and its transformative impact across various industries.\r\n  </p>\r\n\r\n  <h2>Academic Achievements</h2>\r\n\r\n  <p>\r\n    In my academic journey, I excelled in my class, securing the second position with a remarkable GPA of 3.88 out of 4. This accomplishment is a testament to my dedication to learning and my commitment to achieving excellence in my chosen field.\r\n  </p>\r\n\r\n  <h2>Areas of Expertise</h2>\r\n\r\n  <ul>\r\n    <li>Software Engineering: Proficient in developing robust and scalable applications.</li>\r\n    <li>Discrete Mathematics: Well-versed in applying mathematical concepts to solve complex problems.</li>\r\n    <li>Algorithms: Experienced in designing efficient algorithms for optimal solutions.</li>\r\n    <li>Problem Solving: Skilled in approaching problems analytically and finding creative solutions.</li>\r\n  </ul>\r\n\r\n  <h2>Contact Me</h2>\r\n\r\n  <p>\r\n    I welcome any inquiries, collaboration opportunities, or discussions related to my work. Please feel free to reach out to me through any of the following channels:\r\n  </p>\r\n\r\n  <ul>\r\n    <li>Email: <a href=\"mailto:omar.a.hafez.1@gmail.com\">omar.a.hafez.1@gmail.com</a></li>\r\n    <li>GitHub: <a href=\"https://github.com/OmarHafez1\">OmarHafez1</a></li>\r\n    <li>Codeforces: <a href=\"https://codeforces.com/profile/Omar_Hafez\">Codeforces Profile</a></li>\r\n    <li>Project Euler: <a href=\"https://projecteuler.net/profile/Omar_Hafez.png\">Project Euler Profile</a></li>\r\n    <li>Stack Overflow: <a href=\"https://stackoverflow.com/users/11877678/omar-hafez\">Stack Overflow Profile</a></li>\r\n    <li>Math Stack Exchange: <a href=\"https://math.stackexchange.com/users/702548/omar-hafez\">Math Stack Exchange Profile</a></li>\r\n    <li>Code Review Stack Exchange: <a href=\"https://codereview.stackexchange.com/users/209295/omar-hafez\">Code Review Stack Exchange Profile</a></li>\r\n    <li>Phone: +201012274320</li>\r\n  </ul>\r\n\r\n  <p>\r\n    Thank you for taking the time to learn more about me. I am eager to contribute my skills and expertise to the world of software engineering and problem-solving. Let's connect and explore opportunities to make a positive impact together.\r\n  </p>\r\n</body>\r\n</html>\r\n";
            webBrowser.Dock = DockStyle.Fill;
            webBrowser.Navigating += WebBrowser_Navigating;
            this.Controls.Add(webBrowser);
            webBrowser.Size = webBrowser.Parent.Size;
        }

        private void WebBrowser_Navigating(object? sender, WebBrowserNavigatingEventArgs e)
        {
            e.Cancel = true;
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = e.Url.ToString(),
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
