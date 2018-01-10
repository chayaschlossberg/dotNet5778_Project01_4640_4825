using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using System.Collections.ObjectModel;

namespace DAL
{
    public interface IDAL
    {
        #region Nanny Function
        void AddNanny(Nanny nanny);
        bool DeleteNanny(int id);
        void UpdateNanny(Nanny nanny);
        Nanny GetNanny(int id);
        IEnumerable<Nanny> GetListNannies(Func<Nanny, bool> predicat = null);
        ObservableCollection<Nanny> GetNannies();//as ObservableCollection
        IEnumerable<Child> GetListNanniechilds(Nanny n);
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
        void UpdateChild(Child child);
        Child GetChild(int id);
        IEnumerable<Child> GetListChildren(Func<Child, bool> predicat = null);
        ObservableCollection<Child> GetChilds();
        #endregion

        #region Contract Function
        void CreateContract(Contract contract);//creating the num of the contract
        void AddContract(Contract contract);
        bool DeleteContract(int ts);
        void UpdateContract(Contract contract);
        Contract GetContract(int ts);
        IEnumerable<Contract> GetListContracts(Func<Contract, bool> predicat = null);
        #endregion  

    }
}
