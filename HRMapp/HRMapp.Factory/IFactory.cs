﻿using System;
using System.Collections.Generic;
using System.Text;
using HRMapp.DAL.Repositories;
using HRMapp.Logic;

namespace HRMapp.Factory
{
    public interface IFactory <T>
    {
        T Manage();
    }
}
