using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.Elevation.Request;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.StaticMaps;
using GoogleMapsApi.StaticMaps.Entities;
//using System.ComponentModel.DataAnnotations;
//using System.IO;
using BE;
using DAL;
using System.Collections.ObjectModel;
using System.Threading;
using System.Net.Mail;

namespace BL
{
    internal class BL_Basic
    {
        DAL.IDAL dal;
        DateTime localDate = DateTime.Now;

        #region Nanny Function
        void AddNanny(Nanny nanny)
        {
            if (nanny.NannyBirthDate.Year > 2000)
                throw new Exception("The nanny is too young ");
            dal.AddNanny(nanny);
        }
        bool DeleteNanny(int id)
        {
            return dal.DeleteNanny(id);
        }
        void UpdateNanny(Nanny nanny)
        {
            dal.UpdateNanny(nanny);
        }
        Nanny GetNanny(int id)
        {
            return dal.GetNanny(id);
        }
        IEnumerable<Nanny> GetListNannies(Func<Nanny, bool> predicat = null)
        {
            return dal.GetListNannies();
        }
        ObservableCollection<Nanny> GetNannies()
        {
            return dal.GetNannies();
        }
        IEnumerable<Child> GetListNannieChilds(Nanny n)
        {
            return dal.GetListNanniechilds(n);
        }
        #endregion

        #region Mother Function
        void AddMother(Mother mother)
        {
            dal.AddMother(mother);
        }
        bool DeleteMother(int id)
        {
            return dal.DeleteMother(id);
        }
        void UpdateMother(Mother mother)
        {
            dal.UpdateMother(mother);
        }
        Mother GetMother(int id)
        {
            return dal.GetMother(id);
        }
        IEnumerable<Mother> GetListMothers(Func<Mother, bool> predicat = null)
        {
            return dal.GetListMothers();
        }
        IEnumerable<Child> GetListMotherChilds(Mother m)
        {
            return dal.GetListMotherChilds(m);
        }
        #endregion

        #region Child Function
        void AddChild(Child child)
        {
            if (child.ChildBirthDate.Year == localDate.Year && (child.ChildBirthDate.Month + 3) >= localDate.Month)
                throw new Exception("The child is too young...");
            dal.AddChild(child);
        }
        bool DeleteChild(int id)
        {
            return dal.DeleteChild(id);
        }
        void UpdateChild(Child child)
        {
            dal.UpdateChild(child);
        }
        Child GetChild(int id)
        {
            return dal.GetChild(id);
        }
        ObservableCollection<Child> GetChilds()
        {
            return dal.GetChilds();
        }
        IEnumerable<Child> GetListChildren(Func<Child, bool> predicat = null)
        {
            return dal.GetListChildren();
        }
        #endregion

        #region Contract Function
        void AddContract(int idChild, int idNanny, Contract contract)
        {
            Child c = GetChild(idChild);
            Nanny n = GetNanny(idNanny);
            Mother m = GetMother(c.ChildMotherId);
            if (m == null || n == null || c == null)
                throw new Exception("Can not create a new contract");
            if (n.NannyMaxAmountOfChildren == n.NannyAmountOfChildren)
                throw new Exception("The Nanny can't except enymore kids to her daycare");
            dal.AddContract(contract);
            contract.ContractChildID = idChild;
            contract.ContractNannyID = idNanny;
        }
        bool DeleteContract(int ts)
        {
            return dal.DeleteContract(ts);
        }
        void UpdateContract(Contract contract)
        {
            dal.UpdateContract(contract);
        }
        Contract GetContract(int ts)
        {
            return dal.GetContract(ts);
        }
        IEnumerable<Contract> GetListContracts(Func<Contract, bool> predicat = null)
        {
            return dal.GetListContracts();
        }
        #endregion

