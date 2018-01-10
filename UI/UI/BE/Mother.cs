using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace BE
    {
        public class Mother
        {
        public string MotherLastName { get; set; }
        public string MothryFirstName { get; set; }
        public int MotherId { get; set; }
        public long MotherPhoneNumber { get; set; }
        public int MotherNumKids { get; set; }
        public string MotherEmailAddress { get; set; }
        public string MotherStreet { get; set; }
        public string MotherCity { get; set; }
        public string MotherTown { get; set; }
        public string MotherAdress
            {
                get
                {
                    return MotherAdress;
                }

                set
                {
                    string address = MotherStreet + "," + MotherCity + "," + MotherTown;
                }
            }
        public string MotherRagionNannyStreet { get; set; }
        public string MotherRagionNannyCity { get; set; }
        public string MotherRagionNannyTown { get; set; }
        public string MotherRagionNanny
            {
                get
                {
                    return MotherRagionNanny;
                }

                set
                {
                    string address = MotherRagionNannyStreet + "," + MotherRagionNannyCity + "," + MotherRagionNannyTown;
                }
            }
        public bool MotherPayByTheHour { get; set; }//אני הוספתי
        public bool[] MotherNeedNanny;
        public bool this[MyEnum.dayes i]
            {
                get {
                return MotherNeedNanny[(int)i];
            }
                set
            {
                MotherNeedNanny[(int)i] = value;
            }
            }
        //נעשה את זה כך שהעמודות יתחלקו לפי 7 ימים 
        //והשורות יהיו אינטים שיהיו הזמנים שלנו
        public int[,] MotherHoursAtWeek; 
        public int this[int i, MyEnum.dayes j]
            {
                get { return MotherHoursAtWeek[i,(int) j]; }
                set { MotherHoursAtWeek[i,(int)j] = value; }
            }
        public string Remarks { get; set; }
        public ObservableCollection<Child> motherchilds;

        //constractor
        public Mother()//a list of all the mother childrens
        {
            motherchilds = new ObservableCollection<Child>();
        }

        public override string ToString()
        {
            //have to add here the MotherRagionNanny and MotherHoursAtWeek
            return "Mother:\n" + MotherId + " " + MotherLastName + " " + MothryFirstName + '\n' + MotherPhoneNumber + '\n' + MotherAdress + "\nneeds Nanny at: " + MotherRagionNanny + "\nremarks: " + Remarks;
        }

    }
}

