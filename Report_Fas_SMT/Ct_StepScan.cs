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
    
    public partial class Ct_StepScan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ct_StepScan()
        {
            this.Ct_StepResult = new HashSet<Ct_StepResult>();
        }
    
        public short ID { get; set; }
        public string StepName { get; set; }
        public string Description { get; set; }
        public Nullable<short> NumStep { get; set; }
        public string ModelType { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ct_StepResult> Ct_StepResult { get; set; }
    }
}