        double SalaryPerMonth(int nId, int mid)
        {
            Nanny n = GetNanny(nId);
            Mother m = GetMother(mid);
            if (!n.NannyIsPayByTheHour)
                return n.NannySalaryPerMonth;
            int count = 0;
            for (int i = 0; i < 7; i++)
            {
                //שעת סיום מינוס שעת התחלה
                count += m.MotherHoursAtWeek[1, i] - m.MotherHoursAtWeek[0, i];
            }
            return n.NannySalaryPerHour * count * 4;
        }
        //צריך לעשות בדיקה נוספת שכל הילדים הם אצל אותה מטפלת
        double SalaryAfterDiscount(Nanny nanny, int ContractNumber)
        {
            Contract c = GetContract(ContractNumber);
            Nanny n = GetNanny(c.ContractNannyID);
            Mother m = GetMother(c.ContractMotherID);
            double salary = SalaryPerMonth(c.ContractNannyID, c.ContractMotherID);
            if (m.MotherNumKids <= 1)
                return salary;
            double x = m.MotherNumKids * 0.02 * salary;//x is the amount of reduce
            salary -= x;
            return salary;
        }

        ObservableCollection<Nanny> NannyperfectForMother(Mother mother)//התאמה מושלמת בין מטפלת לאמא
        {
            ObservableCollection<Nanny> perfectNannyList = new ObservableCollection<Nanny>();//list of all the pearfect nannys

            Nanny[] arr = new Nanny[GetListNannies().Count()];//array in the suze of our amount of nannies
            GetNannies().CopyTo(arr, 0);

            for (int i = 0; i < GetListNannies().Count(); i++)//chacking all of our nannys if they fit to our mother
            {
                for (int j = 0; j < 7; j++)//cheacking the days
                {
                    if ((mother.MotherNeedNanny[j] == arr[i].NannyWorkDays[j]) &&
                            (mother.MotherHoursAtWeek[0, j] < arr[i].NannyHoursAtWeek[0, j]) ||
                        (mother.MotherHoursAtWeek[0, j] == arr[i].NannyHoursAtWeek[0, j] && mother.MotherHoursAtWeek[1, j] <= arr[i].NannyHoursAtWeek[1, j])
                        && (mother.MotherHoursAtWeek[2, j] < arr[i].NannyHoursAtWeek[2, j]) ||
                        (mother.MotherHoursAtWeek[2, j] == arr[i].NannyHoursAtWeek[2, j] && mother.MotherHoursAtWeek[3, j] <= arr[i].NannyHoursAtWeek[3, j]));//keep going
                    else//go to anthe nanny
                        break;
                    if (j == 6)
                        perfectNannyList.Add(arr[i]);//after cheaking all of the dayes then we add the nanny to the pearfect list
                }
            }
            return perfectNannyList;
        }

        ObservableCollection<Nanny> goodNannysForMother(Mother mother)//התאמה קורבה בין 5 מטפלות לאמא
        {
            //במקרה שיש 5 או פחות מטפלות
            if (GetListNannies().Count() <= 5)
                return GetNannies();
            ObservableCollection<Nanny> GoodNannyList = new ObservableCollection<Nanny>();
            Nanny[] arr = new Nanny[GetListNannies().Count()];
            GetNannies().CopyTo(arr, 0);
            int[] arras = new int[GetListNannies().Count()];

            for (int i = 0; i < GetListNannies().Count(); i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    arras[i] += (mother.MotherHoursAtWeek[2, j] - mother.MotherHoursAtWeek[0, j]) - (arr[i].NannyHoursAtWeek[2, j] - arr[i].NannyHoursAtWeek[0, j]);
                }
            }
            for (int i = 0; i < 5; i++)
            {
                int index = arras.ElementAt(arras.Min());
                GoodNannyList.Add(arr[index]);
                arras[index] = arras.Max() + 1;
            }

            return GoodNannyList;
        }

        ObservableCollection<Child> childWithoutNanny()//רשימה של ילדים בלי מטפלות
        {
            ObservableCollection<Child> childs = GetChilds();
            ObservableCollection<Child> childsWithoutNanny = new ObservableCollection<Child>();
            for (int i = 0; i < childs.Count(); i++)
            {
                //childs[i].ChildNannyId == 0    ---> means that the child doesnt have nanny then add the child to the childsWithoutNanny ObservableCollection
                if (childs[i].ChildNannyId == 0)
                    childsWithoutNanny.Add(childs[i]);
            }
            return childsWithoutNanny;
        }

