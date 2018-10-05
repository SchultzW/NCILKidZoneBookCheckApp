using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LinkCheckApp
{

    public partial class LinkCheckForm : Form
    {
        /// <summary>
        /// Enter the title of the book your looking for exactly
        /// </summary>
        public LinkCheckForm()
        {
            InitializeComponent();
            GCDriver = new ChromeDriver();
            GCDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            

        }
        string titleCheck = "";
        static IWebDriver GCDriver;


        /*
         * to do: clear text boxes after search. 
         *         Rearrange the navigate button so maybe can use the same window each time
         *         create button that closes driver and form
         * 
         */
        private void search_Click(object sender, EventArgs e)
        {
            if(inputTextBox.Text.Length>0&&listenInputTextBox.Text.Length>0)
            {
                MessageBox.Show("Sorry You can only search one at time.");
                inputTextBox.Clear();
                listenInputTextBox.Clear();
            }
            else if(inputTextBox.Text.Length>0)
            {
                SearchRead();
            }
            else if(listenInputTextBox.Text.Length>0)
            {
                SearchListen();
            }
            else if(string.IsNullOrEmpty(inputTextBox.Text)&&string.IsNullOrEmpty(listenInputTextBox.Text))
            {
                MessageBox.Show("Please enter a book to search.");

            }


            //GCDriver.Navigate().GoToUrl("https://improvingliteracy.org/kid-zone/read");
            //bool worldCatFlag, googleFlag;
            ////worldCatOutput.Text = ("");
            ////googleOutputBox.Text = ("");
            //titleCheck = inputTextBox.Text;
            ////GCDriver = new ChromeDriver();
            ////GCDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            ////GCDriver.Navigate().GoToUrl("https://improvingliteracy.org/kid-zone/read");
            //try
            //{
            //    for (int i = 1; i <= 8; i++)
            //    {
            //        int count = i + 1;
            //        System.Threading.Thread.Sleep(1000);
            //        IList<IWebElement> allPageHref = GCDriver.FindElements(By.TagName("a"));
            //        List<string> linkTitles = HrefParser(allPageHref);//returns list of strings with link titles
            //        bool flag = IsBookPresent(linkTitles, titleCheck);
            //        if (flag == true)
            //            break;
            //        else
            //        {
            //            GCDriver.FindElement(By.LinkText(count.ToString())).Click();


            //            linkTitles.Clear();
            //        }

            //    }
            //}
            //catch
            //{
            //    MessageBox.Show("The book could not be found.");
            //}
           
            ////IList<IWebElement> allPageHref = GCDriver.FindElements(By.TagName("a"));
            ////List<string>linkTitles = HrefParser(allPageHref);//returns list of strings with link titles
         
            //GoToPage(titleCheck);
            //GCDriver.FindElement(By.LinkText("Find Book at Your Library")).Click();
            //System.Threading.Thread.Sleep(6000);
            ////checks if there is a title elment if there isnt it prints the name of the book. if it is it goes back.
            ////worldCatFlag = WolrdCatCheck();
            ////googleFlag = GoogleCheck();
            ////if(worldCatFlag==true)
            ////{
            ////    worldCatOutput.Text=("WorldCat link check worked");
            ////    worldCatOutput.ForeColor = Color.Green;
            ////}
            ////else
            ////{
            ////    worldCatOutput.Text = ("WorldCat link check faled");
            ////    worldCatOutput.ForeColor = Color.Red;
            ////}
            ////googleFlag = GoogleCheck();
            ////if (googleFlag == true)
            ////{
            ////    googleOutputBox.Text = ("GoogleBooks link check worked");
            ////    googleOutputBox.ForeColor = Color.Green;
            ////}
            ////else
            ////{
            ////    googleOutputBox.Text = ("GoogleBooks link check faled");
            ////    googleOutputBox.ForeColor = Color.Red;
            ////}
            ////GCDriver.Close();

        }
        public static List<string> HrefParser(IList<IWebElement> linksToCheck)
        {
            //this method puts all the href on the page into a list and strips out in blanks
            List<int> indexList = new List<int>();//list of indexes with book titles
            List<string> HrefList = new List<string>();//all book 
            List<string> TitleList = new List<string>();
            
            //adds all hrefs to list
            foreach (IWebElement link in linksToCheck)
            {
                HrefList.Add(link.Text);
            }

            //first 6 in list are removed they arent books
            int i = 0;
            do
            {
                HrefList.RemoveAt(0);

                i++;
            }
            while (i < 8);
            //complicated way of getting the index of the books with titles and adding them no a new list. this keeps structure of list intact an array may have been better but too late now
            for (int j = 0; j < HrefList.Count - 1; j++)
            {
                if (HrefList[j] != "")
                {
                    indexList.Add(j);
                }


            }
            foreach (int index in indexList)
            {
                int count = index - 1;
                string title = HrefList[index];
                TitleList.Add(title);


            }
            return TitleList;


        }
        public static bool IsBookPresent(List<string>listToCheck, string titleToCheck)
        {
            //this method checks the page to see if the book title is present
            bool flag = false;
            foreach(string title in listToCheck)
            {
                if (title == titleToCheck)
                {
                    flag = true;
                    break;
                }
                else
                    flag = false;
            }
            return flag;
            


        }
        public static void GoToPage(string bookTitle)
        {
            GCDriver.FindElement(By.LinkText(bookTitle)).Click();



        }
        public static bool WolrdCatCheck()
        {
            //this should check the world cat page for a class title. it returns true if a book title is present
            try
            {
                bool flag = true;
                flag = GCDriver.FindElement(By.ClassName("title")).Displayed;
                if (flag == false)
                {
                    
                    GCDriver.SwitchTo().Window(GCDriver.WindowHandles[1]);
                    GCDriver.Close();
                    GCDriver.SwitchTo().Window(GCDriver.WindowHandles[0]);
                    return false;
                }
                else
                {
                    GCDriver.SwitchTo().Window(GCDriver.WindowHandles[1]);
                    GCDriver.Close();
                    GCDriver.SwitchTo().Window(GCDriver.WindowHandles[0]);
                    return true;

                }
            }
            catch
            {
                MessageBox.Show(" something went wrong...");
                
                GCDriver.SwitchTo().Window(GCDriver.WindowHandles[1]);
                GCDriver.Close();
                GCDriver.SwitchTo().Window(GCDriver.WindowHandles[0]);
                return false;
            }


        }
        public static bool GoogleCheck()
        {
            GCDriver.FindElement(By.ClassName("bookpreview")).Click();
            GCDriver.SwitchTo().Window(GCDriver.WindowHandles[1]);

            try
            {
                bool flagGoogle1 = GCDriver.FindElement(By.ClassName("gback")).Displayed;
                if (flagGoogle1 == true)
                {
                    GCDriver.SwitchTo().Window(GCDriver.WindowHandles[1]);
                    GCDriver.Close();
                    GCDriver.SwitchTo().Window(GCDriver.WindowHandles[0]);
                    return true;

                }
                else
                {
                    
                    GCDriver.SwitchTo().Window(GCDriver.WindowHandles[1]);
                    GCDriver.Close();
                    GCDriver.SwitchTo().Window(GCDriver.WindowHandles[0]);
                    return false;
                }
            }
            catch
            {
                
                
                GCDriver.SwitchTo().Window(GCDriver.WindowHandles[1]);
                GCDriver.Close();
                GCDriver.SwitchTo().Window(GCDriver.WindowHandles[0]);
                return false;
            }
        }
        public void SearchRead()
        {
            GCDriver.Navigate().GoToUrl("https://improvingliteracy.org/kid-zone/read");
            
            
            titleCheck = inputTextBox.Text;
            
            try
            {
                for (int i = 1; i <= 8; i++)
                {
                    int count = i + 1;
                    System.Threading.Thread.Sleep(1000);
                    IList<IWebElement> allPageHref = GCDriver.FindElements(By.TagName("a"));
                    List<string> linkTitles = HrefParser(allPageHref);//returns list of strings with link titles
                    bool flag = IsBookPresent(linkTitles, titleCheck);
                    if (flag == true)
                    {
                        break;
                        

                    }

                    else
                    {
                        GCDriver.FindElement(By.LinkText(count.ToString())).Click();


                        linkTitles.Clear();
                    }
                    

                }
                GoToPage(titleCheck);
                inputTextBox.Clear();
                
            }
            catch
            {
                MessageBox.Show("The book could not be found.");
            }
            
        }
        public void SearchListen()
        {
            GCDriver.Navigate().GoToUrl("https://improvingliteracy.org/kid-zone/listen");


            titleCheck = listenInputTextBox.Text;

            try
            {
                for (int i = 1; i <= 8; i++)
                {
                    int count = i + 1;
                    System.Threading.Thread.Sleep(1000);
                    IList<IWebElement> allPageHref = GCDriver.FindElements(By.TagName("a"));
                    List<string> linkTitles = HrefParser(allPageHref);//returns list of strings with link titles
                    bool flag = IsBookPresent(linkTitles, titleCheck);
                    if (flag == true)
                    {
                        break;


                    }

                    else
                    {
                        GCDriver.FindElement(By.LinkText(count.ToString())).Click();


                        linkTitles.Clear();
                    }


                }
                GoToPage(titleCheck);
                listenInputTextBox.Clear();

            }
            catch
            {
                MessageBox.Show("The book could not be found.");
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            GCDriver.Close();
            this.Close();
        }
    }
}
