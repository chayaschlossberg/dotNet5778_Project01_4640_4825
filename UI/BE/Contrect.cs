using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BE
    {
        public class Contract
        {
        //have to make the Transaction contract
        public int TransactionContract { get; set; }
        public int ContractNannyID { get; set; }
        public int ContractMotherID { get; set; }
        public int ContractChildID { get; set; }
        public bool ContractIntroductoryMeeting { get; set; }//האם נערכה פגישת היכרות
        public bool ContractHaveTransactionContract { get; set; }//האם נחתם חוזה העסקה
        public MyEnum.ContractTypePrice TypePrice { get; set; }//goes with Enums
        public int ContractPricePetHours { get; set; }
        public int ContractPricePetMonthes { get; set; }
        public string ContractDateOfStarting { get; set; }
        public string ContractDateOfEnding { get; set; }
        public int ContractTotalPtice { get; set; }//the price itself


        //ToString
        public override string ToString()
        {//can add the spaesific days to the contract
                return TransactionContract + "\nNanny ID: " + ContractNannyID +
                "\ncild ID: " + ContractChildID + "\nstart at: " + ContractDateOfStarting 
                + "\nend at: " + ContractDateOfEnding + "\ntotal price: " + ContractTotalPtice;
        }

        }
    }


