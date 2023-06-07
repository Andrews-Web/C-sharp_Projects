using System;
using System.IO;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Windows.Controls;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Drawing;
using System.Drawing.Printing;
using System.Printing;

namespace CoffeeShop
{
    class Global
    {
        public static bool Valid = false;

    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public int count = 0;
        public String[,] OrderArr = new string[100, 5];

        String display = "";

        public MainWindow()
        { 
            InitializeComponent();
        }
        


        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            String period = "Daily";

            if (rbday.IsChecked == true)
                period = "Daily";
            else if (rbMonth.IsChecked == true)
                period = "Monthly";
            else if (rbYear.IsChecked == true)
                period = "Yearly";
            
            if (Global.Valid == true)
            {
                Manager man = new Manager("","",true,0, dpDate.DisplayDate);
                man.SaveToTXT(period);
            }
            else
            {
                Login login = new Login();
                login.Show();
            }
            

            
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(txtSum.Text == "")
                {
                    throw new Exception();
                }
                else
                {
                    try
                    {
                           StreamReader streamToPrint = new StreamReader
                           (txtSum.Text);
                        try
                        {
                            Font printFont = new Font("Arial", 10);
                            PrintDocument pd = new PrintDocument();
                            Manager man = new Manager("", "", true, 0, dpDate.DisplayDate);
                            pd.PrintPage += new PrintPageEventHandler
                               (man.pd_PrintPage);
                            pd.Print();
                        }
                        finally
                        {
                            streamToPrint.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please display information before printing");
            }
            

        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            String period = "Daily", display = "";

            if (rbday.IsChecked == true)
                period = "Daily";
            else if (rbMonth.IsChecked == true)
                period = "Monthly";
            else if (rbYear.IsChecked == true)
                period = "Yearly";


            DateTime now = DateTime.Now;
            txtSum.AppendText(now.ToLongDateString().ToString());

            txtSum.AppendText("\nQtity\t|Description\t|Price\t|Total\t|");

            

            if (Global.Valid == true)
            {
                Manager man = new Manager("", "", true, 0, dpDate.DisplayDate);
                display += man.DisplayInfos(period);
            }
            else
            {
                Login login = new Login();
                login.Show();
            }
            txtSum.AppendText(display);


        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            String type = "", s = "";
            bool sugar = false, cream = false;
            bool ingredients = false;
            int q = 1;
            double total;
            try
            { 
                q = int.Parse(cmbQuantity.Text);
                type = cmbType.Text;

                if (rbSmall.IsChecked == true)
                    s = "Small";
                else if (rbMedium.IsChecked == true)
                    s = "Medium";
                else
                    s = "Large";

                if (cbSugar.IsChecked == true)
                    sugar = true;
                if (cbCream.IsChecked == true)
                    cream = true;


                if (type == "")
                    type = "Late";
                if (s == "")
                    s = "Small";

                
                if (sugar == true || cream == true)
                    ingredients = true;
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR!! Make sure all fields are entered");
            }
            

            Order order = new Order(type,s,ingredients,q);
            total = order.TotalOrderSummary();
            display += order.TotalOrderSummary(total);

            
                try
                {
                    OrderArr[count, 0] = OrderArr[count-1, 0] + 1;
                }
                catch (Exception)
                {
                    OrderArr[count, 0] = "6";
                }

                OrderArr[count, 1] = q.ToString();
                OrderArr[count, 2] = type;
                OrderArr[count, 3] = order.GetPrice().ToString();
                OrderArr[count, 4] = order.TotalOrderSummary().ToString();
                count++;

        }

        private void Window_Activated(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            txtOrder.Text = now.ToString();
            txtOrder.Text += "\nQtity\t|Description\t\t|Price\t|Total\t|";
        }



        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            String type="";
            int id = 0, quantity = 0, price = 0, total = 0;
            
            txtOrder.Text += display;
            display = "";
            DateTime date = DateTime.Now;
            for (int r = 0; r < count; r++)
            {
                id = int.Parse(OrderArr[r,0]);
                quantity = int.Parse(OrderArr[r,1]);
                type = OrderArr[r, 2];
                price = int.Parse(OrderArr[r, 3]);
                total = int.Parse(OrderArr[r, 4]);
                    
                Manager m = new Manager(type, "", true, quantity, dpDate.DisplayDate);
                m.SaveToDB(id, quantity, type, price, total, date);
            }
        }
    }
}
