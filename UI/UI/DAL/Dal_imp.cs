using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Collections.ObjectModel;
using BE;

namespace DAL
{
    internal class Dal_imp : DS
    {
        #region Nanny:
        void AddNanny(Nanny nanny)
        {
            Nanny n = GetNanny(nanny.NannyId);
            if (n != null)
                throw new Exception("Nanny with the same id already exists");
            nannys.Add(nanny);
        }
        bool DeleteNanny(int id)
        {
            Nanny n = GetNanny(id);
            if (n == null)
                throw new Exception("Nanny with this id doesn't exists");
            return nannys.Remove(n);
        }
        void UpdateNanny(Nanny nanny)
        {
            int index = nannys.IndexOf(nanny);
            if (index == -1)//wasn't found
                throw new Exception("Nanny with this id doesn't exists");
            nannys[index] = nanny;
        }
        Nanny GetNanny(int id)
        {
            return nannys.FirstOrDefault(n => n.NannyId == id);
        }
        IEnumerable<Nanny> GetListNannies(Func<Nanny, bool> predicat = null)
        {
            if (predicat == null)
                return nannys.AsEnumerable();
            return nannys.Where(predicat);
        }
        ObservableCollection<Nanny> GetNannies()
        {
            return nannys;
        }
        IEnumerable<Child> GetListNannieChilds(Nanny n)
        {
            if (n == null)
                throw new Exception("Nanny doesn't exists");
            return n.nannychilds;
        }

        #endregion

        #region Mother:
        void AddMother(Mother mother)
        {
            Mother m = GetMother(mother.MotherId);
            if (m != null)
                throw new Exception("Mother with the same id already exists");
            mothers.Add(mother);
        }
        bool DeleteMother(int id)
        {
            Mother m = GetMother(id);
            if (m == null)
                throw new Exception("Mother with this id doesn't exists");
            return mothers.Remove(m);
        }
        void UpdateMother(Mother mother)
        {
            int index = mothers.IndexOf(mother);
            if (index == -1)
                throw new Exception("Mother with this id doesn't exists");
            mothers[index] = mother;
        }
        Mother GetMother(int id)
        {
            return mothers.FirstOrDefault(m => m.MotherId == id);
        }
        IEnumerable<Mother> GetListMothers(Func<Mother, bool> predicat = null)
        {
            if (predicat == null)
                return mothers.AsEnumerable();
            return mothers.Where(predicat);
        }
        IEnumerable<Child> GetListMotherChilds(Mother m)
        {
            if (m == null)
                throw new Exception("Mother  doesn't exists");
            return m.motherchilds;
        }
        #endregion

        #region Child:
        void AddChild(Child child)
        {
            Child c = GetChild(child.ChildId);
            Mother m = GetMother(c.ChildMotherId);
            Nanny n = GetNanny(c.ChildNannyId);
            if (c != null)
                throw new Exception("Child with the same id already exists");
            childs.Add(child);
            m.MotherNumKids += 1;//מוסיפים ילד למספר ילדים של האמא
            m.motherchilds.Add(c);//נוסיף את הילד לרשימה של האמא
            n.nannychilds.Add(c);//נוסיף ילד לרשימה של המטפלת
        }
        bool DeleteChild(int id)
        {
            Child c = GetChild(id);
            if (c == null)
                throw new Exception("Child with this id doesn't exists");
            return childs.Remove(c);
        }
        void UpdateChild(Child child)
        {
            int index = childs.IndexOf(child);
            if (index == -1)
                throw new Exception("Child with this id doesn't exists");
            childs[index] = child;
        }
        Child GetChild(int id)
        {
            return childs.FirstOrDefault(c => c.ChildId == id);
        }
        IEnumerable<Child> GetListChildren(Func<Child, bool> predicat = null)
        {
            if (predicat == null)
                return childs.AsEnumerable();
            return childs.Where(predicat);
        }
        ObservableCollection<Child> GetChilds()
        {
            return childs;
        }
        #endregion

        #region Contract:
        public static int count = 11111111;
        void CreateContract(Contract contract)
        {
            if (contract.TransactionContract != 0)
                throw new Exception("contract already exists");
            contract.TransactionContract = count;
            count++;
        }
        void AddContract(Contract contract)
        {
            CreateContract(contract);//creating a number for our contract
            Contract c = GetContract(contract.TransactionContract);
            if (c != null)
                throw new Exception("Contract already exists");
            contracts.Add(contract);
        }
        bool DeleteContract(int ts)
        {
            Contract c = GetContract(ts);
            if (c == null)
                throw new Exception("Contract with this transaction number doesn't exists");
            return contracts.Remove(c);
        }
        void UpdateContract(Contract contract)
        {
            int index = contracts.IndexOf(contract);
            if (index == -1)
                throw new Exception("Contract doesn't exists");
            contracts[index] = contract;
        }
        Contract GetContract(int ts)
        {
            return contracts.FirstOrDefault(t => t.TransactionContract == ts);
        }
        IEnumerable<Contract> GetListContracts(Func<Contract, bool> predicat = null)
        {
            if (predicat == null)
                return contracts.AsEnumerable();
            return contracts.Where(predicat);
        }
        #endregion
    }
}