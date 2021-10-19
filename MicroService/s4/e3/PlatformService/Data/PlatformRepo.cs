using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private Context _context;

        public PlatformRepo(Context context)
        {
            _context = context;
        }
        public void CreatePlatform(Platform item)
        {
            if(item == null){
                throw new ArgumentNullException(nameof(item));
            }
            _context.Platforms.Add(item);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Platform GetPlatformById(int id)
        {
           return _context.Platforms.Where(x => x.Id == id).FirstOrDefault();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}