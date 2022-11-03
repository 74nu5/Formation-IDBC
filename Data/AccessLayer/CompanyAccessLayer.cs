namespace Data.AccessLayer;

using Data.AccessLayer.Abstractions;
using Data.Models;

internal class CompanyAccessLayer : BaseAccessLayer<DataContext, Company>, ICompanyAccessLayer
{
    /// <inheritdoc />
    public CompanyAccessLayer(DataContext context)
        : base(context)
    {
    }
}
