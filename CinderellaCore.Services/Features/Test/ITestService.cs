using CinderellaCore.Model.Models;
using System.Collections.Generic;

namespace CinderellaCore.Services.Features.Test
{
    public interface ITestService
    {
        List<TestObj> GetAll();
    }
}