        //רשימה של המטפלות לפי התמ"ת
        List<Nanny> ListTMTNanny()
        {
            List<Nanny> TMTNannyList = new List<Nanny>();

            //create an array to gat the Nannis
            Nanny[] arrTMTNanny = new Nanny[GetListNannies().Count()];

            //copy the Nannies to array
            GetNannies().CopyTo(arrTMTNanny, 0);
            //runs at the array to collect the good nannies
            for (int i = 0; i < GetListNannies().Count(); i++)
            {
                if (arrTMTNanny[i].NannyVacationDays == true)
                    TMTNannyList.Add(arrTMTNanny[i]);
            }
            return TMTNannyList;
        }

        //linq, grouping by age
        public IEnumerable<IGrouping<int, Nanny>> NannyGroupingByChildAge(bool maxMin, bool sort)
        {
            //group by min age
            if (maxMin == false)
            {
                if (sort)
                {//choosed to sort the nannies:
                    var result =
                        from nanny in dal.GetListNannies()
                        orderby nanny.NannyMinChildrenAge //here sort
                        group nanny by (nanny.NannyMinChildrenAge / 3) into g
                        select new { ageRang = g.Key, groups = g };
                    return (IEnumerable<IGrouping<int, Nanny>>)result;
                }
                else
                {//don't care bt sorted
                    var result =
                        from nanny in dal.GetListNannies()
                        group nanny by (nanny.NannyMinChildrenAge / 3) into g
                        select new { ageRang = g.Key, groups = g };
                    return (IEnumerable<IGrouping<int, Nanny>>)result;
                }
            }
            //group by max age
            else
            {
                if (sort)
                {//choosed to sort the nannies:
                    var result =
                        from nanny in dal.GetListNannies()
                        orderby nanny.NannyMaxChildrenAge //here sort
                        group nanny by (nanny.NannyMaxChildrenAge / 3) into g
                        select new { ageRang = g.Key, groups = g };
                    return (IEnumerable<IGrouping<int, Nanny>>)result;
                }
                else
                {//don't care bt sorted
                    var result =
                        from nanny in dal.GetListNannies()
                        group nanny by (nanny.NannyMaxChildrenAge / 3) into g
                        select new { ageRang = g.Key, groups = g };
                    return (IEnumerable<IGrouping<int, Nanny>>)result;
                }
            }
        }

        public static int CalculateDistance(string source, string dest)
        {
            var drivingDirectionRequest = new DirectionsRequest
            {
                TravelMode = TravelMode.Walking,
                Origin = source,
                Destination = dest,
            };
            DirectionsResponse drivingDirections = GoogleMaps.Directions.Query(drivingDirectionRequest);
            Route route = drivingDirections.Routes.First();
            Leg leg = route.Legs.First();
            return leg.Distance.Value;
        }

