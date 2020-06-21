using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGumboInterface.Service;
using DataGumboInterface.Models.Structured;
using DataGumboInterface.Models.RealTime;
using System.IO;

namespace DataGumboInterfaceConsole
{
    class Program
    {
//Chevron = chevron.api@esiAzure.com / J&oWMVh92LIb

//Shell = shell.api@esiAzure.com / Quq83501

//XTO Energy = xto.api@esiAzure.com / Yun11122

//Murphy Oil (no rigs yet) = murphy.api@esiAzure.com / Quv81846

        public static readonly string pbiChevronUserId = "chevron.api@esiAzure.com";
        public static readonly string pbiChevronPassword = "J&oWMVh92LIb";
        public static readonly string pbiShellUserId = "shell.api@esiAzure.com";
        public static readonly string pbiShellPassword = "Quq83501";
        public static readonly string pbiXtoEnergyUserId = "xto.api@esiAzure.com";
        public static readonly string pbiXtoEnergyPassword = "Yun11122";
        public static readonly string pbiMurphyOilUserId = "murphy.api@esiAzure.com";
        public static readonly string pbiMurphyOilPassword = "Quv81846";

        static void Main(string[] args)
        {
            /*WellActivityPeriodService wapSvc = new WellActivityPeriodService();
            IEnumerable<WellActivityPeriod> waps = wapSvc.GetAllWellActivityPeriods();
            Console.WriteLine("wellActivityPeriodId\twellId\trigId\tstartDateTime\tendDateTime");
            foreach (WellActivityPeriod wap in waps)
            {
                object[] elems = new object[] { wap.wellActivityPeriodId, wap.wellId, wap.rigId, wap.startDateTime, wap.endDateTime };
                Console.WriteLine(string.Join("\t", elems.Select(o => o == null ? string.Empty : o.ToString()).ToArray()));
            }
            WellService wSvc = new WellService();
            Well well = wSvc.GetById(664367);
            IEnumerable<Well> ws = wSvc.GetAll();
            Console.WriteLine("wellId\twellName");
            foreach (Well w in ws)
            {
                object[] elems = new object[] { w.wellId, w.wellName };
                Console.WriteLine(string.Join("\t", elems.Select(o => o == null ? string.Empty : o.ToString()).ToArray()));
            }*/

            // CODE TO DELETE WELLS WITH NO LAT/LONGS
            /*
            WellService svc = new WellService();
            IEnumerable<Well> allWells = svc.GetAll();
            IEnumerable<Well> wellsWithBlankLatLong = allWells.Where(w => w.latitude == 0.0 && w.longitude == 0.0);
            
            foreach (Well w in wellsWithBlankLatLong)
            {
                Console.WriteLine("Going to delete well {0}", w.wellId);
                svc.Delete(w.wellId, w);
            }*/

            // CODE TO DELETE WELLS FROM RIG 140
            
            /*RigService rSvc = new RigService();
            IEnumerable<Rig> allRigs = rSvc.GetAll();
            Rig rig140 = allRigs.FirstOrDefault(r => r.rigName == "140");

            WellService svc = new WellService();
            IEnumerable<Well> wellsForRig = svc.GetByParentId(rig140.rigId);

            foreach (Well w in wellsForRig)
            {
                Console.WriteLine("Going to delete well {0}", w.wellId);
                svc.Delete(w.wellId, w);
                Console.WriteLine("Deleted well {0}", w.wellId);
            }*/
            
            // CODE TO DISPLAY LATEST BIT DEPTH FOR RIG 144
            /*using (StreamWriter sw = new StreamWriter("C:\\temp\\rig_144_hole_depth.csv"))
            {
                sw.WriteLine("dtm,hole_depth");
                RealTimeDataService<double> svc = new RealTimeDataService<double>();
                DateTime dateLoop = DateTime.Now;
                DateTime endAt = new DateTime(2019,3,28);

                int loopCounter = 0;
                while (dateLoop > endAt) 
                {
                    if (loopCounter++ % 10 == 0)
                    {
                        Console.WriteLine("in loop - query date is " + dateLoop);
                    }
                    DateTime st = dateLoop.AddMinutes(-60);
                    var res = svc.Get("144", "Hole Measured Depth", st, dateLoop);
                    foreach (var r in res.OrderByDescending(s => s.Timestamp))
                    {
                        sw.WriteLine("{0},{1}", r.Timestamp, r.Value);
                    }
                    dateLoop = st;
                }
            }*/

            // CODE TO COUNT Drilling Assemblies and components

            // CODE TO DELETE WELLS WITH NO WAPS
            /*
            WellService svc = new WellService();
            IEnumerable<Well> allWells = svc.GetAll();
            WellActivityPeriodService wapSvc = new WellActivityPeriodService();

            foreach (Well w in allWells)
            {
                if (wapSvc.GetByParentId(w.wellId).Count() == 0)
                {
                    Console.WriteLine("Going to delete well {0}", w.wellId);
                    svc.Delete(w.wellId, w);
                }
                else
                {
                    Console.WriteLine("NOT going to delete well {0}", w.wellId);
                }
            }
            */

            // CODE TO DELETE WELLS WITH SPUD DATE PRIOR TO 2018
            
            //WellService svc = new WellService();
            //IEnumerable<Well> allWells = svc.GetAll();

            //foreach (Well w in allWells)
            //{
            //    if (w.spudDate < new DateTime(2018,1,1))
            //    {
            //        Console.WriteLine("Going to delete well {0}", w.wellId);
            //        svc.Delete(w.wellId, w);
            //    }
            //}

            // CODE TO DELETE CUSTOMERS WITH NO WELLS
            /*WellService svc = new WellService();
            IEnumerable<Well> allWells = svc.GetAll();
            IEnumerable<int> custIdsInUse = allWells.Select(w => w.customerId).Distinct().ToList();

            CustomerService cSvc = new CustomerService();
            IEnumerable<Customer> allCusts = cSvc.GetAll();
            IEnumerable<int> listOfParentIds = allCusts.Where(c => c.parentCustomerId.HasValue && c.parentCustomerId.Value != c.customerId).Select(c => c.parentCustomerId.Value).Distinct();
            IEnumerable<int> listOfChildIds = allCusts.Where(c => c.parentCustomerId.HasValue && c.parentCustomerId.Value != c.customerId).Select(c => c.customerId).Distinct();

            List<Customer> toDelete = new List<Customer>();
            List<Customer> toKeep = new List<Customer>();

            foreach (Customer c in allCusts)
            {
                if (Math.Abs(c.customerId) == 1 || listOfChildIds.Contains(c.customerId) || custIdsInUse.Contains(c.customerId) || listOfParentIds.Contains(c.customerId))
                {
                    toKeep.Add(c);
                }
                else
                {
                    Console.WriteLine("Adding to delete customer list {0}: {1}", c.customerId, c.customerName);
                    toDelete.Add(c);
                }
            }

            toDelete.ForEach(del => cSvc.Delete(del.customerId));*/

            // CODE TO DELETE WELLS 661122, 661340, 659892, 654982, 654968, 654833, 654736, 653558, 638205, 436388, 418948

            //foreach (int wellId in new[] { 661122, 661340, 659892, 654982, 654968, 654833, 654736, 653558, 638205, 436388, 418948 })
            //{
            //    Console.WriteLine("Going to delete well {0}", wellId);
            //    WellService svc = new WellService();
            //    try
            //    {
            //        Well w = svc.GetById(wellId);

            //        svc.Delete(w.wellId, w);
            //        Console.WriteLine("Deleted well {0}", w.wellId);
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine("Failed to delete well {0}. Exception {1}.", wellId, e.Message);
            //    }
            //}

            // CODE TO UPDATE PBI INFO FOR CUSTOMERS
            
            //CustomerService cSvc = new CustomerService();
            //IEnumerable<Customer> allCusts = cSvc.GetAll();

            //foreach (Customer c in allCusts)
            //{
            //    if (c.customerName != null && c.customerName.ToUpper() == "SHELL")
            //    {
            //        c.pbiApplicationId = "95081d8b-bc10-4045-b302-49d0db2b99d3";
            //        c.pbiWorkspaceId = "c78e8ea1-acdc-411b-959d-a0d2fd0ba84d";
            //        c.pbiUser = pbiShellUserId;
            //        c.pbiPassword = pbiShellPassword;
            //        cSvc.Update(c.customerId, c);
            //    }
            //    if (c.customerName != null && (c.customerName.ToUpper() == "CHEVRON"))
            //    {
            //        c.pbiApplicationId = "95081d8b-bc10-4045-b302-49d0db2b99d3";
            //        c.pbiWorkspaceId = "fd4880d0-5311-4c85-9453-749624631e37";
            //        c.pbiUser = pbiChevronUserId;
            //        c.pbiPassword = pbiChevronPassword;
            //        cSvc.Update(c.customerId, c);
            //    }
            //    if (c.customerName != null && c.customerName.ToUpper().Contains("XTO"))
            //    {
            //        c.pbiApplicationId = "95081d8b-bc10-4045-b302-49d0db2b99d3";
            //        c.pbiWorkspaceId = "fd4880d0-5311-4c85-9453-749624631e37";
            //        c.pbiUser = pbiXtoEnergyUserId;
            //        c.pbiPassword = pbiXtoEnergyPassword;
            //        cSvc.Update(c.customerId, c);
            //    }
            //    if (c.customerName != null && (c.customerName.ToUpper().Contains("MURPHY")))
            //    {
            //        c.pbiApplicationId = "95081d8b-bc10-4045-b302-49d0db2b99d3";
            //        c.pbiWorkspaceId = "fd4880d0-5311-4c85-9453-749624631e37";
            //        c.pbiUser = pbiMurphyOilUserId;
            //        c.pbiPassword = pbiMurphyOilPassword;
            //        cSvc.Update(c.customerId, c);
            //    }
            //    if (c.customerId == -1)
            //    {
            //        c.pbiApplicationId = "95081d8b-bc10-4045-b302-49d0db2b99d3";
            //        c.pbiWorkspaceId = "fd4880d0-5311-4c85-9453-749624631e37";
            //        c.pbiUser = pbiChevronUserId;
            //        c.pbiPassword = pbiChevronPassword;
            //        cSvc.Update(c.customerId, c);
            //    }
            //}
            

            // code to pull rig 785 torque history
            using (StreamWriter sw = new StreamWriter("c:\\temp\\rig_785_torque.csv"))
            {
                sw.WriteLine("dtm,torque");
                RealTimeDataService<double> svc = new RealTimeDataService<double>();
                DateTime dateloop = DateTime.Now;
                DateTime endat = new DateTime(2020, 1, 9, 2, 30, 0);

                int loopcounter = 0;
                while (dateloop > endat)
                {
                    if (loopcounter++ % 10 == 0)
                    {
                        Console.WriteLine("in loop - query date is " + dateloop);
                    }
                    DateTime st = dateloop.AddMinutes(-60);
                    var res = svc.Get("785", "Top Drive Torque", st, dateloop);
                    foreach (var r in res.OrderByDescending(s => s.Timestamp))
                    {
                        sw.WriteLine("{0},{1}", r.Timestamp, r.Value);
                    }
                    dateloop = st;
                }
            }

            // CODE TO DELETE MARK'S LIST OF CUSTOMERS
            //CustomerService cSvc = new CustomerService();
            //IEnumerable<Customer> allCusts = cSvc.GetAll();

            //// first move 7238 to 35008
            //Customer c7238 = allCusts.First(c => c.customerId == 7238);
            //c7238.parentCustomerId = 35008;
            //cSvc.Update(7238, c7238);

            //// remove pbi credentials from 25150
            //Customer c25150 = allCusts.First(c => c.customerId == 25150);
            //c25150.pbiApplicationId = null;
            //c25150.pbiPassword = null;
            //c25150.pbiUser = null;
            //c25150.pbiWorkspaceId = null;
            //cSvc.Update(25150, c25150);

            ////IEnumerable<int> customerIdsToDelete = new[] { 34028 };
            //IEnumerable<int> customerIdsToDelete = new[] { 1, 4390, 5688, 6032, 6311, 7684, 5864, 7905, 7661, 34028 };

            //List<Customer> toDelete = new List<Customer>();

            //foreach (Customer c in allCusts)
            //{
            //    if (customerIdsToDelete.Contains(c.customerId))
            //    {
            //        Console.WriteLine("Adding to delete customer list {0}: {1}", c.customerId, c.customerName);
            //        toDelete.Add(c);
            //    }
            //}

            //foreach (Customer c in toDelete)
            //{
            //    try
            //    {
            //        Console.WriteLine("Deleting customer {0}: {1}", c.customerId, c.customerName);
            //        cSvc.Delete(c.customerId);
            //        Console.WriteLine("Deleted customer {0}: {1}", c.customerId, c.customerName);
            //    }
            //    catch
            //    {
            //        Console.WriteLine("Failed to delete customer {0}: {1}", c.customerId, c.customerName);
            //    }
            //}

//            toDelete.ForEach(del => cSvc.Delete(del.customerId));

            // CODE TO CHECK rig state FOR RIG 144 BACK FILLED DATA
            //RealTimeDataService<string> svc = new RealTimeDataService<string>();
            //DateTimeOffset st = new DateTimeOffset(new DateTime(2019, 11, 3, 15, 40, 0));
            //DateTimeOffset en = new DateTimeOffset(new DateTime(2019, 11, 9, 14, 20, 0));
            //DateTimeOffset loopDtm = st;
            //while (loopDtm.Ticks <= en.Ticks)
            //{
            //    Console.WriteLine("checking for records between {0} and {1}", st, loopDtm.AddHours(1));
            //    try
            //    {
            //        var records = svc.Get("144", "Rig State", st, loopDtm.AddHours(1)).OrderBy(d => d.Timestamp).ToList();
            //        if (records.Any())
            //        {
            //            Console.WriteLine("records returned between {0} and {1}", st, loopDtm.AddHours(1));
            //        }
            //        else
            //        {
            //            Console.WriteLine("***NO records returned between {0} and {1}", st, loopDtm.AddHours(1));
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine("Error getting data between {0} and {1}", st, loopDtm.AddHours(1));
            //    }
            //    finally
            //    {
            //        loopDtm = loopDtm.AddHours(1);
            //    }
            //}

            //RealTimeDataTuple<string> toWrite = new RealTimeDataTuple<string>();
            //toWrite.Timestamp = new DateTimeOffset(new DateTime(2019, 10, 3, 8, 16, 0));
        }
    }
}
