namespace Data.AccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Data.AccessLayer.Abstractions;
using Data.Models;

internal class AddressAccessLayer : BaseAccessLayer<DataContext, Address>, IAddressAccessLayer
{
    /// <inheritdoc />
    public AddressAccessLayer(DataContext context)
        : base(context)
    {
    }
}
