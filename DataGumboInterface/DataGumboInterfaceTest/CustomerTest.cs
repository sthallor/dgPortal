using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using DataGumboInterface.Models.Structured;
using DataGumboInterface.Service;
using BlackGoldData;
using System.Text;

namespace DataGumboInterfaceTest
{
    [TestClass]
    public class CustomerTest
    {
        private static BlackGoldData.Blackgold_PRODEntities EntityDataModel { get; set; }

        static CustomerTest()
        {
            EntityDataModel = new BlackGoldData.Blackgold_PRODEntities();
        }

        [TestMethod]
        public void VerifyParents()
        {
            // get all customers from DG
            CustomerService svc = new CustomerService();
            StringBuilder errMessage = new StringBuilder();

            foreach (Customer cust in svc.GetAll())
            {
                if (cust.customerId == 7238)
                {
                    string s = "stop";
                }
                BA_Alias baa = EntityDataModel.BA_Alias.FirstOrDefault(b => b.ActiveInd && b.BusinessAssociate_BusinessAssociateID == cust.customerId);
                if ((baa != null) && (cust.parentCustomerId.GetValueOrDefault(cust.customerId) != baa.Alias_BusinessAssociateID))
                {
                    errMessage.AppendFormat("Parent customer for {0} should be {1} but is {2}.  ", cust.customerId, baa.Alias_BusinessAssociateID, cust.parentCustomerId.HasValue ? cust.parentCustomerId.ToString() : "blank");
                }
                else if ((baa == null) && (cust.parentCustomerId.HasValue))
                {
                    errMessage.AppendFormat("Parent customer for {0} should be blank but is {1}.  ", cust.customerId, cust.parentCustomerId);
                }
            }

            if (errMessage.Length > 0)
            {
                Assert.Fail(errMessage.ToString());
            }
        }
    }
}
