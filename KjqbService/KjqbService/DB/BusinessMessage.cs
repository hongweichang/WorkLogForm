//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace KjqbService.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class BusinessMessage
    {
        public int Id { get; set; }
        public Nullable<long> StarterID { get; set; }
        public Nullable<long> ReceiveID { get; set; }
        public Nullable<long> BusinessID { get; set; }
        public Nullable<decimal> TimeStamp { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<int> State { get; set; }
        public Nullable<int> IsRead { get; set; }
    }
}
