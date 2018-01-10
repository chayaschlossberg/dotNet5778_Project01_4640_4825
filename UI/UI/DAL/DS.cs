using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using BE;
namespace DAL
{
    class DS
    {
        //create lists for fields:
        public ObservableCollection<Nanny> nannys;
        public ObservableCollection<Mother> mothers;
        public ObservableCollection<Child> childs;
        public ObservableCollection<Contract> contracts;
        
        //ctor
        public DS()
        {
            nannys= new ObservableCollection<Nanny>();
            mothers= new ObservableCollection<Mother>();
            childs= new ObservableCollection<Child>();
            contracts= new ObservableCollection<Contract>();
        }
    }
}
