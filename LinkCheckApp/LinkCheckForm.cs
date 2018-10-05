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
        /// <summary>
        /// checks if the book is present on the current page
        /// </summary>
        /// <param name="listToCheck"></param>
        /// <param name="titleToCheck"></param>
        /// <returns></returns>
        public static bool IsBookPresent(List<string>listToCheck, string titleToCheck)
        {

            //this method checks the page to see if the book title is present
            bool flag = false;
            foreach(string title in listToCheck)
            {
                
                if (title.Contains(titleToCheck))
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
        /// <summary>
        /// was to check the worldcar page but not being used at the moment
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// was to check the google page but not being used at the moment
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// searches the read page for the title
        /// </summary>
        public void SearchRead()
        {
            GCDriver.Navigate().GoToUrl("https://improvingliteracy.org/kid-zone/read");
            
            
            titleCheck = inputTextBox.Text;
            titleCheck = StringChanger(titleCheck);
            
            
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
        /// <summary>
        /// searches the listen page for the title
        /// 
        /// </summary>
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
        /// <summary>
        /// Takes a string and changes it to title font
        /// </summary>
        /// <param name="myString"></param>
        /// <returns></returns>
        public string StringChanger(string myString)
        {
            string newString="";
            string workingString;
        
            string[] myStringArray = myString.Split(' ');
             workingString = myStringArray[0];
            //char[] charArray = workingString.ToCharArray();

            char letter = workingString[0];
            letter=char.ToUpper(letter);
            newString.Remove(0, 0);
            
            newString=newString.Insert(0, letter.ToString());

            //newString +=char.ToUpper(charArray[0]);
            //foreach(char letter in charArray)
            //{
            //    newString += letter;
            //}

            for (int i = 1; i < myStringArray.Length; i++)
            {
                newString += " ";
                if (myStringArray[i] == "in"| myStringArray[i]=="by"| myStringArray[i]=="the"| myStringArray[i]=="of"| myStringArray[i]=="at"| myStringArray[i]=="from"| myStringArray[i]=="and")
                {
                    newString += myStringArray[i];
                }
                else
                {
                    char[] charArray = myStringArray[i].ToCharArray();
                    newString += char.ToUpper(charArray[0]);
                    
                    foreach (char aLetter in charArray.Skip<char>(1))
                    {
                        newString += aLetter;
                    }
                }
            }
            newString.Trim();
            return newString;

        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            GCDriver.Close();
            this.Close();
        }
    }
}
