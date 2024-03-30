using System.Collections.Generic;
using src.Application.Presenter;
using src.Domain;

namespace src.Infra.Presenter;

public class JsonPresenter : IPresenter
{
    public object Present(List<Invoice> output) => output;
}