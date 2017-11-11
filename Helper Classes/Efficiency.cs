/*********************************************************************************************************************************************
 * Author : Monika  Puhazhendhi
 * -----------------------------------Descripton----------------------------------------------------------------------------------------------
 * Binds its objects to the performance chart
 *********************************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToDoList
{
    class Efficiency
    {
        public Efficiency(String MonthArg, double EfficiencyArg)
        {
            month = MonthArg;
            efficiency = EfficiencyArg;

        }

        public Efficiency()
        {
            // TODO: Complete member initialization
        }

        public double efficiency { get; set; }

        public String month { get; set; }

        
    }
}
