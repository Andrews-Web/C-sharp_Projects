using System;
using System.Windows;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using BarcodeLib;
using System.Windows.Controls;
using Image = System.Drawing.Image;
using Color = System.Drawing.Color;
using System.Drawing;
using System.Reflection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using VisioForge.Libs.MediaFoundation;

namespace StockManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void LoadRecords()
        {
            
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("exec dbo.Product_View", con);
                cmd.ExecuteNonQuery();
                SqlDataAdapter data = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("ProductsTable");
                data.Fill(dt);
                prodData.ItemsSource = dt.DefaultView;
                data.Update(dt);
                con.Close();
            }
            catch(Exception ex)
            {
                ex.Source = null;   
            }
            
            
        }

        public void Reset()
        {
            Index.Text = "";
            P_Cat.Text = "";
            P_Name.Text = "";
            C_Price.Text = "";
            S_Price.Text = "";
            Quantity.Text = "";
        }

        SqlConnection con = new SqlConnection("Data Source=DESKTOP-32I3APT;Initial Catalog=StockSystemdb;Integrated Security=True");


        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("exec dbo.Product_Insert '" + int.Parse(Index.Text) + "','" + P_Cat.Text + "','" + P_Name.Text + "','" + double.Parse(C_Price.Text) + "','" + double.Parse(S_Price.Text) + "','" + int.Parse(Quantity.Text) + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Inserted");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Primary key cannot be repeated!");
                MessageBox.Show(ex.Message);
            }
            LoadRecords();
            Reset();




        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("exec dbo.Product_Delete '" + int.Parse(Index.Text) + "'", con);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Deleted");
            Reset();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            if (Index.Text != "")
            {
                SqlCommand cmd = new SqlCommand("exec dbo.Product_Search '" + int.Parse(Index.Text) + "'", con);
                cmd.ExecuteNonQuery();

                SqlDataAdapter data = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("ProductsTable");
                data.Fill(dt);
                prodData.ItemsSource = dt.DefaultView;
                data.Update(dt);
            }
            con.Close();
            Reset();

        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("exec dbo.Product_Check_Min_Quantity '" + int.Parse(Min.Text) + "'", con);
            cmd.ExecuteNonQuery();
            con.Close();

            SqlDataAdapter data = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("ProductsTable");
            data.Fill(dt);
            prodData.ItemsSource = dt.DefaultView;
            data.Update(dt);
            Reset();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void btnGetBar_Click(object sender, RoutedEventArgs e)
        {
            con.Open();

            if (Index.Text != "")
            {
                SqlCommand cmd = new SqlCommand("select Bar_Code from ProductsTable where ID = '" + Index.Text + "'", con);
                SqlDataReader read = cmd.ExecuteReader();
                
                while (read.Read())
                {
                    numCode.Text = read["Bar_Code"].ToString();
                }
            }

            con.Close();
            Reset();
        }

        private void btnPrintBar_Click(object sender, RoutedEventArgs e)
        {
            BarcodeLib.Barcode bar = new Barcode();
            Color fColor = Color.Black;
            Color bColor = Color.Transparent;
            try
            {
                Image img1 = bar.Encode(
                TYPE.UPCA,
                numCode.Text
                );
                img.Source = ToImageSource(img1, ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click_1(object sender, RoutedEventArgs e)
        {
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("exec dbo.Product_Update '" + int.Parse(Index.Text) + "','" + P_Cat.Text + "','" + P_Name.Text + "','" + double.Parse(C_Price.Text) + "','" + double.Parse(S_Price.Text) + "','" + int.Parse(Quantity.Text) + "'", con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            con.Close();
            MessageBox.Show("Updated");
            Reset();
        }

        public static ImageSource ToImageSource(System.Drawing.Image image, ImageFormat imageFormat)
        {
            BitmapImage bitmap = new BitmapImage();

            using (MemoryStream stream = new MemoryStream())
            {
                // Save to the stream
                image.Save(stream, imageFormat);

                // Rewind the stream
                stream.Seek(0, SeekOrigin.Begin);

                // Tell the WPF BitmapImage to use this stream
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
            }

            return bitmap;
        }
    }
}
