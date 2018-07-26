using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolver
{
    [Table("Mutants")]
    [DataObject]
    public class MutantStat
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public Int64 Score { get; set; }
        public Int64 Turns { get; set; }
        public double ScorePer100Turns { get; set; }
        public Int64 Generation { get; set; }
    }
}
