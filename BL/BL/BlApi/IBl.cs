﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BO;
namespace BlApi;

public interface IBl
{
    IOrder Order { get; }
    IProduct Product { get; }
    ICart Cart { get; }
}
