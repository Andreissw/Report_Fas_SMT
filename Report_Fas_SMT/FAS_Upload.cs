//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Report_Fas_SMT
{
    using System;
    using System.Collections.Generic;
    
    public partial class FAS_Upload
    {
        public int SerialNumber { get; set; }
        public string MAC { get; set; }
        public byte LineID { get; set; }
        public long SmartCardID { get; set; }
        public string CASID { get; set; }
        public string SWversion { get; set; }
        public string SWGS1version { get; set; }
        public bool LDS { get; set; }
        public System.DateTime UploadDate { get; set; }
        public short UploadByID { get; set; }
        public string ModelName { get; set; }
    
        public virtual FAS_SerialNumbers FAS_SerialNumbers { get; set; }
        public virtual FAS_Users FAS_Users { get; set; }
    }
}
