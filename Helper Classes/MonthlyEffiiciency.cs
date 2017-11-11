/*********************************************************************************************************************************************
 * Author : Monika  Puhazhendhi
 * -----------------------------------Descripton----------------------------------------------------------------------------------------------
 * Binds its objects to the performance chart
 *********************************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace ToDoList
{
    class CalculateMonthlyEfficiency
    {
        public List<Efficiency> MonthlyTaskCount;
        Efficiency CurrentMonthEfficiency;
        static bool flag = false;
        // List<Efficiency> EfficiencyObj = new List<Efficiency>(12);

        public CalculateMonthlyEfficiency()
        {
            MonthlyTaskCount = new List<Efficiency>(12);

            flag = false;
        }

        public void setDataForEfficencyCalc(Efficiency EffObj)
        {
            CurrentMonthEfficiency = EffObj;
        }
        public IEnumerable<Efficiency> Efficiency
        {

            get
            {
                Operation operationObj = new Operation("performance", null);
                MonthlyTaskCount.Clear();
                MonthlyTaskCount = operationObj.ViewPerformance();
                return MonthlyTaskCount;
            }
        }

        public List<Efficiency> getMonthlyEfficiency()
        {


            setMonthName();
            return MonthlyTaskCount;

        }

        public void WriteToFile(Efficiency CurrentEfficiency)
        {
            List<Efficiency> tempEfficiencyList = new List<Efficiency>();

            tempEfficiencyList = readFromFile();

            tempEfficiencyList[System.DateTime.Now.Month - 1] = CurrentEfficiency;

            StreamWriter writer = new StreamWriter("Efficiency.txt", false);
            for (int i = 0; i < 12; i++)
                writer.WriteLine(tempEfficiencyList.ElementAt(i).efficiency.ToString());
            writer.Close();
        }


        public List<Efficiency> readFromFile()
        {


            StreamReader reader = new StreamReader("Efficiency.txt", true);
            int index = 0;
            String MonthlyEfficiency = reader.ReadLine();
            if (flag == false)
            {

                while (MonthlyEfficiency != null)
                {
                    /*String[] SplitCompleteAndIncompeteTask = MonthlyEfficiency.Split(',');
                    Efficiency tempEfficiencyObj = new Efficiency();
                    tempEfficiencyObj.setCompletedTaskNumber(Convert.ToInt32(SplitCompleteAndIncompeteTask[0]));
                    tempEfficiencyObj.setIncompleteTaskNumber(Convert.ToInt32(SplitCompleteAndIncompeteTask[1]));*/

                    setMonthlyTaskCount(Convert.ToDouble(MonthlyEfficiency), index);
                    index++;
                    MonthlyEfficiency = reader.ReadLine();
                }
                if (MonthlyTaskCount.Count() == 0)
                    for (int i = 0; i < 12; i++)
                        MonthlyTaskCount.Add(new Efficiency(null, 0));
            }
            flag = !flag;
            //for (int i = 0; i < 12; i++)
            //    if (MonthlyTaskCount[i].efficiency == null || MonthlyTaskCount[i].efficiency == 0)
            //        MonthlyTaskCount[i].efficiency = 0;
            reader.Close();
            return getMonthlyEfficiency();

        }
        public void setMonthlyTaskCount(double effobj, int index)
        {

            MonthlyTaskCount.Insert(index, new Efficiency(null, effobj));

        }


        public void setMonthName()
        {
            for (int i = 0; i < 12; i++)
            {
                switch (i)
                {
                    case 0: MonthlyTaskCount[i].month = "January";
                        break;
                    case 1: MonthlyTaskCount[i].month = "Febuary";
                        break;
                    case 2: MonthlyTaskCount[i].month = "March";
                        break;
                    case 3: MonthlyTaskCount[i].month = "April";
                        break;
                    case 4: MonthlyTaskCount[i].month = "May";
                        break;
                    case 5: MonthlyTaskCount[i].month = "June";
                        break;
                    case 6: MonthlyTaskCount[i].month = "July";
                        break;
                    case 7: MonthlyTaskCount[i].month = "August";
                        break;
                    case 8: MonthlyTaskCount[i].month = "September";
                        break;
                    case 9: MonthlyTaskCount[i].month = "October";
                        break;
                    case 10: MonthlyTaskCount[i].month = "November";
                        break;
                    case 11: MonthlyTaskCount[i].month = "December";
                        break;
                }
            }
        }

    }
}