        //אילוצי האם בלי תהליכונים
        List<Nanny> MotherConstraints(string address, int km)
        {
            List<Nanny> nannyGoodForMother = new List<Nanny>();
            //create an array to gat the nannies
            Nanny[] arrayNanntGood = new Nanny[dal.GetListNannies().Count()];
            //copy the nannies to array
            dal.GetNannies().CopyTo(arrayNanntGood,0);

            //here the collectint of the selected nannies
            for (int i = 0; i < dal.GetListNannies().Count(); i++)
            {
                if (CalculateDistance(address, arrayNanntGood[i].NannyAdress) <= km)
                {
                    nannyGoodForMother.Add(arrayNanntGood[i]);
                }
            }

            Nanny[] tempArrayToSort = new Nanny[nannyGoodForMother.Count];
            //copy the choosen nannies to array
            nannyGoodForMother.CopyTo(tempArrayToSort, 0);//copy the observablecollection to array from the first place (0)

            //loop to sort the List "nannyGoodForMother"
            //sort by distance
            //the algorithem is token from wikipedia (bubble sort -> http://www.algorithmist.com/index.php/Bubble_sort)
            bool swapped = true;
            while (swapped == true)
            {
                swapped = false;
                for (int y = 0; y < tempArrayToSort.Length - 1; y++)
                {
                    //checkinf the disrance:
                    int x1 = CalculateDistance(address, arrayNanntGood[y].NannyAdress);
                    int x2 = CalculateDistance(address, arrayNanntGood[y + 1].NannyAdress);

                    if (x1 > x2)
                    {//swap
                        swapped = true;
                        Nanny temp = tempArrayToSort[y];
                        tempArrayToSort[y] = tempArrayToSort[y + 1];
                        tempArrayToSort[y + 1] = temp;
                    }
                }
            }

            //returns sorted List of nannies (by distance)
            return nannyGoodForMother;
        }

        public void SendEmail(string recieverEmailaddress)//send out email to user with randomized reset code to reset password
        {
            Mother mother = GetListMothers(e => e.MotherEmailAddress == recieverEmailaddress).FirstOrDefault();
            string name;
            if (mother == null)//Exeption if the Email not exist
                throw new Exception("Oops, this Email address not valid:(");
            else//if email does exist in the system
            {
                //update the mother
                name = mother.MothryFirstName;
                dal.UpdateMother(mother);
            }
            MailMessage mail = new MailMessage("daycareforbabies@gmail.com", recieverEmailaddress, "HighTech Recruitment Password Reset", "Hi " + name + ",\nAsked for a code to reset your password? You got it. Just enter the code below to reset.\n \nHere is your code:    " + code + " \n \nAlways at your service,\nthe HighTech Recruitment Team");
            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.Port = 587;
            client.Credentials = new System.Net.NetworkCredential("daycareforbabies", "ora&cahaya");
            client.EnableSsl = true;
            Thread newWindowThread = new Thread(new ThreadStart(() =>
            {
                // Create and show the Window
                client.Send(mail);
                // Start the Dispatcher Processing
                System.Windows.Threading.Dispatcher.Run();
            }));
            // Set the apartment state
            newWindowThread.SetApartmentState(ApartmentState.STA);
            // Make the thread a background thread
            newWindowThread.IsBackground = true;
            // Start the thread
            newWindowThread.Start();
        }
        //public string GetRandomString()
        //{
        //    string path = Path.GetRandomFileName();
        //    path = path.Replace(".", ""); // Remove period
        //    return path.Substring(0, 8);  // Return 8 character string
        //}

        public void EmailConfirmation(string recieverEmailaddress) //Send out sign up confirmation to user upon successfully becoming a member
        {
            IEnumerable<Mother> mother = GetListMothers(e => e.MotherEmailAddress == recieverEmailaddress);
            string name;
            if (mother.Any())
                name = mother.First().MothryFirstName;
            else
                throw new Exception("Oops, this Email address not valid:(");
            MailMessage mail = new MailMessage("daycareforbabies@gmail.com", recieverEmailaddress, "Welcome Abroad!", "\nWelcome " + name + ", \n" + " " + "We are glad you have decided to join the Day Care Family.\n" + "we hope you'll find the best adjustment for your needs! \n" + "Your Child Daycare Cheerleading Squad:)");
            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.Port = 587;
            client.Credentials = new System.Net.NetworkCredential("daycareforbabies", "ora&cahaya");
            client.EnableSsl = true;
            Thread newWindowThread = new Thread(new ThreadStart(() =>
            {
                // Create and show the Window
                client.Send(mail);
                // Start the Dispatcher Processing
                System.Windows.Threading.Dispatcher.Run();
            }));
            // Set the apartment state
            newWindowThread.SetApartmentState(ApartmentState.STA);
            // Make the thread a background thread
            newWindowThread.IsBackground = true;
            // Start the thread
            newWindowThread.Start();

        }
    }
}

