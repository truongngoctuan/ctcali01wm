using System.Collections.Generic;

namespace wmWebApp.Models
{
    public interface IWorldReposity
    {
        IEnumerable<Agency> GetAllAgency();
    }
}