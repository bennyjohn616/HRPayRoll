using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public class RelationShip
    {
        public int Id { get; set; }
        public string Name { get; set; }

        private static List<RelationShip> _relationShips;
        public RelationShip()
        {
            if (object.ReferenceEquals(_relationShips, null))
            {
                _relationShips = new List<RelationShip>();
                _relationShips.Add(new RelationShip() { Id = 1, Name = "Father" });
                _relationShips.Add(new RelationShip() { Id = 2, Name = "Mother" });
                _relationShips.Add(new RelationShip() { Id = 3, Name = "Son" });
                _relationShips.Add(new RelationShip() { Id = 4, Name = "Daughter" });
                _relationShips.Add(new RelationShip() { Id = 5, Name = "Sister" });
                _relationShips.Add(new RelationShip() { Id = 6, Name = "Brother" });
                _relationShips.Add(new RelationShip() { Id = 7, Name = "Spouse" });
                _relationShips.Add(new RelationShip() { Id = 8, Name = "Others" });
            }

        }
        public static RelationShip Get(int id)
        {
            if (object.ReferenceEquals(_relationShips, null))
            {
                RelationShip tmp = new RelationShip();
              
            }
            var ret = _relationShips.Where(u => u.Id == id).FirstOrDefault();
            if (object.ReferenceEquals(ret, null))
                ret = new RelationShip();
            return ret;
        }
        public List<RelationShip> GetRelationship()
        {
            if (object.ReferenceEquals(_relationShips, null))
            {
                RelationShip tmp = new RelationShip();
            }
            return _relationShips;
        }
    }
}
