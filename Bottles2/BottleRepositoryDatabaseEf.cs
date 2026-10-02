using Microsoft.EntityFrameworkCore;

namespace Bottles
{
    public class BottleRepositoryDatabaseEf : IBottleRepository
    {
        private readonly BottlesDbContext _context;
        public BottleRepositoryDatabaseEf(BottlesDbContext context)
        {
            _context = context; 
        }


        public IEnumerable<Bottle> GetBottles(string? nameStartsWith = null,
                   double? minVolume = null,
                   string? sortOrder = null)
        {

            IEnumerable<Bottle> result = _context.Bottles;

            if (minVolume != null)
            {
                result = result.Where(b => b.Volume > minVolume);
            }
            if (nameStartsWith != null)
            {
                result = result.Where(b => b.Name != null && b.Name.StartsWith(nameStartsWith));
            }
            if (sortOrder != null)
            {
                switch (sortOrder.ToLower())
                {
                    case "name":
                    case "nameasc":
                        result = result.OrderBy(b => b.Name);
                        break;
                    case "namedesc":
                        result = result.OrderByDescending(b => b.Name);
                        break;
                    case "volume":
                        result = result.OrderBy(b => b.Volume);
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        public IEnumerable<Bottle> GetBottles(string? nameStartsWith = null)
        {
            IEnumerable<Bottle> result = _context.Bottles;

            return _context.Bottles.ToList();
        }
        public Bottle? GetById(int id)
        {
            return _context.Bottles.Find(id);
        }

        public Bottle AddBottle(Bottle b)
        {
            if (b is null)
            {
                throw new ArgumentNullException(nameof(b));
            }
            _context.Bottles.Add(b);
            _context.SaveChanges();
            return b;
        }

        public Bottle? DeleteById(int id)
        {
            Bottle? bottle = GetById(id);
            if (bottle != null)
            {
                _context.Bottles.Remove(bottle);
                _context.SaveChanges();
                return bottle;
            }
            return null;
        }

        public Bottle? UpdateById(int id, Bottle data)
        {
            Bottle? bottle = GetById(id);
            if (bottle != null)
            {
                bottle.Name = data.Name;
                bottle.Volume = data.Volume;
                _context.SaveChanges();
                return bottle;
            }
            return null;
        }

    }
}
