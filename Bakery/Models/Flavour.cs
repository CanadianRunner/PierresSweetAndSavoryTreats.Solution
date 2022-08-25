using System.Collections.Generic;

namespace Bakery.Models
{
  public class Flavour
  {
    public Flavour()
    {
      this.JoinEntities = new HashSet<FlavourTreat>();
    }

    public int FlavourId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual UserId User { get; set; }
    public virtual ICollection<FlavourTreat> JoinEntities { get; set; }
  }
}