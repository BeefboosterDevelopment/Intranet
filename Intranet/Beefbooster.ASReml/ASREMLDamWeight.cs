using System;

namespace Beefbooster.ASREML
{
       public class ASREMLDamWeight
       {
           private readonly Int16 _wt;
           private readonly int _age;
           public ASREMLDamWeight(Int16 damWt, int damAge)
           {
               _wt = damWt;
               _age = damAge;
           }

           public short Wt
           {
               get { return _wt; }
           }

           public int Age
           {
               get { return _age; }
           }
       }
}
