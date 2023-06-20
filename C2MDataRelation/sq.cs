using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MDataRelation
{
    class Sq
    {
        String uom;
        String sqi;
        String tou;

        public Sq(string uom, string sqi, string tou)
        {
            this.uom = uom;
            this.sqi = sqi;
            this.tou = tou;
        }

        public Sq()
        {
        }

        public string Uom { get => uom; set => uom = value; }
        public string Sqi { get => sqi; set => sqi = value; }
        public string Tou { get => tou; set => tou = value; }

        public bool equals(Sq otherSq) {
            if (this.uom.Equals(otherSq.Uom) && this.sqi.Equals(otherSq.Sqi) && this.tou.Equals(otherSq.Tou)) {
                return true;
            }
            return false;
        }
        public string print() {
            return "uom : " + this.uom + ", sqi : " + this.sqi + ", tou : " + this.tou;

        }

        public override bool Equals(object obj)
        {
            return obj is Sq sq &&
                   uom == sq.uom &&
                   sqi == sq.sqi &&
                   tou == sq.tou;
        }

        public override int GetHashCode()
        {
            int hashCode = -1174551901;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(uom);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(sqi);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(tou);
            return hashCode;
        }
    }
}
