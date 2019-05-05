using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace CalcuLatorForPessach
{
   public class Calculator
    {
       public void Insertt()
        {
           int x = 0, y = 0;
           using (SqlConnection conn = new SqlConnection(@"Data Source = DESKTOP-0NBGVN2\MSSQLSERVER01;Initial Catalog=Calculator;Integrated Security=True"))
            {
                  do
                {
                    Console.WriteLine("Please insert two numbers:");
                    x = Convert.ToInt32(Console.ReadLine());
                    if (x <= 0)
                    {
                        Console.WriteLine("BYE BYE");
                        break;
                    }
                    y = Convert.ToInt32(Console.ReadLine());
                    if (y <= 0)
                    {
                        Console.WriteLine("BYE BYE");
                        break;
                    }
                    Console.WriteLine($"x={x},y={y}");
                    SqlCommand cmd = new SqlCommand("iNSERT_PROC", conn);
                    cmd.Parameters.Add(new SqlParameter("@ins", x));
                    cmd.Connection.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);
                    cmd.Connection.Close();
                    SqlCommand cmd2 = new SqlCommand("iNSERT_PROC_Y", conn);
                    cmd2.Parameters.Add(new SqlParameter("@ins_y", y));
                    cmd2.Connection.Open();
                    cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader reader2 = cmd2.ExecuteReader(CommandBehavior.Default);
                    Join_Tables();
                    Insert_Into_Results();
                    //key = TellMeTheOp();
                    Calculate_Good_Params();
                    cmd2.Connection.Close();
                } while (x > 0 || y > 0);
            }
        }
        public void Join_Tables()// join 3 tables - x,y and results.
        {
            using (SqlConnection conn = new SqlConnection(@"Data Source = DESKTOP-0NBGVN2\MSSQLSERVER01;Initial Catalog=Calculator;Integrated Security=True"))
            {
                SqlCommand cmd = new SqlCommand("CROSS_JOIN_3_TABLES", conn);
                cmd.Connection.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);
                cmd.Connection.Close();
            }
        }
        public void Calculate_Good_Params()
        {
            char res = TellMeTheOp();
            if (res == '+')
            {
                using (SqlConnection conn = new SqlConnection(@"Data Source = DESKTOP-0NBGVN2\MSSQLSERVER01;Initial Catalog=Calculator;Integrated Security=True"))
                {
                    Console.WriteLine("There is the sum of the numbers you've entered:");
                    SqlCommand cmd = new SqlCommand("calc_sum_results", conn);
                    cmd.Connection.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);
                    cmd.Connection.Close();
                    SqlCommand Reader_cmd = new SqlCommand("show_add", conn);
                    Reader_cmd.Connection.Open();
                    SqlDataReader Reader_cmd_rdr = Reader_cmd.ExecuteReader(CommandBehavior.Default);
                    while (Reader_cmd_rdr.Read() == true)
                    {
                        Console.WriteLine($"{Reader_cmd_rdr["X"],2} + {Reader_cmd_rdr["Y"],2} = {Reader_cmd_rdr["Results"]}");
                    }
                    Reader_cmd.Connection.Close();
                }
            }
            if (res == '-')
            {
                using (SqlConnection conn = new SqlConnection(@"Data Source = DESKTOP-0NBGVN2\MSSQLSERVER01;Initial Catalog=Calculator;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("calc_subtract_results", conn);
                    cmd.Connection.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);
                    cmd.Connection.Close();
                    SqlCommand Reader_cmd = new SqlCommand("show_subtract", conn);
                    Reader_cmd.Connection.Open();
                    SqlDataReader Reader_cmd_rdr = Reader_cmd.ExecuteReader(CommandBehavior.Default);
                    while (Reader_cmd_rdr.Read() == true)
                    {
                        Console.WriteLine($"{Reader_cmd_rdr["X"],2} - {Reader_cmd_rdr["Y"],2} = {Reader_cmd_rdr["Results"]}");
                    }
                    Reader_cmd.Connection.Close();
                }
            }
            if (res == '*')
            {
                using (SqlConnection conn = new SqlConnection(@"Data Source = DESKTOP-0NBGVN2\MSSQLSERVER01;Initial Catalog=Calculator;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("calc_multiple_results", conn);
                    cmd.Connection.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);
                    cmd.Connection.Close();
                    SqlCommand Reader_cmd = new SqlCommand("show_multiple", conn);
                    Reader_cmd.Connection.Open();
                    SqlDataReader Reader_cmd_rdr = Reader_cmd.ExecuteReader(CommandBehavior.Default);
                    while (Reader_cmd_rdr.Read() == true)
                    {
                        Console.WriteLine($"{Reader_cmd_rdr["X"],2} * {Reader_cmd_rdr["Y"],2} = {Reader_cmd_rdr["Results"]}");
                    }
                    Reader_cmd.Connection.Close();
                }
            }
            if (res == '/')
            {
                using (SqlConnection conn = new SqlConnection(@"Data Source = DESKTOP-0NBGVN2\MSSQLSERVER01;Initial Catalog=Calculator;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("calc_div_results", conn);
                    cmd.Connection.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);
                    cmd.Connection.Close();
                    SqlCommand Reader_cmd = new SqlCommand("show_div", conn);
                    Reader_cmd.Connection.Open();
                    SqlDataReader Reader_cmd_rdr = Reader_cmd.ExecuteReader(CommandBehavior.Default);
                    while (Reader_cmd_rdr.Read() == true)
                    {
                        Console.WriteLine($"{Reader_cmd_rdr["X"],2} / {Reader_cmd_rdr["Y"],2} = {Reader_cmd_rdr["Results"]}");
                    }
                    Reader_cmd.Connection.Close();
                }
            }

        }
        public char TellMeTheOp()
        {
            Console.WriteLine("Please insert the operation you wish to calculate:");
            Console.WriteLine("1. +\n2. -\n3. *\n4. /");
            int res = Convert.ToInt32( Console.ReadLine());
            switch (res)
            {
                case 1: return ('+');
                case 2: return ('-');
                case 3: return ('*');
                case 4: return ('/');
                default:
                    break;
            }
            return '0';
        }
        public void Insert_Into_Results()
        {
            using (SqlConnection conn = new SqlConnection(@"Data Source = DESKTOP-0NBGVN2\MSSQLSERVER01;Initial Catalog=Calculator;Integrated Security=True"))
            {
                SqlCommand cmd = new SqlCommand("INSERT_QUERIES", conn);
                cmd.Connection.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);
                cmd.Connection.Close();
            }
        }
    }
}
