using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using System.Collections.ObjectModel;

namespace BL
{
    public interface IBL
    {
        void InitializeList();

        #region Nanny Function
        void AddNanny(Nanny nanny);
        bool DeleteNanny(int id);
        void UpdateNanny(Nanny nanny);
        Nanny GetNanny(int id);
        IEnumerable<Nanny> GetListNannies(Func<Nanny, bool> predicat = null);
        ObservableCollection<Nanny> GetNannies();
        IEnumerable<Child> GetListNannieChilds(Nanny n);
        #endregion

        #region Mother Function
        void AddMother(Mother mother);
        bool DeleteMother(int id);
        void UpdateMother(Mother mother);
        Mother GetMother(int id);
        IEnumerable<Mother> GetListMothers(Func<Mother, bool> predicat = null);
        IEnumerable<Child> GetListMotherChilds(Mother m);
        #endregion

        #region Child Function
        void AddChild(Child child);
        bool DeleteChild(int id);
        void UpdateChild();
        Child GetChild(int id);
        IEnumerable<Child> GetListChildren(Func<Child, bool> predicat = null);
        ObservableCollection<Child> GetChilds();
        #endregion

        #region Contract Function
        void AddContract(int idChild, int idNanny, Contract contract);
        bool DeleteContract(int id);
        void UpdateContract();
        Contract GetContract(int ts);
        IEnumerable<Contract> GetListContracts(Func<Contract, bool> predicat = null);
        #endregion

        #region salary
        double SalaryPerMonth(int nId, int chId);
        double SalaryAfterDiscount(Nanny nanny, int ContractNumber);
        double PriceForContract(int nId, int chId);
        #endregion
        //void AddNannyToChild(int nannyid, int childid);

        ObservableCollection<Nanny> NannyperfectForMother(Mother mother);//התאמה מושלמת בין מטפלת לאמא
        ObservableCollection<Nanny> GoodNannysForMother(Mother mother);//התאמה קורבה בין 5 מטפלות לאמא
        ObservableCollection<Child> ChildWithoutNanny();//רשימה של ילדים בלי מטפלות

        IEnumerable<IGrouping<Nanny, Child>> NannyGrougByChildAge(bool maxMin, Nanny nanny);
        int Func(Nanny nanny, bool b);

    }
}
