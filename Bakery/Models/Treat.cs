using System.Collections.Generic;

namespace Bakery.Models
{
  public class Treat
  {
    public Treat()
    {
      this.JoinEntities = new HashSet<FlavourTreat>();
    }
   
    public int TreatId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual ApplicationUser User { get; set; }
    public virtual ICollection<FlavourTreat> JoinEntities { get; set; }
  }
}