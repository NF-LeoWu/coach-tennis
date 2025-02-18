using System.Collections.Generic;

namespace lib
{
    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }
}