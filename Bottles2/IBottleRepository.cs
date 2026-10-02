namespace Bottles
{
    public interface IBottleRepository
    {
        IEnumerable<Bottle> GetBottles(string? nameStartsWith = null, double? minVolume = null, string? sortOrder = null);
        Bottle? GetById(int id);
        Bottle AddBottle(Bottle b);
        Bottle? DeleteById(int id);
        Bottle? UpdateById(int id, Bottle data);
    }
}