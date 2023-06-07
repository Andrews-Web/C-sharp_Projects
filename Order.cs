using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CoffeeShop
{
    interface interface1
    {
        double TotalOrderSummary();
    }

    public class Order : interface1
    {
        String type, size;
        bool ingredients;
        int quantity;

        public Order(String type, String size, bool ingredients, int quantity)
        {
            this.type = type;
            this.size = size;
            this.ingredients = ingredients;
            this.quantity = quantity;
        }
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-32I3APT;Initial Catalog=CoffeeShopDB;Integrated Security=True");



        public double GetPrice()
        {
            String cmdString = "";
            String colName = "";
            int price = 0;

            con.Open();

            switch (this.size)
            {
                case "Small":
                    cmdString = "select Small_Price from CoffeePriceTables where Coffee_Type = '";
                    colName = "Small_Price";
                    break;
                case "Medium":
                    cmdString = "select Medium_Price from CoffeePriceTables where Coffee_Type = '";
                    colName = "Medium_Price";
                    break;
                case "Large":
                    cmdString = "select Large_Price from CoffeePriceTables where Coffee_Type = '";
                    colName = "Large_Price";
                    break;
            }
            
            try
            {
                
                SqlCommand cmd = new SqlCommand(cmdString + type + "'", con);
                SqlDataReader read = cmd.ExecuteReader();
                while(read.Read())
                    price = int.Parse(read[colName].ToString());
                read.Close();
                
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            
            con.Close();
            return price;

        }

        
        
       

        public virtual double TotalOrderSummary()
        {
            return GetPrice() * quantity;
        }

        public virtual String TotalOrderSummary(double total)
        {
            return "\n"+quantity + "\t" + type + "," + size + "\tR" + GetPrice() +"\tR"+ total;
        }
    }
}
