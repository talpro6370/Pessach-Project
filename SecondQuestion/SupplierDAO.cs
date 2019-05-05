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
    class SupplierDAO
    {
        public void Exsisting_Supplier()
        {
            int count=0;
            Console.WriteLine("Enter user name and password:");
            var userName = Console.ReadLine();
            var pass = Console.ReadLine();
            try
            {
                using (SqlConnection conn = new SqlConnection(@"Data Source =DESKTOP-0NBGVN2\MSSQLSERVER01; Initial Catalog = cust_orders_sup_prod; Integrated Security = True"))
                {
                    SqlCommand cmd = new SqlCommand("SELECT_SUPPLIER_BY_userName_Pass", conn);
                    SqlCommand add_prods = new SqlCommand("add_products", conn);
                    cmd.Parameters.Add(new SqlParameter("@userName", userName));
                    cmd.Parameters.Add(new SqlParameter("@pass", pass));
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection.Open();
                    //Gal778 , G123456
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default);
                    while (reader.Read() == true)
                    {
                        Console.WriteLine($"Id: {reader["Id"]} User name: {reader["Supp_User_name"]}");
                        if (reader["Id"] != null) count++;
                    }
                    cmd.Connection.Close();
                    if (count > 0)
                    {
                        Console.WriteLine("1.Add product to inventory\n2.Display all products");
                        int res = Convert.ToInt32(Console.ReadLine());
                        int suppNumber = 0, specific_Supp_Num = 0;
                        SqlCommand cmd2 = new SqlCommand("select_specific_product", conn);
                        switch (res)
                        {
                            case 1:
                                Console.WriteLine("Enter product's name:");
                                var prod = Console.ReadLine();
                                cmd2.Parameters.Add(new SqlParameter("@product_name", prod));
                                cmd2.Parameters.Add(new SqlParameter("@Supp_Name", userName));
                                cmd2.Connection.Open();
                                cmd2.CommandType = CommandType.StoredProcedure;
                                SqlDataReader reader2 = cmd2.ExecuteReader(CommandBehavior.Default);
                                while (reader2.Read() == true) // Happans if product's name is Instore.
                                {
                                    Console.WriteLine($"Product:{reader2["Prod_Name"]} {reader2["Supplier_Number"]}");
                                    specific_Supp_Num = Convert.ToInt32(reader2["Supplier_Number"]);
                                }
                                reader2.NextResult();
                                while (reader2.Read() == true)
                                    suppNumber = Convert.ToInt32(reader2["Id"]);
                                cmd2.Connection.Close();
                                add_prods.Connection.Open();
                                add_prods.CommandType = CommandType.StoredProcedure;
                                if (suppNumber == specific_Supp_Num && suppNumber != 0 && specific_Supp_Num != 0)//IF THIS SUPPLIER OWN THE PRODUCT-STORE IT.
                                {
                                    Console.WriteLine("Enter the amount you wish to store in stock:");
                                    var addToStock = Console.ReadLine();
                                    add_prods.Parameters.Add(new SqlParameter("@amount", addToStock));
                                    add_prods.Parameters.Add(new SqlParameter("@name_of_product", prod));
                                }
                                add_prods.Parameters.Add(new SqlParameter("@amount", 1));
                                add_prods.Parameters.Add(new SqlParameter("@name_of_product", prod));
                                add_prods.ExecuteNonQuery();
                                add_prods.Connection.Close(); // this part is working.         
                                 //---IF THE PRODUCT IS AVAILABLE BUT ANOTHER SUPPLIER OWN IT---//
                                if (suppNumber != specific_Supp_Num && suppNumber != 0 && specific_Supp_Num != 0)
                                {
                                    Console.WriteLine("The product is available but another supplier owns it!!");
                                }
                                //---IF THE PRODUCT IS UNAVAILABLE---//
                                SqlCommand insert_new_prod = new SqlCommand("insert_new_product", conn);
                                int count_check = 0;
                                if (specific_Supp_Num == 0) // COULDNT FIND THE SUPPLIER NUMBER - PRODUCT ISNT INSTORE
                                {
                                    Console.WriteLine("Enter product details:\nName,Supplier Number,Price and Total amount:");
                                    var suppName = Console.ReadLine();
                                    var suppNum = Console.ReadLine();
                                    var price = Console.ReadLine();
                                    var amount = Console.ReadLine();
                                    insert_new_prod.Connection.Open();
                                    insert_new_prod.CommandType = CommandType.StoredProcedure;
                                    insert_new_prod.Parameters.Add(new SqlParameter("@prod_Name", suppName));
                                    insert_new_prod.Parameters.Add(new SqlParameter("@supplier_Number", suppNum));
                                    insert_new_prod.Parameters.Add(new SqlParameter("@price", price));
                                    insert_new_prod.Parameters.Add(new SqlParameter("@Stock_Amount", amount));
                                    insert_new_prod.ExecuteNonQuery();
                                    insert_new_prod.Connection.Close();
                                    count_check++;
                                }
                                insert_new_prod.Parameters.Add(new SqlParameter("@prod_Name", null));
                                insert_new_prod.Parameters.Add(new SqlParameter("@supplier_Number", null));
                                insert_new_prod.Parameters.Add(new SqlParameter("@price", 0));
                                insert_new_prod.Parameters.Add(new SqlParameter("@Stock_Amount", 0));
                                if (count_check == 0)//delete row if there is no reason that some params added to Db//
                                {
                                    SqlCommand delete_column = new SqlCommand();
                                    delete_column.CommandType = CommandType.Text;
                                    delete_column.CommandText = "DELETE FROM PRODUCT WHERE ID=(SELECT MAX(Id) FROM PRODUCT)";
                                }
                                suppNumber = 0;
                                specific_Supp_Num = 0;
                                count_check = 0;
                                count = 0;
                                //THIS PART IS WORKING//
                                break;
                            case 2:
                                int suppNumForCheck = 0;
                                SqlCommand Get_supplier_num = new SqlCommand("SELECT_SUPPLIER_BY_userName_Pass", conn);
                                Get_supplier_num.Parameters.Add(new SqlParameter("@userName", userName));
                                Get_supplier_num.Parameters.Add(new SqlParameter("@pass", pass));
                                Get_supplier_num.CommandType = CommandType.StoredProcedure;
                                Get_supplier_num.Connection.Open();
                                SqlDataReader suppreader = Get_supplier_num.ExecuteReader(CommandBehavior.Default);
                                while (suppreader.Read() == true)
                                {
                                    suppNumForCheck = Convert.ToInt32(suppreader["Id"]);
                                }
                                Get_supplier_num.Connection.Close();
                                SqlCommand show_my_products = new SqlCommand("SELECT_ALL_MY_PRODUCTS", conn);
                                show_my_products.CommandType = CommandType.StoredProcedure;
                                show_my_products.Parameters.Add(new SqlParameter("@SUPPnUM", suppNumForCheck));
                                show_my_products.Connection.Open();
                                SqlDataReader reader34 = show_my_products.ExecuteReader(CommandBehavior.Default);
                                while (reader34.Read() == true)
                                {
                                    Console.WriteLine($"ID:{reader34["Id"]} Product: {reader34["Prod_Name"]}Supplier Number: {reader34["Supplier_Number"]}");
                                }
                                show_my_products.Connection.Close();
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public void New_Supplier()
        {
            Console.WriteLine("Enter user name, password and company name:");
            var user_Name = Console.ReadLine();
            var pass = Console.ReadLine();
            var compName = Console.ReadLine();
            int count = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(@"Data Source =DESKTOP-0NBGVN2\MSSQLSERVER01; Initial Catalog = cust_orders_sup_prod; Integrated Security = True"))
                {
                    SqlCommand cmd = new SqlCommand("Insert_new_supplier", conn);
                    SqlCommand cmd3 = new SqlCommand("select_supp_by_username", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.Add(new SqlParameter("@username", user_Name));
                    cmd3.Connection.Open();
                    SqlDataReader reader3 = cmd3.ExecuteReader(CommandBehavior.Default);
                    while (reader3.Read() == true)
                    {
                        count++;
                    }
                
                    if (count > 0) 
                    {
                        Console.WriteLine("This user name is already exsist!");
                    }
                    if(count==0)
                    {
                        cmd3.Connection.Close();
                        cmd.Connection.Open();
                        cmd.Parameters.Add(new SqlParameter("@username", user_Name));
                        cmd.Parameters.Add(new SqlParameter("@pass", pass));
                        cmd.Parameters.Add(new SqlParameter("@comp_name", compName));
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("User has benn succesfully added!");
                    }
                    if (count != 0)
                    {
                        cmd3.Connection.Close();
                        cmd.Connection.Close();
                    }
                    count = 0;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
