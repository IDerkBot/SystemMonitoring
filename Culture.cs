//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SystemMonitoring
{
    using System;
    using System.Collections.Generic;
    
    public partial class Culture
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Culture()
        {
            this.Seedings = new HashSet<Seeding>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Period { get; set; }
        public string Ph { get; set; }
        public string Phosphor { get; set; }
        public string Potassium { get; set; }
        public string Magnesium { get; set; }
        public string Calcium { get; set; }
        public string Humidity { get; set; }
        public string Nitrogen { get; set; }
        public string Temperature { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Seeding> Seedings { get; set; }
    }
}
