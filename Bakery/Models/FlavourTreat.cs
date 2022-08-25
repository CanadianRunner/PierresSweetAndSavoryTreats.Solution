namespace Bakery.Models
{
  public class FlavourTreat
  {       
    public int FlavourTreatId { get; set; }
    public int FlavourId { get; set; }
    public int TreatsId { get; set; }
    public virtual Flavour Flavour { get; set; }
    public virtual Treat Treat { get; set; }
  }
}