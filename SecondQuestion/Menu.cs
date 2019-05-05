using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondQuestion
{
    class Menu
    {
        CustomerDAO c;
        SupplierDAO s;
        public Menu()
        {
            c = new CustomerDAO();
            s = new SupplierDAO();
        }
        public void Menu_func()
        {
            Console.WriteLine("Welcome \n1.Existing customer\n2.New customer\n3.Exsisting supplier\n4.New supplier\n5.Exit");
            int res = Convert.ToInt32(Console.ReadLine());
            switch (res)
            {
                case 1:
                    c.Exsisting_Customer();
                    break;
                case 2:
                    c.New_Customer();
                    Menu_func();
                    break;
                case 3:
                    s.Exsisting_Supplier();
                    Menu_func();
                    break;
                case 4:
                    s.New_Supplier();
                    break;
                case 5:
                    Console.WriteLine("Bye Bye");
                    break;
                default:
                    break;
            }
        }
    }
}
