using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CoffeeShop
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-32I3APT;Initial Catalog=CoffeeShopDB;Integrated Security=True");

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            String name, pass;
            bool validName = false,validPass = false;

            name = txtName.Text;
            pass = txtPass.Text;

            con.Open();
            try
            {
                SqlCommand cmdName = new SqlCommand("select Username from ManagerTable where Username = '"+name+"'",con);
                var newName = cmdName.ExecuteScalar();
                if (newName != null)
                    validName = true;
                else
                    MessageBox.Show("Username does not exist!!");
                
                SqlCommand cmdPass = new SqlCommand("select Password from ManagerTable where Password = '" + pass + "'",con);
                var newPass = cmdPass.ExecuteScalar();
                if (newPass != null)
                    validPass = true;
                else
                    MessageBox.Show("Password does not exist!!");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            con.Close();

            if (validName == true && validPass == true)
            {
                Global.Valid = true;
                Hide();
                MainWindow mw = new MainWindow();
                mw.btn1.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                MessageBox.Show("You are logged in");
            }

                

        }

        private void btnConfirm_Copy_Click(object sender, RoutedEventArgs e)
        {
            String name = txtName.Text, pass = txtPass.Text;
            int id = 0;
            con.Open();
            SqlCommand cmd = new SqlCommand("select Id_Man(PK) from ManagerTable ORDER BY Id_Man(PK) DESC LIMIT 1", con);
            SqlDataReader read = cmd.ExecuteReader();
            while(read.Read())
                id = int.Parse(read["Id_Man(PK)"].ToString());
            con.Close();
            try
            {
                con.Open();
                SqlCommand ins = new SqlCommand("insert into ManagerTable (Id_Man(PK),Username, Password) values ('" + id + "','" + name + "','" + pass+ "'", con);
                ins.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
