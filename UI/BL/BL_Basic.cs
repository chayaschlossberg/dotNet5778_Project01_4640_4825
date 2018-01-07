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
using BE;
using DAL;

namespace BL
{
    internal class BL_Basic
    {

        DAL.IDAL dal;//מה זה
        DateTime localDate = DateTime.Now;
        //DAL.DS n;
        #region Nanny Function
        void AddNanny(Nanny nanny)
        {
            if (nanny.NannyBirthDate.Year < 2000)
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
            return dal.GetListNanniechilds(m);
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
        double salaryAfterDiscount(Nanny nanny, int ContractNumber)
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
        
        //רשימה של המטפלות לפי התמ"ת
        List<Nanny> ListTMTNanny()
        {
            List<Nanny> TMTNannyList = new List<Nanny>();

            //create an array to gat the Nannis
            Nanny[] arrTMTNanny = new Nanny[dal.GetNannyList().Count];

            //copy the Nannies to array
            dal.GetNannyList().CopyTo(arrTMTNanny);

            //runs at the array to collect the good nannies
            for (int i = 0; i < dal.GetNannyList().Count; i++)
            {
                if (arrTMTNanny[i].NannyVacationDays == true)
                    TMTNannyList.Add(arrTMTNanny[i]);
            }
            return TMTNannyList;
        }

        List<Nanny> NannyperfectForMother(Mother mother)//התאמה מושלמת בין מטפלת לאמא
        {
            List<Nanny> perfectNannyList = new List<Nanny>();//list of all the pearfect nannys

            Nanny[] arr = new Nanny[GetNannyList().Count];//array in the suze of our amount of nannies
            GetNannyList().CopyTo(arr);

            bool flag = true;
            for (int i = 0; i < dal.GetNannyList().Count; i++)//all of our nannys
            {
                for (int j = 0; j < 7; j++)//cheacking the days
                {
                    if (mother.MotherNeedNanny[j] == arr[i].NannyWorkDays[j])
                    {
                        for (int k = 0; k < 2; k++)//cheacking the hours
                        {
                            if (mother.MotherHoursAtWeek[k, j].Hour <= arr[i].NannyHoursAtWeek[k, j].Hour)
                            {

                            }
                            else
                                flag = false;
                        }
                    }
                    else
                        flag = false;
                }
                if (flag)
                    perfectNannyList.Add(arr[i]);
            }
            return perfectNannyList;
        }


        //linq
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
            Nanny[] arrayNanntGood = new Nanny[dal.GetNannyList().Count];
            //copy the nannies to array
            dal.GetNannyList().CopyTo(arrayNanntGood);

            //here the collectint of the selected nannies
            for (int i = 0; i < dal.GetNannyList().Count; i++)
            {
                if (CalculateDistance(address, arrayNanntGood[i].NannyAdress) <= km)
                {
                    nannyGoodForMother.Add(arrayNanntGood[i]);
                }
            }

            Nanny[] tempArrayToSort = new Nanny[nannyGoodForMother.Count];
            //copy the choosen nannies to array
            nannyGoodForMother.CopyTo(tempArrayToSort, 5 /* מה זה? *//* סתם שמתי מספר*/);/////////////////////////////////////////////////

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

        ///////////////////////////////////////////
        //מטלגרם
        //public IEnumerable<Contract> GetContracts(Func<Contract, bool> predicate = null)
        //{
        //    if (predicate == null) // returns a new list with the all contracts
        //    {
        //        return DataSource.ContractsList.Clone();
        //    }
        //    else // returns only the wanted contracts
        //    {
        //        return DataSource.ContractsList.Where(predicate);
        //    }
        //}
    }
}

