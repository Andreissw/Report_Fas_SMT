﻿using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Report_Fas_SMT
{
    public class Database
    {
        static public object SelectStringOmronInteger(string cmd) //Достает с базы ОРАКЛ значение целого числа
        {

            OracleCommand c = new OracleCommand();
            OracleDataReader r;
            int k;
            k = 0;
            string Oradb = "Data Source=(DESCRIPTION=(ADDRESS_LIST= (ADDRESS=(PROTOCOL=TCP)(HOST=192.168.37.1)(PORT=1521))) (CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=prismDB))); User Id=PRISM;Password=PRISM;";
            OracleConnection conn = new OracleConnection(Oradb);
            try
            {
                conn.Open();

                c = conn.CreateCommand();
                c.CommandType = CommandType.Text;
                c.CommandText = cmd;

                r = c.ExecuteReader();
                if (r.Read())
                {
                    
                    k = r.GetInt32(0);
                    r.Close();
                }
                conn.Close();
                return k;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return k;
            }


        }
       public static  void loadgridOrbotec(DataGridView grid, string cmd) //Достает с базы PSIGMA FLAT и преобразует в грид, таблицу
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source= 192.168.37.100\orprovision; Initial Catalog= ; User Id = volodin; Password = kjifhf12345;");
                SqlCommand c = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                sqlcon.Open();
                c = sqlcon.CreateCommand();
                c.CommandText = cmd;
                da.SelectCommand = c;
                da.Fill(ds, "Table1");               

                grid.DataSource = ds;
                grid.DataMember = "Table1";
                sqlcon.Close();
            }
            catch (Exception)
            {


            }


        }



        public static object SelectStringIntOrbotec(string cmd) //Достает с базы PSIGMA FLAT строковые значения и числовые
        {
            SqlConnection sqlcon = new SqlConnection(@"Data Source= 192.168.37.100\orprovision; Initial Catalog= ; User Id = volodin; Password = kjifhf12345;");
            SqlCommand c = new SqlCommand();
            SqlDataReader r;
            int k = 0;
            c = sqlcon.CreateCommand();
            c.CommandType = CommandType.Text;
            c.CommandText = cmd;
            try
            {
                sqlcon.Open();
                r = c.ExecuteReader();
                if (r.Read())
                {
                    k = r.GetInt32(0);
                    r.Close();

                }
                sqlcon.Close();
                return k;
                //MessageBox.Show(k);
                //sqlcon.Close();
            }

            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return k;
            }
        }


        static public void LoadGridOmron(DataGridView Grid, string cmd)

        {
            OracleCommand c = new OracleCommand();
            OracleDataAdapter da = new OracleDataAdapter();
            DataSet ds = new DataSet();
            string Oradb = "Data Source=(DESCRIPTION=(ADDRESS_LIST= (ADDRESS=(PROTOCOL=TCP)(HOST=192.168.37.1)(PORT=1521))) (CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=prismDB))); User Id=PRISM;Password=PRISM;";
            OracleConnection conn = new OracleConnection(Oradb);

            try
            {
                conn.Open();
                c = conn.CreateCommand();
                c.CommandText = cmd;
                da.SelectCommand = c;
                da.Fill(ds, "Table1");
                Grid.DataSource = ds;
                Grid.DataMember = "Table1";
                conn.Close();
            }
            catch (Exception e)
            {
                //MessageBox.Show("Ошибка с соединением базой ORACLE");
                MessageBox.Show(e.ToString());


            }
        }

    }
}
