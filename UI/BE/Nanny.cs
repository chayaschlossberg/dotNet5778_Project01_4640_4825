using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace BE
{
    public class Nanny
    {
        public int NannyId { get; set; }
        public string NannyLastName { get; set; }
        public string NannyFirstName { get; set; }
        public DateTime NannyBirthDate { get; set; }//לא לשכוח איך מכניסים תאריך
        public int NannyPhoneNumber { get; set; }
        public int NannyMobileNumber { get; set; }
        public string NannyAdress { get; set; } //לא לשכוח לבדוק שזה משווה עם גוגל מאפ -הערה לעצמי
        public bool NannyHasElevator { get; set; }
        public int NannyWhichFloor { get; set; }
        public int NannyYearsOfExperience { get; set; }
        public int NannyAmountOfChildren { get; set; }//אני הוספתי לא כתוב בחוברת
        public int NumBrothersAdapter { get; set; }//לחשוב איך עושים את זה
        public int NannyMaxAmountOfChildren { get; set; }
        public int NannyMinChildrenAge { get; set; }//בחודשים
        public int NannyMaxChildrenAge { get; set; }//בחודשים
        public bool NannyIsPayByTheHour { get; set; }
        public int NannySalaryPerHour
            {
                get;
                set;
                //{
                //    if (NannyIsPayByTheHour != true)
                //        throw (NannyFirstName + " " + NannyLastName + " don't get paid by the hour.")
                //}
            }
        public int NannySalaryPerMonth { get; set; }
        private bool[] NannyWorkDays;
        public bool this[MyEnum.dayes i]
            {
                get
                {
                    return NannyWorkDays[(int)i];
                }
                set
                {
                    NannyWorkDays[(int)i] = value;
                }
            }
        private int[,] NannyHoursAtWeek;
        public int this[int i, MyEnum.dayes j]
            {
                get
                {
                    return NannyHoursAtWeek[i, (int)j];
                }
                set
                {
                    NannyHoursAtWeek[i, (int)j] = value;
                }
            }
        public bool NannyVacationDays { get; set; }
        public string NannyRecommendations{ get; set;}
        public string NannyRemarkes { get; set; }
        public string NannyBankName { get; set; }
        public int NannyBranchNumber { get; set; }
        public int NannyAccountNumber { get; set; }
        public int NannyBankAccount { get; set; }
        public ObservableCollection<Child> nannychilds;

        //constractor
        public Nanny()
        {
            nannychilds = new ObservableCollection<Child>();
        }

        //TOSTRING
        public override string ToString()
        {
            //have to add the days of working
            //not sure we need all the details
            return NannyId + " " + NannyLastName + " " + NannyFirstName + "\nphone number: " + NannyPhoneNumber +
                "\naddress: " + NannyAdress + "\nhas elevator: " + NannyHasElevator + "\nfloor: " +
                NannyWhichFloor + "\nexperiens: " + NannyYearsOfExperience + "\nmaximum amount of children: " +
                NannyMaxAmountOfChildren + "\nages: betwwen " + NannyMinChildrenAge + " to " + NannyMaxChildrenAge +
                "\npay by hours: " + NannyIsPayByTheHour + "\nsalary per hour: " + NannySalaryPerHour +
                "\nsalary per month: " + NannySalaryPerMonth + "\nvacation days: " + NannyVacationDays +
                "\nrecommandations: " + NannyRecommendations + "\nBankAccount: " + NannyBankAccount;
        }
    }
}
