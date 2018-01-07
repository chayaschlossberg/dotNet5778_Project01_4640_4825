using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Child
    {
        public int ChildId { get; set; }
        public int ChildNannyId { get; set; }
        public int ChildMotherId { get; set; }
        public string ChildFirestName { get; set; }
        public DateTime ChildBirthDate { get; set; }
        public bool ChildHasSpecialNeeds { get; set; }
        public string ChildSpecialNeeds { get; set; }
        
        
        //ToString
        public override string ToString()
        {
            return "Child:\n" + ChildId + " " + ChildFirestName + "\nMother ID:" + ChildMotherId + "\n" + ChildFirestName + " Birthday: " + ChildBirthDate + "\nhes special needs: " + ChildHasSpecialNeeds + "\nspecial needs: " + ChildSpecialNeeds;
        }
    }
}
