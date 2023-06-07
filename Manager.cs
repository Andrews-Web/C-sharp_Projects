using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Security.Policy;

namespace CoffeeShop
{
    class Manager : Order
    {
        String period;
        DateTime date;
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-32I3APT;Initial Catalog=CoffeeShopDB;Integrated Security=True");

        public Manager(String type, String size, bool ingredients, int quantity, DateTime date) : base(type, size, ingredients, quantity)
        {
            this.date = date;
        }




        public void SaveToTXT(String period)
        {
            try
            {
                String command = "";
                String id="", quantity = "", Description = "", Price = "", Total = "", d = "";
                con.Open();
                switch (period)
                {
                    case "Daily":
                        command = "select * from SalesTables where Date = '" + date + "'";
                        break;
                    case "Monthly":
                        command = "select * from SalesTables where month(Date) = '" + date.Month + "'";
                        break;
                    case "Yearly":
                        command = "select * from SalesTables where year(Date) = '" + date.Year + "'";
                        break;
                }
                SqlCommand cmd = new SqlCommand(command, con);
                SqlDataReader read = cmd.ExecuteReader();
                StreamWriter ew = new StreamWriter("C:/Users/andro/source/repos/CofeeShop/CofeeShop/bin/Debug/BackUpSale.txt", true);
                while (read.Read())
                {
                    id = read["Id_Sale(PK)"].ToString();
                    quantity = read["Quantity"].ToString();
                    Description = read["Description"].ToString();
                    Price = read["Price"].ToString();
                    Total = read["Total"].ToString();
                    ew.WriteLine(id+"," + quantity + "," + Description+ ",R" + Price + ",R" +Total);
                }
                ew.Close();
                read.Close();
                MessageBox.Show("Data Saved");
            }catch (Exception ex)
            { 
                MessageBox.Show(ex.Message);
                MessageBox.Show("Enter a date and select a period");
            }

            

            
            
        }

        public void SaveToDB(int id,int q,String d, int price, int total,DateTime date)
        {

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("insert into SalesTables ([Id_Sale(PK)] , Quantity, Description, Price, Total, Date) values ('" + id + "','" + q + "','" + d + "', '" + price + "', '" + total + "','"+date.ToString("yyyy - MM - dd HH: mm:ss")+"')", con);
                cmd.ExecuteNonQuery();
                con.Close();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }




        public String DisplayInfos(String period)
        {
            String id = "", quantity = "", Description = "", Price = "", Total = "";

            String command = "", display ="";
            try
            {
                con.Open();
                switch (period)
                {
                    case "Daily":
                        command = "select * from SalesTables where Date = '" + date + "'";
                        break;
                    case "Monthly":
                        command = "select * from SalesTables where month(Date) = '" + date.Month + "'";
                        break;
                    case "Yearly":
                        command = "select * from SalesTables where year(Date) = '" + date.Year + "'";
                        break;
                }
            
                SqlCommand cmd = new SqlCommand(command, con);
                SqlDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {
                    id = read["Id_Sale(PK)"].ToString();
                    quantity = read["Quantity"].ToString();
                    Description = read["Description"].ToString();
                    Price = read["Price"].ToString();
                    Total = read["Total"].ToString();
                    display += "\n"+quantity + "\t" + Description + "\t\tR" + Price + "\tR" + Total;
                }

                con.Close();    
                read.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return display;
            

            
        }




        public override double TotalOrderSummary()
        {
            return base.TotalOrderSummary();
        }



        public void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            string line = "";

            Font printFont = new Font("Arial", 10);
            MainWindow mainWindow = new MainWindow();
            StreamReader streamToPrint = new StreamReader(mainWindow.txtSum.Text);
            // Calculate the number of lines per page.
            linesPerPage = ev.MarginBounds.Height /
               printFont.GetHeight(ev.Graphics);

            // Print each line of the file.
            while (count < linesPerPage &&
               ((line = streamToPrint.ReadLine()) != null))
            {
                yPos = topMargin + (count *
                   printFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, printFont, Brushes.Black,
                   leftMargin, yPos, new StringFormat());
                count++;
            }

            // If more lines exist, print another page.
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }
    }


    

}
