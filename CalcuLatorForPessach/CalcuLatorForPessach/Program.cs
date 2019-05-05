using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CalcuLatorForPessach
{
    class Program
    {
        static void Main(string[] args)
        {
           Calculator c = new Calculator();
           c.Insertt();          
        }
    }
}
