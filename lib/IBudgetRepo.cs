using System.Collections.Generic;

namespace lib
{
    interface IBudgetRepo
    {
        List<Budget> GetAll();
    }
}