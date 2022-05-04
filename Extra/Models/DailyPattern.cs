using System;

namespace AvCalc.Models
{
    [Serializable]
    public class DailyPattern
    {
        public virtual int ColorId { get; set; }
        public virtual int PatternId { get; set; }
        public virtual DateTime ValidTo { get; set; }
    }
}
