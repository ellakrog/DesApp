using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfApplication1
{

    public partial class MainWindow : Window
    {

        DispatcherTimer timer;
        int interval;
        int step;

        public MainWindow()
        {
            InitializeComponent();

            interval = 1001;
            step = 20;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(interval);
            timer.Start();
            timer.Tick += new EventHandler(timer_Tick);

            DataTable aukcijeTable = new DataTable();
            SqlConnection conn = new SqlConnection(@"Data Source=.;Initial Catalog=Aukcija;Integrated Security=True");
            SqlDataAdapter aukcDa = new SqlDataAdapter("select * from Podaci", conn);

            aukcDa.Fill(aukcijeTable);
            aukcija_bazeDataGrid.DataContext = aukcijeTable;


        }



        void timer_Tick(object sender, EventArgs e)
        {
            if (Canvas.GetLeft(button1) > 400)
                timer.Stop();
            step = (step > 1) ? (step -= 1) : 1;
            Canvas.SetLeft(button1, Canvas.GetLeft(button1) + step);
            if (interval > 1)
                timer.Interval = TimeSpan.FromMilliseconds(interval -= 100);
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = (@"Data Source=.;Initial Catalog=AukcijskaProdaja;Integrated Security=True;");

                SqlCommand command = conn.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "uspPovecajCenuArtiklaZaJedan";
                command.Parameters.AddWithValue("@auction_ID", aukcija_bazeDataGrid.SelectedValue);
                conn.Open();
                command.ExecuteNonQuery();
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Window1 popup = new Window1();
            popup.ShowDialog();

            popup.Dispose();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Window2 popup = new Window2();
            popup.ShowDialog();

            popup.Dispose();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = (@"Data Source=.;Initial Catalog=AukcijskaProdaja;Integrated Security=True;");
                SqlCommand command = conn.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "uspPovecajCenuArtiklaZaJedan";
                command.Parameters.AddWithValue("@ArtikalID", 1);
                conn.Open();
                command.ExecuteNonQuery();
            }
        }

        private void loadGrid()
        {
            SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=Aukcija;Integrated Security=True");
            SqlDataAdapter ad = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand();
            con.Open();
            string strQuery = "select * from Aukcija";
            cmd.CommandText = strQuery;
            ad.SelectCommand = cmd;
            cmd.Connection = con;
            DataSet ds = new DataSet();
            ad.Fill(ds);
            aukcija_bazeDataGrid.DataContext = ds.Tables[0].DefaultView;
            con.Close();
        }

        private void aukcija_bazeDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

        }
    }
}
