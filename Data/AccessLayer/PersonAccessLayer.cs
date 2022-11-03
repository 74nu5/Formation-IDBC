namespace Data.AccessLayer;

using Data.AccessLayer.Abstractions;
using Data.Models;

internal class PersonAccessLayer : BaseAccessLayer<DataContext, Person>, IPersonAccessLayer
{
    public PersonAccessLayer(DataContext context)
        : base(context)
    {
    }
}
