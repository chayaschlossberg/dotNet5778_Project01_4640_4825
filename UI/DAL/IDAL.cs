using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

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
        IEnumerable<Child> GetListNanniechilds(Nanny n);
        #endregion

        #region Mother Function
        void AddMother(Mother mother);
        bool DeleteMother(int id);
        void UpdateMother(Mother mother);
        Mother GetMother(int id);
        IEnumerable<Mother> GetListMothers(Func<Mother, bool> predicat = null);
        #endregion

        #region Child Function
        void AddChild(Child child);
        bool DeleteChild(int id);
        void UpdateChild(Child child);
        Child GetChild(int id);
        IEnumerable<Child> GetListChildren(Func<Child, bool> predicat = null);
        #endregion

        #region Contract Function
        void CreateContract(Contract contract);//creating the num of the contract
        void AddContract(Contract contract);
        bool DeleteContract(int ts);
        void UpdateContract(Contract contract);
        Contract GetContract(int ts);
        IEnumerable<Contract> GetListContracts(Func<Contract, bool> predicat = null);
        #endregion

        //#region Adapter
        //MotherChildAdapter GetMotherChildAdapter(int cid);
        //void AddMotherChild(MotherChildAdapter motherchild);
        //void UpdateMotherChild(MotherChildAdapter motherchild);
        //void AddNannyChild(MotherChildAdapter nannychild);
        //void UpdateNannyChild(NannyChildAdapter nannychild);
        //bool RemoveChildFromNanny(int childid, int nannyid);
        //IEnumerable<NannyChildAdapter> GetAllNannyChilds(Func<NannyChildAdapter, bool> predicat = null);
        //#endregion

      
    }
}
