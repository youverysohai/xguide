//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace X_Guide
{
    using System;
    using System.Collections.Generic;
    
    public partial class Vision
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vision()
        {
            this.Calibrations = new HashSet<Calibration>();
        }
    
        public int Id { get; set; }
        public string Ip { get; set; }
        public Nullable<int> Port { get; set; }
        public string Terminator { get; set; }
        public string Name { get; set; }
        public string Filepath { get; set; }
        public Nullable<int> Software { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Calibration> Calibrations { get; set; }
    }
}
