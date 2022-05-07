using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLabs.Common
{
    public static class Constants
    {
        public static Guid Service_A_Id { get => new Guid("e8ecaf84-21ea-4281-85ba-687871f0853b"); }
        public static Guid Service_B_Id { get => new Guid("186d7650-f18a-4e6f-a018-e5f4693799c4"); }
        public static Guid Service_C_Id { get => new Guid("bda7a4cb-7d7a-4edf-b575-282f70ba29d2"); }

        public static int[] WorkDays
        {
            get => new int[] {
                                ((int)DayOfWeek.Monday),
                                ((int)DayOfWeek.Tuesday),
                                ((int)DayOfWeek.Wednesday),
                                ((int)DayOfWeek.Thursday),
                                ((int)DayOfWeek.Friday)
                };
        }

        public static int[] AllDays
        {
            get => WorkDays
                .Append((int)DayOfWeek.Saturday)
                .Append((int)DayOfWeek.Sunday)
                .ToArray();
        }
    }
}
