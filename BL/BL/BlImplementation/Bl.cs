using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

using BO;
using BlImplementation;
using DalApi;

namespace BlApi;

sealed internal class Bl : IBl
{
    static readonly Bl instance = new();
    static Bl() { }
    Bl() { }
    public static Bl Instance => instance;

    readonly IDal? Dal = DalApi.Factory.Get();
    public IOrder Order { get; } = new BlImplementation.BlOrder();
    public IProduct Product { get; } = new BlImplementation.BlProduct();
    public ICart Cart { get; } = new BlImplementation.BlCart();
}
