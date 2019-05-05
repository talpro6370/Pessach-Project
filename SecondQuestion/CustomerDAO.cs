using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;

namespace SecondQuestion
{
    public class CustomerDAO
    {
        public void Exsisting_Customer()
        {
            int count = 0;

            Console.WriteLine("Please enter user name and password:");
            var UserName = Console.ReadLine();
            var Pass = Console.ReadLine();
            try
            {
                using (SqlConnection conn = new SqlConnection(@"Data Source =DESKTOP-0NBGVN2\MSSQLSERVER01; Initial Catalog = cust_orders_sup_prod; Integrated Security = True"))
                {
                    SqlCommand cmd = new SqlCommand("Exsisting_Customer_Select", conn);
                    SqlCommand cmd2 = new SqlCommand("selec_total_price_by_id", conn);
                    SqlCommand cmd3 = new SqlCommand("SELECT_ALL_PRODUCTS", conn);
                    SqlCommand cmd4 = new SqlCommand("INV_PROD", conn);
                    SqlCommand user_name_find = new SqlCommand("find_name", conn);
                    user_name_find.CommandType = CommandType.StoredProcedure;
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd4.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserName", UserName));
                    cmd.Parameters.Add(new SqlParameter("@Pass", Pass));

                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);
                    while (reader.Read() == true)
                    {
                        Console.WriteLine($"|Id:{reader["Id"]} , Name: {reader["Name"]} , Last name: {reader["Last_Name"]} , User name: {reader["UserName"]} , Password: {reader["User_Password"]} , Credit card number: {reader["Credit_Number"]}|");
                        if (reader["Id"] != null) count++;
                        cmd2.Parameters.Add(new SqlParameter("@Id", (int)reader["Id"]));
                    }
                    cmd.Connection.Close();
                    if (count == 0) Console.WriteLine("NO SUCH A USER");
                    if (count > 0)
                    {
                        cmd2.Connection.Open();
                        cmd2.CommandType = CommandType.StoredProcedure;
                        SqlDataReader reader2 = cmd2.ExecuteReader();
                        Console.WriteLine("1.1 Display my shopping list\n1.2 Display all products\n1.3 Order a product");
                        int UserRes = Convert.ToInt32(Console.ReadLine());
                        switch (UserRes)
                        {
                            case 1:
                                {
                                    Console.WriteLine("My shopping list:");
                                    while (reader2.Read() == true)
                                    {
                                        Console.WriteLine($"Products: {reader2["Prod_Name"]}");

                                        reader2.NextResult();
                                        while (reader2.Read() == true)
                                        {
                                            Console.WriteLine($"Total Price: {reader2["Total_Price"]}");
                                        }
                                    }
                                    cmd2.Connection.Close();
                                    break;
                                }
                            case 2:
                                {
                                    cmd2.Connection.Close();
                                    cmd3.Connection.Open();
                                    SqlDataReader reader3 = cmd3.ExecuteReader(CommandBehavior.Default);
                                    Console.WriteLine("All products:");
                                    while (reader3.Read() == true)
                                    {
                                        Console.WriteLine($"{reader3["Prod_Name"]}");
                                    }
                                    cmd3.Connection.Close();
                                    break;
                                }
                            case 3:
                                int count2 = 0, count3 = 0;
                                string result = null;
                                cmd3.Connection.Close();
                                cmd3.Connection.Open();
                                SqlDataReader Prod_Reader = cmd3.ExecuteReader(CommandBehavior.Default);
                                List<string> products = new List<string>();
                                while (count3 == 0)
                                {
                                    Console.WriteLine("Enter a product name from the list: (you can type the word - 'exit' to leave) ");
                                    while (Prod_Reader.Read() == true)
                                    {
                                        Console.WriteLine($"Product:{Prod_Reader["Prod_Name"]} ----- Stock:{Prod_Reader["Stock_Amount"]} ");
                                        products.Add(Prod_Reader["Prod_Name"].ToString());
                                    }
                                    cmd3.Connection.Close();
                                    result = Console.ReadLine();
                                    count3 = 0;
                                    count2 = products.Count();
                                    for (int i = 0; i < count2; i++)
                                    {
                                        if (products[i] == result) // Checking if the user's input = Data output.
                                        {
                                            count3++;
                                            break;
                                        }
                                    }
                                }
                                if (count3 > 0) // user's input has been found inside database - there is a similiar product
                                {
                                    Console.WriteLine("Please enter the amount you want to order:");
                                    int result2 = Convert.ToInt32(Console.ReadLine());
                                    SqlCommand select_one_prod = new SqlCommand("select_one_product", conn);
                                    select_one_prod.CommandType = CommandType.StoredProcedure;
                                    select_one_prod.Parameters.Add(new SqlParameter("@product_name", result));
                                    select_one_prod.Connection.Open();

                                    SqlDataReader reader_one_stock = select_one_prod.ExecuteReader(CommandBehavior.Default);
                                    int amount = 0;
                                    while (reader_one_stock.Read() == true)
                                    {
                                        amount = Convert.ToInt32(reader_one_stock["Stock_Amount"]);
                                    }
                                    if (result2 > amount || amount == 0) Console.WriteLine("NO STOCK!");
                                    select_one_prod.Connection.Close();
                                    if (result2 <= amount)
                                    {
                                        cmd4.Connection.Open();
                                        cmd4.Parameters.Add(new SqlParameter("@prod_name", result));
                                        cmd4.Parameters.Add(new SqlParameter("@amount", result2));
                                        SqlDataReader reader4 = cmd4.ExecuteReader(CommandBehavior.Default);
                                        while (reader4.Read() == true)
                                        {
                                            Console.WriteLine($"Product name:{reader4["prod_name"]}");
                                        }
                                        reader4.NextResult();
                                        while (reader4.Read() == true)
                                        {
                                            Console.WriteLine($"Stock:{reader4["Stock_Amount"]}");
                                        }
                                        cmd4.Connection.Close();
                                    }
                                }
                                cmd3.Connection.Open();
                                SqlDataReader Sec_Reader = cmd3.ExecuteReader(CommandBehavior.Default);
                                Console.WriteLine("New stock of the products:");
                                while (Sec_Reader.Read() == true)
                                {
                                    Console.WriteLine($"Product:{Sec_Reader["Prod_Name"]} ----- Stock:{Sec_Reader["Stock_Amount"]}");
                                }
                                cmd3.Connection.Close();
                                break;
                            default:
                                break;
                        }
                    }
                    count = 0;

                }
            }
            catch(SqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        /*
         Customer:
         *Id
          UserName
          User_Password
          Name
          Last_Name
          Credit_Number

          */
        public void New_Customer()
        {
            long credit_card = 0; //id?
            string userName = null, userPass = null, name = null, lastName = null;
            Console.WriteLine("Please enter user details: User Name, User's Password, Name, Last Name and Credit Card Number");
            userName = Console.ReadLine();
            userPass = Console.ReadLine();
            name = Console.ReadLine();
            lastName = Console.ReadLine();
            credit_card = Convert.ToInt64(Console.ReadLine());
            int count = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(@"Data Source =DESKTOP-0NBGVN2\MSSQLSERVER01; Initial Catalog = cust_orders_sup_prod; Integrated Security = True"))
                {

                    SqlCommand cmd = new SqlCommand("insert_customers", conn);
                    SqlCommand cmd2 = new SqlCommand("select_customer_by_username", conn);
                    cmd2.Parameters.Add(new SqlParameter("@username", userName));
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Connection.Open();
                    SqlDataReader reader2 = cmd2.ExecuteReader(CommandBehavior.Default);
                    while (reader2.Read() == true)//checking if the user name is  alrdy exsist.
                    {
                        count++;
                    }
                    cmd2.Connection.Close();
                    if (count > 0) Console.WriteLine("User name is alrdy exsist!!");//user found.
                    if (count == 0)//no user has benn found. lets add new one.
                    {
                        cmd.Parameters.Add(new SqlParameter("@userName", userName));
                        cmd.Parameters.Add(new SqlParameter("@userPass", userPass));
                        cmd.Parameters.Add(new SqlParameter("@name", name));
                        cmd.Parameters.Add(new SqlParameter("@lastName", lastName));
                        cmd.Parameters.Add(new SqlParameter("@credit_card", credit_card));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                    count = 0;
                }


            }
            catch (SqlException e)
            {
                Console.WriteLine("sql exception - user is alrdy exsist!", e.Message, e.GetType());
            }
        }
    }
}
