using System.Collections.Generic;
using src.Domain;

namespace src.Application.Presenter;

public interface IPresenter
{
    object Present(List<Invoice> output);
}
