using CinderellaCore.Model.Models;
using System.Collections.Generic;

namespace CinderellaCore.Services.Services.Interfaces
{
    public interface ITestService
    {
        List<TestObj> GetAll();
    }
}