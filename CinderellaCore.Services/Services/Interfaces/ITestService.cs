using System;
using System.Collections.Generic;
using System.Text;
using CinderellaCore.Model.Models;

namespace CinderellaCore.Services.Services.Interfaces
{
    public interface ITestService
    {
        List<TestObj> GetAll();
    }
}