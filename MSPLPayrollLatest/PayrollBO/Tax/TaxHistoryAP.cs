using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDBOperation;
using System.Data.SqlClient;
using System.Data;


namespace PayrollBO.Tax
{
    public class TaxHistoryAP
    {
        public TaxHistoryAP()
        {

        }


        public string Field { get; set; }

        public decimal? Value { get; set; }

        public string Month { get; set; }

        public string Order { get; set; }

    }
    public class TaxHistoryAPList : List<TaxHistoryAP>
    {



        /// <summary>
        /// initialize the object
        /// </summary>
        public TaxHistoryAPList()
        {

        }


    




      


    }
}
